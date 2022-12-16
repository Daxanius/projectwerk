using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using System.Data;
using System.Data.SqlClient;

namespace BezoekersRegistratieSysteemDL.ADOMS {
	public class ParkeerPlaatsMsServer : IParkeerplaatsRepository {
		/// <summary>
		/// Private lokale variabele connectiestring
		/// </summary>
		private string _connectieString;

		/// <summary>
		/// ParkeerPlaatsADOMS constructor krijgt connectie string als parameter.
		/// </summary>
		/// <param name="connectieString">Connectie string database</param>
		/// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
		public ParkeerPlaatsMsServer(string connectieString) {
			_connectieString = connectieString;
		}

		/// <summary>
		/// Zet SQL connectie op met desbetreffende database adv de lokale variabele [_connectieString].
		/// </summary>
		/// <returns>SQL connectie</returns>
		private SqlConnection GetConnection() {
			return new SqlConnection(_connectieString);
		}

		/// <summary>
		/// Controleerd of een nummerplaat ZONDER eindtijd al bestaat 
		/// </summary>
		/// <param name="nummerplaat">nummperplaat die gecontroleerd moet worden</param>
		/// <returns>True = bestaat | False = bestaat NIET</returns>
		public bool BestaatNummerplaat(string nummerplaat) {
			SqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM Parkingplaatsen " +
						   "WHERE NummerPlaat = @nummerplaat " +
						   "AND EindTIjd IS NULL";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.VarChar));
					cmd.Parameters["@nummerplaat"].Value = nummerplaat;
					int i = (int)cmd.ExecuteScalar();
					return (i > 0);
				}
			} catch (Exception ex) {
				throw new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Voegt een nummerplaat toe met correcte begin- en/of eindtijd en bedrijf waarvoor die komt
		/// <paramref name="parkeerplaats">ParkeerPlaats object die de gegens bevat die toegevoegd moeten worden</paramref>
		/// </summary>
		public void CheckNummerplaatIn(Parkeerplaats parkeerplaats) {
			SqlConnection con = GetConnection();
			string query = "INSERT INTO Parkingplaatsen(NummerPlaat, StartTijd, EindTijd, BedrijfId) " +
						   "VALUES(@NummerPlaat, @StartTijd, @EindTijd, @BedrijfId)";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.VarChar));
					cmd.Parameters.Add(new SqlParameter("@StartTijd", SqlDbType.DateTime));
					cmd.Parameters.Add(new SqlParameter("@EindTijd", SqlDbType.DateTime));
					cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.BigInt));
					cmd.Parameters["@nummerplaat"].Value = parkeerplaats.Nummerplaat;
					cmd.Parameters["@StartTijd"].Value = parkeerplaats.Starttijd;
					cmd.Parameters["@EindTijd"].Value = parkeerplaats.Eindtijd.HasValue ? parkeerplaats.Eindtijd.Value : DBNull.Value;
					cmd.Parameters["@BedrijfId"].Value = parkeerplaats.Bedrijf.Id;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				throw new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Geeft een eindtijd aan een nummerplaat, enkel aan de nummerplaat die nog geen eindtijd heeft
		/// <paramref name="nummerplaat">Nummerplaat die een eindtijd moet krijgen</paramref>
		/// </summary>
		public void CheckNummerplaatUit(string nummerplaat) {
			SqlConnection con = GetConnection();
			string query = "UPDATE Parkingplaatsen " +
						   "SET EindTijd = @EindTijd " +
						   "WHERE NummerPlaat = @nummerplaat AND EindTijd IS NOT NULL";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.VarChar));
					cmd.Parameters.Add(new SqlParameter("@EindTijd", SqlDbType.DateTime));
					cmd.Parameters["@nummerplaat"].Value = nummerplaat;
					cmd.Parameters["@EindTijd"].Value = DateTime.Now;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				throw new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
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
				throw new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Returned een lijst van string nummerplaten per bedrijf, kijkt op id of BTWNr
		/// </summary>
		/// <param name="bedrijf">Bedrijf wiens parkeerplaatsen op de parking moeten gereturned worden</param>
		/// <returns>IReadOnlyList<Parkeerplaats> parkeerplaatsen</returns>
		public IReadOnlyList<Parkeerplaats> GeefNummerplatenPerBedrijf(Bedrijf bedrijf) {
			try {
				return GeefNummerplaten(bedrijf, true, 1);
			} catch (Exception ex) {
				throw new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Returned een lijst van string nummerplaten per bedrijf, kijkt op id of BTWNr
		/// </summary>
		/// <param name="bedrijf">Bedrijf wiens nummerplaten op de parking moeten gereturned worden</param>
		/// <param name="bezet">Bedrijf wiens nummerplaten op de parking moeten gereturned worden</param>
		/// <returns>IReadOnlyList<String> Nummerplaten</returns>
		private IReadOnlyList<Parkeerplaats> GeefNummerplaten(Bedrijf bedrijf, bool? bezet, int? statusId) {
			SqlConnection con = GetConnection();
			string query = "SELECT pp.Nummerplaat, pp.StartTijd, pp.EindTijd, pp.StatusId " +
						   "FROM Parkingplaatsen pp";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (bedrijf.Id != 0) {
						query += " WHERE pp.BedrijfId = @BedrijfId";
						cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.BigInt));
						cmd.Parameters["@BedrijfId"].Value = bedrijf.Id;
					} else {
						query += " JOIN Bedrijf b ON(pp.BedrijfId = b.Id) " +
								 "WHERE b.BTWNr = @BTWNr";
						cmd.Parameters.Add(new SqlParameter("@BTWNr", SqlDbType.VarChar));
						cmd.Parameters["@BTWNr"].Value = bedrijf.BTW;
					}
					if (bezet.HasValue) {
						string bezetOfNietBezet = bezet.Value ? "" : "NOT";
						query += $" AND pp.EindTijd IS {bezetOfNietBezet} NULL";
					}
					if (statusId.HasValue) {
						query += " AND pp.StatusId = @StatusId";
						cmd.Parameters.Add(new SqlParameter("@StatusId", SqlDbType.Int));
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
				ParkeerPlaatsMsServerException exx = new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijf", bedrijf);
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
			SqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM Parkingplaatsen " +
						   "WHERE bedrijfId = @BedrijfId AND EindTijd IS NULL";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.BigInt));
					cmd.Parameters["@BedrijfId"].Value = bedrijfId;
					return (int)cmd.ExecuteScalar();
				}
			} catch (Exception ex) {
				ParkeerPlaatsMsServerException exx = new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
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
			SqlConnection con = GetConnection();
			string query = "WITH " +
								"hours AS( " +
								"SELECT 0 AS hour " +
								"UNION ALL " +
								"SELECT hour + 1 FROM hours WHERE hour < 23 " +
								"), " +
							"ParkedHour AS( " +
								"SELECT " +
								"h.hour, " +
								"COUNT(pp.nummerplaat) AS parkedHour, " +
								"(SELECT COUNT(*) " +
								"FROM parkingplaatsen pp " +
								"WHERE " +
								"(pp.BedrijfId = @BedrijfId " +
								"AND ((hour <= DATEPART(HOUR, GETDATE()) " +
								"AND DATEPART(HOUR, pp.StartTijd) <= hour) " +
								"AND CONVERT(DATE, pp.starttijd) = CONVERT(DATE, GETDATE())) OR CONVERT(DATE, pp.starttijd) < CONVERT(DATE, GETDATE())) " +
								"AND (statusid = 1 AND " +
								"(pp.eindtijd is null AND hour <= DATEPART(HOUR, GETDATE())) " +
								"OR ( " +
								"CONVERT(DATE, pp.EindTijd) = CONVERT(DATE, GETDATE()) AND " +
								"DATEPART(HOUR, pp.EindTijd) >= Hour " +
								")) " +
								") AS parkedTotal " +
								"FROM hours h " +
								"LEFT JOIN parkingplaatsen pp ON(h.hour = DATEPART(HOUR, pp.StartTijd)) AND CONVERT(DATE, GETDATE()) = CONVERT(DATE, pp.starttijd) AND pp.BedrijfId = @BedrijfId " +
								"GROUP BY h.hour " +
							") " +
							"SELECT ph.hour, ph.parkedHour, ph.parkedTotal FROM ParkedHour ph ORDER BY ph.hour";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.BigInt));
					cmd.Parameters["@BedrijfId"].Value = bedrijfId;
					IDataReader reader = cmd.ExecuteReader();
					GrafiekDagDetail grafiek = new GrafiekDagDetail();
					while (reader.Read()) {
						string xwaarde = ((int)reader["hour"]).ToString();
						int checkIn = ((int)reader["parkedHour"]);
						int parkedTotal = ((int)reader["parkedTotal"]);
						grafiek.VoegWaardesToe(xwaarde, checkIn, parkedTotal);
					}
					return grafiek;
				}
			} catch (Exception ex) {
				ParkeerPlaatsMsServerException exx = new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
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
			SqlConnection con = GetConnection();
			string query = "SET LANGUAGE Dutch; " +
							"WITH " +
								"offset AS( " +
									"SELECT 0 AS dOffset " +
									"UNION ALL " +
									"SELECT dOffset - 1 FROM offset WHERE dOffset > -6 " +
								"), " +
								"days AS( " +
									"SELECT UPPER(SUBSTRING(DATENAME(WEEKDAY, CONVERT(date, DATEADD(DAY, o.dOffset, GETDATE()))), 1,2)) AS abbrDay, " +
									"CONVERT(date, DATEADD(DAY, o.dOffset, GETDATE())) AS da " +
									"FROM offset o " +
								"), " +
								"parked AS( " +
									"SELECT d.abbrDay, (SELECT COUNT(*) FROM ParkingPlaatsen pl WHERE CONVERT(date, pl.StartTijd) = d.da AND BedrijfId = 1) as totalCheckIn, d.da " +
									"FROM days d " +
								") " +
							"SELECT abbrDay, totalCheckIn FROM parked p ORDER by p.da";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.BigInt));
					cmd.Parameters["@BedrijfId"].Value = bedrijfId;
					IDataReader reader = cmd.ExecuteReader();
					GrafiekDag grafiek = new GrafiekDag();
					while (reader.Read()) {
						string xwaarde = ((int)reader["abbrDay"]).ToString();
						int checkIn = ((int)reader["totalCheckIn"]);
						grafiek.VoegWaardeToe(xwaarde, checkIn);
					}
					return grafiek;
				}
			} catch (Exception ex) {
				ParkeerPlaatsMsServerException exx = new ParkeerPlaatsMsServerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfid", bedrijfId);
				throw exx;
			} finally {
				con.Close();
			}
		}
	}
}
