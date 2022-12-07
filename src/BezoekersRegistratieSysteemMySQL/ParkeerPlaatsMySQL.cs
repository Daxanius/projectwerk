using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;

namespace BezoekersRegistratieSysteemDL.ADOMySQL {
	public class ParkeerPlaatsMySQL : IParkeerplaatsRepository {
		/// <summary>
		/// Private lokale variabele connectiestring
		/// </summary>
		private string _connectieString;

		/// <summary>
		/// ParkeerPlaatsADOMS constructor krijgt connectie string als parameter.
		/// </summary>
		/// <param name="connectieString">Connectie string database</param>
		/// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
		public ParkeerPlaatsMySQL(string connectieString) {
			_connectieString = connectieString;
		}

		/// <summary>
		/// Zet SQL connectie op met desbetreffende database adv de lokale variabele [_connectieString].
		/// </summary>
		/// <returns>SQL connectie</returns>
		private MySqlConnection GetConnection() {
			return new MySqlConnection(_connectieString);
		}

		/// <summary>
		/// Controleerd of een nummerplaat ZONDER eindtijd al bestaat 
		/// </summary>
		/// <param name="nummerplaat">nummperplaat die gecontroleerd moet worden</param>
		/// <returns>True = bestaat | False = bestaat NIET</returns>
		public bool BestaatNummerplaat(string nummerplaat) {
			MySqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM Parkingplaatsen " +
						   "WHERE NummerPlaat = @nummerplaat " +
						   "AND EindTIjd IS NULL";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@nummerplaat", MySqlDbType.VarChar));
					cmd.Parameters["@nummerplaat"].Value = nummerplaat;
					long i = (long)cmd.ExecuteScalar();
					return (i > 0);
				}
			} catch (Exception ex) {
				throw new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Voegt een nummerplaat toe met correcte begin- en/of eindtijd en bedrijf waarvoor die komt
		/// <paramref name="parkeerplaats">ParkeerPlaats object die de gegens bevat die toegevoegd moeten worden</paramref>
		/// </summary>
		public void CheckNummerplaatIn(Parkeerplaats parkeerplaats) {
			MySqlConnection con = GetConnection();
			string query = "INSERT INTO Parkingplaatsen(NummerPlaat, StartTijd, EindTijd, BedrijfId) " +
						   "VALUES(@NummerPlaat, @StartTijd, @EindTijd, @BedrijfId)";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@nummerplaat", MySqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@StartTijd", MySqlDbType.DateTime));
					cmd.Parameters.Add(new MySqlParameter("@EindTijd", MySqlDbType.DateTime));
					cmd.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
					cmd.Parameters["@nummerplaat"].Value = parkeerplaats.Nummerplaat;
					cmd.Parameters["@StartTijd"].Value = parkeerplaats.Starttijd;
					cmd.Parameters["@EindTijd"].Value = parkeerplaats.Eindtijd.HasValue ? parkeerplaats.Eindtijd.Value : DBNull.Value;
					cmd.Parameters["@BedrijfId"].Value = parkeerplaats.Bedrijf.Id;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				throw new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Geeft een eindtijd aan een nummerplaat, enkel aan de nummerplaat die nog geen eindtijd heeft
		/// <paramref name="nummerplaat">Nummerplaat die een eindtijd moet krijgen</paramref>
		/// </summary>
		public void CheckNummerplaatUit(string nummerplaat) {
			MySqlConnection con = GetConnection();
			string query = "UPDATE Parkingplaatsen " +
						   "SET EindTijd = @EindTijd " +
						   "WHERE NummerPlaat = @nummerplaat AND EindTijd IS NOT NULL";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@nummerplaat", MySqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@EindTijd", MySqlDbType.DateTime));
					cmd.Parameters["@nummerplaat"].Value = nummerplaat;
					cmd.Parameters["@EindTijd"].Value = DateTime.Now;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				throw new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Aantal bezette plaatsen
		/// </summary>
		/// <param name="bedrijfId">Bedrijf id wiens huidige bezette aantal moet gereturned worden</param>
		/// <returns>int aantalBezettePlaatsen</returns>
		public int GeefHuidigBezetteParkeerplaatsenPerBedrijf(long bedrijfId) {
			try {
				Bedrijf bedrijf = new Bedrijf();
				bedrijf.ZetId(bedrijfId);
				return GeefNummerplaten(bedrijf, true, 1).Count();
			} catch (Exception ex) {
				throw new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Returned een lijst van string nummerplaten per bedrijf, kijkt op id of BTWNr
		/// </summary>
		/// <param name="bedrijf">Bedrijf wiens nummerplaten op de parking moeten gereturned worden</param>
		/// <returns>IReadOnlyList<String> Nummerplaten</returns>
        public IReadOnlyList<Parkeerplaats> GeefNummerplatenPerBedrijf(Bedrijf bedrijf) {
            try {
                return GeefNummerplaten(bedrijf, true, 1);
            } catch (Exception ex) {
                throw new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Returned een lijst van string nummerplaten per bedrijf, kijkt op id of BTWNr
        /// </summary>
        /// <param name="bedrijf">Bedrijf wiens nummerplaten op de parking moeten gereturned worden</param>
        /// <param name="bezet">Bedrijf wiens nummerplaten op de parking moeten gereturned worden</param>
        /// <returns>IReadOnlyList<String> Nummerplaten</returns>
        private IReadOnlyList<Parkeerplaats> GeefNummerplaten(Bedrijf bedrijf, bool? bezet, int? statusId) {
			MySqlConnection con = GetConnection();
			string query = "SELECT pp.Nummerplaat, pp.StartTijd, pp.EindTijd, pp.statusId " +
						   "FROM Parkingplaatsen pp";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (bedrijf.Id != 0) {
						query += " WHERE pp.BedrijfId = @BedrijfId";
						cmd.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
						cmd.Parameters["@BedrijfId"].Value = bedrijf.Id;
					} else {
						query += " JOIN Bedrijf b ON(pp.BedrijfId = b.Id) " +
								 "WHERE b.BTWNr = @BTWNr";
						cmd.Parameters.Add(new MySqlParameter("@BTWNr", MySqlDbType.VarChar));
						cmd.Parameters["@BTWNr"].Value = bedrijf.BTW;
					}
                    if (bezet.HasValue) {
                        string bezetOfNietBezet = bezet.Value ? "" : "NOT";
                        query += $" AND pp.EindTijd IS {bezetOfNietBezet} NULL";
                    }
                    if (statusId.HasValue) {
                        query += " AND pp.StatusId = @StatusId";
                        cmd.Parameters.Add(new MySqlParameter("@StatusId", SqlDbType.Int));
                        cmd.Parameters["@StatusId"].Value = statusId.Value;
                    }

                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    List<Parkeerplaats> nummerplaten = new List<Parkeerplaats>();
                    while (reader.Read()) {
                        DateTime start = (DateTime)reader["StartTijd"];
                        DateTime? eind = !reader.IsDBNull(reader.GetOrdinal("EindTijd")) ? (DateTime)reader["EindTijd"] : null;
                        nummerplaten.Add(new Parkeerplaats(bedrijf, start, eind, (string)reader["Nummerplaat"]));
                    }
                    return nummerplaten.AsReadOnly();
                }
			} catch (Exception ex) {
				ParkeerPlaatsMySQLException exx = new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijf", bedrijf);
				exx.Data.Add("bezet", bezet);
				throw exx;
			} finally {
				con.Close();
			}
		}

        public int GeefHuidigBezetteParkeerplaatsenVoorBedrijf(long id) {
            throw new NotImplementedException();
        }



        public GrafiekDag GeefUuroverzichtParkingVoorBedrijf(long id) {
            throw new NotImplementedException();
        }

        public GrafiekWeek GeefWeekoverzichtParkingVoorBedrijf(long id) {
            throw new NotImplementedException();
        }


    }
}
