using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class AfspraakRepo : IAfspraakRepository {
		public void BeeindigAfspraakBezoeker(uint id) {
			throw new NotImplementedException();
		}

		public void BeeindigAfspraakSysteem(uint id) {
			throw new NotImplementedException();
		}

		public void BestaatAfspraak(Afspraak afspraak) {
			throw new NotImplementedException();
		}

		public void BewerkAfspraak(Afspraak afspraak) {
			throw new NotImplementedException();
		}

		public Afspraak GeefAfspraak(uint afspraakid) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint werknemerId) {
			throw new NotImplementedException();
		}

		public void VerwijderAfspraak(uint afspraakId) {
			throw new NotImplementedException();
		}

		public void VoegAfspraakToe(Afspraak afspraak) {
			throw new NotImplementedException();
		}
	}
}
