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

namespace BezoekersRegistratieSysteemDL {
    /// <summary>
	/// ADO van werknemer
	/// </summary>
    public class WerknemerRepoADO : IWerknemerRepository {
        private string _connectieString;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectieString"></param>
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
        /// Private methode die kijkt of werknemer in DB bestaat adh werknemer object
        /// </summary>
        /// <param name="werknemer"></param>
        /// <returns>bool</returns>
        /// <exception cref="WerknemerADOException"></exception>
        public bool BestaatWerknemer(Werknemer werknemer) {
            try {
                return BestaatWerknemer(werknemer, null);
            } catch (Exception ex) {
                throw new WerknemerADOException($"WerknemerRepoADO: BestaatWerknemer {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Private methode die kijkt of werknemer in DB bestaat adh werknemer id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        /// <exception cref="WerknemerADOException"></exception>
        public bool BestaatWerknemer(uint id) {
            try {
                return BestaatWerknemer(null, id);
            } catch (Exception ex) {
                throw new WerknemerADOException($"WerknemerRepoADO: BestaatWerknemer {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Private methode die kijkt of werknemer in DB bestaat adh werknemer object, id
        /// </summary>
        /// <param name="werknemer"></param>
        /// <param name="werknemerId"></param>
        /// <returns>bool</returns>
        /// <exception cref="WerknemerADOException"></exception>
        private bool BestaatWerknemer(Werknemer? werknemer, uint? werknemerId) {
            //TODO eh oh
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM Werknemer " +
                           "WHERE 1=1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    var sqltype = (werknemer is not null && werknemer.Id != 0) ? SqlDbType.BigInt : (werknemerId.HasValue) ? SqlDbType.BigInt : SqlDbType.VarChar;
                    cmd.Parameters.Add(new SqlParameter("@querylookup", sqltype));
                    string databaseWhere = (werknemer is not null) ? (werknemer.Id != 0) || (werknemerId.HasValue) ? "id" : "email" : "id";
                    query += $" AND {databaseWhere} = @querylookup";
                    cmd.Parameters["@querylookup"].Value = (werknemer is not null) ? (werknemer.Id != 0) ? werknemer.Id : werknemer.Email : werknemerId;
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
        /// <param name="_werknemerId"></param>
        /// <returns>Werknemer object</returns>
        /// <exception cref="WerknemerADOException"></exception>
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

        public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string voornaam, string achternaam) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint bedrijfId) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// verwijder werknemer
        /// </summary>
        /// <param name="werknemerId"></param>
        /// <exception cref="WerknemerADOException"></exception>
        public void VerwijderWerknemer(uint werknemerId) {
            try {
                VeranderStatusWerknemer(werknemerId, 2);
            } catch (Exception ex) {
                throw new WerknemerADOException($"WerknemerRepoADO: VerwijderWerknemer {ex.Message}", ex);
            }
        }

        /// <summary>
        /// verander status werknemer
        /// </summary>
        /// <param name="werknemerId"></param>
        /// <param name="statusId"></param>
        /// <exception cref="WerknemerADOException"></exception>
        private void VeranderStatusWerknemer(uint werknemerId, int statusId) {
            //TODO PROB THROW IT IN VERWIJDERWERKNEMER AND ADD THE NEC ITEMS LIKE BEDRIJF ID AND FUNCTION NAME
            SqlConnection con = GetConnection();
            string query = "UPDATE Werknemerbedrijf " +
                           "SET Status = @statusId " +
                           "WHERE BedrijfId = @bedrijfId " +
                           "AND FunctieId = (SELECT Id FROM FUNCTIE WHERE FunctieNaam = @FunctieNaam) " +
                           "AND WerknemerId = @werknemerId " +
                           "AND Status = 1";
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
        /// <param name="werknemer"></param>
        /// <exception cref="WerknemerADOException"></exception>
        public Werknemer VoegWerknemerToe(Werknemer werknemer) {
            SqlConnection con = GetConnection();
            string query = "INSERT INTO Werknemer (VNaam, ANaam, Email)";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                    cmd.Parameters["@VNaam"].Value = werknemer.Voornaam;
                    cmd.Parameters["@ANaam"].Value = werknemer.Achternaam;
                    cmd.Parameters["@Email"].Value = werknemer.Email;
                    uint i = (uint)cmd.ExecuteScalar();
                    werknemer.ZetId(i);
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
        /// wijzigd werknemer
        /// </summary>
        /// <param name="werknemer"></param>
        /// <exception cref="WerknemerADOException"></exception>
        public void WijzigWerknemer(Werknemer werknemer) {
            SqlConnection con = GetConnection();
            string query = "UPDATE Werknemer " +
                           "SET VNaam = @Vnaam, " +
                           "ANaam = @ANaam, " +
                           "EMail = @Email " +
                           "WHERE Id = @Id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                    cmd.Parameters["@Id"].Value = werknemer.Id;
                    cmd.Parameters["@VNaam"].Value = werknemer.Voornaam;
                    cmd.Parameters["@ANaam"].Value = werknemer.Achternaam;
                    cmd.Parameters["@Email"].Value = werknemer.Email;
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
