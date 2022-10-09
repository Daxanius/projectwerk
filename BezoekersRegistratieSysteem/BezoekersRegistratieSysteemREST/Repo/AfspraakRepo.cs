using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class AfspraakRepo : IAfspraakRepository {
		public void BeeindigAfspraak(uint id) {
			throw new NotImplementedException();
		}

		public void BewerkAfspraak(uint id) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDatum(uint id, DateTime datum) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint id) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint id) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint id) {
			throw new NotImplementedException();
		}

		public void VerwijderAfspraak(uint id) {
			throw new NotImplementedException();
		}

		public void VoegAfspraakToe(Afspraak afspraak) {
			throw new NotImplementedException();
		}
	}
}
