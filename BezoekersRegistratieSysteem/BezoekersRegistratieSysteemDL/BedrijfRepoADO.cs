using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace BezoekersRegistratieSysteemDL {
    public class BedrijfRepoADO : IBedrijfRepository {
        private string _connectieString;

        public BedrijfRepoADO(string connectieString) {
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
        /// Methode die kijkt of bedrijf in DB bestaat adh bedrijf object
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <returns>bool</returns>
        /// <exception cref="BedrijfADOException"></exception>
        public bool BestaatBedrijf(Bedrijf bedrijf) {
            try {
                return BestaatBedrijf(bedrijf, null, null);
            } catch (Exception ex) {
                throw new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Methode die kijkt of bedrijf in DB bestaat adh bedrijf id
        /// </summary>
        /// <param name="bedrijfId"></param>
        /// <returns>bool</returns>
        /// <exception cref="BedrijfADOException"></exception>
        public bool BestaatBedrijf(uint bedrijfId) {
            try {
                return BestaatBedrijf(null, bedrijfId, null);
            } catch (Exception ex) {
                throw new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Methode die kijkt of bedrijf in DB bestaat adh bedrijf naam
        /// </summary>
        /// <param name="bedrijfsnaam"></param>
        /// <returns>bool</returns>
        /// <exception cref="BedrijfADOException"></exception>
        public bool BestaatBedrijf(string bedrijfsnaam) {
            try {
                return BestaatBedrijf(null, null, bedrijfsnaam);
            } catch (Exception ex) {
                throw new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Private methode die kijkt of bedrijf in DB bestaat adh bedrijf object, id, naam
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <param name="bedrijfId"></param>
        /// <param name="bedrijfsnaam"></param>
        /// <returns>bool</returns>
        /// <exception cref="BedrijfADOException"></exception>
        private bool BestaatBedrijf(Bedrijf? bedrijf, uint? bedrijfId, string? bedrijfsnaam) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM bedrijf " +
                           "WHERE 1=1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    var sqltype = (bedrijf is not null && bedrijf.Id != 0) ? SqlDbType.BigInt : (bedrijfId.HasValue) ? SqlDbType.BigInt : SqlDbType.VarChar;
                    cmd.Parameters.Add(new SqlParameter("@querylookup", sqltype));
                    string databaseWhere = (bedrijf is not null) ? (bedrijf.Id != 0) ? "id" : "BTWNr" : (bedrijfId.HasValue) ? "id" : "Naam";
                    query += $" AND {databaseWhere} = @querylookup";
                    cmd.Parameters["@querylookup"].Value = (bedrijf is not null) ? (bedrijf.Id != 0) ? bedrijf.Id : bedrijf.BTW : (bedrijfId.HasValue) ? bedrijfId : bedrijfsnaam;
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                BedrijfADOException exx = new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf {ex.Message}", ex);
                exx.Data.Add("bedrijf", bedrijf);
                exx.Data.Add("bedrijfId", bedrijfId);
                exx.Data.Add("bedrijfsnaam", bedrijfsnaam);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Bewerkt informatie van een bedrijf
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <exception cref="BedrijfADOException"></exception>
        public void BewerkBedrijf(Bedrijf bedrijf) {
            SqlConnection con = GetConnection();
            string query = "UPDATE bedrijf " +
                           "SET Naam = @naam, " +
                           "BTWNr = @btwnr, " +
                           "TeleNr = @telenr, " +
                           "Email = @email, " +
                           "Adres = @adres " +
                           "WHERE id = @id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@naam",SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@btwnr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@telenr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                    cmd.Parameters["@naam"].Value = bedrijf.Naam;
                    cmd.Parameters["@btwnr"].Value = bedrijf.BTW;
                    cmd.Parameters["@telenr"].Value = bedrijf.TelefoonNummer;
                    cmd.Parameters["@email"].Value = bedrijf.Email;
                    cmd.Parameters["@adres"].Value = bedrijf.Adres;
                    cmd.Parameters["@id"].Value = bedrijf.Id;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                BedrijfADOException exx = new BedrijfADOException($"BedrijfRepoADO: BewerkBedrijf {ex.Message}", ex);
                exx.Data.Add("bedrijf", bedrijf);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// geeft bedrijf object op basis van id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bedrijf object</returns>
        /// <exception cref="BedrijfADOException"></exception>
        public Bedrijf GeefBedrijf(uint id) {
            try {
                return GeefBedrijf(id, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"BedrijfRepoADO: GeefBedrijf {ex.Message}", ex);
            }
        }

        /// <summary>
        /// geeft bedrijf object op basis van naam
        /// </summary>
        /// <param name="bedrijfsnaam"></param>
        /// <returns>Bedrijf object</returns>
        /// <exception cref="BedrijfADOException"></exception>
        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            try {
                return GeefBedrijf(null, bedrijfsnaam);
            } catch (Exception ex) {
                throw new AfspraakADOException($"BedrijfRepoADO: GeefBedrijf {ex.Message}", ex);
            }
        }

        /// <summary>
        /// private geeft bedrijf object op basis van id of naam
        /// </summary>
        /// <param name="_bedrijfId"></param>
        /// <param name="_bedrijfnaam"></param>
        /// <returns>Bedrijf object</returns>
        /// <exception cref="BedrijfADOException"></exception>
        private Bedrijf GeefBedrijf(uint? _bedrijfId, string? _bedrijfnaam) {
            SqlConnection con = GetConnection();
            string query = "SELECT b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, " +
                           "wn.Id as WerknemerId, wn.ANaam as WerknemerAnaam, wn.VNaam as WerknemerVNaam, wn.Email as WerknemerMail, " +
                           "f.FunctieNaam " +
                           "FROM Bedrijf b " +
                           "JOIN WerknemerBedrijf wb ON(b.id = wb.BedrijfId)" +
                           "JOIN Werknemer wn ON(wn.id = wb.WerknemerBedrijf)" +
                           "JOIN Functie f ON(wn.FunctieId = f.Id)" +
                           "WHERE wb.status = 1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (_bedrijfId.HasValue) {
                        query += " b.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = _bedrijfId;
                    } else {                      
                        query += " b.Naam = @Naam";
                        cmd.Parameters.Add(new SqlParameter("@Naam", SqlDbType.VarChar));
                        cmd.Parameters["@Naam"].Value = _bedrijfnaam;
                    }
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    Bedrijf bedrijf = null;
                    while (reader.Read()) {
                        if (bedrijf is null) {
                            uint bedrijfId = (uint)reader["BedrijfId"];
                            string bedrijfNaam = (string)reader["BedrijfNaam"];
                            string bedrijfBTW = (string)reader["BedrijfBTW"];
                            string bedrijfTeleNr = (string)reader["BedrijfTeleNr"];
                            string bedrijfMail = (string)reader["BedrijfMail"];
                            string bedrijfAdres = (string)reader["BedrijfAdres"];
                            bedrijf = new Bedrijf(bedrijfId,bedrijfNaam,bedrijfBTW,bedrijfTeleNr,bedrijfMail,bedrijfAdres);
                        }
                        uint werknemerId = (uint)reader["WerknemerId"];
                        string werknemerVNaam = (string)reader["WerknemerVNaam"];
                        string werknemerAnaam = (string)reader["WerknemerAnaam"];
                        string werknemerMail = (string)reader["WerknemerMail"];
                        string functieNaam = (string)reader["FunctieNaam"];
                        bedrijf.VoegWerknemerToeInBedrijf(new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam, werknemerMail), functieNaam);
                    }
                    return bedrijf;
                }
            } catch (Exception ex) {
                BedrijfADOException exx = new BedrijfADOException($"BedrijfRepoADO: GetBedrijf {ex.Message}", ex);
                exx.Data.Add("bedrijfid", _bedrijfId);
                exx.Data.Add("bedrijfnaam", _bedrijfnaam);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// geeft lijst van bedrijf objecten
        /// </summary>
        /// <returns>Lijst Bedrijf object</returns>
        /// <exception cref="BedrijfADOException"></exception>
        public IReadOnlyList<Bedrijf> Geefbedrijven() {
            SqlConnection con = GetConnection();
            string query = "SELECT b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, " +
                           "wn.Id as WerknemerId, wn.ANaam as WerknemerAnaam, wn.VNaam as WerknemerVNaam, wn.Email as WerknemerMail, " +
                           "f.FunctieNaam " +
                           "FROM Bedrijf b " +
                           "JOIN Werknemerbedrijf wb ON(b.id = wb.BedrijfId) " +
                           "JOIN Werknemer wn ON(wn.id = wb.WerknemerId) " +
                           "JOIN Functie f ON(wb.FunctieId = f.Id) " +
                           "WHERE wb.status = 1 " +
                           "ORDER BY b.Id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    List<Bedrijf> bedrijven = new List<Bedrijf>();
                    Bedrijf bedrijf = null;
                    while (reader.Read()) {
                        if (bedrijf is null || bedrijf.Id != (uint)reader["BedrijfId"]) {
                            uint bedrijfId = (uint)reader["BedrijfId"];
                            string bedrijfNaam = (string)reader["BedrijfNaam"];
                            string bedrijfBTW = (string)reader["BedrijfBTW"];
                            string bedrijfTeleNr = (string)reader["BedrijfTeleNr"];
                            string bedrijfMail = (string)reader["BedrijfMail"];
                            string bedrijfAdres = (string)reader["BedrijfAdres"];
                            bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
                            bedrijven.Add(bedrijf);
                        }
                        uint werknemerId = (uint)reader["WerknemerId"];
                        string werknemerVNaam = (string)reader["WerknemerVNaam"];
                        string werknemerAnaam = (string)reader["WerknemerAnaam"];
                        string werknemerMail = (string)reader["WerknemerMail"];
                        string functieNaam = (string)reader["FunctieNaam"];
                        bedrijf.VoegWerknemerToeInBedrijf(new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam, werknemerMail), functieNaam);
                    }
                    return bedrijven;
                }
            } catch (Exception ex) {
                throw new BedrijfADOException($"BedrijfRepoADO: Geefbedrijven {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Veranderd status van bedrijf naar verwijderd
        /// </summary>
        /// <param name="bedrijfId"></param>
        /// <exception cref="BedrijfADOException"></exception>
        public void VerwijderBedrijf(uint bedrijfId) {
            try {
                VeranderStatusBedrijf(bedrijfId, 2);
            } catch (Exception ex) {
                throw new AfspraakADOException($"BedrijfRepoADO: VerwijderBedrijf {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Veranderd status van bedrijf
        /// </summary>
        /// <param name="bedrijfId"></param>
        /// <param name="statusId"></param>
        /// <exception cref="BedrijfADOException"></exception>
        private void VeranderStatusBedrijf(uint bedrijfId, int statusId) {
            //Wanneer bedrijf word verwijderd (status 2), medewerkers zijn dan ontslagen (status 2)
            SqlConnection con = GetConnection();
            string queryBedrijf = "UPDATE bedrijf " +
                                  "SET Status = @statusId " +
                                  "WHERE Id = @bedrijfid";
            SqlTransaction trans = con.BeginTransaction();
            try {
                using (SqlCommand cmdMedewerker = con.CreateCommand())
                using (SqlCommand cmdBedrijf = con.CreateCommand()) {
                    con.Open();
                    //Medewerker sectie
                    if (statusId == 2) {
                        string queryMedewerker = "UPDATE WerknemerBedrijf " +
                                                 "SET Status = @statusId " +
                                                 "WHERE BedrijfId = @bedrijfid";
                        cmdMedewerker.Parameters.Add(new SqlParameter("@bedrijfid", SqlDbType.BigInt));
                        cmdMedewerker.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
                        cmdMedewerker.Parameters["@bedrijfid"].Value = bedrijfId;
                        cmdMedewerker.Parameters["@statusId"].Value = statusId;
                        cmdMedewerker.CommandText = queryMedewerker;
                        cmdMedewerker.ExecuteNonQuery();
                    }
                    //Bedrijf Sectie
                    cmdBedrijf.CommandText = queryBedrijf;
                    cmdBedrijf.Parameters.Add(new SqlParameter("@bedrijfid", SqlDbType.BigInt));
                    cmdBedrijf.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
                    cmdBedrijf.Parameters["@bedrijfid"].Value = bedrijfId;
                    cmdBedrijf.Parameters["@statusId"].Value = statusId;
                    cmdBedrijf.ExecuteNonQuery();
                    trans.Commit();
                }
            } catch (Exception ex) {
                trans.Rollback();
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: VeranderStatusBedrijf {ex.Message}", ex);
                exx.Data.Add("bedrijfId", bedrijfId);
                exx.Data.Add("statusId", statusId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Voegt bedrijf toe aan db
        /// </summary>
        /// <param name="bedrijf"></param>
        /// <returns>Bedrijf object</returns>
        /// <exception cref="BedrijfADOException"></exception>
        public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
            SqlConnection con = GetConnection();
            string query = "INSERT INTO Bedrijf(Naam, BTWNr, TeleNr, Email, Adres) " +
                           "output INSERTED.ID " +
                           "VALUES(@naam,@btwNr,@TeleNr,@Email,@Adres)";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@btwNr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@TeleNr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@Adres", SqlDbType.VarChar));
                    cmd.Parameters["@naam"].Value = bedrijf.Naam;
                    cmd.Parameters["@btwNr"].Value = bedrijf.BTW;
                    cmd.Parameters["@TeleNr"].Value = bedrijf.TelefoonNummer;
                    cmd.Parameters["@Email"].Value = bedrijf.Email;
                    cmd.Parameters["@Adres"].Value = bedrijf.Adres;
                    uint i = (uint)cmd.ExecuteScalar();
                    bedrijf.ZetId(i);
                    return bedrijf;
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"BedrijfRepoADO: VoegBedrijfToe {ex.Message}", ex);
                exx.Data.Add("bedrijf", bedrijf);
                throw exx;
            } finally {
                con.Close();
            }
        }
    }
}
