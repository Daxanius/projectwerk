using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using System.Data;
using System.Data.SqlClient;

namespace BezoekersRegistratieSysteemDL.ADOMySQL {

	public class WerknemerRepoADOMySQL : IWerknemerRepository {

		/// <summary>
		/// Private lokale variabele connectiestring
		/// </summary>
		private string _connectieString;

		/// <summary>
		/// WerknemerRepoADO constructor krijgt connectie string als parameter.
		/// </summary>
		/// <param name="connectieString">Connectie string database</param>
		/// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
		public WerknemerRepoADOMySQL(string connectieString) {
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
		/// Gaat na of werknemer bestaat adhv een werknemer object.
		/// </summary>
		/// <param name="werknemer">Werknemer object dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="WerknemerADOException">Faalt om bestaan werknemer te verifiëren op basis van het werknemer object.</exception>
		public bool BestaatWerknemer(Werknemer werknemer) {
			try {
				return BestaatWerknemer(werknemer, null);
			} catch (Exception ex) {
				throw new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Gaat na of werknemer bestaat adhv parameter werknemer id.
		/// </summary>
		/// <param name="id">Id van de werknemer die gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="WerknemerADOException">Faalt om bestaan werknemer te verifiëren op basis van het werknemer id.</exception>
		public bool BestaatWerknemer(long id) {
			try {
				return BestaatWerknemer(null, id);
			} catch (Exception ex) {
				throw new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

        /// <summary>
        /// Private methode gaat na of werknemer bestaat adhv een werknemer object of parameters werknemer id.
        /// </summary>
        /// <param name="werknemer">Optioneel: Werknemer object dat gecontroleerd wenst te worden.</param>
        /// <param name="werknemerId">Optioneel: Id van de werknemer die gecontroleerd wenst te worden.</param>
        /// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
        /// <exception cref="WerknemerADOException">Faalt om bestaan werknemer te verifiëren op basis van werknemer id of werknemer object.</exception>
        /// <exception cref="WerknemerADOException">Als het email pad neemt en naam wijkt af is er exception.</exception>
        private bool BestaatWerknemer(Werknemer? werknemer, long? werknemerId) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM Werknemer wn ";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (werknemer is not null) {
                        if (werknemer.Id != 0) {
                            query += "WHERE wn.id = @id";
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                            cmd.Parameters["@id"].Value = werknemer.Id;
                        } else {
                            query += "JOIN Werknemerbedrijf wb ON(wn.id = wb.werknemerId) " +
                                     "WHERE wb.werknemerEmail IN(";
                            int mailCount = 0;
                            foreach (var werknemerInfo in werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Values) {
                                query += $"@mail{mailCount},";
                                cmd.Parameters.Add(new SqlParameter($"@mail{mailCount}", SqlDbType.VarChar));
                                cmd.Parameters[$"@mail{mailCount}"].Value = werknemerInfo.Email;
                                mailCount++;
                            }
                            query = query.Substring(0, query.Length - 1);
                            query += ")";
                        }
                    }
                    if (werknemerId.HasValue) {
                        query += "WHERE wn.id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = werknemerId;

                    }
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();

                    //Kan uitgecomment worden als voor moest werknemer naam niet uitmaken wnr email bestaat
                    if (werknemer is not null && i > 0 && werknemer.Id == 0) {
                        using (SqlCommand cmdWerknemerNaam = con.CreateCommand()) {
                            string queryWerknemerNaam = "SELECT COUNT(*) " +
                                                        "FROM Werknemer wn " +
                                                        "JOIN Werknemerbedrijf wb ON(wn.id = wb.werknemerId)" +
                                                        "WHERE wn.ANaam = @Anaam AND wn.VNaam = @Vnaam AND wb.werknemerEmail IN(";
                            int mailCount = 0;
                            foreach (var werknemerInfo in werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Values) {
                                queryWerknemerNaam += $"@mail{mailCount},";
                                cmdWerknemerNaam.Parameters.Add(new SqlParameter($"@mail{mailCount}", SqlDbType.VarChar));
                                cmdWerknemerNaam.Parameters[$"@mail{mailCount}"].Value = werknemerInfo.Email;
                                mailCount++;
                            }
                            queryWerknemerNaam = queryWerknemerNaam.Substring(0, queryWerknemerNaam.Length - 1);
                            queryWerknemerNaam += ")";
                            cmdWerknemerNaam.CommandText = queryWerknemerNaam;
                            cmdWerknemerNaam.Parameters.Add(new SqlParameter("@Anaam", SqlDbType.VarChar));
                            cmdWerknemerNaam.Parameters.Add(new SqlParameter("@Vnaam", SqlDbType.VarChar));
                            cmdWerknemerNaam.Parameters["@Anaam"].Value = werknemer.Achternaam;
                            cmdWerknemerNaam.Parameters["@Vnaam"].Value = werknemer.Voornaam;
                            int j = (int)cmdWerknemerNaam.ExecuteScalar();
                            if (j == 0) {
                                throw new Exception("Werknemer mail is niet aan deze werknemer naam gelinked");
                            }
                        }
                    }
                    return (i > 0);
                    
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("werknemer", werknemer);
                exx.Data.Add("werknemerId", werknemerId);
                throw exx;
            } finally {
                con.Close();
            }
        }

		/// <summary>
		/// Haalt werknemer op adhv parameter werknemer id.
		/// </summary>
		/// <param name="_werknemerId">Id van de gewenste werknemer.</param>
		/// <returns>Gewenst werknemer object</returns>
		/// <exception cref="WerknemerADOException">Faalt om werknemer object op te halen op basis van het id.</exception>
		public Werknemer GeefWerknemer(long _werknemerId) {
			SqlConnection con = GetConnection();
			string query = "SELECT wn.id as WerknemerId, wn.Vnaam as WerknemerVnaam, wn.Anaam as WerknemerAnaam, wb.WerknemerEmail, " +
						   "b.id as BedrijfId, b.naam as BedrijfNaam, b.btwnr as bedrijfBTW, b.telenr as bedrijfTele, b.email as BedrijfMail, b.adres as BedrijfAdres, b.BTWChecked, " +
						   "f.functienaam, (SELECT COUNT(*) " +
										   "FROM Afspraak a " +
										   "JOIN Werknemerbedrijf wbb ON wbb.Id = a.WerknemerBedrijfId " +
										   "WHERE wbb.WerknemerId = wn.Id " +
										   "AND a.AfspraakStatusId = 1) AS HuidigeAfsprakenAantal " +
						   "FROM Werknemer wn " +
						   "LEFT JOIN Werknemerbedrijf wb ON(wn.id = wb.werknemerid) AND wb.Status = 1 " +
						   "LEFT JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
						   "LEFT JOIN functie f ON(f.id = wb.functieid) " +
                           "WHERE wn.id = @werknemerId";
            try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
					cmd.Parameters["@werknemerId"].Value = _werknemerId;
					IDataReader reader = cmd.ExecuteReader();
					Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					while (reader.Read()) {
						if (werknemer is null) {
							string werknemerVnaam = (string)reader["WerknemerVnaam"];
							string werknemerAnaam = (string)reader["WerknemerAnaam"];							
							werknemer = new Werknemer(_werknemerId, werknemerVnaam, werknemerAnaam);
						}
						//TODO: Deze ordinal check mag waars weg
						if (!reader.IsDBNull(reader.GetOrdinal("WerknemerEmail"))) {
							if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"]) {
								long bedrijfId = (long)reader["BedrijfId"];
								string bedrijfNaam = (string)reader["BedrijfNaam"];
								string bedrijfBTW = (string)reader["bedrijfBTW"];
								string bedrijfTele = (string)reader["bedrijfTele"];
								string bedrijfMail = (string)reader["BedrijfMail"];
								string bedrijfAdres = (string)reader["BedrijfAdres"];
								bool bedrijfBTWChecked = (bool)reader["BTWChecked"];
								bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfBTWChecked, bedrijfTele, bedrijfMail, bedrijfAdres);
							}
							string werknemerMail = (string)reader["WerknemerEmail"];
							string functieNaam = (string)reader["functienaam"];
							werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
                            string statusNaam = (int)reader["HuidigeAfsprakenAantal"] == 0 ? "Vrij" : "Bezet";
                            werknemer.ZetStatusNaamPerBedrijf(bedrijf, statusNaam);
						}
					}
					return werknemer;
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("werknemerId", _werknemerId);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Stelt lijst van werknemers samen met enkel lees rechten adhv parameter bedrijf id.
		/// </summary>
		/// <param name="_bedrijfId">Id van het gewenste bedrijf.</param>
		/// <returns>IReadOnlyList van werknemer objecten waar Werknemer Bedrijf id = bedrijf id.</returns>
		/// <exception cref="WerknemerADOException">Faalt lijst van werknemer objecten samen te stellen op basis van het bedrijf id.</exception>
		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(long _bedrijfId) {
			try {
				return GeefWerknemers(_bedrijfId, null, null, null);
			} catch (Exception ex) {
				throw new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van werknemers samen met enkel lees rechten adhv parameters werknemer voornaam/achternaam en bedrijf id.
		/// </summary>
		/// <param name="voornaam">Voornaam van de gewenste werknemer.</param>
		/// <param name="achternaam">Achternaam van de gewenste werknemer.</param>
		/// <param name="bedrijfId">Bedrijf van de gewenste werknemer</param>
		/// <returns>IReadOnlyList van werknemer objecten op werknemernaam PER bedrijf.</returns>
		/// <exception cref="WerknemerADOException">Faalt lijst van werknemer objecten samen te stellen op basis van Werknemer voornaam/achternaam en bedrijf id.</exception>
		public IReadOnlyList<Werknemer> GeefWerknemersOpNaamPerBedrijf(string voornaam, string achternaam, long bedrijfId) {
			try {
				return GeefWerknemers(bedrijfId, voornaam, achternaam, null);
			} catch (Exception ex) {
				throw new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Stelt lijst van werknemers samen met enkel lees rechten adhv parameters werknemer functie en bedrijf id.
		/// </summary>
		/// <param name="functie">Functie van de gewenste werknemer</param>
		/// <param name="bedrijfId">Bedrijf van de gewenste werknemer</param>
		/// <returns>IReadOnlyList van werknemer objecten op werknemerfunctie PER bedrijf.</returns>
		/// <exception cref="WerknemerADOException">Faalt lijst van werknemer objecten samen te stellen op basis van Werknemer functie en bedrijf id.</exception>
		public IReadOnlyList<Werknemer> GeefWerknemersOpFunctiePerBedrijf(string functie, long bedrijfId) {
			try {
				return GeefWerknemers(bedrijfId, null, null, functie);
			} catch (Exception ex) {
				throw new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// private methode stelt lijst van werknemers samen met enkel lees rechten adhv parameters werknemer functie/voornaam/achternaam en bedrijf id.
		/// </summary>
		/// <param name="_bedrijfId">Id van het gewenste bedrijf.</param>
		/// <param name="_voornaam">Voornaam van de gewenste werknemer.</param>
		/// <param name="_achternaam">Achternaam van de gewenste werknemer.</param>
		/// <param name="_functie">Functie van de gewenste werknemer</param>
		/// <returns>IReadOnlyList van werknemer objecten op werknemerfunctie/naam PER bedrijf.</returns>
		/// <exception cref="WerknemerADOException">Faalt lijst van werknemer objecten samen te stellen op basis van Werknemer functie/voornaam/achternaam en bedrijf id.</exception>
		private IReadOnlyList<Werknemer> GeefWerknemers(long? _bedrijfId, string? _voornaam, string? _achternaam, string? _functie) {
			SqlConnection con = GetConnection();
			string query = "SELECT wn.id as WerknemerId, wn.ANaam as WerknemerANaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEmail, " +
						   "b.id as BedrijfId, b.Naam as BedrijfNaam, b.btwnr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, b.BTWChecked, " +
						   "f.Functienaam, (SELECT COUNT(*) " +
										   "FROM Afspraak a " +
										   "JOIN Werknemerbedrijf wbb ON wbb.Id = a.WerknemerBedrijfId " +
										   "WHERE wbb.WerknemerId = wn.Id " +
										   "AND a.AfspraakStatusId = 1) AS HuidigeAfsprakenAantal " +
						   "FROM Werknemer wn " +
						   "LEFT JOIN Werknemerbedrijf wb ON(wb.werknemerId = wn.id) AND wb.Status = 1 " +
						   "LEFT JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
						   "LEFT JOIN Functie f ON(f.id = wb.FunctieId) " +
						   "WHERE 1=1";
            try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (_bedrijfId.HasValue) {
						query += " AND b.id = @bedrijfId";
						cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
						cmd.Parameters["@bedrijfId"].Value = _bedrijfId;
					}
					if (!String.IsNullOrWhiteSpace(_voornaam)) {
						query += " AND wn.VNaam LIKE @VNaam";
						cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
						cmd.Parameters["@VNaam"].Value = $"%{_voornaam}%";
					}
					if (!String.IsNullOrWhiteSpace(_achternaam)) {
						query += " AND wn.ANaam LIKE @ANaam";
						cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
						cmd.Parameters["@ANaam"].Value = $"%{_achternaam}%";
					}
					if (!String.IsNullOrWhiteSpace(_functie)) {
						query += " AND f.FunctieNaam = @functie";
						cmd.Parameters.Add(new SqlParameter("@functie", SqlDbType.VarChar));
						cmd.Parameters["@functie"].Value = _functie;
					}
					query += " ORDER BY wn.VNaam, wn.ANaam, b.id, wn.id";
					cmd.CommandText = query;
					List<Werknemer> werknemers = new List<Werknemer>();
					Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					IDataReader reader = cmd.ExecuteReader();
					while (reader.Read()) {
						if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
							long werknemerId = (long)reader["WerknemerId"];
							string werknemerVNaam = (string)reader["WerknemerVNaam"];
							string werknemerAnaam = (string)reader["WerknemerAnaam"];
                            werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam);
							werknemers.Add(werknemer);
						}
						if (!reader.IsDBNull(reader.GetOrdinal("WerknemerEmail"))) {
							if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"]) {
								long bedrijfId = (long)reader["BedrijfId"];
								string bedrijfNaam = (string)reader["BedrijfNaam"];
								string bedrijfBTW = (string)reader["bedrijfBTW"];
								string bedrijfTele = (string)reader["BedrijfTeleNr"];
								string bedrijfMail = (string)reader["BedrijfMail"];
								string bedrijfAdres = (string)reader["BedrijfAdres"];
								bool bedrijfBTWChecked = (bool)reader["BTWChecked"];
								bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfBTWChecked, bedrijfTele, bedrijfMail, bedrijfAdres);
							}
							string werknemerMail = (string)reader["WerknemerEmail"];
							string functieNaam = (string)reader["functienaam"];
							werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
                            string statusNaam = (int)reader["HuidigeAfsprakenAantal"] == 0 ? "Vrij" : "Bezet";
							werknemer.ZetStatusNaamPerBedrijf(bedrijf, statusNaam);
						}
					}
					return werknemers;
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfId", _bedrijfId);
				exx.Data.Add("voornaam", _voornaam);
				exx.Data.Add("achternaam", _achternaam);
				exx.Data.Add("functie", _functie);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Verwijdert gewenste werknemer uit specifiek bedrijf.
		/// </summary>
		/// <param name="werknemer">Werknemer object dat verwijderd wenst te worden.</param>
		/// <param name="bedrijf">Bedrijf object waaruit werknemer verwijderd wenst te worden.</param>
		/// <exception cref="WerknemerADOException">Faalt werknemer te verwijderen uit specifiek.</exception>
		/// <remarks>Werknemer krijgt statuscode 2 = 'Niet langer in dienst' => Bedrijf specifiek.</remarks>
		public void VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf) {
			try {
				VerwijderWerknemer(werknemer, bedrijf, null);
			} catch (Exception ex) {
				throw new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Verwijdert gewenste functie van werknemer uit specifiek bedrijf.
		/// </summary>
		/// <param name="werknemer">Werknemer object waar men de functie van wenst te verwijderen.</param>
		/// <param name="bedrijf">Bedrijf object waar werknemer werkzaam is onder functie.</param>
		/// <param name="functie">Functie die verwijderd wenst te worden.</param>
		/// <exception cref="WerknemerADOException">Faalt werknemer functie te verwijderen voor specifiek bedrijf.</exception>
		/// <remarks>Ontneemt functie van werknemer voor specifiek bedrijf.</remarks>
		public void VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie) {
			try {
				VerwijderWerknemer(werknemer, bedrijf, functie);
			} catch (Exception ex) {
				throw new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Private methode verwijderd gewenste functie van werknemer uit specifiek bedrijf of verwijder werknemer uit specifiek bedrijf.
		/// </summary>
		/// <param name="werknemer">Werknemer object dan men wenst te verwijderen of waar men de functie van wenst te verwijderen.</param>
		/// <param name="bedrijf">Bedrijf object waar werknemer werkzaam is onder functie(s).</param>
		/// <param name="functie">Optioneel: Functie die verwijderd wenst te worden.</param>
		/// <exception cref="WerknemerADOException">Faalt werknemer of werknemer functie te verwijderen voor specifiek bedrijf.</exception>
		/// <remarks>Werknemer krijgt statuscode 2 = 'Niet langer in dienst' of Ontneemt functie van werknemer voor specifiek bedrijf.</remarks>
		private void VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf, string? functie) {
			SqlConnection con = GetConnection();
			string query = "UPDATE Werknemerbedrijf " +
						   "SET Status = 2 " +
						   "WHERE BedrijfId = @bedrijfId " +
						   "AND WerknemerId = @werknemerId " +
						   "AND Status = 1";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					if (!String.IsNullOrWhiteSpace(functie)) {
						query += " AND FunctieId = (SELECT Id FROM FUNCTIE WHERE FunctieNaam = @FunctieNaam)";
						cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));
						cmd.Parameters["@FunctieNaam"].Value = functie;
					}
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
					cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
					cmd.Parameters["@bedrijfId"].Value = bedrijf.Id;
					cmd.Parameters["@werknemerId"].Value = werknemer.Id;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				exx.Data.Add("bedrijf", bedrijf);
				exx.Data.Add("functie", functie);
				throw exx;
			} finally {
				con.Close();
			}
		}


		/// <summary>
		/// Voegt gewenste functie toe aan werknemer uit specifiek bedrijf.
		/// </summary>
		/// <param name="werknemer">Werknemer object die functie toegewezen wenst te worden.</param>
		/// <param name="werknemerInfo">WerknemerInfo object object waar werknemer werkzaam.</param>
		/// <param name="functie">Functie die toegevoegd wenst te worden.</param>
		/// <exception cref="WerknemerADOException">Faalt werknemer functie toe te kennen voor specifiek bedrijf.</exception>
		/// <remarks>Voegt een entry toe aan de werknemer bedrijf tabel in de databank.</remarks>
		public void VoegWerknemerFunctieToe(Werknemer werknemer, WerknemerInfo werknemerInfo, string functie) {
			SqlConnection con = GetConnection();
			string queryInsert = "INSERT INTO WerknemerBedrijf (BedrijfId, WerknemerId, WerknemerEmail, FunctieId) " +
								 "VALUES(@bedrijfId,@werknemerId, @email,(SELECT Id FROM Functie WHERE FunctieNaam = @FunctieNaam))";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = queryInsert;
					cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
					cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
					cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
					cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));
					cmd.Parameters["@werknemerId"].Value = werknemer.Id;
					cmd.Parameters["@bedrijfId"].Value = werknemerInfo.Bedrijf.Id;
					cmd.Parameters["@email"].Value = werknemerInfo.Email;
					cmd.Parameters["@FunctieNaam"].Value = functie;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				exx.Data.Add("werknemerinfo", werknemerInfo);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Voegt werknemer toe.
		/// </summary>
		/// <param name="werknemer">Werknemer object dat toegevoegd wenst te worden.</param>
		/// <exception cref="WerknemerADOException">Faalt werknemer toe te voegen.</exception>
		/// <remarks>Voegt een entry toe aan de werknemer tabel in de databank.</remarks>
		public Werknemer VoegWerknemerToe(Werknemer werknemer) {
			SqlConnection con = GetConnection();
			string query = "INSERT INTO Werknemer (VNaam, ANaam) OUTPUT INSERTED.Id VALUES (@VNaam, @ANaam)";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
					cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
					cmd.Parameters["@VNaam"].Value = werknemer.Voornaam;
					cmd.Parameters["@ANaam"].Value = werknemer.Achternaam;
					long i = (long)cmd.ExecuteScalar();
					werknemer.ZetId(i);
					////Dit voegt de bedrijven/functie toe aan uw werknemer in de DB
					//VoegFunctieToeAanWerknemer(werknemer);
					return werknemer;
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				throw exx;
			} finally {
				con.Close();
			}
		}

