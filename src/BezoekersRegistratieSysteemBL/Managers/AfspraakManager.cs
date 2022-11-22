using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class AfspraakManager {

		/// <summary>
		/// Private lokale Interface variabele.
		/// </summary>
		private readonly IAfspraakRepository _afspraakRepository;

		/// <summary>
		/// AfspraakManager constructor krijgt een instantie van de IAfspraakRepository interface als parameter.
		/// </summary>
		/// <param name="afspraakRepository">Interface</param>
		/// <remarks>Deze constructor stelt de lokale variabele [_afspraakRepository] gelijk aan een instantie van de IAfspraakRepository.</remarks>
		public AfspraakManager(IAfspraakRepository afspraakRepository) {
			this._afspraakRepository = afspraakRepository;
		}

		/// <summary>
		/// Voegt afspraak toe in de databank adhv een afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat toegevoegd wenst te worden.</param>
		/// <returns>Afspraak object MET id</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - VoegAfspraakToe - afspraak mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - VoegAfspraakToe - afspraak bestaat al"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public Afspraak VoegAfspraakToe(Afspraak afspraak) {
            try {
				if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn.");
				if (_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("er is nog een lopende afspraak voor dit email adres.");
				if (_afspraakRepository.HeeftWerknemerVanAnderBedrijfEenLopendeAfspraak(afspraak)) throw new AfspraakManagerException("deze werknemer heeft al een afspraak maar met een ander bedrijf.");
				return _afspraakRepository.VoegAfspraakToe(afspraak);
			} catch (Exception ex) {
				throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
			}
		}

		/// <summary>
		/// Verwijder gewenste afspraak.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat verwijderd wenst te worden.</param>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - VerwijderAfspraak - afspraak mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - VerwijderAfspraak - afspraak bestaat niet"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public void VerwijderAfspraak(Afspraak afspraak) {
			try {
				if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn");
				if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("afspraak bestaat niet");
                _afspraakRepository.VerwijderAfspraak(afspraak.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Bewerkt gegevens van een afspraak adhv afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat gewijzigd wenst te worden in de databank.</param>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BewerkAfspraak - afspraak mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BewerkAfspraak - afspraak bestaat niet"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BewerkAfspraak - afspraak is niet gewijzigd"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public void BewerkAfspraak(Afspraak afspraak) {
			try {
				if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn");
				if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("afspraak bestaat niet");
				if (_afspraakRepository.GeefAfspraak(afspraak.Id).AfspraakIsGelijk(afspraak)) throw new AfspraakManagerException("afspraak is niet gewijzigd");
				_afspraakRepository.BewerkAfspraak(afspraak);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Beëindigd afspraak via het normale pad adhv afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object die beëindigd wenst te worden.</param>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BeeindigAfspraakBezoeker - afspraak mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BeeindigAfspraakBezoeker - afspraak is al beeindigd"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BeeindigAfspraakBezoeker - afspraak bestaat niet"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public void BeeindigAfspraakBezoeker(Afspraak afspraak) {
			try {
				if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn");
				if (afspraak.Eindtijd is not null) throw new AfspraakManagerException("afspraak is al beeindigd");
				if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("afspraak bestaat niet");
				_afspraakRepository.BeeindigAfspraakBezoeker(afspraak.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Beëindigd afspraak via het fallback pad adhv afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object die beëindigd wenst te worden.</param>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BeeindigAfspraakSysteem - afspraak mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BeeindigAfspraakSysteem - afspraak is al beeindigd"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BeeindigAfspraakSysteem - afspraak bestaat niet"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public void BeeindigAfspraakSysteem(Afspraak afspraak) {
			try {
				if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn");
				if (afspraak.Eindtijd is not null) throw new AfspraakManagerException("afspraak is al beeindigd");
				if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("afspraak bestaat niet");
				_afspraakRepository.BeeindigAfspraakSysteem(afspraak.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Beëindigd afspraak adhv bezoeker email adhv parameter bezoeker email.
		/// </summary>
		/// <param name="email">Emailadres van de  bezoeker wiens afspraak beëindigd wenst te worden.</param>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BeeindigAfspraakOpEmail - email mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public void BeeindigAfspraakOpEmail(string email) {
			try {
				if (string.IsNullOrWhiteSpace(email)) throw new AfspraakManagerException("email mag niet leeg zijn");
				if (!_afspraakRepository.BestaatAfspraak(email)) throw new AfspraakManagerException("afspraak bestaat niet");
				_afspraakRepository.BeeindigAfspraakOpEmail(email);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Gaat na of lopende afspraak bestaat adhv een afspraak object.
		/// </summary>
		/// <param name="afspraak">Afspraak object dat gecontroleerd wenst te worden.</param>
		/// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - BestaatLopendeAfspraak - afspraak mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public bool BestaatLopendeAfspraak(Afspraak afspraak) {
			try {
				if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn");
				return _afspraakRepository.BestaatLopendeAfspraak(afspraak);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Haalt afspraak op uit de databank adhv parameter afspraak id.
		/// </summary>
		/// <param name="afspraakId">Id van de gewenste afspraak.</param>
		/// <returns>Gewenst afspraak object</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfspraak - afspraak bestaat niet"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public Afspraak GeefAfspraak(long afspraakId) {
			try {
				if (!_afspraakRepository.BestaatAfspraak(afspraakId)) throw new AfspraakManagerException("afspraak bestaat niet");
				return _afspraakRepository.GeefAfspraak(afspraakId);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten.
		/// </summary>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
			try {
				return _afspraakRepository.GeefHuidigeAfspraken();
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv het bedrijf object.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefHuidigeAfsprakenPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(Bedrijf bedrijf) {
			try {
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				return _afspraakRepository.GeefHuidigeAfsprakenPerBedrijf(bedrijf.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv Bedrijf object en parameter datum.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="datum">datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerBedrijfOpDag - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerBedrijfOpDag - opvraag datum kan niet in de toekomst liggen"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(Bedrijf bedrijf, DateTime datum) {
			try {
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("opvraag datum kan niet in de toekomst liggen");
				return _afspraakRepository.GeefAfsprakenPerBedrijfOpDag(bedrijf.Id, datum);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv werknemer object en bedrijf object.
		/// </summary>
		/// <param name="werknemer">Werknemer object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefHuidigeAfsprakenPerWerknemerPerBedrijf - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefHuidigeAfsprakenPerWerknemerPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemerPerBedrijf(Werknemer werknemer, Bedrijf bedrijf) {
			try {
				if (werknemer == null) throw new AfspraakManagerException("werknemer mag niet leeg zijn");
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				return _afspraakRepository.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(werknemer.Id, bedrijf.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameters werknemer id en berdijf id.
		/// </summary>
		/// <param name="werknemer">Werknemer object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAlleAfsprakenPerWerknemerPerBedrijf - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAlleAfsprakenPerWerknemerPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemerPerBedrijf(Werknemer werknemer, Bedrijf bedrijf) {
			try {
				if (werknemer == null) throw new AfspraakManagerException("werknemer mag niet leeg zijn");
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				return _afspraakRepository.GeefAlleAfsprakenPerWerknemerPerBedrijf(werknemer.Id, bedrijf.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv werknemer object, bedrijf object en parameter datum.
		/// </summary>
		/// <param name="werknemer">Werknemer object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="datum">datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - opvraag datum kan niet in de toekomst liggen"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDagPerBedrijf(Werknemer werknemer, DateTime datum, Bedrijf bedrijf) {
			try {
				if (werknemer == null) throw new AfspraakManagerException("werknemer mag niet leeg zijn");
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("opvraag datum kan niet in de toekomst liggen");
				return _afspraakRepository.GeefAfsprakenPerWerknemerOpDagPerBedrijf(werknemer.Id, datum, bedrijf.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv bedrijf object en parameters bezoeker voornaam/achternaam/email.
		/// </summary>
		/// <param name="voornaam">Optioneel: Voornaam van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="achternaam">Optioneel: Achternaam van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="email">Optioneel: Email van de bezoeker waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - naam of email mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(string voornaam, string achternaam, string email, Bedrijf bedrijf) {
			try {
				if (string.IsNullOrWhiteSpace(voornaam) && (string.IsNullOrWhiteSpace(achternaam)) && string.IsNullOrWhiteSpace(email)) throw new AfspraakManagerException("naam of email mag niet leeg zijn");
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				return _afspraakRepository.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(voornaam, achternaam, email, bedrijf.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv bezoeker object, bedrijf object en parameter datum.
		/// </summary>
		/// <param name="bezoeker">Bezoeker object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="datum">Datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - bezoeker mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - opvraag datum kan niet in de toekomst liggen"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpDagPerBedrijf(long bezoekerId, DateTime datum, Bedrijf bedrijf) {
			try {
				if (bezoekerId < 0) throw new AfspraakManagerException("bezoeker mag niet minder dan 0 zijn");
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("opvraag datum kan niet in de toekomst liggen");
				return _afspraakRepository.GeefAfsprakenPerBezoekerOpDagPerBedrijf(bezoekerId, datum, bedrijf.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv bezoeker object en bedrijf object.
		/// </summary>
		/// <param name="bezoekerId">Bezoeker object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <param name="bedrijf">Bedrijf object waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>Gewenste afspraak object waar statuscode.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefHuidigeAfspraakBezoekerPerBedrijf - bezoeker mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefHuidigeAfspraakBezoekerPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public Afspraak GeefHuidigeAfspraakBezoekerPerBedrijf(long bezoekerId, Bedrijf bedrijf) {
			try {
				if (bezoekerId < 0) throw new AfspraakManagerException("bezoeker id moet groter dan 0 zijn");
				if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
				return _afspraakRepository.GeefHuidigeAfspraakBezoekerPerBerijf(bezoekerId, bedrijf.Id);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van huidige afspraken samen met enkel lees rechten adhv parameter datum.
		/// </summary>
		/// <param name="datum">datum waar de afspraken van opgevraagd wensen te worden.</param>
		/// <returns>IReadOnlyList van afspraak objecten.</returns>
		/// <exception cref="AfspraakManagerException">"AfspraakManager - GeefAfsprakenPerDag - opvraag datum kan niet in de toekomst liggen"</exception>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
			try {
				if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("opvraag datum kan niet in de toekomst liggen");
				return _afspraakRepository.GeefAfsprakenPerDag(datum);
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}

		/// <summary>
		/// Stelt lijst van alle aanwezige bezoekers samen met enkel lees rechten.
		/// </summary>
		/// <returns>IReadOnlyList van bezoeker objecten.</returns>
		/// <exception cref="AfspraakManagerException">ex.Message</exception>
		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			try {
				return _afspraakRepository.GeefAanwezigeBezoekers();
			} catch (Exception ex) {
                throw new AfspraakManagerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}", ex);
            }
		}
	}
}