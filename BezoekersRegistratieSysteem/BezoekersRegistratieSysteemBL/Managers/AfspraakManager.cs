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
            try
			{
				return _afspraakRepository.VoegAfspraakToe(afspraak);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public void VerwijderAfspraak(Afspraak afspraak) {
            if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - VerwijderAfspraak - afspraak mag niet leeg zijn");
            if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - VerwijderAfspraak - afspraak bestaat niet");
            try
			{
				_afspraakRepository.VerwijderAfspraak(afspraak.Id);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }
        
		public void BewerkAfspraak(Afspraak afspraak) {
            if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - BewerkAfspraak - Afspraak mag niet leeg zijn");
            if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - BewerkAfspraak - Afspraak bestaat niet");
            if (_afspraakRepository.GeefAfspraak(afspraak.Id).AfspraakIsGelijk(afspraak)) throw new AfspraakManagerException("AfspraakManager - BewerkAfspraak - Afspraak is niet gewijzigd");
            try
			{
				_afspraakRepository.BewerkAfspraak(afspraak);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public void BeeindigAfspraakBezoeker(Afspraak afspraak) {
            if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraak - Afspraak mag niet leeg zijn");
            if (afspraak.Eindtijd is not null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraak - Afspraak is al beeindigd");
            if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraak - Afspraak bestaat niet");
            try
			{
				_afspraakRepository.BeeindigAfspraakBezoeker(afspraak.Id);                
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public void BeeindigAfspraakSysteem(Afspraak afspraak)
        {
            if (afspraak == null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraak - Afspraak mag niet leeg zijn");
            if (afspraak.Eindtijd is not null) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraak - Afspraak is al beeindigd");
            if (!_afspraakRepository.BestaatAfspraak(afspraak)) throw new AfspraakManagerException("AfspraakManager - BeeindigAfspraak - Afspraak bestaat niet");
            try
			{
				_afspraakRepository.BeeindigAfspraakSysteem(afspraak.Id);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

        public Afspraak GeefAfspraak(uint afspraakId) {
            if (!_afspraakRepository.BestaatAfspraak(afspraakId)) throw new AfspraakManagerException("AfspraakManager - GeefAfspraak - Afspraak bestaat niet");
            try
			{
				return _afspraakRepository.GeefAfspraak(afspraakId);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
			try
			{
				return _afspraakRepository.GeefHuidigeAfspraken();
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new AfspraakManagerException("AfspraakManager - GeefHuidigeAfsprakenPerBedrijf - Bedrijf mag niet leeg zijn");
			try
			{
				return _afspraakRepository.GeefHuidigeAfsprakenPerBedrijf(bedrijf.Id);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }
		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(Werknemer werknemer) {
            if (werknemer == null) throw new AfspraakManagerException("AfspraakManager - GeefHuidigeAfsprakenPerWerknemer - Werknemer mag niet leeg zijn");
			try
			{
				return _afspraakRepository.GeefHuidigeAfsprakenPerWerknemer(werknemer.Id);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(Werknemer werknemer) {
            if (werknemer == null) throw new AfspraakManagerException("AfspraakManager - GeefAlleAfsprakenPerWerknemer - Werknemer mag niet leeg zijn");
			try
			{
				return _afspraakRepository.GeefAlleAfsprakenPerWerknemer(werknemer.Id);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(Werknemer werknemer, DateTime datum) {
            if (werknemer == null) throw new AfspraakManagerException("AfspraakManager - GeefAfsprakenPerWerknemerOpDag - Werknemer mag niet leeg zijn");
			try
			{
				return _afspraakRepository.GeefAfsprakenPerWerknemerOpDag(werknemer.Id, datum);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
			try
			{
				return _afspraakRepository.GeefAfsprakenPerDag(datum);
			}
            catch (Exception ex)
            {
                throw new AfspraakManagerException(ex.Message);
            }
        }
	}
}