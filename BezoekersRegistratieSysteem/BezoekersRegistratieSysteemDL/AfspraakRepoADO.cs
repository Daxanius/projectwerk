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
    public class AfspraakRepoADO : IAfspraakRepository {
        private string _connectieString;

        public AfspraakRepoADO(string connectieString) {
            _connectieString = connectieString;
        }
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectieString);
        }
        //ELKE QUERY MOET NOG GETEST WORDEN
        //CONTROLES OP STATUS OP BEPAALDE ZAKEN?
        public void BeeindigAfspraakBezoeker(uint id) {
            throw new NotImplementedException();
        }

        public void BeeindigAfspraakSysteem(uint id) {
            throw new NotImplementedException();
        }
        private void BeeindigAfspraak(uint afspraakId, int statusId) {
            SqlConnection con = GetConnection();
            //TEMP prob gonna get split, set this private and change 2 to @status
            string query = "UPDATE Afspraak " +
                           "SET AfspraakStatusId = 2 " +
                           "WHERE Id = @afspraakid";
            try {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand()) {
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@afspraakid", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@statusid", SqlDbType.Int));
                    cmd.Parameters["@afspraakid"].Value = afspraakId;
                    cmd.Parameters["@statusid"].Value = statusId;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: BeeindigAfspraak {ex.Message}", ex);
                exx.Data.Add("afspraakId", afspraakId);
                throw exx;
            } finally {
                con.Close();
            }
        }
        public void VerwijderAfspraak(uint afspraakId) {
            try {
                BeeindigAfspraak(afspraakId, 2);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: VerwijderAfspraak {ex.Message}", ex);
            }
        }
        public void BewerkAfspraak(Afspraak afspraak) {
            SqlConnection con = GetConnection();
            string query = "UPDATE Afspraak " +
                           "SET StartTijd = @start, " +
                           "EindTijd = @eind, " +
                           "WerknemerBedrijfId = (SELECT wb.Id " +
                                                 "FROM WerknemerBedrijf wb " +
                                                 "WHERE wb.BedrijfId = @bedrijfId AND " +
                                                 "wb.WerknemerId = @werknemerId AND " +
                                                 "wb.FunctieId = (SELECT f.Id " +
                                                                 "FROM Functie f " +
                                                                 "WHERE f.FunctieNaam = @functienaam " +
                                                                 ") " +
                                                 "), " +
                           "BezoekerId = @bezoekerId " +
                           "WHERE Id = @afspraakid";
            try {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand()) {
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@afspraakid", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@start", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@eind", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@bezoekerId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@functienaam", SqlDbType.VarChar));
                    cmd.Parameters["@afspraakid"].Value = afspraak.Id;
                    cmd.Parameters["@start"].Value = afspraak.Starttijd;
                    cmd.Parameters["@eind"].Value = afspraak.Eindtijd is not null ? afspraak.Eindtijd : DBNull.Value;
                    //FUNCTIE GETBEDRIJF
                    //cmd.Parameters["@bedrijfId"].Value = afspraak.Werknemer.Bedrijf.Id;
                    //cmd.Parameters["@functienaam"].Value = afspraak.Werknemer.Bedrijf.Naam;
                    cmd.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
                    cmd.Parameters["@bezoekerId"].Value = afspraak.Bezoeker.Id;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: BewerkAfspraak {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                throw exx;
            } finally {
                con.Close();
            }
        }

        public Afspraak GeefAfspraak(uint afspraakId) {
            SqlConnection con = GetConnection();
            /* INFO SELECT
             * Afspraak
             * Bezoeker
             * Bedrijf
             * Werknemer
             * Functie Medewerker
             */
            string query = "SELECT a.Id as AfspraakId, a.StartTijd, a.EindTijd, " +
                           "bz.Id as BezoekerId, bz.ANaam as BezoekerANaam, bz.VNaam as BezoekerVNaam, bz.Email as BezoekerMail, bz.EigenBedrijf as BezoekerBedrijf, " +
                           "b.Id as BedrijfId, b.Naam as BedrijfNaam, b.BTWNr, b.TeleNr, b.Email as BedrijfEmail, b.Adres as BedrijfAdres, " +
                           "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, " +
                           "f.FuntieNaam " +
                           "FROM Afspraak a " +
                           "JOIN WerknemerBedrijf as wb ON(a.WerknemerBedrijfId = wb.Id) " +
                           "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
                           "JOIN Werknemer w ON(wb.WerknemerId = w.Id) " +
                           "JOIN bedrijf b ON(wb.BedrijfId = b.Id) " +
                           "JOIN Functie f ON(wb.FunctieId = f.Id) " +
                           "WHERE a.Id = @afspraakid";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@afspraakid", SqlDbType.BigInt));
                    cmd.Parameters["@afspraakid"].Value = afspraakId;
                    IDataReader reader = cmd.ExecuteReader();
                    Afspraak afspraak = null;
                    while (reader.Read()) {
                        //Afspraak portie
                        DateTime start = (DateTime)reader["StartTijd"];
                        DateTime? eind = !reader.IsDBNull(reader.GetOrdinal("EindTijd")) ? (DateTime)reader["EindTijd"] : null;
                        //bezoeker portie
                        uint bezoekerId = (uint)reader["BezoekerId"];
                        string bezoekerAnaam = (string)reader["BezoekerANaam"];
                        string bezoekerVnaam = (string)reader["BezoekerVNaam"];
                        string bezoekerMail = (string)reader["BezoekerMail"];
                        string bezoekerBedrijf = (string)reader["BezoekerBedrijf"];
                        //bedrijf portie
                        uint bedrijfId = (uint)reader["BedrijfId"];
                        string bedrijfNaam = (string)reader["BedrijfNaam"];
                        string bedrijfBTWNr = (string)reader["BTWNr"];
                        string bedrijfTeleNr = (string)reader["TeleNr"];
                        string bedrijfMail = (string)reader["BedrijfEmail"];
                        string bedrijfAdres = (string)reader["BedrijfAdres"];
                        //werknemer portie
                        uint werknemerId = (uint)reader["WerknemerId"];
                        string werknemerANaam = (string)reader["WerknemerANaam"];
                        string werknemerVNaam = (string)reader["WerknemerVNaam"];
                        //functie portie
                        string functieNaam = (string)reader["FuntieNaam"];

                        //afspraak = new Afspraak();
                    }
                    return afspraak;
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: GeefAfspraak {ex.Message}", ex);
                exx.Data.Add("afspraakId", afspraakId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint werknemerId) {
            throw new NotImplementedException();
        }

        public void VoegAfspraakToe(Afspraak afspraak) {
            SqlConnection con = GetConnection();
            string query = "INSERT INTO Afspraak(StartTijd, EindTijd, WerknemerbedrijfId, BezoekerId) " +
                           "output INSERTED.ID " +
                           "VALUES(@start,@eind,@werknemerId,@bezoekerId)";
            try {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand()) {
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@start", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@eind", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@bezoekerId", SqlDbType.BigInt));
                    cmd.Parameters["@start"].Value = afspraak.Starttijd;
                    cmd.Parameters["@eind"].Value = afspraak.Eindtijd is not null ? afspraak.Eindtijd : DBNull.Value;
                    cmd.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
                    cmd.Parameters["@bezoekerId"].Value = afspraak.Bezoeker.Id;
                    uint i = (uint)cmd.ExecuteScalar();
                    afspraak.ZetId(i);
                    //return afspraak;
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: VoegAfspraakToe {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                throw exx;
            } finally {
                con.Close();
            }
        }

        public void BestaatAfspraak(Afspraak afspraak) {
            throw new NotImplementedException();
        }
    }
}