		///// <summary>
		///// voegt functie toe aan werknemer voor een specifiek bedrijf.
		///// </summary>
		///// <param name="werknemer">Werknemer object die functie toegewezen wenst te worden voor een bedrijf.</param>
		///// <exception cref="WerknemerADOException">Faalt werknemer functie toe te kennen voor specifiek bedrijf.</exception>
		//private void VoegFunctieToeAanWerknemer(Werknemer werknemer) {

		//    bool bestaatJob = false;

		//    SqlConnection con = GetConnection();
		//    string queryInsert = "INSERT INTO WerknemerBedrijf (BedrijfId, WerknemerId, WerknemerEmail, FunctieId) " +
		//                         "VALUES(@bedrijfId,@werknemerId, @email,(SELECT Id FROM Functie WHERE FunctieNaam = @FunctieNaam))";
		//    try {
		//        using (SqlCommand cmdCheck = con.CreateCommand())
		//        using (SqlCommand cmd = con.CreateCommand()) {
		//            con.Open();
		//            foreach (var kvpBedrijf in werknemer.GeefBedrijvenEnFunctiesPerWerknemer()) {
		//                foreach (var functieNaam in kvpBedrijf.Value.GeefWerknemerFuncties()) {
		//                    string queryDoesJobExist = "SELECT COUNT(*) " +
		//                                               "FROM WerknemerBedrijf " +
		//                                               "WHERE WerknemerId = @werknemerId " +
		//                                               "AND bedrijfId = @bedrijfId " +
		//                                               "AND FunctieId = (SELECT Id FROM Functie WHERE FunctieNaam = @functieNaam) " +
		//                                               "AND Status = 1";

