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
    //CONTROLES OP STATUS OP BEPAALDE ZAKEN?
    /// <summary>
	/// ADO van afspraak
	/// </summary>
    public class AfspraakRepoADO : IAfspraakRepository {
        private string _connectieString;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectieString"></param>
        public AfspraakRepoADO(string connectieString) {
            _connectieString = connectieString;
        }

        /// <summary>
		/// Maakt connectie met databank
		/// </summary>
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectieString);
        }

        /// <summary>
		/// Beindig afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <exception cref="AfspraakADOException"></exception>
        public void BeeindigAfspraakBezoeker(uint afspraakId) {
            try {
                VeranderStatusAfspraak(afspraakId, 3);
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: BeeindigAfspraakBezoeker {ex.Message}", ex);
                exx.Data.Add("afspraakId", afspraakId);
                throw exx;
            }
        }
        /// <summary>
		/// Beindig afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <exception cref="AfspraakADOException"></exception>
        public void BeeindigAfspraakSysteem(uint afspraakId) {
            try {
                VeranderStatusAfspraak(afspraakId, 4);
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: BeeindigAfspraakSysteem {ex.Message}", ex);
                exx.Data.Add("afspraakId", afspraakId);
                throw exx;
            }
        }

        /// <summary>
        /// verwijder afspraak
        /// </summary>
        /// <param name="afspraakId"></param>
        /// <exception cref="AfspraakADOException"></exception>
        public void VerwijderAfspraak(uint afspraakId) {
            try {
                VeranderStatusAfspraak(afspraakId, 2);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: VerwijderAfspraak {ex.Message}", ex);
            }
        }

        /// <summary>
		/// Beindig afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <param name="statusId"></param>
		/// <exception cref="AfspraakADOException"></exception>
        private void VeranderStatusAfspraak(uint afspraakId, int statusId) {
            SqlConnection con = GetConnection();
            string query = "UPDATE Afspraak " +
                           "SET AfspraakStatusId = @statusId " +
                           "WHERE Id = @afspraakid";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@afspraakid", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
                    cmd.Parameters["@afspraakid"].Value = afspraakId;
                    cmd.Parameters["@statusId"].Value = statusId;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: VeranderStatusAfspraak {ex.Message}", ex);
                exx.Data.Add("afspraakId", afspraakId);
                exx.Data.Add("statusId", statusId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
		/// bewerk afspraak
		/// </summary>
		/// <param name="afspraak"></param>
		/// <exception cref="AfspraakADOException"></exception>
        public void BewerkAfspraak(Afspraak afspraak) {
            SqlConnection con = GetConnection();
            //SELECT WORD GEBRUIKT OM EEN ACCURATE STATUSID IN TE STELLEN
            string querySelect = "SELECT COUNT(*) " +
                                 "FROM Afspraak " +
                                 "WHERE Id = @afspraakid AND AfspraakStatusId = 1";

            string queryUpdate = "UPDATE Afspraak " +
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
                                  "BezoekerId = @bezoekerId, " +
                                  "AfspraakstatusId = @afspraakstatusId  " +
                                  "WHERE Id = @afspraakid";
            try {
                using (SqlCommand cmdSelect = con.CreateCommand())
                using (SqlCommand cmdUpdate = con.CreateCommand()) {
                    con.Open();
                    //Geeft de statusID van de afspraak die gevraagd werd
                    cmdSelect.CommandText = querySelect;
                    cmdSelect.Parameters.Add(new SqlParameter("@afspraakid", SqlDbType.BigInt));
                    cmdSelect.Parameters["@afspraakid"].Value = afspraak.Id;
                    int currentAfspraakStatusId = (int)cmdSelect.ExecuteScalar();
                    //Bewerkt de gevraagde afspraak
                    cmdUpdate.CommandText = queryUpdate;
                    cmdUpdate.Parameters.Add(new SqlParameter("@afspraakid", SqlDbType.BigInt));
                    cmdUpdate.Parameters.Add(new SqlParameter("@start", SqlDbType.DateTime));
                    cmdUpdate.Parameters.Add(new SqlParameter("@eind", SqlDbType.DateTime));
                    cmdUpdate.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                    cmdUpdate.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                    cmdUpdate.Parameters.Add(new SqlParameter("@bezoekerId", SqlDbType.BigInt));
                    cmdUpdate.Parameters.Add(new SqlParameter("@afspraakstatusId", SqlDbType.Int));
                    cmdUpdate.Parameters.Add(new SqlParameter("@functienaam", SqlDbType.VarChar));
                    cmdUpdate.Parameters["@afspraakid"].Value = afspraak.Id;
                    cmdUpdate.Parameters["@start"].Value = afspraak.Starttijd;
                    cmdUpdate.Parameters["@eind"].Value = afspraak.Eindtijd is not null ? afspraak.Eindtijd : DBNull.Value;
                    cmdUpdate.Parameters["@afspraakstatusId"].Value = afspraak.Eindtijd is not null && currentAfspraakStatusId == 1 ? 5 : afspraak.Eindtijd is not null && currentAfspraakStatusId != 1 ? currentAfspraakStatusId : 1;
                    //FUNCTIE GETBEDRIJF
                    var bedrijf = afspraak.Werknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First();
                    var functie = afspraak.Werknemer.GeefBedrijfEnFunctiesPerWerknemer().Values.First().First();
                    cmdUpdate.Parameters["@bedrijfId"].Value = bedrijf.Id;
                    cmdUpdate.Parameters["@functienaam"].Value = functie;
                    cmdUpdate.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
                    cmdUpdate.Parameters["@bezoekerId"].Value = afspraak.Bezoeker.Id;
                    cmdUpdate.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: BewerkAfspraak {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
		/// Geef afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
        /// <returns>Afspraak object</returns>
		/// <exception cref="AfspraakADOException"></exception>
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
                           "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, w.Email as WerknemerMail, " +
                           "f.FunctieNaam " +
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
                        string werknemerMail = (string)reader["WerknemerMail"];
                        //functie portie
                        string functieNaam = (string)reader["FuntieNaam"];
                        Werknemer werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerANaam, werknemerMail);
                        werknemer.VoegBedrijfEnFunctieToeAanWerknemer(new Bedrijf(bedrijfId,bedrijfNaam, bedrijfBTWNr, bedrijfTeleNr,bedrijfMail, bedrijfAdres),functieNaam);
                        afspraak = new Afspraak(afspraakId, start, eind, new Bezoeker(bezoekerId,bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer);
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

        /// <summary>
		/// Geef alle afspraken op datum
		/// </summary>
		/// <param name="datum"></param>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
            try {
                return GeefAlleAfspraken(null, datum);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: GeefAfsprakenPerDag {ex.Message}", ex);
            }
        }

        /// <summary>
		/// Geef alle afspraken op werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId) {
            try {
                return GeefAlleAfspraken(werknemerId, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: GeefAlleAfsprakenPerWerknemer {ex.Message}", ex);
            }
        }

        /// <summary>
		/// Geef alle afspraken op datum en werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="datum"></param>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum) {
            try {
                return GeefAlleAfspraken(werknemerId, datum);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: GeefAfsprakenPerWerknemerOpDag {ex.Message}", ex);
            }
        }

        /// <summary>
		/// Geef alle afspraken op combinatie datum of/en werknemer
		/// </summary>
		/// <param name="_werknemerId"></param>
		/// <param name="_datum"></param>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        private IReadOnlyList<Afspraak> GeefAlleAfspraken(uint? _werknemerId, DateTime? _datum) {
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
                           "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, w.Email as WerknemerMail, " +
                           "f.FunctieNaam " +
                           "FROM Afspraak a " +
                           "JOIN WerknemerBedrijf as wb ON(a.WerknemerBedrijfId = wb.Id) " +
                           "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
                           "JOIN Werknemer w ON(wb.WerknemerId = w.Id) " +
                           "JOIN bedrijf b ON(wb.BedrijfId = b.Id) " +
                           "JOIN Functie f ON(wb.FunctieId = f.Id) " +
                           "WHERE 1=1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (_werknemerId.HasValue) {
                        query += " AND w.id = @werknemerId";
                        cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                        cmd.Parameters["@werknemerId"].Value = _werknemerId.Value;
                    }
                    if (_datum.HasValue) {
                        query += " AND CONVERT(DATE, a.StartTijd) = @date";
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.Date));
                        cmd.Parameters["@date"].Value = _datum.Value.Date;
                    }
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    List<Afspraak> afspraken = new List<Afspraak>();
                    while (reader.Read()) {
                        //Afspraak portie
                        uint afspraakId = (uint)reader["AfspraakId"];
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
                        string werknemerMail = (string)reader["WerknemerMail"];
                        //functie portie
                        string functieNaam = (string)reader["FuntieNaam"];
                        Werknemer werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerANaam, werknemerMail);
                        werknemer.VoegBedrijfEnFunctieToeAanWerknemer(new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTWNr, bedrijfTeleNr, bedrijfMail, bedrijfAdres), functieNaam);
                        afspraken.Add(new Afspraak(afspraakId, start, eind, new Bezoeker(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer));
                    }
                    return afspraken.AsReadOnly();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: GeefAlleAfspraken {ex.Message}", ex);
                exx.Data.Add("datum", _datum);
                exx.Data.Add("werknemerId", _werknemerId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
		/// Geeft lijst van huidige Afspraken
		/// </summary>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
            try {
                return GeefHuidigeAfspraken(null, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: GeefHuidigeAfspraken {ex.Message}", ex);
            }
        }

        /// <summary>
		/// Geeft lijst van huidige Afspraken per bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId) {
            try {
                return GeefHuidigeAfspraken(bedrijfId, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: GeefHuidigeAfsprakenPerBedrijf {ex.Message}", ex);
            }
        }

        /// <summary>
		/// Geeft lijst van huidige Afspraken per werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint werknemerId) {
            try {
                return GeefHuidigeAfspraken(null, werknemerId);
            } catch (Exception ex) {
                throw new AfspraakADOException($"AfspraakRepoADO: GeefHuidigeAfsprakenPerWerknemer {ex.Message}", ex);
            }
        }

        /// <summary>
		/// PRIVATE: Geeft lijst van huidige Afspraken
		/// </summary>
		/// <param name="_bedrijfId"></param>
		/// <param name="_werknemerId"></param>
        /// <returns>ReadOnlyList Afspraak</returns>
		/// <exception cref="AfspraakADOException"></exception>
        private IReadOnlyList<Afspraak> GeefHuidigeAfspraken(uint? _bedrijfId, uint? _werknemerId) {
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
                           "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, w.Email as WerknemerMail, " +
                           "f.FunctieNaam " +
                           "FROM Afspraak a " +
                           "JOIN WerknemerBedrijf as wb ON(a.WerknemerBedrijfId = wb.Id) " +
                           "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
                           "JOIN Werknemer w ON(wb.WerknemerId = w.Id) " +
                           "JOIN bedrijf b ON(wb.BedrijfId = b.Id) " +
                           "JOIN Functie f ON(wb.FunctieId = f.Id) " +
                           "WHERE a.AfspraakStatusId = 1";            
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (_bedrijfId.HasValue) {
                        query += " AND b.id = @bedrijfId";
                        cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                        cmd.Parameters["@bedrijfId"].Value = _bedrijfId.Value;
                    } else if (_werknemerId.HasValue) {
                        query += " AND w.id = @werknemerId";
                        cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                        cmd.Parameters["@werknemerId"].Value = _werknemerId.Value;
                    }
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    List<Afspraak> afspraken = new List<Afspraak>();
                    while (reader.Read()) {
                        //Afspraak portie
                        uint afspraakId = (uint)reader["AfspraakId"];
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
                        string werknemerMail = (string)reader["WerknemerMail"];
                        //functie portie
                        string functieNaam = (string)reader["FuntieNaam"];
                        Werknemer werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerANaam, werknemerMail);
                        werknemer.VoegBedrijfEnFunctieToeAanWerknemer(new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTWNr, bedrijfTeleNr, bedrijfMail, bedrijfAdres), functieNaam);
                        afspraken.Add(new Afspraak(afspraakId, start, eind, new Bezoeker(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer));
                    }
                    return afspraken.AsReadOnly();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: GeefHuidigeAfspraken {ex.Message}", ex);
                exx.Data.Add("bedrijfId", _bedrijfId);
                exx.Data.Add("werknemerId", _werknemerId);
                throw exx;
            } finally {
                con.Close();
            }
        }


        /// <summary>
		/// Maakt afspraak aan in DB
		/// </summary>
		/// <param name="afspraak"></param>
        /// <returns>Afspraak object</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public Afspraak VoegAfspraakToe(Afspraak afspraak) {
            SqlConnection con = GetConnection();
            string queryBezoeker = "INSERT INTO Bezoeker(ANaam, VNaam, EMail, EigenBedrijf) " +
                                   "output INSERTED.ID " +
                                   "VALUES(@ANaam,@VNaam,@EMail,@EigenBedrijf)";

            string queryAfspraak = "INSERT INTO Afspraak(StartTijd, EindTijd, WerknemerbedrijfId, BezoekerId) " +
                                   "output INSERTED.ID " +
                                   "VALUES(@start,@eind,@werknemerId,@bezoekerId)";
            SqlTransaction trans = con.BeginTransaction();
            try {
                using (SqlCommand cmdBezoeker = con.CreateCommand())
                using (SqlCommand cmdAfspraak = con.CreateCommand()) {
                    con.Open();
                    //Bezoeker portie
                    cmdBezoeker.Transaction = trans;
                    cmdBezoeker.CommandText = queryBezoeker;
                    cmdBezoeker.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
                    cmdBezoeker.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
                    cmdBezoeker.Parameters.Add(new SqlParameter("@EMail", SqlDbType.VarChar));
                    cmdBezoeker.Parameters.Add(new SqlParameter("@EigenBedrijf", SqlDbType.VarChar));
                    cmdBezoeker.Parameters["@ANaam"].Value = afspraak.Bezoeker.Achternaam;
                    cmdBezoeker.Parameters["@VNaam"].Value = afspraak.Bezoeker.Voornaam;
                    cmdBezoeker.Parameters["@EMail"].Value = afspraak.Bezoeker.Email;
                    cmdBezoeker.Parameters["@EigenBedrijf"].Value = afspraak.Bezoeker.Bedrijf;
                    uint afspraakId = (uint)cmdBezoeker.ExecuteScalar();
                    //Afspraak portie
                    cmdAfspraak.Transaction = trans;
                    cmdAfspraak.CommandText = queryAfspraak;
                    cmdAfspraak.Parameters.Add(new SqlParameter("@start", SqlDbType.DateTime));
                    cmdAfspraak.Parameters.Add(new SqlParameter("@eind", SqlDbType.DateTime));
                    cmdAfspraak.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                    cmdAfspraak.Parameters.Add(new SqlParameter("@bezoekerId", SqlDbType.BigInt));
                    cmdAfspraak.Parameters["@start"].Value = afspraak.Starttijd;
                    cmdAfspraak.Parameters["@eind"].Value = afspraak.Eindtijd is not null ? afspraak.Eindtijd : DBNull.Value;
                    cmdAfspraak.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
                    cmdAfspraak.Parameters["@bezoekerId"].Value = afspraakId;
                    uint i = (uint)cmdAfspraak.ExecuteScalar();
                    afspraak.ZetId(i);
                    afspraak.Bezoeker.ZetId(afspraakId);
                    trans.Commit();
                    return afspraak;
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: VoegAfspraakToe {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                trans.Rollback();
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
		/// Kijkt of afspraak bestaat op basis van afspraak object
		/// </summary>
		/// <param name="afspraak"></param>
        /// <returns>bool</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public bool BestaatAfspraak(Afspraak afspraak) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM Afspraak a ";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (afspraak.Id != 0) {
                        query += "WHERE a.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = afspraak.Id;
                    } else {
                        //Maybe check with statusID rather than eindTijd
                        query += "WHERE a.BezoekerId = @bezoekerId AND a.eindTijd is null";
                        cmd.Parameters.Add(new SqlParameter("@bezoekerId", SqlDbType.BigInt));
                        cmd.Parameters["@bezoekerId"].Value = afspraak.Bezoeker.Id;
                    }
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: BestaatAfspraak object {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
		/// Kijkt of afspraak bestaat op basis van afspraak id
		/// </summary>
		/// <param name="afspraakid"></param>
        /// <returns>bool</returns>
		/// <exception cref="AfspraakADOException"></exception>
        public bool BestaatAfspraak(uint afspraakid) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM Afspraak a " +
                           "WHERE a.Id = @id";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    query += "WHERE a.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                    cmd.Parameters["@id"].Value = afspraakid;
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"AfspraakRepoADO: BestaatAfspraak id {ex.Message}", ex);
                exx.Data.Add("afspraakid", afspraakid);
                throw exx;
            } finally {
                con.Close();
            }
        }
    }
}
