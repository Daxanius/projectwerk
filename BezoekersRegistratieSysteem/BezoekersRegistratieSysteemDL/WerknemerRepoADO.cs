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
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM bedrijf " +
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
        public Werknemer GeefWerknemer(uint id) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string voornaam, string achternaam) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id) {
            throw new NotImplementedException();
        }

        public void VerwijderWerknemer(uint id) {
            throw new NotImplementedException();
        }

        public Werknemer VoegWerknemerToe(Werknemer werknemer) {
            throw new NotImplementedException();
        }

        public void WijzigWerknemer(Werknemer werknemer) {
            throw new NotImplementedException();
        }
    }
}
