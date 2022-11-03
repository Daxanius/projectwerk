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

namespace BezoekersRegistratieSysteemDL.ADO {
    /// <summary>
    /// Repo ADO van bedrijven
    /// </summary>
    public class BedrijfRepoADO : IBedrijfRepository {
        private string _connectieString;
        /// <summary>
        /// Constructor, initialiseerd een BedrijfRepoADO klasse die een connectiestring met de DB accepteerd
        /// </summary>
        /// <param name="connectieString">Connectiestring met de DB</param>
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
        /// Kijkt of bedrijf bestaat op basis van bedrijf object
        /// </summary>
        /// <param name="bedrijf">Bedrijf object die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="BedrijfADOException">Faalt om te kijken of bedrijf bestaat</exception>
        public bool BestaatBedrijf(Bedrijf bedrijf) {
            try {
                return BestaatBedrijf(bedrijf, null, null);
            } catch (Exception ex) {
                throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kijkt of bedrijf bestaat op basis van bedrijf id
        /// </summary>
        /// <param name="bedrijfId">Id van bedrijf die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="BedrijfADOException">Faalt om te kijken of bedrijf bestaat</exception>
        public bool BestaatBedrijf(long bedrijfId) {
            try {
                return BestaatBedrijf(null, bedrijfId, null);
            } catch (Exception ex) {
                throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kijkt of bedrijf bestaat op basis van bedrijf naam
        /// </summary>
        /// <param name="bedrijfsnaam">Naam van bedrijf die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="BedrijfADOException">Faalt om te kijken of bedrijf bestaat</exception>
        public bool BestaatBedrijf(string bedrijfsnaam) {
            try {
                return BestaatBedrijf(null, null, bedrijfsnaam);
            } catch (Exception ex) {
                throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prive methode die kijkt of bedrijf in DB bestaat adh bedrijf object, id, naam
        /// </summary>
        /// <param name="bedrijf">Optioneel: bedrijf object die gecontroleerd moet worden</param>
        /// <param name="bedrijfId">Optioneel: Id van bedrijf die gecontroleerd moet worden</param>
        /// <param name="bedrijfsnaam">Optioneel: Naam van bedrijf die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="BedrijfADOException">Faalt om te kijken of bedrijf bestaat op basis van bedrijf object, id of naam</exception>
        private bool BestaatBedrijf(Bedrijf? bedrijf, long? bedrijfId, string? bedrijfsnaam) {
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
        /// Bewerkt de gegevens van een bedrijf
        /// </summary>
        /// <param name="bedrijf">Bedrijf object die moet gewijzigd worden in de DB</param>
        /// <exception cref="BedrijfADOException">Faalt om bedrijf te bewerken</exception>
        public void BewerkBedrijf(Bedrijf bedrijf) {
            SqlConnection con = GetConnection();
            string query = "UPDATE bedrijf " +
                           "SET Naam = @naam, " +
                           "BTWNr = @btwnr, " +
                           "TeleNr = @telenr, " +
                           "Email = @email, " +
                           "Adres = @adres, " +
                           "BTWChecked = @btwcheck " +
                           "WHERE id = @id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@btwnr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@telenr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@adres", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@btwcheck", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
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
        /// geeft bedrijf object op basis van id
        /// </summary>
        /// <param name="id">Id van gewenste bedrijf</param>
        /// <returns>Bedrijf object</returns>
        /// <exception cref="BedrijfADOException">Faalt om een bedrijf object weer te geven op basis van id</exception>
        public Bedrijf GeefBedrijf(long id) {
            try {
                return GeefBedrijf(id, null);
            } catch (Exception ex) {
                throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// geeft bedrijf object op basis van naam
        /// </summary>
        /// <param name="bedrijfsnaam">Naam van gewenste bedrijf</param>
        /// <returns>Bedrijf object</returns>
        /// <exception cref="BedrijfADOException">Faalt om een bedrijf object weer te geven op basis van naam</exception>
        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            try {
                return GeefBedrijf(null, bedrijfsnaam);
            } catch (Exception ex) {
                throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// prive methode die een bedrijf object op basis van id of naam geeft
        /// </summary>
        /// <param name="_bedrijfId">Optioneel: Gewenste id van bedrijf</param>
        /// <param name="_bedrijfnaam">Optioneel: Gewenste naam van bedrijf</param>
        /// <returns>Bedrijf object</returns>
        /// <exception cref="BedrijfADOException">Faalt om een bedrijf object weer te geven op basis van id of naam</exception>
        private Bedrijf GeefBedrijf(long? _bedrijfId, string? _bedrijfnaam) {
            SqlConnection con = GetConnection();
            string query = "SELECT b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, b.BTWChecked, " +
                           "wn.Id as WerknemerId, wn.ANaam as WerknemerAnaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEMail, " +
                           "f.FunctieNaam " +
                           "FROM Bedrijf b " +
                           "LEFT JOIN WerknemerBedrijf wb ON(b.id = wb.BedrijfId) AND wb.Status = 1 " +
                           "LEFT JOIN Werknemer wn ON(wn.id = wb.WerknemerId) " +
                           "LEFT JOIN Functie f ON(wb.FunctieId = f.Id) " +
                           "WHERE 1=1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (_bedrijfId.HasValue) {
                        query += " AND b.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = _bedrijfId;
                    } else {
                        query += " AND b.Naam = @Naam";
                        cmd.Parameters.Add(new SqlParameter("@Naam", SqlDbType.VarChar));
                        cmd.Parameters["@Naam"].Value = _bedrijfnaam;
                    }
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    Bedrijf bedrijf = null;
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
                            long werknemerId = (long)reader["WerknemerId"];
                            string werknemerVNaam = (string)reader["WerknemerVNaam"];
                            string werknemerAnaam = (string)reader["WerknemerAnaam"];
                            string werknemerMail = (string)reader["WerknemerEMail"];
                            string functieNaam = (string)reader["FunctieNaam"];
                            bedrijf.VoegWerknemerToeInBedrijf(new Werknemer(werknemerId, werknemerVNaam, werknemerAnaam), werknemerMail, functieNaam);
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
        /// geeft lijst van bedrijf objecten
        /// </summary>
        /// <returns>Lijst van bedrijf object</returns>
        /// <exception cref="BedrijfADOException">Faalt om een lijst van bedrijven op te roepen</exception>
        public IReadOnlyList<Bedrijf> GeefBedrijven() {
            SqlConnection con = GetConnection();
            string query = "SELECT b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr as BedrijfBTW, b.TeleNr as BedrijfTeleNr, b.Email as BedrijfMail, b.Adres as BedrijfAdres, b.BTWChecked, " +
                           "wn.Id as WerknemerId, wn.ANaam as WerknemerAnaam, wn.VNaam as WerknemerVNaam, wb.WerknemerEMail, " +
                           "f.FunctieNaam " +
                           "FROM Bedrijf b " +
                           "LEFT JOIN Werknemerbedrijf wb ON(b.id = wb.BedrijfId) AND wb.Status = 1 " +
                           "LEFT JOIN Werknemer wn ON(wn.id = wb.WerknemerId) " +
                           "LEFT JOIN Functie f ON(wb.FunctieId = f.Id) " +
                           "ORDER BY b.Naam, wn.id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
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
        /// Veranderd status van bedrijf naar verwijderd
        /// </summary>
        /// <param name="bedrijfId">Id van bedrijf dat verwijderd moet worden</param>
        /// <exception cref="BedrijfADOException">Faalt om status van bedrijf naar verwijderd te zetten</exception>
        public void VerwijderBedrijf(long bedrijfId) {
            try {
                VeranderStatusBedrijf(bedrijfId, 2);
            } catch (Exception ex) {
                throw new BedrijfADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prive methode die status van bedrijf veranderd
        /// </summary>
        /// <param name="bedrijfId">Id van gewenste bedrijf</param>
        /// <param name="statusId">Id van status die toegekend moet worden</param>
        /// <exception cref="BedrijfADOException">Faalt om status van bedrijf te veranderen</exception>
        private void VeranderStatusBedrijf(long bedrijfId, int statusId) {
            //Wanneer bedrijf word verwijderd (status 2), medewerkers zijn dan ontslagen (status 2)
            SqlConnection con = GetConnection();
            string queryBedrijf = "UPDATE bedrijf " +
                                  "SET Status = @statusId " +
                                  "WHERE Id = @bedrijfid";
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try {
                using (SqlCommand cmdMedewerker = con.CreateCommand())
                using (SqlCommand cmdBedrijf = con.CreateCommand()) {
                    //Medewerker sectie
                    if (statusId == 2) {
                        string queryMedewerker = "UPDATE WerknemerBedrijf " +
                                                 "SET Status = @statusId " +
                                                 "WHERE BedrijfId = @bedrijfid";
                        cmdMedewerker.Transaction = trans;
                        cmdMedewerker.Parameters.Add(new SqlParameter("@bedrijfid", SqlDbType.BigInt));
                        cmdMedewerker.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
                        cmdMedewerker.Parameters["@bedrijfid"].Value = bedrijfId;
                        cmdMedewerker.Parameters["@statusId"].Value = statusId;
                        cmdMedewerker.CommandText = queryMedewerker;
                        cmdMedewerker.ExecuteNonQuery();
                    }
                    //Bedrijf Sectie
                    cmdBedrijf.CommandText = queryBedrijf;
                    cmdBedrijf.Transaction = trans;
                    cmdBedrijf.Parameters.Add(new SqlParameter("@bedrijfid", SqlDbType.BigInt));
                    cmdBedrijf.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
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
        /// Voegt bedrijf toe aan db
        /// </summary>
        /// <param name="bedrijf">Bedrijf object die toegevoegd moet worden</param>
        /// <returns>Bedrijf object met id</returns>
        /// <exception cref="BedrijfADOException">Faalt om een bedrijf toe te voegen</exception>
        public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
            SqlConnection con = GetConnection();
            string query = "INSERT INTO Bedrijf(Naam, BTWNr, TeleNr, Email, Adres, BTWChecked) " +
                           "output INSERTED.ID " +
                           "VALUES(@naam,@btwNr,@TeleNr,@Email,@Adres,@BTWChecked)";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@naam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@btwNr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@TeleNr", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@Adres", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@BTWChecked", SqlDbType.Bit));
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
