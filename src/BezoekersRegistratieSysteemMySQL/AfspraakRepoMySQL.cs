﻿using BezoekersRegistratieSysteemBL;
using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;

namespace BezoekersRegistratieSysteemDL.ADOMySQL {
	public class AfspraakRepoMySQL : IAfspraakRepository {

		/// <summary>
		/// Private lokale variabele connectiestring
		/// </summary>
		private string _connectieString;

		/// <summary>
		/// AfspraakRepoADO constructor krijgt connectie string als parameter.
		/// </summary>
		/// <param name="connectieString">Connectie string database</param>
		/// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
		public AfspraakRepoMySQL(string connectieString) {
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
		/// Beëindigd afspraak via het fallback pad adhv parameter afspraak id.
		/// </summary>
		/// <exception cref="AfspraakMySQLException">Faalt afspraak te beëindigd</exception>
		/// <remarks>Afspraak krijgt statuscode 4 = 'Stopgezet door systeem'.</remarks>
		public void BeeindigAfspraakSysteem() {
			MySqlConnection con = GetConnection();
			string query = "UPDATE Afspraak " +
						   "SET AfspraakStatusId = 4, " +
						   "EindTijd = DATE_ADD(CONVERT(CONVERT(NOW(), DATE), DATETIME), INTERVAL -1 SECOND) " +
						   "WHERE AfspraakStatusId = 1 AND CONVERT(StartTijd, DATE) < CONVERT(NOW(), DATE)";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Beëindigd afspraak adhv bezoeker email adhv parameter bezoeker email.
		/// </summary>
		/// <param name="email">Emailadres van de  bezoeker wiens afspraak beëindigd wenst te worden.</param>
		/// <exception cref="AfspraakMySQLException">Faalt afspraak te beëindigd</exception>
		/// <remarks>Afspraak krijgt statuscode 3 = 'Stopgezet door gebruiker'.</remarks>
		public void BeeindigAfspraakOpEmail(string email) {
			try {
				BeeindigAfspraak(email, null, 3);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Beëindigd afspraak via het normale pad adhv parameter afspraak id.
		/// </summary>
		/// <param name="afspraakId">Id van de afspraak die beëindigd wenst te worden.</param>
		/// <exception cref="AfspraakMySQLException">Faalt afspraak te beëindigd</exception>
		/// <remarks>Afspraak krijgt statuscode 5 = 'Stopgezet door administratief medewerker'.</remarks>
		public void BeeindigAfspraakBezoeker(long afspraakId) {
			try {
				BeeindigAfspraak(null, afspraakId, 3);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Private methode beëindigd afspraak, kent een eindtijd toe en update afspraakstatus.
		/// </summary>
		/// <param name="bezoekerMail">Emailadres van de bezoeker wiens afspraak gewijzigd wenst te worden.</param>
		/// <param name="afspraakId">Id voor de afspraak die gewijzigd wenst te worden.</param>
		/// <param name="statusId">Id voor het toekennen van een status.</param>
		/// <exception cref="AfspraakMySQLException">Faalt afspraak te beëindigd en/of status toe te kennen.</exception>
		private void BeeindigAfspraak(string? bezoekerMail, long? afspraakId, int statusId) {
			MySqlConnection con = GetConnection();
			string query = "UPDATE Afspraak " +
						   "SET AfspraakStatusId = @statusId, " +
						   "EindTijd = @Eindtijd " +
						   "WHERE AfspraakStatusId = 1";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (!String.IsNullOrWhiteSpace(bezoekerMail)) {
						query += " AND BezoekerId = (SELECT Id FROM Bezoeker WHERE Email = @mail ORDER BY Id DESC LIMIT 1)";
						cmd.Parameters.Add(new MySqlParameter("@mail", MySqlDbType.VarChar));
						cmd.Parameters["@mail"].Value = bezoekerMail;
					} else {
						query += " AND id = @afspraakId";
						cmd.Parameters.Add(new MySqlParameter("@afspraakId", MySqlDbType.Int64));
						cmd.Parameters["@afspraakId"].Value = afspraakId.Value;
					}
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@statusId", MySqlDbType.Int32));
					cmd.Parameters.Add(new MySqlParameter("@Eindtijd", MySqlDbType.DateTime));
					cmd.Parameters["@statusId"].Value = statusId;
					cmd.Parameters["@Eindtijd"].Value = DateTime.Now;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("mail", bezoekerMail);
				exx.Data.Add("afspraakId", afspraakId);
				exx.Data.Add("statusId", statusId);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Verwijder gewenste afspraak.
		/// </summary>
		/// <param name="afspraakId">Id van afspraak die verwijderd wenst te worden.</param>
		/// <exception cref="AfspraakMySQLException">Faalt afspraak te verwijderen.</exception>
		/// <remarks>Afspraak krijgt statuscode 2 = 'Verwijderd'.</remarks>
		public void VerwijderAfspraak(long afspraakId) {
			try {
				VeranderStatusAfspraak(afspraakId, 2);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Private methode wijzigd statuscode afspraak.
		/// </summary>
		/// <param name="afspraakId">Id van de afspraak die gewijzigd wenst te worden.</param>
		/// <param name="statusId">Id voor het wijzigen van een status.</param>
		/// <exception cref="AfspraakMySQLException">Faalt om status van een afspraak te wijzigen.</exception>
		private void VeranderStatusAfspraak(long afspraakId, int statusId) {
			MySqlConnection con = GetConnection();
			string query = "UPDATE Afspraak " +
						   "SET AfspraakStatusId = @statusId " +
						   "WHERE Id = @afspraakid";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@afspraakid", MySqlDbType.Int64));
					cmd.Parameters.Add(new MySqlParameter("@statusId", MySqlDbType.Int32));
					cmd.Parameters["@afspraakid"].Value = afspraakId;
					cmd.Parameters["@statusId"].Value = statusId;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("afspraakId", afspraakId);
				exx.Data.Add("statusId", statusId);
				throw exx;
			} finally {
				con.Close();
			}
		}
		/// <summary>
		/// Gaat na of werknemer al een afspraak heeft bij een ander bedrijf
		/// </summary>
		/// <param name="afspraak">Afspraak object dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="AfspraakMySQLException">Faalt om bestaan afspraak te verifiëren op basis van het afspraak object.</exception>
		public bool HeeftWerknemerVanAnderBedrijfEenLopendeAfspraak(Afspraak afspraak) {
			MySqlConnection con = GetConnection();
			string query = "SElECT COUNT(*) " +
						   "FROM Werknemerbedrijf wb " +
						   "WHERE wb.WerknemerId = @werknemerId AND wb.BedrijfId != @bedrijfId " +
						   "AND (SELECT COUNT(*) FROM Afspraak a WHERE wb.Id = a.WerknemerBedrijfId AND a.AfspraakStatusId = 1) >= 1";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@werknemerId", MySqlDbType.Int64));
					cmd.Parameters.Add(new MySqlParameter("@bedrijfId", MySqlDbType.Int64));
					cmd.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
					cmd.Parameters["@bedrijfId"].Value = afspraak.Bedrijf.Id;
					int i = (int)cmd.ExecuteScalar();
					return (i > 0);
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("afspraak", afspraak);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Gaat na of afspraak bestaat adhv een afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="AfspraakMySQLException">Faalt om bestaan afspraak te verifiëren op basis van het afspraak object.</exception>
		public bool BestaatAfspraak(Afspraak afspraak) {
			try {
				return BestaatAfspraak(afspraak, null, null, null);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} object {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Gaat na of afspraak bestaat adhv parameter afspraak id.
		/// </summary>
		/// <param name="afspraakid">Id van de afspraak die gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="AfspraakMySQLException">Faalt om bestaan afspraak te verifiëren op basis van het id.</exception>
		public bool BestaatAfspraak(long afspraakid) {
			try {
				return BestaatAfspraak(null, afspraakid, null, null);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} id {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Gaat na of lopende afspraak bestaat adhv een afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="AfspraakMySQLException">Faalt om bestaan afspraak te verifiëren op basis van het afspraak object.</exception>
		/// <remarks>Controle mbv statuscode 1 = 'In gang'</remarks>
		public bool BestaatLopendeAfspraak(Afspraak afspraak) {
			try {
				return BestaatAfspraak(afspraak, null, null, 1);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} object {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Gaat na of lopende afspraak bestaat adhv een bezoeker email.
		/// </summary>
		/// <param name="bezoekerMail">Bezoeker mail dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="AfspraakMySQLException">Faalt om bestaan afspraak te verifiëren op basis van het afspraak object.</exception>
		/// <remarks>Controle mbv statuscode 1 = 'In gang'</remarks>
		public bool BestaatAfspraak(string bezoekerMail) {
			try {
				return BestaatAfspraak(null, null, bezoekerMail, 1);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} object {ex.Message}", ex);
			}
		}
		/// <summary>
		/// Private methode gaat na of lopende afspraak bestaat adhv een afspraak object of parameter afspraak id.
		/// </summary>
		/// <param name="afspraak">Optioneel: Afspraak object dat gecontroleerd wenst te worden.</param>
		/// <param name="afspraakid">Optioneel: Id van de afspraak die gecontroleerd wenst te worden.</param>
		/// <param name="bezoekerMail">Optioneel: Mail van bezoeker die gecontroleerd wenst te worden.</param>
		/// <param name="afspraakStatus">Optioneel: Afspraakstatus.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="AfspraakMySQLException">Faalt om bestaan afspraak te verifiëren op basis van het afspraak object of id.</exception>
		private bool BestaatAfspraak(Afspraak afspraak, long? afspraakid, string? bezoekerMail, int? afspraakStatus) {
			MySqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM Afspraak a ";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					//Afspraak object
					if (afspraak is not null) {
						if (afspraak.Id != 0) {
							query += "WHERE a.Id = @id";
							cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
							cmd.Parameters["@id"].Value = afspraak.Id;
						} else {
							query += "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
									 "WHERE bz.Email = @bmail AND a.eindTijd is null";
							cmd.Parameters.Add(new MySqlParameter("@bmail", MySqlDbType.VarChar));
							cmd.Parameters["@bmail"].Value = afspraak.Bezoeker.Email;
						}
					}
					//Afspraak Id
					if (afspraakid.HasValue) {
						query += "WHERE a.Id = @id";
						cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
						cmd.Parameters["@id"].Value = afspraakid.Value;
					}
					//Bezoeker mail
					if (!String.IsNullOrWhiteSpace(bezoekerMail)) {
						query += "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
								 "WHERE bz.Email = @bmail AND a.eindTijd is null";
						cmd.Parameters.Add(new MySqlParameter("@bmail", MySqlDbType.VarChar));
						cmd.Parameters["@bmail"].Value = bezoekerMail;
					}
					//Status Id
					if (afspraakStatus.HasValue) {
						query += " AND a.AfspraakStatusId = @statusId";
						cmd.Parameters.Add(new MySqlParameter("@statusId", MySqlDbType.Int32));
						cmd.Parameters["@statusId"].Value = afspraakStatus.Value;
					}
					cmd.CommandText = query;
					long i = (long)cmd.ExecuteScalar();
					return (i > 0);
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("afspraak", afspraak);
				exx.Data.Add("afspraakId", afspraakid);
				exx.Data.Add("bezoekerMail", bezoekerMail);
				exx.Data.Add("statusId", afspraakStatus);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Bewerkt gegevens van een afspraak adhv afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat gewijzigd wenst te worden in de databank.</param>
		/// <exception cref="AfspraakMySQLException">Faalt afspraak te wijzigen.</exception>
		public void BewerkAfspraak(Afspraak afspraak) {
			MySqlConnection con = GetConnection();
			//SELECT WORD GEBRUIKT OM EEN ACCURATE STATUSID IN TE STELLEN.
			string querySelect = "SELECT AfspraakStatusId " +
								 "FROM Afspraak " +
								 "WHERE Id = @afspraakid";

			string queryUpdateAfspraak = "UPDATE Afspraak " +
										 "SET StartTijd = @start, " +
										 "EindTijd = @eind, " +
										 "WerknemerBedrijfId = (SELECT wb.Id " +
																"FROM Werknemerbedrijf wb " +
																"WHERE wb.BedrijfId = @bedrijfId AND " +
																"wb.WerknemerId = @werknemerId AND " +
																"wb.FunctieId = (SELECT f.Id " +
																				"FROM Functie f " +
																				"WHERE f.FunctieNaam = @functienaam " +
																				") " +
																"AND Status = 1" +
																"), " +
										 "AfspraakstatusId = @afspraakstatusId  " +
										 "WHERE Id = @afspraakid";

			string queryUpdateBezoeker = "UPDATE Bezoeker " +
										 "SET ANaam = @ANaam, " +
										 "VNaam = @VNaam, " +
										 "Email = @Email, " +
										 "EigenBedrijf = @EigenBedrijf " +
										 "WHERE Id = @id";

			con.Open();
			MySqlTransaction trans = con.BeginTransaction();
			try {
				using (MySqlCommand cmdSelect = con.CreateCommand())
				using (MySqlCommand cmdUpdateBezoeker = con.CreateCommand())
				using (MySqlCommand cmdUpdateAfspraak = con.CreateCommand()) {
					//Geeft de statusID van de afspraak die gevraagd werd.
					cmdSelect.Transaction = trans;
					cmdSelect.CommandText = querySelect;
					cmdSelect.Parameters.Add(new MySqlParameter("@afspraakid", MySqlDbType.Int64));
					cmdSelect.Parameters["@afspraakid"].Value = afspraak.Id;
					int currentAfspraakStatusId = (int)cmdSelect.ExecuteScalar();
					//Bewerkt de gevraagde afspraak.
					cmdUpdateAfspraak.Transaction = trans;
					cmdUpdateAfspraak.CommandText = queryUpdateAfspraak;
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@afspraakid", MySqlDbType.Int64));
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@start", MySqlDbType.DateTime));
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@eind", MySqlDbType.DateTime));
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@bedrijfId", MySqlDbType.Int64));
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@werknemerId", MySqlDbType.Int64));
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@bezoekerId", MySqlDbType.Int64));
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@afspraakstatusId", MySqlDbType.Int32));
					cmdUpdateAfspraak.Parameters.Add(new MySqlParameter("@functienaam", MySqlDbType.VarChar));
					cmdUpdateAfspraak.Parameters["@afspraakid"].Value = afspraak.Id;
					cmdUpdateAfspraak.Parameters["@start"].Value = afspraak.Starttijd;
					cmdUpdateAfspraak.Parameters["@eind"].Value = afspraak.Eindtijd is not null ? afspraak.Eindtijd : DBNull.Value;
					cmdUpdateAfspraak.Parameters["@afspraakstatusId"].Value = afspraak.Eindtijd is not null && currentAfspraakStatusId == 1 ? 5 : afspraak.Eindtijd is not null && currentAfspraakStatusId != 1 ? currentAfspraakStatusId : 1;
					//FUNCTIE GETBEDRIJF
					var bedrijf = afspraak.Werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Keys.First();
					var functie = afspraak.Werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Values.First().GeefWerknemerFuncties().First();
					cmdUpdateAfspraak.Parameters["@bedrijfId"].Value = bedrijf.Id;
					cmdUpdateAfspraak.Parameters["@functienaam"].Value = functie;
					cmdUpdateAfspraak.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
					cmdUpdateAfspraak.ExecuteNonQuery();
					//Update Bezoeker
					cmdUpdateBezoeker.Transaction = trans;
					cmdUpdateBezoeker.CommandText = queryUpdateBezoeker;
					cmdUpdateBezoeker.Parameters.Add(new MySqlParameter("@ANaam", MySqlDbType.VarChar));
					cmdUpdateBezoeker.Parameters.Add(new MySqlParameter("@VNaam", MySqlDbType.VarChar));
					cmdUpdateBezoeker.Parameters.Add(new MySqlParameter("@Email", MySqlDbType.VarChar));
					cmdUpdateBezoeker.Parameters.Add(new MySqlParameter("@EigenBedrijf", MySqlDbType.VarChar));
					cmdUpdateBezoeker.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
					cmdUpdateBezoeker.Parameters["@ANaam"].Value = afspraak.Bezoeker.Achternaam;
					cmdUpdateBezoeker.Parameters["@VNaam"].Value = afspraak.Bezoeker.Voornaam;
					cmdUpdateBezoeker.Parameters["@Email"].Value = afspraak.Bezoeker.Email;
					cmdUpdateBezoeker.Parameters["@EigenBedrijf"].Value = afspraak.Bezoeker.Bedrijf;
					cmdUpdateBezoeker.Parameters["@id"].Value = afspraak.Bezoeker.Id;
					cmdUpdateBezoeker.ExecuteNonQuery();
					trans.Commit();
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("afspraak", afspraak);
				trans.Rollback();
				throw exx;
			} finally {
				con.Close();
			}
		}

		#region GeefAfspraak
		/// <summary>
		/// Haalt afspraak op uit de databank adhv parameter afspraak id.
		/// </summary>
		/// <param name="afspraakId">Id van de gewenste afspraak.</param>
		/// <returns>Gewenst afspraak object</returns>
		/// <exception cref="AfspraakMySQLException">Faalt om afspraak object op te halen op basis van het id.</exception>
		public Afspraak GeefAfspraak(long afspraakId) {
			MySqlConnection con = GetConnection();
			/* INFO SELECT
             * Afspraak
             * Bezoeker
             * Bedrijf
             * Werknemer
             * Functie Medewerker
             */
			string query = "SELECT a.Id as AfspraakId, a.StartTijd, a.EindTijd, " +
						   "bz.Id as BezoekerId, bz.ANaam as BezoekerANaam, bz.VNaam as BezoekerVNaam, bz.Email as BezoekerMail, bz.EigenBedrijf as BezoekerBedrijf, " +
						   "b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr, b.TeleNr, b.Email as BedrijfEmail, b.Adres as BedrijfAdres, b.BTWChecked, " +
						   "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, wb.WerknemerEmail, " +
						   "f.FunctieNaam, afs.AfspraakStatusNaam " +
						   "FROM Afspraak a " +
						   "JOIN Werknemerbedrijf as wb ON(a.WerknemerBedrijfId = wb.Id) " +
						   "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
						   "JOIN Werknemer w ON(wb.WerknemerId = w.Id) " +
						   "JOIN Bedrijf b ON(wb.BedrijfId = b.Id) " +
						   "JOIN Functie f ON(wb.FunctieId = f.Id) " +
						   "JOIN AfspraakStatus afs ON (afs.Id = a.AfspraakStatusId) " +
						   "WHERE a.Id = @afspraakid";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@afspraakid", MySqlDbType.Int64));
					cmd.Parameters["@afspraakid"].Value = afspraakId;
					IDataReader reader = cmd.ExecuteReader();
					Afspraak afspraak = null;
					while (reader.Read()) {
						//Afspraak portie
						DateTime start = (DateTime)reader["StartTijd"];
						DateTime? eind = !reader.IsDBNull(reader.GetOrdinal("EindTijd")) ? (DateTime)reader["EindTijd"] : null;
						//bezoeker portie
						long bezoekerId = (long)reader["BezoekerId"];
						string bezoekerAnaam = (string)reader["BezoekerANaam"];
						string bezoekerVnaam = (string)reader["BezoekerVNaam"];
						string bezoekerMail = (string)reader["BezoekerMail"];
						string bezoekerBedrijf = (string)reader["BezoekerBedrijf"];
						//bedrijf portie
						long bedrijfId = (long)reader["BedrijfId"];
						string bedrijfNaam = (string)reader["BedrijfNaam"];
						string bedrijfBTWNr = (string)reader["BTWNr"];
						string bedrijfTeleNr = (string)reader["TeleNr"];
						string bedrijfMail = (string)reader["BedrijfEmail"];
						string bedrijfAdres = (string)reader["BedrijfAdres"];
						bool bedrijfBTWChecked = (ulong)reader["BTWChecked"] == 0 ? false : true;
						//werknemer portie
						long werknemerId = (long)reader["WerknemerId"];
						string werknemerANaam = (string)reader["WerknemerANaam"];
						string werknemerVNaam = (string)reader["WerknemerVNaam"];
						string werknemerMail = (string)reader["WerknemerEmail"];
						//functie portie
						string functieNaam = (string)reader["FunctieNaam"];
						Werknemer werknemer = new(werknemerId, werknemerVNaam, werknemerANaam);
						Bedrijf bedrijf = new(bedrijfId, bedrijfNaam, bedrijfBTWNr, bedrijfBTWChecked, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
						werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
						string AfspraakStatus = (string)reader["AfspraakStatusNaam"];
						afspraak = new Afspraak(afspraakId, start, eind, bedrijf, new Bezoeker(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer, AfspraakStatus);
					}
					return afspraak;
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("afspraakId", afspraakId);
				throw exx;
			} finally {
				con.Close();
			}
		}
		#endregion

		#region VoegAfspraakToe
		/// <summary>
		/// Voegt afspraak toe in de databank adhv een afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat toegevoegd wenst te worden.</param>
		/// <returns>Afspraak object MET id</returns>
		/// <exception cref="AfspraakMySQLException">Faalt afspraak toe te voegen op basis van het afspraak object.</exception>
		public Afspraak VoegAfspraakToe(Afspraak afspraak) {
			MySqlConnection con = GetConnection();
			string queryBezoeker = "INSERT INTO Bezoeker(ANaam, VNaam, EMail, EigenBedrijf) " +
								   "VALUES(@ANaam,@VNaam,@EMail,@EigenBedrijf);";
			string queryBezoekerSelect = "SELECT id FROM Bezoeker WHERE id = LAST_INSERT_ID();";

			string queryAfspraak = "INSERT INTO Afspraak(StartTijd, EindTijd, WerknemerbedrijfId, AfspraakStatusId, BezoekerId) " +
								   "VALUES(@start,@eind,(SELECT Id FROM Werknemerbedrijf WHERE WerknemerId = @werknemerId AND BedrijfId = @bedrijfId LIMIT 1), @AfspraakStatusId ,@bezoekerId);";
			string qeuryAfspraakSelect = "SELECT id FROM Afspraak WHERE id = LAST_INSERT_ID();";
			con.Open();
			MySqlTransaction trans = con.BeginTransaction();
			try {
				using (MySqlCommand cmdBezoekerSelect = con.CreateCommand())
				using (MySqlCommand cmdAfspraakSelect = con.CreateCommand())
				using (MySqlCommand cmdBezoekerInsert = con.CreateCommand())
				using (MySqlCommand cmdAfspraakInsert = con.CreateCommand()) {
					//Bezoeker portie
					cmdBezoekerInsert.Transaction = trans;
					cmdBezoekerInsert.CommandText = queryBezoeker;
					cmdBezoekerInsert.Parameters.Add(new MySqlParameter("@ANaam", MySqlDbType.VarChar));
					cmdBezoekerInsert.Parameters.Add(new MySqlParameter("@VNaam", MySqlDbType.VarChar));
					cmdBezoekerInsert.Parameters.Add(new MySqlParameter("@EMail", MySqlDbType.VarChar));
					cmdBezoekerInsert.Parameters.Add(new MySqlParameter("@EigenBedrijf", MySqlDbType.VarChar));
					cmdBezoekerInsert.Parameters["@ANaam"].Value = afspraak.Bezoeker.Achternaam;
					cmdBezoekerInsert.Parameters["@VNaam"].Value = afspraak.Bezoeker.Voornaam;
					cmdBezoekerInsert.Parameters["@EMail"].Value = afspraak.Bezoeker.Email;
					cmdBezoekerInsert.Parameters["@EigenBedrijf"].Value = afspraak.Bezoeker.Bedrijf;
					cmdBezoekerInsert.ExecuteNonQuery();
					//TODO: miss dispose gebruiken

					cmdBezoekerSelect.Transaction = trans;
					cmdBezoekerSelect.CommandText = queryBezoekerSelect;
					long bezoekerId = (long)cmdBezoekerSelect.ExecuteScalar();
					//Afspraak portie
					cmdAfspraakInsert.Transaction = trans;
					cmdAfspraakInsert.CommandText = queryAfspraak;
					cmdAfspraakInsert.Parameters.Add(new MySqlParameter("@start", MySqlDbType.DateTime));
					cmdAfspraakInsert.Parameters.Add(new MySqlParameter("@eind", MySqlDbType.DateTime));
					cmdAfspraakInsert.Parameters.Add(new MySqlParameter("@werknemerId", MySqlDbType.Int64));
					cmdAfspraakInsert.Parameters.Add(new MySqlParameter("@bedrijfId", MySqlDbType.Int64));
					cmdAfspraakInsert.Parameters.Add(new MySqlParameter("@AfspraakStatusId", MySqlDbType.Int32));
					cmdAfspraakInsert.Parameters.Add(new MySqlParameter("@bezoekerId", MySqlDbType.Int64));
					cmdAfspraakInsert.Parameters["@start"].Value = afspraak.Starttijd;
					cmdAfspraakInsert.Parameters["@eind"].Value = afspraak.Eindtijd is not null ? afspraak.Eindtijd : DBNull.Value;
					cmdAfspraakInsert.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
					cmdAfspraakInsert.Parameters["@bedrijfId"].Value = afspraak.Bedrijf.Id;
					cmdAfspraakInsert.Parameters["@AfspraakStatusId"].Value = afspraak.Eindtijd is not null ? 5 : 1;
					cmdAfspraakInsert.Parameters["@bezoekerId"].Value = bezoekerId;
					cmdAfspraakInsert.ExecuteNonQuery();

					cmdAfspraakSelect.Transaction = trans;
					cmdAfspraakSelect.CommandText = qeuryAfspraakSelect;
					long i = (long)cmdAfspraakSelect.ExecuteScalar();
					afspraak.ZetId(i);
					afspraak.Bezoeker.ZetId(bezoekerId);
					trans.Commit();
					return afspraak;
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("afspraak", afspraak);
				trans.Rollback();
				throw exx;
			} finally {
				con.Close();
			}
		}
		#endregion

		#region HuidigeAfspraken
		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten.
		/// </summary>
		/// <returns>IReadOnlyList van afspraak objecten waar statuscode gelijk is aan 1 = 'In gang'.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen.</exception>
		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
			try {
				return GeefHuidigeAfspraken(null, null, null);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameter bedrijf id.
		/// </summary>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten waar statuscode gelijk is aan 1 = 'In gang' PER bedrijf.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van het bedrijf id.</exception>
		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(long bedrijfId) {
			try {
				return GeefHuidigeAfspraken(bedrijfId, null, null);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters bezoeker id en bedrijf id.
		/// </summary>
		/// <param name="bezoekerId">Id van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>Gewenste afspraak object waar statuscode gelijk is aan 1 = 'In gang' PER bezoeker per bedrijf.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van het bezoeker id en bedrijf id.</exception>
		public Afspraak GeefHuidigeAfspraakBezoekerPerBerijf(long bezoekerId, long bedrijfId) {
			try {
				return GeefHuidigeAfspraken(bedrijfId, null, bezoekerId).First();
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters werknemer id en bedrijf id.
		/// </summary>
		/// <param name="werknemerId">Id van de werknemer waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten waar statuscode gelijk is aan 1 = 'In gang' PER werknemer per bedrijf.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van het werknemer id en bedrijf id.</exception>
		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemerPerBedrijf(long werknemerId, long bedrijfId) {
			try {
				return GeefHuidigeAfspraken(bedrijfId, werknemerId, null);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}


		/// <summary>
		/// Private methode die een lijst samenstelt van huidige afspraken met enkel lees rechten adhv parameters bedrijf id, werknemer id of bezoeker id.
		/// </summary>
		/// <param name="_bedrijfId">Optioneel: Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_werknemerId">Optioneel: Id van de werknemer waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_bezoekerId">Optioneel: Id van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten waar statuscode gelijk is aan 1 = 'In gang' PER bedrijf en/of werknemer en/of bezoeker.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van het werknemer id en bedrijf id.</exception>
		private IReadOnlyList<Afspraak> GeefHuidigeAfspraken(long? _bedrijfId, long? _werknemerId, long? _bezoekerId) {
			MySqlConnection con = GetConnection();
			/* INFO SELECT
             * Afspraak
             * Bezoeker
             * Bedrijf
             * Werknemer
             * Functie Medewerker
             */
			string query = "SELECT a.Id as AfspraakId, a.StartTijd, a.EindTijd, " +
						   "bz.Id as BezoekerId, bz.ANaam as BezoekerANaam, bz.VNaam as BezoekerVNaam, bz.Email as BezoekerMail, bz.EigenBedrijf as BezoekerBedrijf, " +
						   "b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr, b.TeleNr, b.Email as BedrijfEmail, b.Adres as BedrijfAdres, b.BTWChecked, " +
						   "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, wb.WerknemerEmail, " +
						   "f.FunctieNaam, afs.AfspraakStatusNaam " +
						   "FROM Afspraak a " +
						   "JOIN Werknemerbedrijf as wb ON(a.WerknemerBedrijfId = wb.Id) " +
						   "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
						   "JOIN Werknemer w ON(wb.WerknemerId = w.Id) " +
						   "JOIN Bedrijf b ON(wb.BedrijfId = b.Id) " +
						   "JOIN Functie f ON(wb.FunctieId = f.Id) " +
						   "JOIN AfspraakStatus afs ON (afs.Id = a.AfspraakStatusId) " +
						   "WHERE a.AfspraakStatusId = 1";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (_bedrijfId.HasValue) {
						query += " AND b.id = @bedrijfId";
						cmd.Parameters.Add(new MySqlParameter("@bedrijfId", MySqlDbType.Int64));
						cmd.Parameters["@bedrijfId"].Value = _bedrijfId.Value;
					}
					if (_werknemerId.HasValue) {
						query += " AND w.id = @werknemerId";
						cmd.Parameters.Add(new MySqlParameter("@werknemerId", MySqlDbType.Int64));
						cmd.Parameters["@werknemerId"].Value = _werknemerId.Value;
					}
					if (_bezoekerId.HasValue) {
						query += " AND bz.id = @bezoekerId";
						cmd.Parameters.Add(new MySqlParameter("@bezoekerId", MySqlDbType.Int64));
						cmd.Parameters["@bezoekerId"].Value = _bezoekerId.Value;
					}
					query += " ORDER BY b.id, w.id, f.FunctieNaam";
					cmd.CommandText = query;
					IDataReader reader = cmd.ExecuteReader();
					List<Afspraak> afspraken = new List<Afspraak>();
					Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					string functieNaam = "";
					string werknemerMail = "";
					while (reader.Read()) {
						//Afspraak portie
						long afspraakId = (long)reader["AfspraakId"];
						DateTime start = (DateTime)reader["StartTijd"];
						DateTime? eind = !reader.IsDBNull(reader.GetOrdinal("EindTijd")) ? (DateTime)reader["EindTijd"] : null;
						//bezoeker portie
						long bezoekerId = (long)reader["BezoekerId"];
						string bezoekerAnaam = (string)reader["BezoekerANaam"];
						string bezoekerVnaam = (string)reader["BezoekerVNaam"];
						string bezoekerMail = (string)reader["BezoekerMail"];
						string bezoekerBedrijf = (string)reader["BezoekerBedrijf"];
						//bedrijf portie
						if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"]) {
							long bedrijfId = (long)reader["BedrijfId"];
							string bedrijfNaam = (string)reader["BedrijfNaam"];
							string bedrijfBTWNr = (string)reader["BTWNr"];
							string bedrijfTeleNr = (string)reader["TeleNr"];
							string bedrijfMail = (string)reader["BedrijfEmail"];
							string bedrijfAdres = (string)reader["BedrijfAdres"];
							bool bedrijfBTWChecked = (ulong)reader["BTWChecked"] == 0 ? false : true;
							bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTWNr, bedrijfBTWChecked, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
						}
						//werknemer portie
						if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
							long werknemerId = (long)reader["WerknemerId"];
							string werknemerANaam = (string)reader["WerknemerANaam"];
							string werknemerVNaam = (string)reader["WerknemerVNaam"];
							werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerANaam);
						}
						//functie portie
						if (String.IsNullOrWhiteSpace(functieNaam) || !werknemer.GeefBedrijvenEnFunctiesPerWerknemer().ContainsKey(bedrijf) || !werknemer.GeefBedrijvenEnFunctiesPerWerknemer()[bedrijf].GeefWerknemerFuncties().Contains(Nutsvoorziening.NaamOpmaak((string)reader["FunctieNaam"]))) {
							functieNaam = (string)reader["FunctieNaam"];
							werknemerMail = (string)reader["WerknemerEmail"];
							werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
						}
						string AfspraakStatus = (string)reader["AfspraakStatusNaam"];
						afspraken.Add(new Afspraak(afspraakId, start, eind, bedrijf, new Bezoeker(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer, AfspraakStatus));
					}
					return afspraken.AsReadOnly();
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfId", _bedrijfId);
				exx.Data.Add("werknemerId", _werknemerId);
				exx.Data.Add("bezoekerId", _bezoekerId);
				throw exx;
			} finally {
				con.Close();
			}
		}
		#endregion

		#region AlleAfspraken

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameter datum.
		/// </summary>
		/// <param name="datum">Datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten waar starttijd.Date = datum.Date.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van datum.</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
			try {
				return GeefAlleAfspraken(null, null, null, null, null, null, datum);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters bedrijf id en datum.
		/// </summary>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="datum">Datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten PER bedrijf en waar starttijd.Date = datum.Date.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van bedrijf id en datum.</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(long bedrijfId, DateTime datum) {
			try {
				return GeefAlleAfspraken(bedrijfId, null, null, null, null, null, datum);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters bezoeker id, bedrijf id en datum.
		/// </summary>
		/// <param name="bezoekerId">Id van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="datum">Datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten PER bedrijf per bezoeker en waar starttijd.Date = datum.Date</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van bezoeker id, bedrijf id en datum.</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpDagPerBedrijf(long bezoekerId, DateTime datum, long bedrijfId) {
			try {
				return GeefAlleAfspraken(bedrijfId, null, bezoekerId, null, null, null, datum);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters bezoeker voornaam/achternaam/email en bedrijf id.
		/// </summary>
		/// <param name="bezoekerVNaam">Optioneel: Voornaam van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bezoekerANaam">Optioneel: Achternaam van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bezoekerMail">Optioneel: Email van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten PER bedrijf per bezoeker op voornaam/achternaam/email en waar starttijd.Date = datum.Date.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van bezoeker voornaam/achternaam/email en bedrijf id.</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(string bezoekerVNaam, string bezoekerANaam, string bezoekerMail, long bedrijfId) {
			try {
				return GeefAlleAfspraken(bedrijfId, null, null, bezoekerVNaam, bezoekerANaam, bezoekerMail, null);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}


		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters werknemer id en berdijf id.
		/// </summary>
		/// <param name="werknemerId">Id van de werknemer waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten PER bedrijf per werknemer.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van werknemer id en bedrijf id.</exception>
		public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemerPerBedrijf(long werknemerId, long bedrijfId) {
			try {
				return GeefAlleAfspraken(bedrijfId, werknemerId, null, null, null, null, null);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters werknemer id, bedrijf id en datum.
		/// </summary>
		/// <param name="werknemerId">Id van de werknemer waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="datum">datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijfId">Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten PER bedrijf per werknemer en waar starttijd.Date = datum.Date.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van werknemer id, bedrijf id en datum.</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDagPerBedrijf(long werknemerId, DateTime datum, long bedrijfId) {
			try {
				return GeefAlleAfspraken(bedrijfId, werknemerId, null, null, null, null, datum);
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Private methode die alle afspraken op combinatie datum of/en werknemer ophaalt uit de databank.
		/// </summary>
		/// <param name="_bedrijfId">Optioneel: Id van het bedrijf waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_werknemerId">Optioneel: Id van de werknemer waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_bezoekerId">Optioneel: Id van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_bezoekerVNaam">Optioneel: Voornaam van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_bezoekerANaam">Optioneel: Achternaam van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_bezoekerMail">Optioneel: Email van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="_datum">Optioneel: datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten PER bedrijf per werknemer/bezoeker en/of waar starttijd.Date = datum.Date</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van afspraak objecten samen te stellen op basis van bedrijf id, werknemer- of bezoekerid/info en datum.</exception>
		private IReadOnlyList<Afspraak> GeefAlleAfspraken(long? _bedrijfId, long? _werknemerId, long? _bezoekerId, string? _bezoekerVNaam, string? _bezoekerANaam, string? _bezoekerMail, DateTime? _datum) {
			MySqlConnection con = GetConnection();
			/* INFO SELECT
             * Afspraak
             * Bezoeker
             * Bedrijf
             * Werknemer
             * Functie Medewerker
             */
			List<Afspraak> afspraken = new List<Afspraak>();
			string query = "SELECT a.Id as AfspraakId, a.StartTijd, a.EindTijd, " +
						   "bz.Id as BezoekerId, bz.ANaam as BezoekerANaam, bz.VNaam as BezoekerVNaam, bz.Email as BezoekerMail, bz.EigenBedrijf as BezoekerBedrijf, " +
						   "b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr, b.TeleNr, b.Email as BedrijfEmail, b.Adres as BedrijfAdres, b.BTWChecked, " +
						   "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, wb.WerknemerEmail, " +
						   "f.FunctieNaam, afs.AfspraakStatusNaam " +
						   "FROM Afspraak a " +
						   "JOIN Werknemerbedrijf as wb ON(a.WerknemerBedrijfId = wb.Id) " +
						   "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
						   "JOIN Werknemer w ON(wb.WerknemerId = w.Id) " +
						   "JOIN Bedrijf b ON(wb.BedrijfId = b.Id) " +
						   "JOIN Functie f ON(wb.FunctieId = f.Id) " +
						   "JOIN AfspraakStatus afs ON (afs.Id = a.AfspraakStatusId) " +
						   "WHERE 1=1";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (_bedrijfId.HasValue) {
						query += " AND b.id = @bedrijfId";
						cmd.Parameters.Add(new MySqlParameter("@bedrijfId", MySqlDbType.Int64));
						cmd.Parameters["@bedrijfId"].Value = _bedrijfId.Value;
					}
					if (_werknemerId.HasValue) {
						query += " AND w.id = @werknemerId";
						cmd.Parameters.Add(new MySqlParameter("@werknemerId", MySqlDbType.Int64));
						cmd.Parameters["@werknemerId"].Value = _werknemerId.Value;
					}
					if (_bezoekerId.HasValue) {
						query += " AND bz.id = @bezoekerId";
						cmd.Parameters.Add(new MySqlParameter("@bezoekerId", MySqlDbType.Int64));
						cmd.Parameters["@bezoekerId"].Value = _bezoekerId.Value;
					}
					if (!String.IsNullOrWhiteSpace(_bezoekerVNaam)) {
						query += " AND bz.VNaam LIKE @VNaam";
						cmd.Parameters.Add(new MySqlParameter("@VNaam", MySqlDbType.VarChar));
						cmd.Parameters["@VNaam"].Value = $"%{_bezoekerVNaam}%";
					}
					if (!String.IsNullOrWhiteSpace(_bezoekerANaam)) {
						query += " AND bz.ANaam LIKE @ANaam";
						cmd.Parameters.Add(new MySqlParameter("@ANaam", MySqlDbType.VarChar));
						cmd.Parameters["@ANaam"].Value = $"%{_bezoekerANaam}%";
					}
					if (!String.IsNullOrWhiteSpace(_bezoekerMail)) {
						query += " AND bz.Email LIKE @Email";
						cmd.Parameters.Add(new MySqlParameter("@Email", MySqlDbType.VarChar));
						cmd.Parameters["@Email"].Value = $"%{_bezoekerMail}%";
					}
					if (_datum.HasValue) {
						query += " AND CONVERT(a.StartTijd, DATE) = @date";
						cmd.Parameters.Add(new MySqlParameter("@date", MySqlDbType.Date));
						cmd.Parameters["@date"].Value = _datum.Value.Date;
					}
					query += " ORDER BY a.StartTijd DESC, b.id, w.id, f.FunctieNaam";
					cmd.CommandText = query;
					IDataReader reader = cmd.ExecuteReader();

					Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					string functieNaam = "";
					string werknemerMail = "";

					while (reader.Read()) {
						//Afspraak portie
						long afspraakId = (long)reader["AfspraakId"];
						DateTime start = (DateTime)reader["StartTijd"];
						DateTime? eind = !reader.IsDBNull(reader.GetOrdinal("EindTijd")) ? (DateTime)reader["EindTijd"] : null;
						//bezoeker portie
						long bezoekerId = (long)reader["BezoekerId"];
						string bezoekerAnaam = (string)reader["BezoekerANaam"];
						string bezoekerVnaam = (string)reader["BezoekerVNaam"];
						string bezoekerMail = (string)reader["BezoekerMail"];
						string bezoekerBedrijf = (string)reader["BezoekerBedrijf"];
						//bedrijf portie
						if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"]) {
							long bedrijfId = (long)reader["BedrijfId"];
							string bedrijfNaam = (string)reader["BedrijfNaam"];
							string bedrijfBTWNr = (string)reader["BTWNr"];
							string bedrijfTeleNr = (string)reader["TeleNr"];
							string bedrijfMail = (string)reader["BedrijfEmail"];
							string bedrijfAdres = (string)reader["BedrijfAdres"];
							bool bedrijfBTWChecked = (ulong)reader["BTWChecked"] == 0 ? false : true;
							bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTWNr, bedrijfBTWChecked, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
						}
						//werknemer portie
						if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
							long werknemerId = (long)reader["WerknemerId"];
							string werknemerANaam = (string)reader["WerknemerANaam"];
							string werknemerVNaam = (string)reader["WerknemerVNaam"];
							werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerANaam);
						}
						//functie portie
						if (String.IsNullOrWhiteSpace(functieNaam) || !werknemer.GeefBedrijvenEnFunctiesPerWerknemer().ContainsKey(bedrijf) || !werknemer.GeefBedrijvenEnFunctiesPerWerknemer()[bedrijf].GeefWerknemerFuncties().Contains(Nutsvoorziening.NaamOpmaak((string)reader["FunctieNaam"]))) {
							functieNaam = (string)reader["FunctieNaam"];
							werknemerMail = (string)reader["WerknemerEmail"];
							werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
						}
						string AfspraakStatus = (string)reader["AfspraakStatusNaam"];
						afspraken.Add(new Afspraak(afspraakId, start, eind, bedrijf, new Bezoeker(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer, AfspraakStatus));
					}
					return afspraken.AsReadOnly();
				}
			} catch (Exception ex) {
				AfspraakMySQLException exx = new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfId", _bedrijfId);
				exx.Data.Add("werknemerId", _werknemerId);
				exx.Data.Add("bezoekerId", _bezoekerId);
				exx.Data.Add("bezoekerVNaam", _bezoekerVNaam);
				exx.Data.Add("bezoekerANaam", _bezoekerANaam);
				exx.Data.Add("bezoekermail", _bezoekerMail);
				exx.Data.Add("bezoekermail2", afspraken);
				exx.Data.Add("datum", _datum);
				throw exx;
			} finally {
				con.Close();
			}
		}
		#endregion

		#region Bezoeker
		/// <summary>
		/// Stelt lijst van alle aanwezige bezoekers samen met enkel lees rechten.
		/// </summary>
		/// <returns>IReadOnlyList van bezoeker objecten.</returns>
		/// <exception cref="AfspraakMySQLException">Faalt lijst van bezoeker objecten samen te stellen.</exception>
		/// <remarks>Geeft alle bezoekers terug waar statuscode afspraak gelijk is aan 1 = 'In gang'.</remarks>
		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			MySqlConnection con = GetConnection();
			string query = "SELECT b.Id, b.VNaam, b.ANaam, b.Email, b.EigenBedrijf " +
						   "FROM Afspraak a " +
						   "JOIN Bezoeker b ON(a.BezoekerId = b.Id) " +
						   "WHERE a.AfspraakStatusId = 1 AND a.EindTijd IS NULL " +
						   "ORDER BY b.Vnaam, b.ANaam";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					IDataReader reader = cmd.ExecuteReader();
					List<Bezoeker> bezoekers = new List<Bezoeker>();
					while (reader.Read()) {
						long bezoekerId = (long)reader["Id"];
						string bezoekerVNaam = (string)reader["VNaam"];
						string bezoekerANaam = (string)reader["ANaam"];
						string bezoekerMail = (string)reader["Email"];
						string bezoekerBedrijf = (string)reader["EigenBedrijf"];
						bezoekers.Add(new Bezoeker(bezoekerId, bezoekerVNaam, bezoekerANaam, bezoekerMail, bezoekerBedrijf));
					}
					return bezoekers.AsReadOnly();
				}
			} catch (Exception ex) {
				throw new AfspraakMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
			} finally {
				con.Close();
			}
		}
		#endregion
	}
}