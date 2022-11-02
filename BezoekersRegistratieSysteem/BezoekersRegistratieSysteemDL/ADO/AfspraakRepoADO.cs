using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.ADO {
    //CONTROLES OP STATUS OP BEPAALDE ZAKEN?
    /// <summary>
    /// Repo ADO van afspraken
    /// </summary>
    public class AfspraakRepoADO : IAfspraakRepository {
        private string _connectieString;

        /// <summary>
        /// Constructor, initialiseerd een AfspraakRepoADO klasse die een connectiestring met de DB accepteerd
        /// </summary>
        /// <param name="connectieString">Connectiestring met de DB</param>
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
        /// Beindigd afspraak via de bezoeker zijn kant
        /// </summary>
        /// <param name="email">mail bezoeker van afspraak die beeindigd moet worden</param>
        /// <exception cref="AfspraakADOException">Faalt om afspraak te beeindigen</exception>
        public void BeeindigAfspraakOpEmail(string email) {
            try {
                BeeindigAfspraak(email, null, 3);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Beindigd afspraak via de bezoeker zijn kant
        /// </summary>
        /// <param name="afspraakId">Id van afspraak die beeindigd moet worden</param>
        /// <exception cref="AfspraakADOException">Faalt om afspraak te beeindigen</exception>
        public void BeeindigAfspraakBezoeker(long afspraakId) {
            try {
                BeeindigAfspraak(null, afspraakId, 3);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Beindigd afspraak via het systeem zijn kant
        /// </summary>
        /// <param name="afspraakId">Id van afspraak die beeindigd moet worden</param>
        /// <exception cref="AfspraakADOException">Faalt om afspraak te beeindigen</exception>
        public void BeeindigAfspraakSysteem(long afspraakId) {
            try {
                BeeindigAfspraak(null, afspraakId, 4);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prive methode die de status van een afspraak wijzigd naar beeindigd en eindtijd insteld
        /// </summary>
        /// <param name="afspraakId">Id van afspraak die gewijzigd moet worden</param>
        /// <param name="statusId">Id van status die toegekend moet worden</param>
        /// <exception cref="AfspraakADOException">Faalt om status van een afspraak te wijzigen</exception>
        private void BeeindigAfspraak(string? bezoekerMail, long? afspraakId, int statusId) {
            SqlConnection con = GetConnection();
            string query = "UPDATE Afspraak " +
                           "SET AfspraakStatusId = @statusId, " +
                           "EindTijd = @Eindtijd " +
                           "WHERE AfspraakStatusId = 1";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (!String.IsNullOrWhiteSpace(bezoekerMail)) {
                        query += " AND BezoekerId = (SELECT Id FROM Bezoeker WHERE Email = @mail)";
                        cmd.Parameters.Add(new SqlParameter("@mail", SqlDbType.VarChar));
                        cmd.Parameters["@mail"].Value = bezoekerMail;
                    } else {
                        query += " AND id = @afspraakId";
                        cmd.Parameters.Add(new SqlParameter("@afspraakId", SqlDbType.BigInt));
                        cmd.Parameters["@afspraakId"].Value = afspraakId.Value;
                    }
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Eindtijd", SqlDbType.DateTime));
                    cmd.Parameters["@statusId"].Value = statusId;
                    cmd.Parameters["@Eindtijd"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("mail", bezoekerMail);
                exx.Data.Add("afspraakId", afspraakId);
                exx.Data.Add("statusId", statusId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// verwijderd afspraak
        /// </summary>
        /// <param name="afspraakId">Id van afspraak die verwijderd moet worden</param>
        /// <exception cref="AfspraakADOException">Faalt om afspraak te verwijderen</exception>
        public void VerwijderAfspraak(long afspraakId) {
            try {
                VeranderStatusAfspraak(afspraakId, 2);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prive methode die de status van een afspraak wijzigd
        /// </summary>
        /// <param name="afspraakId">Id van afspraak die gewijzigd moet worden</param>
        /// <param name="statusId">Id van status die toegekend moet worden</param>
        /// <exception cref="AfspraakADOException">Faalt om status van een afspraak te wijzigen</exception>
        private void VeranderStatusAfspraak(long afspraakId, int statusId) {
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
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("afspraakId", afspraakId);
                exx.Data.Add("statusId", statusId);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// Kijkt of afspraak bestaat op basis van afspraak object
        /// </summary>
        /// <param name="afspraak">Afspraak object die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="AfspraakADOException">Faalt om te kijken of een afspraak bestaat op basis van afspraak object</exception>
        public bool BestaatAfspraak(Afspraak afspraak) {
            try {
                return BestaatAfspraak(afspraak, null, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} object {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kijkt of afspraak bestaat op basis van afspraak id
        /// </summary>
        /// <param name="afspraakid">Afspraak id die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="AfspraakADOException">Faalt om te kijken of een afspraak bestaat op basis van afspraak object</exception>
        public bool BestaatAfspraak(long afspraakid) {
            try {
                return BestaatAfspraak(null, afspraakid, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} id {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kijkt of lopende afspraak bestaat op basis van afspraak object
        /// </summary>
        /// <param name="afspraak">Afspraak object die gecontroleerd moet worden</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="AfspraakADOException">Faalt om te kijken of een afspraak bestaat op basis van afspraak object</exception>
        public bool BestaatLopendeAfspraak(Afspraak afspraak) {
            try {
                return BestaatAfspraak(afspraak, null, 1);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} object {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kijkt of lopende afspraak bestaat op basis van afspraak object of id
        /// </summary>
        /// <param name="afspraak">Optioneel: Afspraak object die gecontroleerd moet worden</param>
        /// <param name="afspraakid">Optioneel: Afspraak id die gecontroleerd moet worden</param>
        /// <param name="afspraakStatus">Optioneel: Status</param>
        /// <returns>bool (True = bestaat)</returns>
        /// <exception cref="AfspraakADOException">Faalt om te kijken of een afspraak bestaat op basis van afspraak object</exception>
        private bool BestaatAfspraak(Afspraak afspraak, long? afspraakid, int? afspraakStatus) {
            SqlConnection con = GetConnection();
            string query = "SELECT COUNT(*) " +
                           "FROM Afspraak a ";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    if (afspraak is not null) {
                        if (afspraak.Id != 0) {
                            query += "WHERE a.Id = @id";
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                            cmd.Parameters["@id"].Value = afspraak.Id;
                        } else {
                            //TODO: Maybe check with statusID rather than eindTijd
                            query += "JOIN Bezoeker bz ON(a.BezoekerId = bz.Id) " +
                                     "WHERE bz.Email = @bmail AND a.eindTijd is null";
                            cmd.Parameters.Add(new SqlParameter("@bmail", SqlDbType.VarChar));
                            cmd.Parameters["@bmail"].Value = afspraak.Bezoeker.Email;
                        }
                    } else {
                        query += "WHERE a.Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = afspraakid.Value;
                    }
                    if (afspraakStatus.HasValue) {
                        query += " AND a.AfspraakStatusId = @statusId";
                        cmd.Parameters.Add(new SqlParameter("@statusId", SqlDbType.Int));
                        cmd.Parameters["@statusId"].Value = afspraakStatus.Value;
                    }
                    cmd.CommandText = query;
                    int i = (int)cmd.ExecuteScalar();
                    return (i > 0);
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                throw exx;
            } finally {
                con.Close();
            }
        }

        /// <summary>
        /// bewerkt de gegevens van een afspraak
        /// </summary>
        /// <param name="afspraak">Afspraak object die moet gewijzigd worden in de DB</param>
        /// <exception cref="AfspraakADOException">Faalt om afspraak te wijzigen</exception>
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
                    //TODO: GWILOM prob gets replaced by werknemerInfo
                    var bedrijf = afspraak.Werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Keys.First();
                    var functie = afspraak.Werknemer.GeefBedrijvenEnFunctiesPerWerknemer().Values.First().GeefWerknemerFuncties().First();
                    cmdUpdate.Parameters["@bedrijfId"].Value = bedrijf.Id;
                    cmdUpdate.Parameters["@functienaam"].Value = functie;
                    cmdUpdate.Parameters["@werknemerId"].Value = afspraak.Werknemer.Id;
                    cmdUpdate.Parameters["@bezoekerId"].Value = afspraak.Bezoeker.Id;
                    cmdUpdate.ExecuteNonQuery();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                throw exx;
            } finally {
                con.Close();
            }
        }
        #region GeefAfspraak
        /// <summary>
        /// Geeft een afspraak op basis van een id
        /// </summary>
        /// <param name="afspraakId">Id van het gewenste afspraak</param>
        /// <returns>Gewenste afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraak object weer te geven</exception>
        public Afspraak GeefAfspraak(long afspraakId) {
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
                           "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, wb.WerknemerEmail, " +
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
                        long bezoekerId = (long)reader["BezoekerId"];
                        string bezoekerAnaam = (string)reader["BezoekerANaam"];
                        string bezoekerVnaam = (string)reader["BezoekerVNaam"];
                        string bezoekerMail = (string)reader["BezoekerMail"];
                        string bezoekerBedrijf = (string)reader["BezoekerBedrijf"];
                        //bedrijf portie
                        long bedrijfId = (long)reader["BedrijfId"];
                        string bedrijfNaam = (string)reader["BedrijfNaam"];
                        string bedrijfBTWNr = (string)reader["BTWNr"];
                        string bedrijfTeleNr = (string)reader["TeleNr"];
                        string bedrijfMail = (string)reader["BedrijfEmail"];
                        string bedrijfAdres = (string)reader["BedrijfAdres"];
                        //werknemer portie
                        long werknemerId = (long)reader["WerknemerId"];
                        string werknemerANaam = (string)reader["WerknemerANaam"];
                        string werknemerVNaam = (string)reader["WerknemerVNaam"];
                        string werknemerMail = (string)reader["WerknemerEmail"];
                        //functie portie
                        string functieNaam = (string)reader["FunctieNaam"];
                        Werknemer werknemer = new(werknemerId, werknemerVNaam, werknemerANaam);
                        Bedrijf bedrijf = new(bedrijfId, bedrijfNaam, bedrijfBTWNr, true, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
                        werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
                        afspraak = new Afspraak(afspraakId, start, eind, bedrijf, new(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer);
                    }
                    return afspraak;
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("afspraakId", afspraakId);
                throw exx;
            } finally {
                con.Close();
            }
        }
        #endregion
        #region VoegAfspraakToe
        /// <summary>
        /// Maakt afspraak aan via een afspraak object
        /// </summary>
        /// <param name="afspraak">Afspraak object die toegevoegd moet worden</param>
        /// <returns>Afspraak object met id</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public Afspraak VoegAfspraakToe(Afspraak afspraak) {
            SqlConnection con = GetConnection();
            string queryBezoeker = "INSERT INTO Bezoeker(ANaam, VNaam, EMail, EigenBedrijf) " +
                                   "output INSERTED.ID " +
                                   "VALUES(@ANaam,@VNaam,@EMail,@EigenBedrijf)";

            string queryAfspraak = "INSERT INTO Afspraak(StartTijd, EindTijd, WerknemerbedrijfId, BezoekerId) " +
                                   "output INSERTED.ID " +
                                   "VALUES(@start,@eind,@werknemerId,@bezoekerId)";
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            try {
                using (SqlCommand cmdBezoeker = con.CreateCommand())
                using (SqlCommand cmdAfspraak = con.CreateCommand()) {
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
                    long bezoekerId = (long)cmdBezoeker.ExecuteScalar();
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
                    cmdAfspraak.Parameters["@bezoekerId"].Value = bezoekerId;
                    long i = (long)cmdAfspraak.ExecuteScalar();
                    afspraak.ZetId(i);
                    afspraak.Bezoeker.ZetId(bezoekerId);
                    trans.Commit();
                    return afspraak;
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("afspraak", afspraak);
                trans.Rollback();
                throw exx;
            } finally {
                con.Close();
            }
        }
        #endregion
        #region HuidigeAfspraken
        /// <summary>
        /// Geeft lijst van huidige afspraken
        /// </summary>
        /// <returns>Lijst van afspraken die nog bezig zijn (AfspraakStatus = 1)</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
            try {
                return GeefHuidigeAfspraken(null, null,null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Geeft lijst van huidige afspraken op basis van bedrijf id
        /// </summary>
        /// <param name="bedrijfId">bedrijf id die gezocht moet worden</param>
        /// <returns>Lijst van afspraken die nog bezig zijn (AfspraakStatus = 1) per bedrijf</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(long bedrijfId) {
            try {
                return GeefHuidigeAfspraken(bedrijfId, null,null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Geeft een huidige afspraak op basis van bezoeker id en bedrijf id
        /// </summary>
        /// <param name="bezoekerId">bezoeker id die gezocht moet worden</param>
        /// <param name="bedrijfId">bedrijf id die gezocht moet worden</param>
        /// <returns>Afspraak object die bezig zou zijn (AfspraakStatus = 1) per bezoeker en bedrijf</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public Afspraak GeefHuidigeAfspraakBezoekerPerBerijf(long bezoekerId, long bedrijfId) {
            try {
                return GeefHuidigeAfspraken(bedrijfId, null, bedrijfId).First();
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Geeft lijst van huidige afspraken op basis van bedrijf id en werknemer id
        /// </summary>
        /// <param name="werknemerId">werknemer id die gezocht moet worden</param>
        /// <param name="bedrijfId">bedrijf id die gezocht moet worden</param>
        /// <returns>Lijst van afspraken die nog bezig zijn (AfspraakStatus = 1) per bedrijf en/of werknemer</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemerPerBedrijf(long werknemerId, long bedrijfId) {
            try {
                return GeefHuidigeAfspraken(bedrijfId, werknemerId,null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Prive methode die een lijst van huidige afspraken geeft van bedrijf of werknemer
        /// </summary>
        /// <param name="_bedrijfId">Optioneel: id van bedrijf</param>
        /// <param name="_werknemerId">Optioneel: id van werknemer</param>
        /// <returns>Lijst van afspraken die nog bezig zijn (AfspraakStatus = 1) per bedrijf en/of werknemer</returns>
        /// <exception cref="AfspraakADOException">Faalt om een lijst op te roepen van huidige afspraken per bedrijf en/of werknemer</exception>
        private IReadOnlyList<Afspraak> GeefHuidigeAfspraken(long? _bedrijfId, long? _werknemerId, long? _bezoekerId) {
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
                           "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, wb.WerknemerEmail, " +
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
                    }
                    if (_werknemerId.HasValue) {
                        query += " AND w.id = @werknemerId";
                        cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                        cmd.Parameters["@werknemerId"].Value = _werknemerId.Value;
                    }
                    if (_bezoekerId.HasValue) {                       
                        query += " AND bz.id = @bezoekerId";
                        cmd.Parameters.Add(new SqlParameter("@bezoekerId", SqlDbType.BigInt));
                        cmd.Parameters["@bezoekerId"].Value = _bezoekerId.Value;
                    }
                    query += " ORDER BY b.id, w.id";
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    List<Afspraak> afspraken = new List<Afspraak>();
                    Werknemer werknemer = null;
                    Bedrijf bedrijf = null;
                    while (reader.Read()) {
                        //Afspraak portie
                        long afspraakId = (long)reader["AfspraakId"];
                        DateTime start = (DateTime)reader["StartTijd"];
                        DateTime? eind = !reader.IsDBNull(reader.GetOrdinal("EindTijd")) ? (DateTime)reader["EindTijd"] : null;
                        //bezoeker portie
                        long bezoekerId = (long)reader["BezoekerId"];
                        string bezoekerAnaam = (string)reader["BezoekerANaam"];
                        string bezoekerVnaam = (string)reader["BezoekerVNaam"];
                        string bezoekerMail = (string)reader["BezoekerMail"];
                        string bezoekerBedrijf = (string)reader["BezoekerBedrijf"];
                        //bedrijf portie
                        if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"]) {
                            long bedrijfId = (long)reader["BedrijfId"];
                            string bedrijfNaam = (string)reader["BedrijfNaam"];
                            string bedrijfBTWNr = (string)reader["BTWNr"];
                            string bedrijfTeleNr = (string)reader["TeleNr"];
                            string bedrijfMail = (string)reader["BedrijfEmail"];
                            string bedrijfAdres = (string)reader["BedrijfAdres"];
                            bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTWNr, true, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
                        }
                        //werknemer portie
                        if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
                            long werknemerId = (long)reader["WerknemerId"];
                            string werknemerANaam = (string)reader["WerknemerANaam"];
                            string werknemerVNaam = (string)reader["WerknemerVNaam"];
                            werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerANaam);
                        }
                        //functie portie
                        string werknemerMail = (string)reader["WerknemerEmail"];
                        string functieNaam = (string)reader["FunctieNaam"];
                        werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
                        afspraken.Add(new Afspraak(afspraakId, start, eind, bedrijf, new Bezoeker(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer));
                    }
                    return afspraken.AsReadOnly();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("bedrijfId", _bedrijfId);
                exx.Data.Add("werknemerId", _werknemerId);
                throw exx;
            } finally {
                con.Close();
            }
        }
        #endregion
        #region AlleAfspraken

        /// <summary>
        /// Geeft lijst van afspraken op basis van datum
        /// </summary>
        /// <param name="datum">datum die gezocht moet worden</param>
        /// <returns>Lijst van Afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
            try {
                return GeefAlleAfspraken(null, null, null, null, null, null, datum);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Geeft lijst van afspraken op basis van bedrijf en datum
        /// </summary>
        /// <param name="bedrijfId">bedrijf die gezocht moet worden</param>
        /// <param name="datum">datum die gezocht moet worden</param>
        /// <returns>Lijst van Afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(long bedrijfId, DateTime datum) {
            try {
                return GeefAlleAfspraken(bedrijfId, null, null, null, null, null, datum);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Geeft lijst van afspraken op basis van bezoeker en datum
        /// </summary>
        /// <param name="bezoekerId">bezoeker id die gezocht moet worden</param>
        /// <param name="datum">datum die gezocht moet worden</param>
        /// <returns>Lijst van Afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpDagPerBedrijf(long bezoekerId, DateTime datum, long bedrijfId) {
            try {
                return GeefAlleAfspraken(bedrijfId, null, bezoekerId, null, null, null, datum);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Geeft lijst van afspraken op basis van bezoeker vNaam, aNaam, of Mail
        /// </summary>
        /// <param name="bezoekerVNaam">bezoeker Vnaam die gezocht moet worden</param>
        /// <param name="bezoekerANaam">bezoeker Anaam die gezocht moet worden</param>
        /// <param name="bezoekerMail">bezoeker mail die gezocht moet worden</param>
        /// <param name="bedrijfId">bedrijf id die gezocht moet worden</param>
        /// <returns>Lijst van Afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(string bezoekerVNaam, string bezoekerANaam, string bezoekerMail, long bedrijfId) {
            try {
                return GeefAlleAfspraken(bedrijfId, null, null, bezoekerVNaam, bezoekerANaam, bezoekerMail, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Geeft lijst van afspraken op basis van werknemer
        /// </summary>
        /// <param name="werknemerId">werknemer id die gezocht moet worden</param>
        /// <param name="bedrijfId">bedrijf id die gezocht moet worden</param>
        /// <returns>Lijst van Afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemerPerBedrijf(long werknemerId, long bedrijfId) {
            try {
                return GeefAlleAfspraken(bedrijfId, werknemerId, null, null, null, null, null);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Geeft lijst van afspraken op basis van werknemer
        /// </summary>
        /// <param name="werknemerId">werknemer id die gezocht moet worden</param>
        /// <param name="datum">datum waarop die gezocht moet worden</param>
        /// <param name="bedrijfId">bedrijf id die gezocht moet worden</param>
        /// <returns>Lijst van Afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDagPerBedrijf(long werknemerId, DateTime datum, long bedrijfId) {
            try {
                return GeefAlleAfspraken(bedrijfId, werknemerId, null, null, null, null, datum);
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prive methode die alle afspraken op combinatie datum of/en werknemer
        /// </summary>
        /// <param name="_bedrijfId">Optioneel: id van bedrijf</param>
        /// <param name="_werknemerId">Optioneel: id van werknemer</param>
        /// <param name="_bezoekerId">Optioneel: gewenste bezoeker id</param>
        /// <param name="_bezoekerVNaam">Optioneel: gewenste bezoeker voornaam</param>
        /// <param name="_bezoekerANaam">Optioneel: gewenste bezoeker achternaam</param>
        /// <param name="_bezoekerMail">Optioneel: gewenste bezoeker mail</param>
        /// <param name="_datum">Optioneel: gewenste datum</param>
        /// <returns>Lijst van afspraken op combinatie van datum en/of werknemer</returns>
        /// <exception cref="AfspraakADOException">Faalt om een lijst van afspraken op werknemer en/of datum op te roepen</exception>
        private IReadOnlyList<Afspraak> GeefAlleAfspraken(long? _bedrijfId, long? _werknemerId, long? _bezoekerId, string? _bezoekerVNaam, string? _bezoekerANaam, string? _bezoekerMail, DateTime? _datum) {
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
                           "w.Id as WerknemerId, w.VNaam as WerknemerVNaam, w.ANaam as WerknemerANaam, wb.WerknemerEmail, " +
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
                    if (_bedrijfId.HasValue) {
                        query += " AND b.id = @bedrijfId";
                        cmd.Parameters.Add(new SqlParameter("@bedrijfId", SqlDbType.BigInt));
                        cmd.Parameters["@bedrijfId"].Value = _bedrijfId.Value;
                    }
                    if (_werknemerId.HasValue) {
                        query += " AND w.id = @werknemerId";
                        cmd.Parameters.Add(new SqlParameter("@werknemerId", SqlDbType.BigInt));
                        cmd.Parameters["@werknemerId"].Value = _werknemerId.Value;
                    }
                    if (_bezoekerId.HasValue) {
                        query += " AND bz.id = @bezoekerId";
                        cmd.Parameters.Add(new SqlParameter("@bezoekerId", SqlDbType.BigInt));
                        cmd.Parameters["@bezoekerId"].Value = _bezoekerId.Value;
                    }
                    if (!String.IsNullOrWhiteSpace(_bezoekerVNaam)) {
                        query += " AND bz.VNaam LIKE @VNaam";
                        cmd.Parameters.Add(new SqlParameter("@VNaam", SqlDbType.VarChar));
                        cmd.Parameters["@VNaam"].Value = $"%{_bezoekerVNaam}%";
                    }
                    if (!String.IsNullOrWhiteSpace(_bezoekerANaam)) {
                        query += " AND bz.ANaam LIKE @ANaam";
                        cmd.Parameters.Add(new SqlParameter("@ANaam", SqlDbType.VarChar));
                        cmd.Parameters["@ANaam"].Value = $"%{_bezoekerANaam}%";
                    }
                    if (!String.IsNullOrWhiteSpace(_bezoekerMail)) {
                        query += " AND bz.Email LIKE @Email";
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
                        cmd.Parameters["@Email"].Value = $"%{_bezoekerMail}%";
                    }
                    if (_datum.HasValue) {
                        query += " AND CONVERT(DATE, a.StartTijd) = @date";
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.Date));
                        cmd.Parameters["@date"].Value = _datum.Value.Date;
                    }
                    query += " ORDER BY a.StartTijd DESC, b.id, w.id";
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    List<Afspraak> afspraken = new List<Afspraak>();
                    Werknemer werknemer = null;
                    Bedrijf bedrijf = null;
                    while (reader.Read()) {
                        //Afspraak portie
                        long afspraakId = (long)reader["AfspraakId"];
                        DateTime start = (DateTime)reader["StartTijd"];
                        DateTime? eind = !reader.IsDBNull(reader.GetOrdinal("EindTijd")) ? (DateTime)reader["EindTijd"] : null;
                        //bezoeker portie
                        long bezoekerId = (long)reader["BezoekerId"];
                        string bezoekerAnaam = (string)reader["BezoekerANaam"];
                        string bezoekerVnaam = (string)reader["BezoekerVNaam"];
                        string bezoekerMail = (string)reader["BezoekerMail"];
                        string bezoekerBedrijf = (string)reader["BezoekerBedrijf"];
                        //bedrijf portie
                        if (bedrijf is null || bedrijf.Id != (long)reader["BedrijfId"]) {
                            long bedrijfId = (long)reader["BedrijfId"];
                            string bedrijfNaam = (string)reader["BedrijfNaam"];
                            string bedrijfBTWNr = (string)reader["BTWNr"];
                            string bedrijfTeleNr = (string)reader["TeleNr"];
                            string bedrijfMail = (string)reader["BedrijfEmail"];
                            string bedrijfAdres = (string)reader["BedrijfAdres"];
                            bedrijf = new Bedrijf(bedrijfId, bedrijfNaam, bedrijfBTWNr, true, bedrijfTeleNr, bedrijfMail, bedrijfAdres);
                        }
                        //werknemer portie
                        if (werknemer is null || werknemer.Id != (long)reader["WerknemerId"]) {
                            long werknemerId = (long)reader["WerknemerId"];
                            string werknemerANaam = (string)reader["WerknemerANaam"];
                            string werknemerVNaam = (string)reader["WerknemerVNaam"];
                            werknemer = new Werknemer(werknemerId, werknemerVNaam, werknemerANaam);
                        }
                        //functie portie
                        string functieNaam = (string)reader["FunctieNaam"];
                        string werknemerMail = (string)reader["WerknemerEmail"];
                        werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, werknemerMail, functieNaam);
                        afspraken.Add(new Afspraak(afspraakId, start, eind, bedrijf, new Bezoeker(bezoekerId, bezoekerVnaam, bezoekerAnaam, bezoekerMail, bezoekerBedrijf), werknemer));
                    }
                    return afspraken.AsReadOnly();
                }
            } catch (Exception ex) {
                AfspraakADOException exx = new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
                exx.Data.Add("datum", _datum);
                exx.Data.Add("werknemerId", _werknemerId);
                throw exx;
            } finally {
                con.Close();
            }
        }
        #endregion
        #region Bezoeker

        /// <summary>
        /// Geeft lijst van bezoekers die aanwezig zijn
        /// </summary>
        /// <returns>Lijst van Afspraak object</returns>
        /// <exception cref="AfspraakADOException">Faalt om een afspraakobject toe te voegen</exception>
        public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
            SqlConnection con = GetConnection();
            string query = "SELECT b.Id, b.VNaam, b.ANaam, b.Email, b.EigenBedrijf " +
                           "FROM Afspraak a " +
                           "JOIN Bezoeker b ON(a.BezoekerId = b.Id) " +
                           "WHERE a.AfspraakStatusId = 1 AND a.EindTijd IS NULL " +
                           "ORDER BY b.Vnaam, b.ANaam";
            try {
                using (SqlCommand cmd = con.CreateCommand()) {
                    con.Open();
                    cmd.CommandText = query;
                    IDataReader reader = cmd.ExecuteReader();
                    List<Bezoeker> bezoekers = new List<Bezoeker>();
                    while (reader.Read()) {
                        long bezoekerId = (long)reader["Id"];
                        string bezoekerVNaam = (string)reader["VNaam"];
                        string bezoekerANaam = (string)reader["ANaam"];
                        string bezoekerMail = (string)reader["Email"];
                        string bezoekerBedrijf = (string)reader["EigenBedrijf"];
                        bezoekers.Add(new Bezoeker(bezoekerId, bezoekerVNaam, bezoekerANaam, bezoekerMail, bezoekerBedrijf));
                    }
                    return bezoekers.AsReadOnly();
                }
            } catch (Exception ex) {
                throw new AfspraakADOException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}", ex);
            } finally {
                con.Close();
            }
        }
        #endregion
    }
}