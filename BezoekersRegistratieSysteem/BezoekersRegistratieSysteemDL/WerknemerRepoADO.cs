﻿using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL {
    /// <summary>
	/// Repo ADO van werknemers
	/// </summary>
    public class WerknemerRepoADO : IWerknemerRepository {
        private string _connectieString;
        /// <summary>
        /// Constructor, initialiseerd een WerknemerRepoADO klasse die een connectiestring met de DB accepteerd
        /// </summary>
        /// <param name="connectieString">Connectiestring met de DB</param>
        public WerknemerRepoADO(string connectieString) {
            _connectieString = connectieString;
        }

        /// <summary>
        /// Maakt connectie met DB
        /// </summary>
        /// <returns>SqlConnection</returns>
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectieString);
        }

        /// <summary>
        /// Kijkt of werknemer in DB bestaat adh werknemer object
        /// </summary>
        /// <param name="werknemer">Werknemer object die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="WerknemerADOException">Faalt om te kijken of werknemer bestaat</exception>
        public bool BestaatWerknemer(Werknemer werknemer) {
            try {
                return BestaatWerknemer(werknemer, null);
            } catch (Exception ex) {
                throw new WerknemerADOException($"WerknemerRepoADO: BestaatWerknemer {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kijkt of werknemer in DB bestaat adh werknemer id
        /// </summary>
        /// <param name="id">Id van werknemer die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="WerknemerADOException">Faalt om te kijken of werknemer bestaat</exception>
        public bool BestaatWerknemer(uint id) {
            try {
                return BestaatWerknemer(null, id);
            } catch (Exception ex) {
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
        private bool BestaatWerknemer(Werknemer? werknemer, uint? werknemerId) {
            //TODO eh oh
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM Werknemer wn ";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (werknemer is not null) {
                        if (werknemer.Id != 0) {
                            query += "WHERE wn.id = @id";
                            cmd.Parameters.Add(new SqlParameter("@id",SqlDbType.BigInt));
                            cmd.Parameters["@id"].Value = werknemer.Id;
                        } else {
                            query += "JOIN Werknemerbedrijf wb ON(wn.id = wb.werknemerId) " +
                                     "WHERE wb.werknemerEmail = @mail";
                            cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar));
                            cmd.Parameters["@mail"].Value = werknemer.Email;
                        }
                    }
                    if (werknemerId.HasValue) {
                        query += "WHERE wn.id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = werknemerId;
                    }
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: BestaatWerknemer {ex.Message}", ex);
                exx.Data.Add("werknemer", werknemer);
                exx.Data.Add("werknemerId", werknemerId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Geeft werknemer object op basis van werknemer id
        /// </summary>
        /// <param name="_werknemerId">Id van gewenste werknemer</param>
        /// <returns>Werknemer object</returns>
        /// <exception cref="WerknemerADOException">Faalt om een werknemer object weer te geven</exception>
        public Werknemer GeefWerknemer(uint _werknemerId) {
            SqlConnection con = GetConnection();
            string query = "SELECT wn.id as WerknemerId, wn.Vnaam as WerknemerVnaam, wn.Anaam as WerknemerAnaam, wb.Email as WerknemerMail, " +
                           "b.id as BedrijfId, b.naam as BedrijfNaam, b.btwnr as bedrijfBTW, b.telenr as bedrijfTele, b.email as BedrijfMail, b.adres as BedrijfAdres, " +
                           "f.functienaam " +
                           "FROM Werknemer wn " +
                           "JOIN Werknemerbedrijf wb ON(wn.id = wb.werknemerid) " +
                           "JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
                           "JOIN functie f ON(f.id = wb.functieid) " +
                           "WHERE wn.id = @werknemerId";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@werknemerId",SqlDbType.BigInt));
                    cmd.Parameters["@werknemerId"].Value = _werknemerId;
                    IDataReader reader = cmd.ExecuteReader();
                    Werknemer werknemer = null;
                    Bedrijf bedrijf = null;
                    while (reader.Read()) {
                        if (werknemer is null) {
                            string werknemerVnaam = (string)reader["WerknemerVnaam"];
                            string werknemerAnaam = (string)reader["WerknemerAnaam"];
                            string werknemerMail = (string)reader["WerknemerMail"];
                            werknemer = new Werknemer(_werknemerId, werknemerVnaam, werknemerAnaam, werknemerMail);
                        }
                        if (bedrijf is null || bedrijf.Id != (uint)reader["BedrijfId"]) {
                            uint bedrijfId = (uint)reader["BedrijfId"];
                            string bedrijfNaam = (string)reader["BedrijfNaam"];
                            string bedrijfBTW = (string)reader["bedrijfBTW"];
                            string bedrijfTele = (string)reader["bedrijfTele"];
                            string bedrijfMail = (string)reader["BedrijfMail"];
                            string bedrijfAdres = (string)reader["BedrijfAdres"];
                            bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfTele, bedrijfMail, bedrijfAdres);
                        }
                        string functieNaam = (string)reader["functienaam"];
                        werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf,functieNaam);
                    }
                    return werknemer;
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: GeefWerknemer {ex.Message}", ex);
                exx.Data.Add("werknemerId", _werknemerId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Geeft lijst van werknemers op basis van voor- en/of achternaam
        /// </summary>
        /// <param name="voornaam">voornaam van gewenste medewerker</param>
        /// <param name="achternaam">achternaam van gewenste medewerker</param>
        /// <returns>Lijst Werknemer objecten op basis van voor- en/of achternaam</returns>
        /// <exception cref="WerknemerADOException">Faalt om een lijst van werknemers op te roepen</exception>
        public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string voornaam, string achternaam) {
            SqlConnection con = GetConnection();
            string query = "SELECT wn.id as WerknemerId, wn.ANaam as WerknemerANaam, wn.VNaam as WerknemerVNaam, wn.Email as WerknemerMail, " +
                           "b.id as BedrijfId, b.Naam as BedrijfNaam, b.btwnr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, " +
                           "f.Functienaam " +
                           "FROM Werknemer wn " +
                           "JOIN Werknemerbedrijf wb ON(wb.werknemerId = wn.id) " +
                           "JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
                           "JOIN Functie f ON(f.id = wb.FunctieId) " +
                           "WHERE wn.ANaam LIKE @ANaam " +
                           "AND wn.VNaam LIKE @VNaam " +
                           "ORDER BY wn.id, b.id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
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
                    while (reader.Read()) {
                        if (!reader.IsDBNull(reader.GetOrdinal("WerknemerId"))) {
                            if (werknemer is null || werknemer.Id != (uint)reader["WerknemerId"]) {
                                uint werknemerId = (uint)reader["WerknemerId"];
                                string werknemerVNaam = (string)reader["WerknemerVNaam"];
                                string werknemerAnaam = (string)reader["WerknemerAnaam"];
                                string werknemerMail = (string)reader["WerknemerMail"];
                                werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam, werknemerMail);                            
                            }
                            if (bedrijf is null || bedrijf.Id != (uint)reader["BedrijfId"]) {
                                uint bedrijfId = (uint)reader["BedrijfId"];
                                string bedrijfNaam = (string)reader["BedrijfNaam"];
                                string bedrijfBTW = (string)reader["BedrijfBTW"];
                                string bedrijfTeleNr = (string)reader["BedrijfTeleNr"];
                                string bedrijfMail = (string)reader["BedrijfMail"];
                                string bedrijfAdres = (string)reader["BedrijfAdres"];
                                bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
                            }
                            string functieNaam = (string)reader["FunctieNaam"];
                            werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf,functieNaam);
                        }
                    }
                    return werknemers;
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: GeefWerknemersOpNaam {ex.Message}", ex);
                exx.Data.Add("voornaam", voornaam);
                exx.Data.Add("achternaam", achternaam);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Geeft lijst van werknemers op basis van bedrijf id
        /// </summary>
        /// <param name="_bedrijfId">Id van bedrijf</param>
        /// <returns>Lijst Werknemer objecten</returns>
        /// <exception cref="WerknemerADOException">Faalt om een lijst van werknemers op te roepen</exception>
        public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint _bedrijfId) {
            SqlConnection con = GetConnection();
            string query = "SELECT wn.id as WerknemerId, wn.ANaam as WerknemerANaam, wn.VNaam as WerknemerVNaam, wn.Email as WerknemerMail, " +
                           "b.id as BedrijfId, b.Naam as BedrijfNaam, b.btwnr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, " +
                           "f.Functienaam " +
                           "FROM Werknemer wn " +
                           "JOIN Werknemerbedrijf wb ON(wb.werknemerId = wn.id) " +
                           "JOIN bedrijf b ON(b.id = wb.bedrijfid) " +
                           "JOIN Functie f ON(f.id = wb.FunctieId) " +
                           "WHERE AND b.id = @bedrijfId " +
                           "ORDER BY wn.id, b.id";
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
                        if (!reader.IsDBNull(reader.GetOrdinal("WerknemerId"))) {
                            if (werknemer is null || werknemer.Id != (uint)reader["WerknemerId"]) {
                                uint werknemerId = (uint)reader["WerknemerId"];
                                string werknemerVNaam = (string)reader["WerknemerVNaam"];
                                string werknemerAnaam = (string)reader["WerknemerAnaam"];
                                string werknemerMail = (string)reader["WerknemerMail"];
                                werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam, werknemerMail);
                            }
                            if (bedrijf is null || bedrijf.Id != (uint)reader["BedrijfId"]) {
                                uint bedrijfId = (uint)reader["BedrijfId"];
                                string bedrijfNaam = (string)reader["BedrijfNaam"];
                                string bedrijfBTW = (string)reader["BedrijfBTW"];
                                string bedrijfTeleNr = (string)reader["BedrijfTeleNr"];
                                string bedrijfMail = (string)reader["BedrijfMail"];
                                string bedrijfAdres = (string)reader["BedrijfAdres"];
                                bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
                            }
                            string functieNaam = (string)reader["FunctieNaam"];
                            werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, functieNaam);
                        }
                    }
                    return werknemers;
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: GeefWerknemersPerBedrijf {ex.Message}", ex);
                exx.Data.Add("bedrijfId", _bedrijfId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// verwijder werknemer op basis van id
        /// </summary>
        /// <param name="werknemerId">Id van gewenste werknemer</param>
        /// <exception cref="WerknemerADOException">Faalt om een werknemer status naar verwijderd te veranderen</exception>
        public void VerwijderWerknemer(uint werknemerId) {
            try {
                VeranderStatusWerknemer(werknemerId, 2);
            } catch (Exception ex) {
                throw new WerknemerADOException($"WerknemerRepoADO: VerwijderWerknemer {ex.Message}", ex);
            }
        }

        /// <summary>
        /// verwijder werknemer op basis van id
        /// </summary>
        /// <param name="werknemer">Id van gewenste werknemer</param>
        /// <exception cref="WerknemerADOException">Faalt om een werknemer status naar verwijderd te veranderen</exception>
        public void VerwijderWerknemer(Werknemer werknemer) {
            //Dit moet verwijderWerknemerFunctie zijn
            throw new NotImplementedException();
        }

        /// <summary>
        /// prive methode die status van werknemer veranderd
        /// </summary>
        /// <param name="werknemerId">Id van gewesnte werknemer</param>
        /// <param name="statusId">Id van gewesnte status</param>
        /// <exception cref="WerknemerADOException">Faalt om een werknemer status te veranderen</exception>
        private void VeranderStatusWerknemer(uint werknemerId, int statusId) {
            //TODO PROB THROW IT IN VERWIJDERWERKNEMER AND ADD THE NEC ITEMS LIKE BEDRIJF ID AND FUNCTION NAME
            SqlConnection con = GetConnection();
            string query = "UPDATE Werknemerbedrijf " +
                           "SET Status = @statusId " +
                           "WHERE BedrijfId = @bedrijfId " +
                           "AND FunctieId = (SELECT Id FROM FUNCTIE WHERE FunctieNaam = @FunctieNaam) " +
                           "AND WerknemerId = @werknemerId ";
                         //"AND Status = 1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
                    cmd.Parameters["@bedrijfId"].Value = null; // werknemer.Bedrijf.id;
                    cmd.Parameters["@FunctieNaam"].Value = null; //Functienaam
                    cmd.Parameters["@werknemerId"].Value = werknemerId;
                    cmd.Parameters["@statusId"].Value = statusId;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VeranderStatusWerknemer {ex.Message}", ex);
                exx.Data.Add("werknemerId", werknemerId);
                exx.Data.Add("statusId", statusId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// voegt werknemer toe
        /// </summary>
        /// <param name="werknemer">Werknemer object die toegevoegd moet worden</param>
        /// <exception cref="WerknemerADOException">Faalt om een werknemer toe te voegen</exception>
        public Werknemer VoegWerknemerToe(Werknemer werknemer) {
            SqlConnection con = GetConnection();
            string query = "INSERT INTO Werknemer (VNaam, ANaam) VALUES(@VNaam, @ANaam)";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
                    cmd.Parameters["@VNaam"].Value = werknemer.Voornaam;
                    cmd.Parameters["@ANaam"].Value = werknemer.Achternaam;
                    uint i = (uint)cmd.ExecuteScalar();
                    werknemer.ZetId(i);
                    //Dit voegt de bedrijven/functie toe aan uw werknemer in de DB
                    VoegFunctieToeAanWerknemer(werknemer);
                    return werknemer;
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VoegWerknemerToe {ex.Message}", ex);
                exx.Data.Add("werknemer", werknemer);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// voegt bedrijf en functie aan werknemer toe
        /// </summary>
        /// <param name="werknemer">Bedrijf en functie die aan werknemer word toegevoegd</param>
        /// <exception cref="WerknemerADOException">Faalt om bedrijf en functie aan werknemer toe te voegen</exception>
        private void VoegFunctieToeAanWerknemer(Werknemer werknemer) {
            SqlConnection con = GetConnection();
            string queryInsert = "INSERT INTO WerknemerBedrijf (BedrijfId, WerknemerId, WerknemerEmail, FunctieId) " +
                                 "VALUES(@bedrijfId,@werknemerId, @email,(SELECT Id FROM Functie WHERE FunctieNaam = @FunctieNaam))";
            try {
                using (SqlCommand cmdCheck = con.CreateCommand())
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    //TODO: This gets replaced with werknemerInfo
                    foreach (var kvpBedrijf in werknemer.GeefBedrijvenEnFunctiesPerWerknemer()) {
                        foreach (var functieNaam in kvpBedrijf.Value) {
                            string queryDoesJobExist = "SELECT COUNT(*) " +
                                                       "FROM WerknemerBedrijf " +
                                                       "WHERE WerknemerId = @werknemerId " +
                                                       "AND bedrijfId = @bedrijfId " +
                                                       "AND FunctieId = (SELECT Id FROM Functie WHERE FunctieNaam = @functieNaam)";
                            cmdCheck.CommandText = queryDoesJobExist;
                            cmdCheck.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                            cmdCheck.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                            cmdCheck.Parameters.Add(new SqlParameter("@functieNaam", SqlDbType.VarChar));
                            cmdCheck.Parameters["@werknemerId"].Value = werknemer.Id;
                            cmdCheck.Parameters["@bedrijfId"].Value = kvpBedrijf.Key.Id;                        
                            cmdCheck.Parameters["@functieNaam"].Value = functieNaam;
                            int i = (int)cmdCheck.ExecuteScalar();
                            if (i == 0) {
                                cmd.CommandText = queryInsert;
                                cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                                cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                                cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
                                cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));
                                cmd.Parameters["@werknemerId"].Value = werknemer.Id;
                                cmd.Parameters["@bedrijfId"].Value = kvpBedrijf.Key.Id;
                                cmd.Parameters["@email"].Value = null; //TODO:email
                                cmd.Parameters["@FunctieNaam"].Value = functieNaam;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VoegFunctieToeAanWerkNemer {ex.Message}", ex);
                exx.Data.Add("werknemer", werknemer);
                throw exx;
            } finally {
                con.Close();
            }
        }

        public void VoegWerknemerFunctieToe(Werknemer werknemer, Bedrijf bedrijf, string functie) {
            SqlConnection con = GetConnection();
            string query = "INSERT INTO WerknemerBedrijf (BedrijfId, WerknemerId, FunctieId) " +
                           "VALUES(@bedrijfId,@werknemerId,(SELECT Id FROM Functie WHERE FunctieNaam = @FunctieNaam))";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@FunctieNaam", SqlDbType.VarChar));
                    cmd.Parameters["@werknemerId"].Value = werknemer.Id;
                    cmd.Parameters["@bedrijfId"].Value = bedrijf.Id;
                    cmd.Parameters["@FunctieNaam"].Value = functie;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: VoegFunctieToeAanWerkNemer {ex.Message}", ex);
                exx.Data.Add("werknemer", werknemer);
                throw exx;
            } finally {
                con.Close();
            }
        }


        /// <summary>
        /// wijzigd werknemer op basis van werknemer object
        /// </summary>
        /// <param name="werknemer">Werknemer object die gewijzigd moet worden</param>
        /// <exception cref="WerknemerADOException">Faalt om werknemer te wijzigen</exception>
        public void WijzigWerknemer(Werknemer werknemer) {
            //TODO: moet email per bedrijf ook veranderd worden?
            SqlConnection con = GetConnection();
            string query = "UPDATE Werknemer " +
                           "SET VNaam = @Vnaam, " +
                           "ANaam = @ANaam, " +
                         //"EMail = @Email " +
                           "WHERE Id = @Id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
                  //cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                    cmd.Parameters["@Id"].Value = werknemer.Id;
                    cmd.Parameters["@VNaam"].Value = werknemer.Voornaam;
                    cmd.Parameters["@ANaam"].Value = werknemer.Achternaam;
                  //cmd.Parameters["@Email"].Value = werknemer.Email;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                WerknemerADOException exx = new WerknemerADOException($"WerknemerRepoADO: WijzigWerknemer {ex.Message}", ex);
                exx.Data.Add("werknemer", werknemer);
                throw exx;
            } finally {
                con.Close();
            }
        }
    }
}
