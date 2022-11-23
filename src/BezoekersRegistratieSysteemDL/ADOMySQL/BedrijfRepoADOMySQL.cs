using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using MySql.Data.MySqlClient;
using System.Data;

namespace BezoekersRegistratieSysteemDL.ADOMySQL {

	public class BedrijfRepoADOMySQL : IBedrijfRepository {

		/// <summary>
		/// Private lokale variabele connectiestring
		/// </summary>
		private string _connectieString;

		/// <summary>
		/// BedrijfRepoADO constructor krijgt connectie string als parameter.
		/// </summary>
		/// <param name="connectieString">Connectie string database</param>
		/// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
		public BedrijfRepoADOMySQL(string connectieString) {
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
		/// Gaat na of bedrijf bestaat adhv een bedrijf object.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="BedrijfADOException">Faalt om bestaan bedrijf te verifiëren op basis van het bedrijf object.</exception>
		public bool BestaatBedrijf(Bedrijf bedrijf) {
			try {
				return BestaatBedrijf(bedrijf, null, null);
			} catch (Exception ex) {
				throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Gaat na of bedrijf bestaat adhv een bedrijf id.
		/// </summary>
		/// <param name="bedrijfId">Id van het bedrijf dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="BedrijfADOException">Faalt om bestaan bedrijf te verifiëren op basis van het bedrijf id.</exception>
		public bool BestaatBedrijf(long bedrijfId) {
			try {
				return BestaatBedrijf(null, bedrijfId, null);
			} catch (Exception ex) {
				throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Gaat na of bedrijf bestaat adhv een bedrijfsnaam.
		/// </summary>
		/// <param name="bedrijfsnaam">Naam van het bedrijf dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="BedrijfADOException">Faalt om bestaan bedrijf te verifiëren op basis van het bedrijf id.</exception>
		public bool BestaatBedrijf(string bedrijfsnaam) {
			try {
				return BestaatBedrijf(null, null, bedrijfsnaam);
			} catch (Exception ex) {
				throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Private methode gaat na of bedrijf bestaat adhv een bedrijf object of parameters bedrijf id/naam.
		/// </summary>
		/// <param name="bedrijf">Optioneel: Bedrijf object dat gecontroleerd wenst te worden.</param>
		/// <param name="bedrijfId">Optioneel: Id van het bedrijf dat gecontroleerd wenst te worden.</param>
		/// <param name="bedrijfsnaam">Optioneel: Naam van het bedrijf dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="BedrijfADOException">Faalt om bestaan bedrijf te verifiëren op basis van de bedrijfid/naam of bedrijf object.</exception>
		private bool BestaatBedrijf(Bedrijf? bedrijf, long? bedrijfId, string? bedrijfsnaam) {
			MySqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM bedrijf " +
						   "WHERE 1=1";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					var sqltype = (bedrijf is not null && bedrijf.Id != 0) ? SqlDbType.BigInt : (bedrijfId.HasValue) ? SqlDbType.BigInt : SqlDbType.VarChar;
					cmd.Parameters.Add(new MySqlParameter("@querylookup", sqltype));
					string databaseWhere = (bedrijf is not null) ? (bedrijf.Id != 0) ? "id" : "BTWNr" : (bedrijfId.HasValue) ? "id" : "Naam";
					query += $" AND {databaseWhere} = @querylookup";
					cmd.Parameters["@querylookup"].Value = (bedrijf is not null) ? (bedrijf.Id != 0) ? bedrijf.Id : bedrijf.BTW : (bedrijfId.HasValue) ? bedrijfId : bedrijfsnaam;
					cmd.CommandText = query;
					int i = (int)cmd.ExecuteScalar();
					return (i > 0);
				}
			} catch (Exception ex) {
				BedrijfADOException exx = new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijf", bedrijf);
				exx.Data.Add("bedrijfId", bedrijfId);
				exx.Data.Add("bedrijfsnaam", bedrijfsnaam);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Bewerkt gegevens van een bedrijf adhv bedrijf object.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object dat gewijzigd wenst te worden in de databank.</param>
		/// <exception cref="BedrijfADOException">Faalt bedrijf te wijzigen.</exception>
		public void BewerkBedrijf(Bedrijf bedrijf) {
			MySqlConnection con = GetConnection();
			string query = "UPDATE bedrijf " +
						   "SET Naam = @naam, " +
						   "BTWNr = @btwnr, " +
						   "TeleNr = @telenr, " +
						   "Email = @email, " +
						   "Adres = @adres, " +
						   "BTWChecked = @btwcheck " +
						   "WHERE id = @id";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@naam", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@btwnr", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@telenr", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@email", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@adres", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@btwcheck", SqlDbType.Bit));
					cmd.Parameters.Add(new MySqlParameter("@id", SqlDbType.BigInt));
					cmd.Parameters["@naam"].Value = bedrijf.Naam;
					cmd.Parameters["@btwnr"].Value = bedrijf.BTW;
					cmd.Parameters["@telenr"].Value = bedrijf.TelefoonNummer;
					cmd.Parameters["@email"].Value = bedrijf.Email;
					cmd.Parameters["@adres"].Value = bedrijf.Adres;
					cmd.Parameters["@btwcheck"].Value = bedrijf.BtwGeverifieerd;
					cmd.Parameters["@id"].Value = bedrijf.Id;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				BedrijfADOException exx = new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijf", bedrijf);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Haalt bedrijf op adhv parameter bedrijf id.
		/// </summary>
		/// <param name="id">Id van het gewenste bedrijf.</param>
		/// <returns>Gewenst bedrijf object</returns>
		/// <exception cref="BedrijfADOException">Faalt om bedrijf object op te halen op basis van het id.</exception>
		public Bedrijf GeefBedrijf(long id) {
			try {
				return GeefBedrijf(id, null);
			} catch (Exception ex) {
				throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Haalt bedrijf op adhv parameter bedrijfsnaam.
		/// </summary>
		/// <param name="bedrijfsnaam">Naam van het gewenste bedrijf.</param>
		/// <returns>Gewenst bedrijf object</returns>
		/// <exception cref="BedrijfADOException">Faalt om bedrijf object op te halen op basis van de bedrijfsnaam.</exception>
		public Bedrijf GeefBedrijf(string bedrijfsnaam) {
			try {
				return GeefBedrijf(null, bedrijfsnaam);
			} catch (Exception ex) {
				throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Haalt bedrijf op adhv parameter bedrijf id.
		/// </summary>
		/// <param name="_bedrijfId">Id van het gewenste bedrijf.</param>
		/// <param name="_bedrijfnaam">Naam van het gewenste bedrijf.</param>
		/// <returns>Gewenst bedrijf object</returns>
		/// <exception cref="BedrijfADOException">Faalt om bedrijf object op te halen op basis van het id of naam.</exception>
		private Bedrijf GeefBedrijf(long? _bedrijfId, string? _bedrijfnaam) {
			MySqlConnection con = GetConnection();
			string query = "SELECT b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, b.BTWChecked, " +
						   "wn.Id as WerknemerId, wn.ANaam as WerknemerAnaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEMail, " +
                           "f.FunctieNaam " +
						   "FROM Bedrijf b " +
						   "LEFT JOIN WerknemerBedrijf wb ON(b.id = wb.BedrijfId) AND wb.Status = 1 " +
						   "LEFT JOIN Werknemer wn ON(wn.id = wb.WerknemerId) " +
						   "LEFT JOIN Functie f ON(wb.FunctieId = f.Id) " +
						   "WHERE 1=1";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (_bedrijfId.HasValue) {
						query += " AND b.Id = @id";
						cmd.Parameters.Add(new MySqlParameter("@id", SqlDbType.BigInt));
						cmd.Parameters["@id"].Value = _bedrijfId;
					} else {
						query += " AND b.Naam = @Naam";
						cmd.Parameters.Add(new MySqlParameter("@Naam", SqlDbType.VarChar));
						cmd.Parameters["@Naam"].Value = _bedrijfnaam;
					}
					query += " ORDER BY wn.Id, f.Id";
					cmd.CommandText = query;
					IDataReader reader = cmd.ExecuteReader();
					Bedrijf bedrijf = null;
					Werknemer werknemer = null;
					while (reader.Read()) {
						if (bedrijf is null) {
							long bedrijfId = (long)reader["BedrijfId"];
							string bedrijfNaam = (string)reader["BedrijfNaam"];
							string bedrijfBTW = (string)reader["BedrijfBTW"];
							string bedrijfTeleNr = (string)reader["BedrijfTeleNr"];
							string bedrijfMail = (string)reader["BedrijfMail"];
							string bedrijfAdres = (string)reader["BedrijfAdres"];
							bool bedrijfBtwCheck = (bool)reader["BTWChecked"];
							bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfBtwCheck, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
						}
						if (!reader.IsDBNull(reader.GetOrdinal("WerknemerId"))) {
							if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
								long werknemerId = (long)reader["WerknemerId"];
								string werknemerVNaam = (string)reader["WerknemerVNaam"];
								string werknemerAnaam = (string)reader["WerknemerAnaam"];
								werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam);
							}
							string werknemerMail = (string)reader["WerknemerEMail"];
							string functieNaam = (string)reader["FunctieNaam"];
							bedrijf.VoegWerknemerToeInBedrijf(werknemer, werknemerMail, functieNaam);
                            //Halen momenteel enkel werknemers op die werkzaam zijn
                            //Voeg dit aan qeury toe 'wb.Status as WerknemerBedrijfStatus'
                            //string werknemerbedrijfStatus = (int)reader["WerknemerBedrijfStatus"] == 1 ? "Werkzaam" : "Niet werkzaam";
                            werknemer.ZetStatusNaamPerBedrijf(bedrijf, "Werkzaam");
						}
					}
					return bedrijf;
				}
			} catch (Exception ex) {
				BedrijfADOException exx = new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfid", _bedrijfId);
				exx.Data.Add("bedrijfnaam", _bedrijfnaam);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Private methode die alle bedrijven ophaalt uit de databank.
		/// </summary>
		/// <returns>IReadOnlyList van bedrijf objecten.</returns>
		/// <exception cref="BedrijfADOException">Faalt lijst van bedrijf objecten samen te stellen.</exception>
		public IReadOnlyList<Bedrijf> GeefBedrijven() {
			MySqlConnection con = GetConnection();
			string query = "SELECT b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, b.BTWChecked, " +
						   "wn.Id as WerknemerId, wn.ANaam as WerknemerAnaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEMail, " +
						   "f.FunctieNaam " +
						   "FROM Bedrijf b " +
						   "LEFT JOIN Werknemerbedrijf wb ON(b.id = wb.BedrijfId) AND wb.Status = 1 " +
						   "LEFT JOIN Werknemer wn ON(wn.id = wb.WerknemerId) " +
						   "LEFT JOIN Functie f ON(wb.FunctieId = f.Id) " +
						   "WHERE b.Status = 1 " +
						   "ORDER BY b.Naam, wn.id";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					IDataReader reader = cmd.ExecuteReader();
					List<Bedrijf> bedrijven = new List<Bedrijf>();
					Bedrijf bedrijf = null;
					Werknemer werknemer = null;
					while (reader.Read()) {
						if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"]) {
							long bedrijfId = (long)reader["BedrijfId"];
							string bedrijfNaam = (string)reader["BedrijfNaam"];
							string bedrijfBTW = (string)reader["BedrijfBTW"];
							string bedrijfTeleNr = (string)reader["BedrijfTeleNr"];
							string bedrijfMail = (string)reader["BedrijfMail"];
							string bedrijfAdres = (string)reader["BedrijfAdres"];
							bool bedrijfBtwCheck = (bool)reader["BTWChecked"];
							bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfBtwCheck, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
							bedrijven.Add(bedrijf);
						}
						if (!reader.IsDBNull(reader.GetOrdinal("WerknemerId"))) {
							if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
								long werknemerId = (long)reader["WerknemerId"];
								string werknemerVNaam = (string)reader["WerknemerVNaam"];
								string werknemerAnaam = (string)reader["WerknemerAnaam"];

								werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam);
							}
							string werknemerMail = (string)reader["WerknemerEMail"];
							string functieNaam = (string)reader["FunctieNaam"];

							bedrijf.VoegWerknemerToeInBedrijf(werknemer, werknemerMail, functieNaam);
                            //Halen momenteel enkel werknemers op die werkzaam zijn
                            //Voeg dit aan qeury toe 'wb.Status as WerknemerBedrijfStatus'
                            //string werknemerbedrijfStatus = (int)reader["WerknemerBedrijfStatus"] == 1 ? "Werkzaam" : "Niet werkzaam";
							werknemer.ZetStatusNaamPerBedrijf(bedrijf, "Werkzaam");
						}
					}
					return bedrijven;
				}
			} catch (Exception ex) {
				throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Verwijdert gewenste bedrijf.
		/// </summary>
		/// <param name="bedrijfId">Id van bedrijf dat verwijderd wenst te worden.</param>
		/// <exception cref="BedrijfADOException">Faalt bedrijf te verwijderen.</exception>
		/// <remarks>bedrijf krijgt statuscode 2 = 'Verwijderd'.</remarks>
		public void VerwijderBedrijf(long bedrijfId) {
			try {
				VeranderStatusBedrijf(bedrijfId, 2);
			} catch (Exception ex) {
				throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Private methode wijzigd statuscode bedrijf.
		/// </summary>
		/// <param name="bedrijfId">Id van het bedrijf dat gewijzigd wenst te worden.</param>
		/// <param name="statusId">Id voor het wijzigen van een status.</param>
		/// <exception cref="BedrijfADOException">Faalt om status van een bedrijf te wijzigen.</exception>
		/// <remarks>Wanneer bedrijf status wijzigt 2 = 'verwijderd' => Werknemer status wijzigt 2 = 'Niet langer in dienst'</remarks>
		private void VeranderStatusBedrijf(long bedrijfId, int statusId) {
			//Wanneer bedrijf word verwijderd (status 2), medewerkers zijn dan ontslagen (status 2)
			MySqlConnection con = GetConnection();
			string queryBedrijf = "UPDATE bedrijf " +
								  "SET Status = @statusId " +
								  "WHERE Id = @bedrijfid";
			con.Open();
            MySqlTransaction trans = con.BeginTransaction();
			try {
				using (MySqlCommand cmdMedewerker = con.CreateCommand())
				using (MySqlCommand cmdBedrijf = con.CreateCommand()) {
					//Medewerker sectie
					if (statusId == 2) {
						string queryMedewerker = "UPDATE WerknemerBedrijf " +
												 "SET Status = @statusId " +
												 "WHERE BedrijfId = @bedrijfid";
						cmdMedewerker.Transaction = trans;
						cmdMedewerker.Parameters.Add(new MySqlParameter("@bedrijfid", SqlDbType.BigInt));
						cmdMedewerker.Parameters.Add(new MySqlParameter("@statusId", SqlDbType.Int));
						cmdMedewerker.Parameters["@bedrijfid"].Value = bedrijfId;
						cmdMedewerker.Parameters["@statusId"].Value = statusId;
						cmdMedewerker.CommandText = queryMedewerker;
						cmdMedewerker.ExecuteNonQuery();
					}
					//Bedrijf Sectie
					cmdBedrijf.CommandText = queryBedrijf;
					cmdBedrijf.Transaction = trans;
					cmdBedrijf.Parameters.Add(new MySqlParameter("@bedrijfid", SqlDbType.BigInt));
					cmdBedrijf.Parameters.Add(new MySqlParameter("@statusId", SqlDbType.Int));
					cmdBedrijf.Parameters["@bedrijfid"].Value = bedrijfId;
					cmdBedrijf.Parameters["@statusId"].Value = statusId;
					cmdBedrijf.ExecuteNonQuery();
					trans.Commit();
				}
			} catch (Exception ex) {
				trans.Rollback();
				BedrijfADOException exx = new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfId", bedrijfId);
				exx.Data.Add("statusId", statusId);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Voegt bedrijf toe in de databank adhv een bedrijf object.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object dat toegevoegd wenst te worden.</param>
		/// <returns>Gewenste bedrijf object MET id</returns>
		/// <exception cref="BedrijfADOException">Faalt bedrijf toe te voegen op basis van het bedrijf object.</exception>
		public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
			MySqlConnection con = GetConnection();
			string query = "INSERT INTO Bedrijf(Naam, BTWNr, TeleNr, Email, Adres, BTWChecked) " +
						   "VALUES(@naam,@btwNr,@TeleNr,@Email,@Adres,@BTWChecked);" +
                           "SELECT id FROM Bedrijf WHERE id = LAST_INSERT_ID();";
			try {
				using (MySqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new MySqlParameter("@naam", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@btwNr", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@TeleNr", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@Email", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@Adres", SqlDbType.VarChar));
					cmd.Parameters.Add(new MySqlParameter("@BTWChecked", SqlDbType.Bit));
					cmd.Parameters["@naam"].Value = bedrijf.Naam;
					cmd.Parameters["@btwNr"].Value = bedrijf.BTW;
					cmd.Parameters["@TeleNr"].Value = bedrijf.TelefoonNummer;
					cmd.Parameters["@Email"].Value = bedrijf.Email;
					cmd.Parameters["@Adres"].Value = bedrijf.Adres;
					cmd.Parameters["@BTWChecked"].Value = bedrijf.BtwGeverifieerd;
					long i = (long)cmd.ExecuteScalar();
					bedrijf.ZetId(i);
					return bedrijf;
				}
			} catch (Exception ex) {
				BedrijfADOException exx = new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijf", bedrijf);
				throw exx;
			} finally {
				con.Close();
			}
		}
	}
}
