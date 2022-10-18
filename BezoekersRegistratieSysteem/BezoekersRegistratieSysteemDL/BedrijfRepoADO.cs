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

        public Bedrijf GeefBedrijf(uint id) {
            throw new NotImplementedException();
        }

        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Bedrijf> Geefbedrijven() {
            throw new NotImplementedException();
        }

        public void VerwijderBedrijf(uint id) {
            throw new NotImplementedException();
        }
        private void VeranderStatusBedrijf(uint bedrijfId, int statusId) { 
            
        }
        public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
            throw new NotImplementedException();
        }
    }
}
