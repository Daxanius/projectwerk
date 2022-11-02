using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class AfspraakManager {
		private readonly IAfspraakRepository _afspraakRepository;

		public AfspraakManager(IAfspraakRepository afspraakRepository) {
			this._afspraakRepository = afspraakRepository;
		}

		public Afspraak VoegAfspraakToe(Afspraak afspraak) {
			if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - VoegAfspraakToe - afspraak mag niet leeg zijn");
			if (_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - VoegAfspraakToe - afspraak bestaat al");
			try {
				return _afspraakRepository.VoegAfspraakToe(afspraak);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

		public void VerwijderAfspraak(Afspraak afspraak) {
			if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - VerwijderAfspraak - afspraak mag niet leeg zijn");
			if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - VerwijderAfspraak - afspraak bestaat niet");
			try {
				_afspraakRepository.VerwijderAfspraak(afspraak.Id);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}
        
		public void BewerkAfspraak(Afspraak afspraak) {
			if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - BewerkAfspraak - afspraak mag niet leeg zijn");
			if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - BewerkAfspraak - afspraak bestaat niet");
			if (_afspraakRepository.GeefAfspraak(afspraak.Id).AfspraakIsGelijk(afspraak)) throw new AfspraakManagerException("AfspraakManager - BewerkAfspraak - afspraak is niet gewijzigd");
			try {
				_afspraakRepository.BewerkAfspraak(afspraak);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

		public void BeeindigAfspraakBezoeker(Afspraak afspraak) {
			if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraakBezoeker - afspraak mag niet leeg zijn");
			if (afspraak.Eindtijd is not null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraakBezoeker - afspraak is al beeindigd");
			if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("BeeindigAfspraakBezoeker - BeeindigAfspraak - afspraak bestaat niet");
			try {
				_afspraakRepository.BeeindigAfspraakBezoeker(afspraak.Id);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

		public void BeeindigAfspraakSysteem(Afspraak afspraak) {
			if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraakSysteem - afspraak mag niet leeg zijn");
			if (afspraak.Eindtijd is not null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraakSysteem - afspraak is al beeindigd");
			if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraakSysteem - afspraak bestaat niet");
			try {
				_afspraakRepository.BeeindigAfspraakSysteem(afspraak.Id);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

        public void BeeindigAfspraakOpEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraakOpEmail - email mag niet leeg zijn");
            try
            {
                _afspraakRepository.BeeindigAfspraakOpEmail(email);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public bool BestaatLopendeAfspraak(Afspraak afspraak)
        {
            if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - BestaatLopendeAfspraak - afspraak mag niet leeg zijn");
            try
            {
                return _afspraakRepository.BestaatLopendeAfspraak(afspraak);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public Afspraak GeefAfspraak(long afspraakId) {
			if (!_afspraakRepository.BestaatAfspraak(afspraakId)) throw new AfspraakManagerException("AfspraakManager - GeefAfspraak - afspraak bestaat niet");
			try {
				return _afspraakRepository.GeefAfspraak(afspraakId);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
			try {
				return _afspraakRepository.GeefHuidigeAfspraken();
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefHuidigeAfsprakenPerBedrijf - bedrijf mag niet leeg zijn");
			try {
				return _afspraakRepository.GeefHuidigeAfsprakenPerBedrijf(bedrijf.Id);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

        public IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(Bedrijf bedrijf, DateTime datum)
        {
            if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerBedrijfOpDag - bedrijf mag niet leeg zijn");
            if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerBedrijfOpDag - opvraag datum kan niet in de toekomst liggen");
            try
            {
                return _afspraakRepository.GeefAfsprakenPerBedrijfOpDag(bedrijf.Id, datum);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemerPerBedrijf(Werknemer werknemer, Bedrijf bedrijf) {
			if (werknemer == null) throw new AfspraakManagerException("AfspraakManager - GeefHuidigeAfsprakenPerWerknemerPerBedrijf - werknemer mag niet leeg zijn");
			if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefHuidigeAfsprakenPerWerknemerPerBedrijf - bedrijf mag niet leeg zijn");
			try {
				return _afspraakRepository.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(werknemer.Id, bedrijf.Id);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

        public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemerPerBedrijf(Werknemer werknemer , Bedrijf bedrijf)
        {
            if (werknemer == null) throw new AfspraakManagerException("AfspraakManager - GeefAlleAfsprakenPerWerknemerPerBedrijf - werknemer mag niet leeg zijn");
            if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefAlleAfsprakenPerWerknemerPerBedrijf - bedrijf mag niet leeg zijn");
            try
            {
                return _afspraakRepository.GeefAlleAfsprakenPerWerknemerPerBedrijf(werknemer.Id, bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDagPerBedrijf(Werknemer werknemer, DateTime datum, Bedrijf bedrijf)
        {
            if (werknemer == null) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - werknemer mag niet leeg zijn");
            if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - bedrijf mag niet leeg zijn");
            if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - opvraag datum kan niet in de toekomst liggen");
            try
            {
                return _afspraakRepository.GeefAfsprakenPerWerknemerOpDagPerBedrijf(werknemer.Id, datum, bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(string voornaam, string achternaam, string email, Bedrijf bedrijf)
        {
            if (string.IsNullOrWhiteSpace(voornaam) && (string.IsNullOrWhiteSpace(achternaam)) && string.IsNullOrWhiteSpace(email)) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - naam of email mag niet leeg zijn");
            if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - bedrijf mag niet leeg zijn");
            try
            {
                return _afspraakRepository.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(voornaam, achternaam, email, bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpDagPerBedrijf(Bezoeker bezoeker, DateTime datum, Bedrijf bedrijf)
        {
            if (bezoeker == null) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - bezoeker mag niet leeg zijn");
            if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - bedrijf mag niet leeg zijn");
            if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - opvraag datum kan niet in de toekomst liggen");
            try
            {
                return _afspraakRepository.GeefAfsprakenPerBezoekerOpDagPerBedrijf(bezoeker.Id, datum, bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public Afspraak GeefHuidigeAfspraakBezoekerPerBedrijf(Bezoeker bezoeker, Bedrijf bedrijf)
        {
            if (bezoeker == null) throw new AfspraakManagerException("AfspraakManager - GeefHuidigeAfspraakBezoekerPerBedrijf - bezoeker mag niet leeg zijn");
            if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefHuidigeAfspraakBezoekerPerBedrijf - bedrijf mag niet leeg zijn");
            try
            {
                return _afspraakRepository.GeefHuidigeAfspraakBezoekerPerBerijf(bezoeker.Id, bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }
        
        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
            if (datum.Date > DateTime.Now.Date) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerDag - opvraag datum kan niet in de toekomst liggen");
            try {
				return _afspraakRepository.GeefAfsprakenPerDag(datum);
			} catch (Exception ex) {
				throw new AfspraakManagerException(ex.Message);
			}
		}

        public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers()
        {
            try
            {
                return _afspraakRepository.GeefAanwezigeBezoekers();
            }
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }
    }
}