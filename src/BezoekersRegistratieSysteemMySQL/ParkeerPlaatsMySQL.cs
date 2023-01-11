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
						   "SET EindTijd = @EindTijd, " +
						   "StatusId = NULL " +
                           "WHERE NummerPlaat = @nummerplaat AND EindTijd IS NULL AND StatusId = 1";
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
						cmd.Parameters.Add(new MySqlParameter("@StatusId", MySqlDbType.Int32));
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

		/// <summary>
		/// Returned het aantal bezette plaatsen op de parking per bedrijf
		/// </summary>
		/// <param name="bedrijfId">Bedrijf wiens data moet teruggeven worden</param>
		/// <returns>int aantal bezette plaatsen</returns>
		public int GeefHuidigBezetteParkeerplaatsenVoorBedrijf(long bedrijfId) {
			MySqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM Parkingplaatsen " +
						   "WHERE bedrijfId = @BedrijfId AND EindTijd IS NULL";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
					cmd.Parameters["@BedrijfId"].Value = bedrijfId;
					return Convert.ToInt32((long)cmd.ExecuteScalar());
				}
			} catch (Exception ex) {
				ParkeerPlaatsMySQLException exx = new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfid", bedrijfId);
				throw exx;
			} finally {
				con.Close();
			}
		}


		/// <summary>
		/// Returned een grafiek met dag details over de parking voor een specifiek bedrijf
		/// </summary>
		/// <param name="bedrijfId">Bedrijf wiens data moet teruggeven worden</param>
		/// <returns>Grafiek object</returns>
		public GrafiekDagDetail GeefUuroverzichtParkingVoorBedrijf(long bedrijfId) {
			MySqlConnection con = GetConnection();
			string query = "WITH " +
							"RECURSIVE hours AS( " +
								"SELECT 0 AS hour " +
								"UNION ALL " +
								"SELECT hour + 1 FROM hours WHERE hour < HOUR(NOW()) " +
								"), " +
							"ParkedHour AS( " +
								"SELECT " +
								"h.hour, " +
								"COUNT(pp.nummerplaat) AS parkedHour, " +
								"(SELECT COUNT(*) " +
								"FROM groupswork.parkingplaatsen pp " +
                                "WHERE pp.BedrijfId = @bedrijfId " +
								"AND (((hour <= HOUR(now()) " +
								"AND HOUR(pp.StartTijd) <= hour) " +
								"AND DATE(pp.starttijd) = DATE(now())) OR DATE(pp.starttijd) < DATE(now())) " +
								"AND (statusid = 1 AND " +
								"(pp.eindtijd is null AND hour <= HOUR(now())) " +
								"OR ( " +
								"DATE(pp.EindTijd) = DATE(now()) AND " +
								"HOUR(pp.EindTijd) >= Hour " +
								")) " +
								") AS parkedTotal " +
								"FROM hours h " +
                                "LEFT JOIN groupswork.parkingplaatsen pp ON(h.hour = HOUR(pp.StartTijd)) AND DATE(now()) = DATE(pp.starttijd) AND pp.BedrijfId = @bedrijfId " +
								"GROUP BY h.hour " +
							") " +
                            "SELECT CONCAT(ph.hour,'u') as hour, ph.parkedHour, ph.parkedTotal FROM ParkedHour ph ORDER BY ph.hour";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
					cmd.Parameters["@BedrijfId"].Value = bedrijfId;
					IDataReader reader = cmd.ExecuteReader();
					GrafiekDagDetail grafiek = new GrafiekDagDetail();
					while (reader.Read()) {
						string xwaarde = (string)reader["hour"];
						int checkIn = Convert.ToInt32((long)reader["parkedHour"]);
						int parkedTotal = Convert.ToInt32((long)reader["parkedTotal"]);
						grafiek.VoegWaardesToe(xwaarde, checkIn, parkedTotal);
					}
					return grafiek;
				}
			} catch (Exception ex) {
				ParkeerPlaatsMySQLException exx = new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfid", bedrijfId);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Returned een grafiek met data over de parking voor een specifiek bedrijf
		/// </summary>
		/// <param name="bedrijfId">Bedrijf wiens data moet teruggeven worden</param>
		/// <returns>Grafiek object</returns>
		public GrafiekDag GeefWeekoverzichtParkingVoorBedrijf(long bedrijfId) {
			MySqlConnection con = GetConnection();
			string query = "SET lc_time_names = 'nl_BE'; " +
							"WITH " +
								"RECURSIVE offset AS( " +
									"SELECT 0 AS dOffset " +
									"UNION ALL " +
									"SELECT dOffset - 1 FROM offset WHERE dOffset > -6 " +
								"), " +
								"days AS( " +
									"SELECT UPPER(SUBSTRING(DAYNAME(CONVERT(DATE_ADD(NOW(), INTERVAL o.dOffset DAY), date)), 1,2)) AS abbrDay, " +
									"CONVERT(DATE_ADD(NOW(), INTERVAL o.dOffset DAY), date) AS da " +
									"FROM offset o " +
								"), " +
								"parked AS( " +
                                    "SELECT d.abbrDay, (SELECT COUNT(*) FROM ParkingPlaatsen pl WHERE CONVERT(pl.StartTijd, date) = d.da AND bedrijfId = @BedrijfId) as totalCheckIn, d.da " +
									"FROM days d " +
								") " +
							"SELECT abbrDay, totalCheckIn FROM parked p ORDER by p.da";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
					cmd.Parameters["@BedrijfId"].Value = bedrijfId;
					IDataReader reader = cmd.ExecuteReader();
					GrafiekDag grafiek = new GrafiekDag();
					while (reader.Read()) {
						string xwaarde = (string)reader["abbrDay"];
						int checkIn = Convert.ToInt32((long)reader["totalCheckIn"]);
						grafiek.VoegWaardeToe(xwaarde, checkIn);
					}
					return grafiek;
				}
			} catch (Exception ex) {
				ParkeerPlaatsMySQLException exx = new ParkeerPlaatsMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfid", bedrijfId);
				throw exx;
			} finally {
				con.Close();
			}
		}
	}
}
