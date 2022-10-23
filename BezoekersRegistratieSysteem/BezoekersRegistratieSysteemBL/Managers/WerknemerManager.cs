using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class WerknemerManager {
		private readonly IWerknemerRepository _werknemerRepository;

		public WerknemerManager(IWerknemerRepository werknemerRepository) {
            this._werknemerRepository = werknemerRepository;
        }

		public Werknemer VoegWerknemerToe(Werknemer werknemer) {
            if (werknemer == null) throw new WerknemerManagerException("WerknemerManager - VoegWerknemerToe - werknemer mag niet leeg zijn");
            if (_werknemerRepository.BestaatWerknemer(werknemer)) throw new WerknemerManagerException("WerknemerManager - VoegWerknemerToe - werknemer bestaat al");
            try
			{
				return _werknemerRepository.VoegWerknemerToe(werknemer);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

		public void VerwijderWerknemer(Werknemer werknemer) {
            if (werknemer == null) throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemer - werknemer mag niet leeg zijn");
            if (!_werknemerRepository.BestaatWerknemer(werknemer)) throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemer - werknemer bestaat niet");
            try
			{
				_werknemerRepository.VerwijderWerknemer(werknemer);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
		}

        public void VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie)
        {
            if (werknemer == null) throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - werknemer mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - functie mag niet leeg zijn");
            if (!_werknemerRepository.BestaatWerknemer(werknemer)) throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - werknemer bestaat niet");
            try
            {
                _werknemerRepository.VerwijderWerknemerFunctie(werknemer, bedrijf, functie);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

        public void WijzigWerknemer(Werknemer werknemer) {
            if (werknemer == null) throw new WerknemerManagerException("WerknemerManager - WijzigWerknemer - werknemer mag niet leeg zijn");
            if (!_werknemerRepository.BestaatWerknemer(werknemer)) throw new WerknemerManagerException("WerknemerManager - WijzigWerknemer - werknemer bestaat niet");
            if (_werknemerRepository.GeefWerknemer(werknemer.Id).WerknemerIsGelijk(werknemer)) throw new WerknemerManagerException("WerknemerManager - WijzigWerknemer - werknemer bestaat niet");
            try
			{
				_werknemerRepository.WijzigWerknemer(werknemer);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

		public Werknemer GeefWerknemer(uint id) {
            if (!_werknemerRepository.BestaatWerknemer(id)) throw new WerknemerManagerException("WerknemerManager - GeefWerknemer - werknemer bestaat niet");
            try
			{
				return _werknemerRepository.GeefWerknemer(id);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string voornaam, string achternaam) {
            if (string.IsNullOrWhiteSpace(voornaam) || string.IsNullOrWhiteSpace(achternaam)) throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpNaam - naam mag niet leeg zijn");
            try
			{
				return _werknemerRepository.GeefWerknemersOpNaam(voornaam, achternaam);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new WerknemerManagerException("WerknemerManager - GeefWerknemersPerBedrijf - bedrijf mag niet leeg zijn");
			try
			{
				return _werknemerRepository.GeefWerknemersPerBedrijf(bedrijf.Id);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }
	}
}