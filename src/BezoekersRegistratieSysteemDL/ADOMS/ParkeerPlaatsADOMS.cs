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

namespace BezoekersRegistratieSysteemDL.ADOMS {
    internal class ParkeerPlaatsADOMS : IParkeerplaatsRepository {
        /// <summary>
		/// Private lokale variabele connectiestring
		/// </summary>
		private string _connectieString;

        /// <summary>
        /// ParkeerPlaatsADOMS constructor krijgt connectie string als parameter.
        /// </summary>
        /// <param name="connectieString">Connectie string database</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
        public ParkeerPlaatsADOMS(string connectieString) {
            _connectieString = connectieString;
        }

        /// <summary>
        /// Zet SQL connectie op met desbetreffende database adv de lokale variabele [_connectieString].
        /// </summary>
        /// <returns>SQL connectie</returns>
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectieString);
        }
        public bool BestaatNummerplaat(string nummerplaat) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM Parkingplaatsen " +
                           "WHERE NummerPlaat = @nummerplaat " +
                           "AND EindTIjd IS NULL";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.VarChar));
                    cmd.Parameters["@nummerplaat"].Value = nummerplaat;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                throw new ParkeerPlaatsADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        public void CheckNummerplaatIn(Parkeerplaats parkeerplaats) {
            SqlConnection con = GetConnection();
            string query = "INSERT INTO Parkingplaatsen(NummerPlaat, StartTijd, EindTijd, BedrijfId) " +
                           "VALUES(@NummerPlaat, @StartTijd, @EindTijd, @BedrijfId)";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@StartTijd", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@EindTijd", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.Int));
                    cmd.Parameters["@nummerplaat"].Value = parkeerplaats.Nummerplaat;
                    cmd.Parameters["@StartTijd"].Value = parkeerplaats.Starttijd;
                    cmd.Parameters["@EindTijd"].Value = parkeerplaats.Eindtijd.HasValue ? parkeerplaats.Eindtijd.Value : DBNull.Value;
                    cmd.Parameters["@BedrijfId"].Value = parkeerplaats.Bedrijf.Id;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new ParkeerPlaatsADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        public void CheckNummerplaatUit(string nummerplaat) {
            SqlConnection con = GetConnection();
            string query = "UPDATE Parkingplaatsen " +
                           "SET EindTijd = @EindTijd " +
                           "WHERE NummerPlaat = @nummerplaat";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@nummerplaat", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@EindTijd", SqlDbType.DateTime));
                    cmd.Parameters["@nummerplaat"].Value = nummerplaat;
                    cmd.Parameters["@EindTijd"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                throw new ParkeerPlaatsADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }

        public IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf) {
            SqlConnection con = GetConnection();
            string query = "SELECT pp.Nummerplaat " +
                           "FROM Parkingplaatsen pp " +
                           "WHERE 1=1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    if (bedrijf.Id != 0) {
                        query += " AND pp.BedrijfId = @BedrijfId";
                        cmd.Parameters.Add(new SqlParameter("@BedrijfId", SqlDbType.Int));
                        cmd.Parameters["@BedrijfId"].Value = bedrijf.Id;
                    } else {
                        query += " JOIN Bedrijf b ON(pp.BedrijfId = b.Id) " +
                                 "WHERE b.Naam = @Naam";
                        cmd.Parameters.Add(new SqlParameter("@Naam", SqlDbType.VarChar));
                        cmd.Parameters["@Naam"].Value = bedrijf.Naam;
                    }
                    IDataReader reader = cmd.ExecuteReader();
                    List<string> nummerplaten = new List<string>();
                    while (reader.Read()) {
                        nummerplaten.Add((string)reader["Nummerplaat"]);
                    }
                    return nummerplaten.AsReadOnly();
                }
            } catch (Exception ex) {
                throw new ParkeerPlaatsADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            } finally {
                con.Close();
            }
        }
    }
}