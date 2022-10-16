using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class WerknemerManager {
		private readonly IWerknemerRepository _werknemerRepository;

		public WerknemerManager(IWerknemerRepository werknemerRepository) {
            this._werknemerRepository = werknemerRepository;
        }

		public void VoegWerknemerToe(Werknemer werknemer) {
			try
			{
				_werknemerRepository.VoegWerknemerToe(werknemer);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

		public void VerwijderWerknemer(uint id) {
			try
			{
				_werknemerRepository.VerwijderWerknemer(id);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
		}
        
		public void WijzigWerknemer(Werknemer werknemer) {
            if (werknemer == null) throw new WerknemerManagerException("WerknemerManager - WijzigWerknemer - werknemer mag niet leeg zijn");
			try
			{
				_werknemerRepository.WijzigWerknemer(werknemer.Id, werknemer);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

		public Werknemer GeefWerknemer(uint id) {
			try
			{
				return _werknemerRepository.GeefWerknemer(id);
			}
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string naam) {
            if (string.IsNullOrWhiteSpace(naam)) throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpNaam - naam mag niet leeg zijn");
			try
			{
				return _werknemerRepository.GeefWerknemersOpNaam(naam);
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