		//                    cmdCheck.CommandText = queryDoesJobExist;

		//                    if (!bestaatJob) {
		//                        cmdCheck.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
		//                        cmdCheck.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
		//                        cmdCheck.Parameters.Add(new SqlParameter("@functieNaam", SqlDbType.VarChar));
		//                    }

		//                    cmdCheck.Parameters["@werknemerId"].Value = werknemer.Id;
		//                    cmdCheck.Parameters["@bedrijfId"].Value = kvpBedrijf.Key.Id;
		//                    cmdCheck.Parameters["@functieNaam"].Value = functieNaam;

		//                    int i = (int)cmdCheck.ExecuteScalar();
		//                    if (i == 0) {
		//                        cmd.CommandText = queryInsert;

		//                        if (!bestaatJob) {
		//                            cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
		//                            cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
		//                            cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
		//                            cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));

		//                            bestaatJob = true;
		//                        }

		//                        cmd.Parameters["@werknemerId"].Value = werknemer.Id;
		//                        cmd.Parameters["@bedrijfId"].Value = kvpBedrijf.Key.Id;
		//                        cmd.Parameters["@email"].Value = kvpBedrijf.Value.Email;
		//                        cmd.Parameters["@FunctieNaam"].Value = functieNaam;
		//                        cmd.ExecuteNonQuery();
		//                    }
		//                }
		//            }
		//        }
		//    } catch (Exception ex) {
		//        WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
		//        exx.Data.Add("werknemer", werknemer);
		//        throw exx;
		//    } finally {
		//        con.Close();
		//    }
		//}

