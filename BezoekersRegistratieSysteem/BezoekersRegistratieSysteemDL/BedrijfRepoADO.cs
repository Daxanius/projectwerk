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
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM bedrijf " +
                           "WHERE 1=1";
            try {              
                con.Open();
                using (SqlCommand cmd = con.CreateCommand()) {
                    if (bedrijf.Id != 0) {
                        query += " AND bedrijfid = @bedrijfid";
                        cmd.Parameters.Add(new SqlParameter("@bedrijfid", SqlDbType.BigInt));
                        cmd.Parameters["@bedrijfid"].Value = bedrijf.Id;
                    } else {
                        query += " AND BTWNr = @BTWNr";
                        cmd.Parameters.Add(new SqlParameter("@BTWNr", SqlDbType.VarChar));
                        cmd.Parameters["@BTWNr"].Value = bedrijf.BTW;
                    }
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                BedrijfADOException exx = new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf object {ex.Message}", ex);
                exx.Data.Add("bedrijf", bedrijf);
                throw exx;
            } finally {
                con.Close();
            }
        }

        public bool BestaatBedrijf(uint bedrijfId) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM bedrijf " +
                           "WHERE bedrijfid = @bedrijfid";
            try {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand()) {
                    cmd.Parameters.Add(new SqlParameter("@bedrijfid", SqlDbType.BigInt));
                    cmd.Parameters["@bedrijfid"].Value = bedrijfId;
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                BedrijfADOException exx = new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf id {ex.Message}", ex);
                exx.Data.Add("bedrijfId", bedrijfId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        public bool BestaatBedrijf(string bedrijfsnaam) {
            throw new NotImplementedException();
        }

        public void BewerkBedrijf(Bedrijf bedrijf) {
            throw new NotImplementedException();
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

        public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
            throw new NotImplementedException();
        }
    }
}
