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

        public bool BestaatBedrijf(uint bedrijfId) {
            try {
                return BestaatBedrijf(null, bedrijfId, null);
            } catch (Exception ex) {
                throw new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf {ex.Message}", ex);
            }
        }

        public bool BestaatBedrijf(string bedrijfsnaam) {
            try {
                return BestaatBedrijf(null, null, bedrijfsnaam);
            } catch (Exception ex) {
                throw new BedrijfADOException($"BedrijfRepoADO: BestaatBedrijf {ex.Message}", ex);
            }
        }

        private bool BestaatBedrijf(Bedrijf? bedrijf, uint? bedrijfId, string? bedrijfsnaam) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM bedrijf " +
                           "WHERE 1=1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    var sqltype = (bedrijf is not null && bedrijf.Id != 0) ? SqlDbType.BigInt : (bedrijfId.HasValue) ? SqlDbType.BigInt : SqlDbType.VarChar;
                    query += " AND bedrijfid = @querylookup";
                    cmd.Parameters.Add(new SqlParameter("@querylookup", sqltype));
                    cmd.Parameters["@querylookup"].Value = (bedrijf is not null && bedrijf.Id != 0) ? bedrijf.Id : (bedrijfId.HasValue) ? bedrijfId : bedrijfsnaam;
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
