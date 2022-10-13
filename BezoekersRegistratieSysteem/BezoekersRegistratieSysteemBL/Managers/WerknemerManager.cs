using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class WerknemerManager {
		private readonly IWerknemerRepository _werknemerRepository;

		public WerknemerManager(IWerknemerRepository werknemerRepository) {
			this._werknemerRepository = werknemerRepository;
		}

		public void VoegWerknemerToe(string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie) {
			Werknemer werknemer = new(voornaam, achternaam, email, bedrijf, functie);
			_werknemerRepository.VoegWerknemerToe(werknemer);
		}

		public void VerwijderWerknemer(uint id) {
			_werknemerRepository.VerwijderWerknemer(id);
		}
        
		public void WijzigWerknemer(Werknemer werknemer) {
            if (werknemer == null) throw new WerknemerManagerException("WerknemerManager - WijzigWerknemer - werknemer mag niet leeg zijn");
            _werknemerRepository.WijzigWerknemer(werknemer.Id, werknemer);
		}

		public Werknemer GeefWerknemer(uint id) {
            return _werknemerRepository.GeefWerknemer(id);
        }

		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string naam) {
            if (string.IsNullOrWhiteSpace(naam)) throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpNaam - naam mag niet leeg zijn");
            return _werknemerRepository.GeefWerknemersOpNaam(naam);
		}

		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new WerknemerManagerException("WerknemerManager - GeefWerknemersPerBedrijf - bedrijf mag niet leeg zijn");
            return _werknemerRepository.GeefWerknemersPerBedrijf(bedrijf.Id);
		}
	}
}