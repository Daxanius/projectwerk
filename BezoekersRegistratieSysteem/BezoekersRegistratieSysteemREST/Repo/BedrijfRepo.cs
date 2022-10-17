using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class BedrijfRepo : IBedrijfRepository {
		public void BestaatBedrijf(Bedrijf bedrijf) {
			throw new NotImplementedException();
		}

		public void BewerkBedrijf(uint id, Bedrijf bedrijf) {
			throw new NotImplementedException();
		}

		public Bedrijf GeefBedrijf(string bedrijfsnaam) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Bedrijf> Geefbedrijven() {
			throw new NotImplementedException();
		}

		public Bedrijf GetBedrijf(uint id) {
			throw new NotImplementedException();
		}

		public void VerwijderBedrijf(uint id) {
			throw new NotImplementedException();
		}

		public void VoegBedrijfToe(Bedrijf bedrijf) {
			throw new NotImplementedException();
		}
	}
}
