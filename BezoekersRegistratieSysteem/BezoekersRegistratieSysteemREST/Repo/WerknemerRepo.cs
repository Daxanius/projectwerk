using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class WerknemerRepo : IWerknemerRepository {
		public Werknemer GeefWerknemerOpNaam(string naam) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Werknemer> GeefWerknemersPerFunctie(string functie) {
			throw new NotImplementedException();
		}

		public void VerwijderWerknemer(uint id) {
			throw new NotImplementedException();
		}

		public void VoegWerknemerToe(Werknemer werknemer) {
			throw new NotImplementedException();
		}

		public void WijzigWerknemer(uint id, Werknemer werknemer) {
			throw new NotImplementedException();
		}
	}
}
