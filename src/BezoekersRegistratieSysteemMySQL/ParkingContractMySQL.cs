using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.ADOMySQL {
    public class ParkingContractMySQL : IParkingContractRepository {
        /// <summary>
        /// Private lokale variabele connectiestring
        /// </summary>
        private string _connectieString;

        /// <summary>
        /// ParkingContractADOMS constructor krijgt connectie string als parameter.
        /// </summary>
        /// <param name="connectieString">Connectie string database</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
        public ParkingContractMySQL(string connectieString) {
            _connectieString = connectieString;
        }

        /// <summary>
        /// Zet SQL connectie op met desbetreffende database adv de lokale variabele [_connectieString].
        /// </summary>
        /// <returns>SQL connectie</returns>
        private MySqlConnection GetConnection() {
            return new MySqlConnection(_connectieString);
        }

        /// <summary>
        /// Kijkt of een parkingcontract bestaat aan de hand van bedrijfId of BTWNr, start-, einddatum en aantalplaatsen
        /// </summary>
        /// <returns>True = bestaat | False = bestaat NIET</returns>
        public bool BestaatParkingContract(ParkingContract parkingContract) {
            MySqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM ParkingContract pc";
            try {
                using (MySqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (parkingContract.Bedrijf.Id != 0) {
                        query += " WHERE pc.BedrijfId = @BedrijfId";
                        cmd.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
                        cmd.Parameters["@BedrijfId"].Value = parkingContract.Bedrijf.Id;
                    } else {
                        query += " JOIN Bedrijf b ON(pc.bedrijfId = b.Id) " +
                                 "WHERE b.BTWNr = @BTWNr";
                        cmd.Parameters.Add(new MySqlParameter("@BTWNr", MySqlDbType.VarChar));
                        cmd.Parameters["@BTWNr"].Value = parkingContract.Bedrijf.BTW;
                    }
                    query += " AND pc.StartTijd = @StartTijd " +
                             "AND pc.EindTijd = @EindTijd " +
                             "AND pc.AantalPlaatsen = @AantalPlaatsen";
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new MySqlParameter("@StartTijd", MySqlDbType.Date));
                    cmd.Parameters.Add(new MySqlParameter("@EindTijd", MySqlDbType.Date));
                    cmd.Parameters.Add(new MySqlParameter("@AantalPlaatsen", MySqlDbType.Int32));
                    cmd.Parameters["@StartTijd"].Value = parkingContract.Starttijd.Date;
                    cmd.Parameters["@EindTijd"].Value = parkingContract.Eindtijd.Date;
                    cmd.Parameters["@AantalPlaatsen"].Value = parkingContract.AantalPlaatsen;
                    long i = (long)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                throw new ParkingContractMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }
        /// <summary>
        /// Bewerkt parkingcontract: Start- EindTijd en aantal plaatsen op basis van id
        /// <paramref name="parkingContract">Parkingcontract die aangepast moet worden</paramref>
        /// </summary>
        public void BewerkParkingContract(ParkingContract parkingContract) {
            MySqlConnection con = GetConnection();
            string query = "UPDATE ParkingContract " +
                           "SET StartTijd = @startTijd, " +
                           "EindTijd = @EindTijd, " +
                           "AantalPlaatsen = @aantalPlaatsen " +
                           "WHERE Id = @id";
            try {
                using (MySqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new MySqlParameter("@StartTijd", MySqlDbType.Date));
                    cmd.Parameters.Add(new MySqlParameter("@EindTijd", MySqlDbType.Date));
                    cmd.Parameters.Add(new MySqlParameter("@AantalPlaatsen", MySqlDbType.Int32));
                    cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
                    cmd.Parameters["@StartTijd"].Value = parkingContract.Starttijd.Date;
                    cmd.Parameters["@EindTijd"].Value = parkingContract.Eindtijd.Date;
                    cmd.Parameters["@AantalPlaatsen"].Value = parkingContract.AantalPlaatsen;
                    cmd.Parameters["@id"].Value = parkingContract.Id;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new ParkingContractMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Geeft laatste parkingcontract van een bedrijf
        /// </summary>
        /// <param name="bedrijfId">bedrijf wiens parkingcontract weergegeven wordt</param>
        /// <returns>True = bestaat | False = bestaat NIET</returns>
        public ParkingContract GeefParkingContract(long bedrijfId) {
            MySqlConnection con = GetConnection();
            string query = "SELECT pc.Id, pc.StartTijd, pc.Eindtijd, pc.AantalPlaatsen, " +
                           "b.Id As BedrijfId, b.Naam, b.BTWNr, b.TeleNR, b.Email, b.Adres, b.BTWChecked" +
                           "FROM ParkingContract pc " +
                           "JOIN bedrijf b ON(pc.bedrijfId = b.Id) " +
                           "WHERE (@vandaagDatum BETWEEN pc.StartTijd AND pc.EindTijd) AND pc.bedrijfId = @bedrijfId";
            try {
                using (MySqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new MySqlParameter("@vandaagDatum", MySqlDbType.Date));
                    cmd.Parameters.Add(new MySqlParameter("@bedrijfId", MySqlDbType.Int64));
                    cmd.Parameters["@StartTijd"].Value = DateTime.Today;
                    cmd.Parameters["@bedrijfId"].Value = bedrijfId;
                    IDataReader reader = cmd.ExecuteReader();
                    ParkingContract contract = null;
                    while (reader.Read()) {
                        //Contract gedeelte
                        long contractId = (long)reader["Id"];
                        DateTime contractStart = (DateTime)reader["StartTijd"];
                        DateTime contractEind = (DateTime)reader["Eindtijd"];
                        int contractPlaatsen = (int)reader["AantalPlaatsen"];
                        //Bedrijf gedeelte
                        string bedrijfNaam = (string)reader["Naam"];
                        string bedrijfBTW = (string)reader["BTWNr"];
                        string bedrijfTele = (string)reader["TeleNR"];
                        string bedrijfMail = (string)reader["Email"];
                        string bedrijfAdres = (string)reader["Adres"];
                        bool bedrijfBTWChecked = (bool)reader["BTWChecked"];
                        Bedrijf bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTW, bedrijfBTWChecked, bedrijfTele, bedrijfMail, bedrijfAdres);
                        contract = new ParkingContract(contractId, bedrijf, contractStart, contractEind, contractPlaatsen);
                    }
                    return contract;
                }
            } catch (Exception ex) {
                throw new ParkingContractMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Bewerkt status van parkingcotnract
        /// <paramref name="parkingContract">Parkingcontract die aangepast/verwijderd moet worden</paramref>
        /// </summary>
        public void VerwijderParkingContract(ParkingContract parkingContract) {
            MySqlConnection con = GetConnection();
            string query = "UPDATE ParkingContract " +
                           "SET Statusid = 2 " +
                           "WHERE Id = @id";
            try {
                using (MySqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int64));
                    cmd.Parameters["@id"].Value = parkingContract.Id;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new ParkingContractMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }
        /// <summary>
        /// Voegt een parkingcontract toe
        /// <paramref name="parkingContract">Parkingcontract die toegevoegd moet worden</paramref>
        /// </summary>
        public void VoegParkingContractToe(ParkingContract parkingContract) {
            MySqlConnection con = GetConnection();
            string queryInsert = "INSERT INTO ParkingContract(StartTijd, EindTijd, BedrijfId, AantalPlaatsen) " +
                           "VALUES(@StartTijd, @EindTijd, @BedrijfId, @AantalPlaatsen);";
            string querySelect = "SELECT id FROM ParkingContract WHERE id = LAST_INSERT_ID();";
            try {
                con.Open();
                MySqlTransaction trans = con.BeginTransaction();
                using (MySqlCommand cmdSelect = con.CreateCommand())
                using (MySqlCommand cmdInsert = con.CreateCommand()) {
                    cmdInsert.Transaction = trans;
                    cmdInsert.CommandText = queryInsert;
                    cmdInsert.Parameters.Add(new MySqlParameter("@StartTijd", MySqlDbType.Date));
                    cmdInsert.Parameters.Add(new MySqlParameter("@EindTijd", MySqlDbType.Date));
                    cmdInsert.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
                    cmdInsert.Parameters.Add(new MySqlParameter("@AantalPlaatsen", MySqlDbType.Int32));
                    cmdInsert.Parameters["@StartTijd"].Value = parkingContract.Starttijd.Date;
                    cmdInsert.Parameters["@EindTijd"].Value = parkingContract.Eindtijd.Date;
                    cmdInsert.Parameters["@BedrijfId"].Value = parkingContract.Bedrijf.Id;
                    cmdInsert.Parameters["@AantalPlaatsen"].Value = parkingContract.AantalPlaatsen;
                    cmdInsert.ExecuteNonQuery();

                    cmdSelect.Transaction = trans;
                    cmdSelect.CommandText = querySelect;
                    long i = (long)cmdSelect.ExecuteScalar();
                    parkingContract.ZetId(i);
                }
            } catch (Exception ex) {
                throw new ParkingContractMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Kijkt of een bedrijf al een overlapende contract heeft
        /// </summary>
        /// <param name="parkingContract">ParkingContract die gecontroleerd moet worden</param>
        /// <returns>True = bestaat | False = bestaat NIET</returns>
        public bool IsOverLappend(ParkingContract parkingContract) {
            MySqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM ParkingContract pc";
            try {
                using (MySqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (parkingContract.Bedrijf.Id != 0) {
                        query += " WHERE pc.BedrijfId = @BedrijfId";
                        cmd.Parameters.Add(new MySqlParameter("@BedrijfId", MySqlDbType.Int64));
                        cmd.Parameters["@BedrijfId"].Value = parkingContract.Bedrijf.Id;
                    } else {
                        query += " JOIN Bedrijf b ON(pc.bedrijfId = b.Id) " +
                                 "WHERE b.BTWNr = @BTWNr";
                        cmd.Parameters.Add(new MySqlParameter("@BTWNr", MySqlDbType.VarChar));
                        cmd.Parameters["@BTWNr"].Value = parkingContract.Bedrijf.BTW;
                    }
                    query += " AND ((@startTijd BETWEEN pc.StartTijd AND pc.EindTijd) OR " +
                             "(@eindTijd BETWEEN pc.StartTijd AND pc.EindTijd) OR " +
                             "(@startTijd < pc.StartTijd AND @eindTijd > pc.EindTijd) OR " +
                             "(@startTijd > pc.StartTijd AND @eindTijd < pc.EindTijd))";
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new MySqlParameter("@startTijd", MySqlDbType.Date));
                    cmd.Parameters.Add(new MySqlParameter("@eindTijd", MySqlDbType.Date));
                    cmd.Parameters["@startTijd"].Value = parkingContract.Starttijd.Date;
                    cmd.Parameters["@eindTijd"].Value = parkingContract.Eindtijd.Date;
                    long i = (long)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                throw new ParkingContractMySQLException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }
    }
}