		/// <summary>
		/// Bewerkt gegevens van een werknemer adhv werknemer object en bedrijf object.
		/// </summary>
		/// <param name="werknemer">Werknemer object dat gewijzigd wenst te worden in de databank.</param>
		/// <param name="bedrijf">bedrijf object waaruit werknemer gewijzigd wenst te worden in de databank.</param>
		/// <exception cref="WerknemerADOException">Faalt werknemer te wijzigen.</exception>
		public void BewerkWerknemer(Werknemer werknemer, Bedrijf bedrijf) {
			SqlConnection con = GetConnection();
			string queryWerknemer = "UPDATE Werknemer " +
									"SET VNaam = @Vnaam, " +
									"ANaam = @ANaam " +
									"WHERE Id = @Id";
			con.Open();
			SqlTransaction trans = con.BeginTransaction();
			try {
				using (SqlCommand cmdWerknemerBedrijf = con.CreateCommand())
				using (SqlCommand cmdWerknemer = con.CreateCommand()) {

					cmdWerknemer.Transaction = trans;
					cmdWerknemerBedrijf.Transaction = trans;
					//Portie werknemer
					cmdWerknemer.CommandText = queryWerknemer;
					cmdWerknemer.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt));
					cmdWerknemer.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
					cmdWerknemer.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
					cmdWerknemer.Parameters["@Id"].Value = werknemer.Id;
					cmdWerknemer.Parameters["@VNaam"].Value = werknemer.Voornaam;
					cmdWerknemer.Parameters["@ANaam"].Value = werknemer.Achternaam;
					cmdWerknemer.ExecuteNonQuery();
					//Portie werknemerBedrijf
					string queryWerknemerBedrijf = "UPDATE WerknemerBedrijf SET WerknemerEMail = @mail WHERE WerknemerId = @Wid AND Bedrijfid = @Bid";
					cmdWerknemerBedrijf.CommandText = queryWerknemerBedrijf;
					cmdWerknemerBedrijf.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar));
					cmdWerknemerBedrijf.Parameters.Add(new SqlParameter("@Wid", SqlDbType.BigInt));
					cmdWerknemerBedrijf.Parameters.Add(new SqlParameter("@Bid", SqlDbType.BigInt));
					cmdWerknemerBedrijf.Parameters["@mail"].Value = werknemer.GeefBedrijvenEnFunctiesPerWerknemer()[bedrijf].Email;
					cmdWerknemerBedrijf.Parameters["@Wid"].Value = werknemer.Id;
					cmdWerknemerBedrijf.Parameters["@Bid"].Value = bedrijf.Id;
					cmdWerknemerBedrijf.ExecuteNonQuery();
					trans.Commit();
				}
			} catch (Exception ex) {
				trans.Rollback();
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Gaat na of functie bestaat adhv parameter functienaam.
		/// </summary>
		/// <param name="functieNaam">Functie die gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="WerknemerADOException">Faalt om bestaan functie te verifiëren op basis van de functie naam.</exception>
		public bool BestaatFunctie(string functieNaam) {
			SqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM Functie " +
						   "WHERE FunctieNaam = @fNaam";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@fNaam", SqlDbType.VarChar));
					cmd.Parameters["@fNaam"].Value = functieNaam;
					int i = (int)cmd.ExecuteScalar();
					return (i > 0);
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("functieNaam", functieNaam);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Voegt functie toe adhv parameter functienaam.
		/// </summary>
		/// <param name="functieNaam">Functie die toegevoegd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="WerknemerADOException">Faalt om functie toe te voegen op basis van de functie naam.</exception>
		public void VoegFunctieToe(string functieNaam) {
			SqlConnection con = GetConnection();
			string query = "INSERT INTO Functie(FunctieNaam) VALUES(@fNaam)";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@fNaam", SqlDbType.VarChar));
					cmd.Parameters["@fNaam"].Value = functieNaam;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("functieNaam", functieNaam);
				throw exx;
			} finally {
				con.Close();
			}
		}


		/// <summary>
		/// Stelt lijst van afspraakloze werknemers samen met enkel lees rechten adhv parameter bedrijf id.
		/// </summary>
		/// <param name="_bedrijfId">Id van het bedrijf waar men de werknemer van wenst op te vragen die niet in afspraak zijn.</param>
		/// <returns>IReadOnlyList van werknemer objecten waar statuscode niet gelijk is aan 1 = 'In gang'.</returns>
		/// <exception cref="WerknemerADOException">Faalt lijst van afspraakloze werknemer objecten samen te stellen op basis van bedrijf id.</exception>
		public IReadOnlyList<Werknemer> GeefVrijeWerknemersOpDitMomentVoorBedrijf(long _bedrijfId) {
			SqlConnection con = GetConnection();
			string query = "SELECT wn.id as WerknemerId, wn.ANaam as WerknemerANaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEmail, " +
					       "b.id as BedrijfId, b.Naam as BedrijfNaam, b.btwnr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, b.BTWChecked, " +
						   "f.Functienaam " +
						   "FROM Werknemer wn " +
						   "LEFT JOIN Werknemerbedrijf wb ON(wb.werknemerId = wn.id) AND wb.Status = 1 " +
						   "LEFT JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
						   "LEFT JOIN Functie f ON(f.id = wb.FunctieId) " +
                           "WHERE b.Id = @bedrijfId AND (SELECT COUNT(*) " +
													    "FROM Afspraak a " +
														"JOIN Werknemerbedrijf wbb ON wbb.Id = a.WerknemerBedrijfId " +
														"WHERE wbb.WerknemerId = wn.Id " +
														"AND a.AfspraakStatusId = 1) = 0 " +
                           "ORDER BY wn.VNaam, wn.ANaam, wn.Id";
            try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
					cmd.Parameters["@bedrijfId"].Value = _bedrijfId;
                    List<Werknemer> werknemers = new List<Werknemer>();
                    Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					IDataReader reader = cmd.ExecuteReader();
					while (reader.Read()) {
						if (bedrijf is null) {
							long bedrijfId = (long)reader["BedrijfId"];
							string bedrijfNaam = (string)reader["BedrijfNaam"];
							string bedrijfBTW = (string)reader["bedrijfBTW"];
							string bedrijfTele = (string)reader["BedrijfTeleNr"];
							string bedrijfMail = (string)reader["BedrijfMail"];
							string bedrijfAdres = (string)reader["BedrijfAdres"];
							bool bedrijfBTWChecked = (bool)reader["BTWChecked"];
							bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfBTWChecked, bedrijfTele, bedrijfMail, bedrijfAdres);
						}
						if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
							long werknemerId = (long)reader["WerknemerId"];
							string werknemerVNaam = (string)reader["WerknemerVNaam"];
							string werknemerAnaam = (string)reader["WerknemerAnaam"];
							werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam);
                            werknemers.Add(werknemer);
						}
						string werknemerMail = (string)reader["WerknemerEmail"];
						string functieNaam = (string)reader["FunctieNaam"];
						werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
                        werknemer.ZetStatusNaamPerBedrijf(bedrijf, "Vrij");
                    }
					return werknemers.AsReadOnly();
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfId", _bedrijfId);
				throw exx;
			} finally {
				con.Close();
			}
		}

		/// <summary>
		/// Stelt lijst van bezette werknemers samen met enkel lees rechten adhv parameter bedrijf id.
		/// </summary>
		/// <param name="_bedrijfId">Id van het bedrijf waar men de werknemer van wenst op te vragen die momenteel in afspraak zijn.</param>
		/// <returns>IReadOnlyList van werknemer objecten waar statuscode gelijk is aan 1 = 'In gang'.</returns>
		/// <exception cref="WerknemerADOException">Faalt lijst van bezette werknemer objecten samen te stellen op basis van bedrijf id.</exception>
		public IReadOnlyList<Werknemer> GeefBezetteWerknemersOpDitMomentVoorBedrijf(long _bedrijfId) {
			SqlConnection con = GetConnection();
			string query = "SELECT wn.id as WerknemerId, wn.ANaam as WerknemerANaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEmail, " +
                           "b.id as BedrijfId, b.Naam as BedrijfNaam, b.btwnr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, b.BTWChecked, " +
                           "f.Functienaam " +
                           "FROM Werknemer wn " +
                           "LEFT JOIN Werknemerbedrijf wb ON(wb.werknemerId = wn.id) AND wb.Status = 1 " +
                           "LEFT JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
                           "LEFT JOIN Functie f ON(f.id = wb.FunctieId) " +
                           "WHERE b.Id = @bedrijfId AND (SELECT COUNT(*) " +
                                                        "FROM Afspraak a " +
                                                        "JOIN Werknemerbedrijf wbb ON wbb.Id = a.WerknemerBedrijfId " +
                                                        "WHERE wbb.WerknemerId = wn.Id " +
                                                        "AND a.AfspraakStatusId = 1) > 0 " +
                           "ORDER BY wn.VNaam, wn.ANaam, wn.Id";
            try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
					cmd.Parameters["@bedrijfId"].Value = _bedrijfId;
                    List<Werknemer> werknemers = new List<Werknemer>();
                    Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					IDataReader reader = cmd.ExecuteReader();
					while (reader.Read()) {
						if (bedrijf is null) {
							long bedrijfId = (long)reader["BedrijfId"];
							string bedrijfNaam = (string)reader["BedrijfNaam"];
							string bedrijfBTW = (string)reader["bedrijfBTW"];
							string bedrijfTele = (string)reader["BedrijfTeleNr"];
							string bedrijfMail = (string)reader["BedrijfMail"];
							string bedrijfAdres = (string)reader["BedrijfAdres"];
							bool bedrijfBTWChecked = (bool)reader["BTWChecked"];
							bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfBTWChecked, bedrijfTele, bedrijfMail, bedrijfAdres);
						}
						if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
							long werknemerId = (long)reader["WerknemerId"];
							string werknemerVNaam = (string)reader["WerknemerVNaam"];
							string werknemerAnaam = (string)reader["WerknemerAnaam"];
							werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam);
                            werknemers.Add(werknemer);
						}
						string werknemerMail = (string)reader["WerknemerEmail"];
						string functieNaam = (string)reader["FunctieNaam"];
						werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
                        werknemer.ZetStatusNaamPerBedrijf(bedrijf, "Bezet");
                    }
					return werknemers.AsReadOnly();
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("bedrijfId", _bedrijfId);
				throw exx;
			} finally {
				con.Close();
			}
		}

        /// <summary>
        /// Stelt id van werknemer in in het werknemer object.
        /// </summary>
        /// <param name="werknemer">werknemer object dat id moet krijgen.</param>
        /// <exception cref="WerknemerADOException">Faalt om id van werknemer te zetten.</exception>

        public void GeefWerknemerId(Werknemer werknemer) {
			SqlConnection con = GetConnection();
			string query = "SELECT TOP(1) w.id " +
							"FROM Werknemer w " +
							"JOIN Werknemerbedrijf wb ON(w.Id = wb.WerknemerId) " +
							"WHERE wb.WerknemerEmail IN(";
			try {
				using (SqlCommand cmd = con.CreateCommand()) {
					con.Open();							
                    int mailCount = 0;
                    foreach (var werknemerInfo in werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Values) {
                        query += $"@mail{mailCount},";
                        cmd.Parameters.Add(new SqlParameter($"@mail{mailCount}", SqlDbType.VarChar));
                        cmd.Parameters[$"@mail{mailCount}"].Value = werknemerInfo.Email;
                        mailCount++;
                    }
                    query = query.Substring(0, query.Length - 1);
                    query += ")";
                    cmd.CommandText = query;
					long i = (long)cmd.ExecuteScalar();
					werknemer.ZetId(i);
				}
			} catch (Exception ex) {
				WerknemerADOException exx = new WerknemerADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				throw exx;
			} finally {
				con.Close();
			}
		}
	}
}
