using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL
{
	/// <summary>
	/// Repo ADO van werknemers
	/// </summary>
	public class WerknemerRepoADO : IWerknemerRepository
	{
		private string _connectieString;
		/// <summary>
		/// Constructor, initialiseerd een WerknemerRepoADO klasse die een connectiestring met de DB accepteerd
		/// </summary>
		/// <param name="connectieString">Connectiestring met de DB</param>
		public WerknemerRepoADO(string connectieString)
		{
			_connectieString = connectieString;
		}

		/// <summary>
		/// Maakt connectie met DB
		/// </summary>
		/// <returns>SqlConnection</returns>
		private SqlConnection GetConnection()
		{
			return new SqlConnection(_connectieString);
		}
		/// <summary>
		/// Kijkt of werknemer in DB bestaat adh werknemer object
		/// </summary>
		/// <param name="werknemer">Werknemer object die gecontroleerd moet worden</param>
		/// <returns>bool (True = bestaat)</returns>
		/// <exception cref="WerknemerADOException">Faalt om te kijken of werknemer bestaat</exception>
		public bool BestaatWerknemer(Werknemer werknemer)
		{
			try
			{
				return BestaatWerknemer(werknemer, null);
			} catch (Exception ex)
			{
				throw new WerknemerADOException($"WerknemerRepoADO: BestaatWerknemer {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Kijkt of werknemer in DB bestaat adh werknemer id
		/// </summary>
		/// <param name="id">Id van werknemer die gecontroleerd moet worden</param>
		/// <returns>bool (True = bestaat)</returns>
		/// <exception cref="WerknemerADOException">Faalt om te kijken of werknemer bestaat</exception>
		public bool BestaatWerknemer(long id)
		{
			try
			{
				return BestaatWerknemer(null, id);
			} catch (Exception ex)
			{
				throw new WerknemerADOException($"WerknemerRepoADO: BestaatWerknemer {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Prive methode die kijkt of werknemer in DB bestaat (Table Werknemer) adh werknemer object, id
		/// </summary>
		/// <param name="werknemer">Optioneel: Werknemer object die gecontroleerd moet worden</param>
		/// <param name="werknemerId">Optioneel: Werknemer id die gecontroleerd moet worden</param>
		/// <returns>bool</returns>
		/// <exception cref="WerknemerADOException">Faalt om te kijken of werknemer bestaat</exception>
		private bool BestaatWerknemer(Werknemer? werknemer, long? werknemerId)
		{
			SqlConnection con = GetConnection();
			string query = "SELECT COUNT(*) " +
						   "FROM Werknemer wn ";
			try
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					if (werknemer is not null)
					{
						if (werknemer.Id != 0)
						{
							query += "WHERE wn.id = @id";
							cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
							cmd.Parameters["@id"].Value = werknemer.Id;
						} else
						{
							query += "JOIN Werknemerbedrijf wb ON(wn.id = wb.werknemerId) " +
									 "WHERE wb.werknemerEmail IN(";
							int mailCount = 0;
							foreach (var werknemerInfo in werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Values)
							{
								query += $"@mail{mailCount}";
								cmd.Parameters.Add(new SqlParameter($"@mail{mailCount}", SqlDbType.VarChar));
								cmd.Parameters[$"@mail{mailCount}"].Value = werknemerInfo.Email;
								mailCount++;
							}
							query += ")";
						}
					}
					if (werknemerId.HasValue)
					{
						query += "WHERE wn.id = @id";
						cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
						cmd.Parameters["@id"].Value = werknemerId;
					}
					cmd.CommandText = query;
					int i = (int)cmd.ExecuteScalar();
					return (i > 0);
				}
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: BestaatWerknemer {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				exx.Data.Add("werknemerId", werknemerId);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// Geeft werknemer object op basis van werknemer id, geeft ENKEL jobs waar hem actief is
		/// </summary>
		/// <param name="_werknemerId">Id van gewenste werknemer</param>
		/// <returns>Werknemer object</returns>
		/// <exception cref="WerknemerADOException">Faalt om een werknemer object weer te geven</exception>
		public Werknemer GeefWerknemer(long _werknemerId)
		{
			SqlConnection con = GetConnection();
			string query = "SELECT wn.id as WerknemerId, wn.Vnaam as WerknemerVnaam, wn.Anaam as WerknemerAnaam, wb.WerknemerEmail, " +
						   "b.id as BedrijfId, b.naam as BedrijfNaam, b.btwnr as bedrijfBTW, b.telenr as bedrijfTele, b.email as BedrijfMail, b.adres as BedrijfAdres, " +
						   "f.functienaam " +
						   "FROM Werknemer wn " +
						   "LEFT JOIN Werknemerbedrijf wb ON(wn.id = wb.werknemerid) AND wb.Status = 1 " +
						   "LEFT JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
						   "LEFT JOIN functie f ON(f.id = wb.functieid) " +
						   "WHERE wn.id = @werknemerId";
			try
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
					cmd.Parameters["@werknemerId"].Value = _werknemerId;
					IDataReader reader = cmd.ExecuteReader();
					Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					while (reader.Read())
					{
						if (werknemer is null)
						{
							string werknemerVnaam = (string)reader["WerknemerVnaam"];
							string werknemerAnaam = (string)reader["WerknemerAnaam"];
							werknemer = new Werknemer(_werknemerId, werknemerVnaam, werknemerAnaam);
						}
						if (!reader.IsDBNull(reader.GetOrdinal("WerknemerEmail")))
						{
							if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"])
							{
								long bedrijfId = (long)reader["BedrijfId"];
								string bedrijfNaam = (string)reader["BedrijfNaam"];
								string bedrijfBTW = (string)reader["bedrijfBTW"];
								string bedrijfTele = (string)reader["bedrijfTele"];
								string bedrijfMail = (string)reader["BedrijfMail"];
								string bedrijfAdres = (string)reader["BedrijfAdres"];

								// TODO VOOR GEKKE BJORN: vervang true door statuscode
								bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, true, bedrijfTele, bedrijfMail, bedrijfAdres);
							}
							string werknemerMail = (string)reader["WerknemerEmail"];
							string functieNaam = (string)reader["functienaam"];
							werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
						}
					}
					return werknemer;
				}
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: GeefWerknemer {ex.Message}", ex);
				exx.Data.Add("werknemerId", _werknemerId);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// Geeft lijst van werknemers op basis van voor- en/of achternaam, geeft ENKEL jobs waar hem actief is
		/// </summary>
		/// <param name="voornaam">voornaam van gewenste medewerker</param>
		/// <param name="achternaam">achternaam van gewenste medewerker</param>
		/// <returns>Lijst Werknemer objecten op basis van voor- en/of achternaam</returns>
		/// <exception cref="WerknemerADOException">Faalt om een lijst van werknemers op te roepen</exception>
		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string voornaam, string achternaam)
		{
			SqlConnection con = GetConnection();
			string query = "SELECT wn.id as WerknemerId, wn.ANaam as WerknemerANaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEmail, " +
						   "b.id as BedrijfId, b.Naam as BedrijfNaam, b.btwnr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, " +
						   "f.Functienaam " +
						   "FROM Werknemer wn " +
						   "LEFT JOIN Werknemerbedrijf wb ON(wb.werknemerId = wn.id) AND wb.Status = 1 " +
						   "LEFT JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
						   "LEFT JOIN Functie f ON(f.id = wb.FunctieId) " +
						   "WHERE wn.ANaam LIKE @ANaam " +
						   "AND wn.VNaam LIKE @VNaam " +
						   "ORDER BY wn.id, b.id";
			try
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
					cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
					cmd.Parameters["@VNaam"].Value = $"%{voornaam}%";
					cmd.Parameters["@ANaam"].Value = $"%{achternaam}%";
					List<Werknemer> werknemers = new List<Werknemer>();
					Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					IDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						if (!reader.IsDBNull(reader.GetOrdinal("WerknemerId")))
						{
							if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"])
							{
								long werknemerId = (long)reader["WerknemerId"];
								string werknemerVNaam = (string)reader["WerknemerVNaam"];
								string werknemerAnaam = (string)reader["WerknemerAnaam"];
								werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam);
								werknemers.Add(werknemer);
							}
							if (!reader.IsDBNull(reader.GetOrdinal("WerknemerEmail")))
							{
								if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"])
								{
									long bedrijfId = (long)reader["BedrijfId"];
									string bedrijfNaam = (string)reader["BedrijfNaam"];
									string bedrijfBTW = (string)reader["bedrijfBTW"];
									string bedrijfTele = (string)reader["BedrijfTeleNr"];
									string bedrijfMail = (string)reader["BedrijfMail"];
									string bedrijfAdres = (string)reader["BedrijfAdres"];

									// TODO VOOR GEKKE BJORN: vervang true door statuscode
									bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, true, bedrijfTele, bedrijfMail, bedrijfAdres);
								}
								string werknemerMail = (string)reader["WerknemerEmail"];
								string functieNaam = (string)reader["functienaam"];
								werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
							}
						}
					}
					return werknemers;
				}
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: GeefWerknemersOpNaam {ex.Message}", ex);
				exx.Data.Add("voornaam", voornaam);
				exx.Data.Add("achternaam", achternaam);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// Geeft lijst van werknemers op basis van bedrijf id, geeft ENKEL jobs waar hem actief is
		/// </summary>
		/// <param name="_bedrijfId">Id van bedrijf</param>
		/// <returns>Lijst Werknemer objecten</returns>
		/// <exception cref="WerknemerADOException">Faalt om een lijst van werknemers op te roepen</exception>
		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(long _bedrijfId)
		{
			SqlConnection con = GetConnection();
			string query = "SELECT wn.id as WerknemerId, wn.ANaam as WerknemerANaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEmail, " +
						   "b.id as BedrijfId, b.Naam as BedrijfNaam, b.btwnr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, " +
						   "f.Functienaam " +
						   "FROM Werknemer wn " +
						   "LEFT JOIN Werknemerbedrijf wb ON(wb.werknemerId = wn.id) AND wb.Status = 1 " +
						   "LEFT JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
						   "LEFT JOIN Functie f ON(f.id = wb.FunctieId) " +
						   "WHERE b.id = @bedrijfId " +
						   "ORDER BY wn.id, b.id";
			try
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
					cmd.Parameters["@bedrijfId"].Value = _bedrijfId;
					List<Werknemer> werknemers = new List<Werknemer>();
					Werknemer werknemer = null;
					Bedrijf bedrijf = null;
					IDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						if (!reader.IsDBNull(reader.GetOrdinal("WerknemerId")))
						{
							if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"])
							{
								long werknemerId = (long)reader["WerknemerId"];
								string werknemerVNaam = (string)reader["WerknemerVNaam"];
								string werknemerAnaam = (string)reader["WerknemerAnaam"];

								werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam);
								werknemers.Add(werknemer);
							}
							if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"])
							{
								long bedrijfId = (long)reader["BedrijfId"];
								string bedrijfNaam = (string)reader["BedrijfNaam"];
								string bedrijfBTW = (string)reader["BedrijfBTW"];
								string bedrijfTeleNr = (string)reader["BedrijfTeleNr"];
								string bedrijfMail = (string)reader["BedrijfMail"];
								string bedrijfAdres = (string)reader["BedrijfAdres"];

								// TODO VOOR GEKKE BJORN: vervang true door statuscode
								bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, true, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
							}
							string werknemerMail = (string)reader["WerknemerEmail"];
							string functieNaam = (string)reader["FunctieNaam"];
							werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
						}
					}
					return werknemers;
				}
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: GeefWerknemersPerBedrijf {ex.Message}", ex);
				exx.Data.Add("bedrijfId", _bedrijfId);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// verwijder werknemer op basis van werknemer en bedrijf object
		/// </summary>
		/// <param name="werknemer">gewenste werknemer</param>
		/// <param name="bedrijf">in welk bedrijf dat hij/zij verwijderd moet worden</param>
		/// <exception cref="WerknemerADOException">Faalt om een werknemer status naar verwijderd te veranderen</exception>
		public void VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf)
		{
			try
			{
				VerwijderWerknemer(werknemer, bedrijf, null);
			} catch (Exception ex)
			{
				throw new WerknemerADOException($"WerknemerRepoADO: VerwijderWerknemer {ex.Message}", ex);
			}
		}

		/// <summary>
		/// verwijder werknemer op basis van werknemer en bedrijf object
		/// </summary>
		/// <param name="werknemer">gewenste werknemer</param>
		/// <param name="bedrijf">in welk bedrijf dat hij/zij verwijderd moet worden</param>
		/// <exception cref="WerknemerADOException">Faalt om een werknemer status naar verwijderd te veranderen</exception>
		public void VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie)
		{
			try
			{
				VerwijderWerknemer(werknemer, bedrijf, functie);
			} catch (Exception ex)
			{
				throw new WerknemerADOException($"WerknemerRepoADO: VerwijderWerknemerFunctie {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Prive Methode verwijder werknemer op basis van werknemer, bedrijf object, en of functie
		/// </summary>
		/// <param name="werknemer">gewenste werknemer</param>
		/// <param name="bedrijf">in welk bedrijf dat hij/zij verwijderd moet worden</param>
		/// <exception cref="WerknemerADOException">Faalt om een werknemer status naar verwijderd te veranderen</exception>
		private void VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf, string? functie)
		{
			SqlConnection con = GetConnection();
			string query = "UPDATE Werknemerbedrijf " +
						   "SET Status = 2 " +
						   "WHERE BedrijfId = @bedrijfId " +
						   "AND WerknemerId = @werknemerId " +
						   "AND Status = 1";
			try
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					if (!String.IsNullOrWhiteSpace(functie))
					{
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
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VerwijderWerknemer {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				exx.Data.Add("bedrijf", bedrijf);
				exx.Data.Add("functie", functie);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// voegt bedrijf en functie toe aan werknemer
		/// </summary>
		/// <param name="werknemer">werknemer waar aan word toegevoegd</param>
		/// <param name="bedrijf">Bedrijf waar de werknemer werkt</param>
		/// <param name="functie">functie die aan werknemer word toegevoegd</param>
		/// <exception cref="WerknemerADOException">Faalt om bedrijf en functie aan werknemer toe te voegen</exception>
		public void VoegWerknemerFunctieToe(Werknemer werknemer, Bedrijf bedrijf, string functie)
		{
			SqlConnection con = GetConnection();
			string queryInsert = "INSERT INTO WerknemerBedrijf (BedrijfId, WerknemerId, WerknemerEmail, FunctieId) " +
								 "VALUES(@bedrijfId,@werknemerId, @email,(SELECT Id FROM Functie WHERE FunctieNaam = @FunctieNaam))";
			try
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = queryInsert;
					cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
					cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
					cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
					cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));
					cmd.Parameters["@werknemerId"].Value = werknemer.Id;
					cmd.Parameters["@bedrijfId"].Value = bedrijf.Id;
					cmd.Parameters["@email"].Value = werknemer.GeefBedrijvenEnFunctiesPerWerknemer()[bedrijf].Email;
					cmd.Parameters["@FunctieNaam"].Value = functie;
					cmd.ExecuteNonQuery();
				}
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VoegFunctieToeAanWerkNemer {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				exx.Data.Add("bedrijf", bedrijf);
				exx.Data.Add("functie", functie);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// voegt werknemer toe
		/// </summary>
		/// <param name="werknemer">Werknemer object die toegevoegd moet worden</param>
		/// <exception cref="WerknemerADOException">Faalt om een werknemer toe te voegen</exception>
		public Werknemer VoegWerknemerToe(Werknemer werknemer)
		{
			SqlConnection con = GetConnection();
			string query = "INSERT INTO Werknemer (VNaam, ANaam) OUTPUT INSERTED.Id VALUES (@VNaam, @ANaam)";
			try
			{
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					cmd.CommandText = query;
					cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
					cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
					cmd.Parameters["@VNaam"].Value = werknemer.Voornaam;
					cmd.Parameters["@ANaam"].Value = werknemer.Achternaam;
					long i = (long)cmd.ExecuteScalar();
					werknemer.ZetId(i);
					//Dit voegt de bedrijven/functie toe aan uw werknemer in de DB
					VoegFunctieToeAanWerknemer(werknemer);
					return werknemer;
				}
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VoegWerknemerToe {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// voegt bedrijf en functie aan werknemer toe
		/// </summary>
		/// <param name="werknemer">Bedrijf en functie die aan werknemer word toegevoegd</param>
		/// <exception cref="WerknemerADOException">Faalt om bedrijf en functie aan werknemer toe te voegen</exception>
		private void VoegFunctieToeAanWerknemer(Werknemer werknemer)
		{

			bool bestaatJob = false;

			SqlConnection con = GetConnection();
			string queryInsert = "INSERT INTO WerknemerBedrijf (BedrijfId, WerknemerId, WerknemerEmail, FunctieId) " +
								 "VALUES(@bedrijfId,@werknemerId, @email,(SELECT Id FROM Functie WHERE FunctieNaam = @FunctieNaam))";
			try
			{
				using (SqlCommand cmdCheck = con.CreateCommand())
				using (SqlCommand cmd = con.CreateCommand())
				{
					con.Open();
					foreach (var kvpBedrijf in werknemer.GeefBedrijvenEnFunctiesPerWerknemer())
					{
						foreach (var functieNaam in kvpBedrijf.Value.GeefWerknemerFuncties())
						{
							string queryDoesJobExist = "SELECT COUNT(*) " +
													   "FROM WerknemerBedrijf " +
													   "WHERE WerknemerId = @werknemerId " +
													   "AND bedrijfId = @bedrijfId " +
													   "AND FunctieId = (SELECT Id FROM Functie WHERE FunctieNaam = @functieNaam) " +
													   "AND Status = 1";
							
							cmdCheck.CommandText = queryDoesJobExist;

							if (!bestaatJob) {
								cmdCheck.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
								cmdCheck.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
								cmdCheck.Parameters.Add(new SqlParameter("@functieNaam", SqlDbType.VarChar));
							}
							
							cmdCheck.Parameters["@werknemerId"].Value = werknemer.Id;
							cmdCheck.Parameters["@bedrijfId"].Value = kvpBedrijf.Key.Id;
							cmdCheck.Parameters["@functieNaam"].Value = functieNaam;
							
							int i = (int)cmdCheck.ExecuteScalar();
							if (i == 0)
							{
								cmd.CommandText = queryInsert;
								
								if (!bestaatJob) {
									cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
									cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
									cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
									cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));

									bestaatJob = true;
								}
								
								cmd.Parameters["@werknemerId"].Value = werknemer.Id;
								cmd.Parameters["@bedrijfId"].Value = kvpBedrijf.Key.Id;
								cmd.Parameters["@email"].Value = kvpBedrijf.Value.Email;
								cmd.Parameters["@FunctieNaam"].Value = functieNaam;
								cmd.ExecuteNonQuery();
							}
						}
					}
				}
			} catch (Exception ex)
			{
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VoegFunctieToeAanWerkNemer {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				throw exx;
			} finally
			{
				con.Close();
			}
		}

		/// <summary>
		/// wijzigd werknemer op basis van werknemer object
		/// </summary>
		/// <param name="werknemer">Werknemer object die gewijzigd moet worden</param>
		/// <exception cref="WerknemerADOException">Faalt om werknemer te wijzigen</exception>
		public void WijzigWerknemer(Werknemer werknemer, Bedrijf bedrijf)
		{
			SqlConnection con = GetConnection();
			string queryWerknemer = "UPDATE Werknemer " +
									"SET VNaam = @Vnaam, " +
									"ANaam = @ANaam " +
									"WHERE Id = @Id";
			SqlTransaction? trans = null;
			try
			{
				using (SqlCommand cmdWerknemerBedrijf = con.CreateCommand())
				using (SqlCommand cmdWerknemer = con.CreateCommand())
				{
					con.Open();
					trans = con.BeginTransaction();
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
			} catch (Exception ex)
			{
				trans?.Rollback();
				WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: WijzigWerknemer {ex.Message}", ex);
				exx.Data.Add("werknemer", werknemer);
				throw exx;
			} finally
			{
				con.Close();
			}
		}
	}
}
