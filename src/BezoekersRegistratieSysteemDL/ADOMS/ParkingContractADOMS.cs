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

namespace BezoekersRegistratieSysteemDL.ADOMS {
    public class ParkingContractADOMS : IParkingContractRepository {
        /// <summary>
        /// Private lokale variabele connectiestring
        /// </summary>
        private string _connectieString;

        /// <summary>
        /// ParkingContractADOMS constructor krijgt connectie string als parameter.
        /// </summary>
        /// <param name="connectieString">Connectie string database</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
        public ParkingContractADOMS(string connectieString) {
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
        /// Kijkt of een parkingcontract bestaat aan de hand van bedrijfId of BTWNr, start-, einddatum en aantalplaatsen
        /// </summary>
        /// <returns>True = bestaat | False = bestaat NIET</returns>
        public bool BestaatParkingContract(ParkingContract parkingContract) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM ParkingContract pc";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    if (parkingContract.Bedrijf.Id != 0) {
                        query += " WHERE pc.BedrijfId = @BedrijfId";
                        cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.BigInt));
                        cmd.Parameters["@BedrijfId"].Value = parkingContract.Bedrijf.Id;
                    } else {
                        query += " JOIN Bedrijf b ON(pc.bedrijfId = b.Id) " +
                                 "WHERE b.BTWNr = @BTWNr";
                        cmd.Parameters.Add(new SqlParameter("@BTWNr", SqlDbType.VarChar));
                        cmd.Parameters["@BTWNr"].Value = parkingContract.Bedrijf.BTW;
                    }
                    query += " AND pc.StartTijd = @StartTijd " +
                             "AND pc.EindTijd = @EindTijd " +
                             "AND pc.AantalPlaatsen = @AantalPlaatsen";
                    cmd.Parameters.Add(new SqlParameter("@StartTijd", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@EindTijd", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@AantalPlaatsen", SqlDbType.Int));
                    cmd.Parameters["@StartTijd"].Value = parkingContract.Starttijd.Date;
                    cmd.Parameters["@EindTijd"].Value = parkingContract.Eindtijd.Date;
                    cmd.Parameters["@AantalPlaatsen"].Value = parkingContract.AantalPlaatsen;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                throw new ParkingContractADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }
        /// <summary>
        /// Bewerkt parkingcontract: Start- EindTijd en aantal plaatsen op basis van id
        /// </summary>
        public void BewerkParkingContract(ParkingContract parkingContract) {
            SqlConnection con = GetConnection();
            string query = "UPDATE ParkingContract " +
                           "SET StartTijd = @startTijd, " +
                           "EindTijd = @EindTijd, " +
                           "AantalPlaatsen = @aantalPlaatsen " +
                           "WHERE Id = @id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@StartTijd", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@EindTijd", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@AantalPlaatsen", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                    cmd.Parameters["@StartTijd"].Value = parkingContract.Starttijd.Date;
                    cmd.Parameters["@EindTijd"].Value = parkingContract.Eindtijd.Date;
                    cmd.Parameters["@AantalPlaatsen"].Value = parkingContract.AantalPlaatsen;
                    cmd.Parameters["@id"].Value = parkingContract.Id;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new ParkingContractADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        public ParkingContract GeefParkingContract(long bedrijfId) {
            throw new NotImplementedException();
        }

        public void VerwijderParkingContract(ParkingContract parkingContract) {
            throw new NotImplementedException();
        }

        public void VoegParkingContractToe(ParkingContract parkingContract) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Kijkt of een bedrijf al een overlapende contract heeft
        /// </summary>
        /// <returns>True = bestaat | False = bestaat NIET</returns>
        public bool IsOverLappend(ParkingContract parkingContract) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM ParkingContract pc";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    if (parkingContract.Bedrijf.Id != 0) {
                        query += " WHERE pc.BedrijfId = @BedrijfId";
                        cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.BigInt));
                        cmd.Parameters["@BedrijfId"].Value = parkingContract.Bedrijf.Id;
                    } else {
                        query += " JOIN Bedrijf b ON(pc.bedrijfId = b.Id) " +
                                 "WHERE b.BTWNr = @BTWNr";
                        cmd.Parameters.Add(new SqlParameter("@BTWNr", SqlDbType.VarChar));
                        cmd.Parameters["@BTWNr"].Value = parkingContract.Bedrijf.BTW;
                    }
                    query += " AND (@startTijd BETWEEN pc.StartTijd AND pc.EindTijd OR @eindTijd BETWEEN pc.StartTijd AND pc.EindTijd)";
                    cmd.Parameters.Add(new SqlParameter("@StartTijd", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@EindTijd", SqlDbType.Date));
                    cmd.Parameters["@StartTijd"].Value = parkingContract.Starttijd.Date;
                    cmd.Parameters["@EindTijd"].Value = parkingContract.Eindtijd.Date;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                throw new ParkingContractADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }
    }
}
