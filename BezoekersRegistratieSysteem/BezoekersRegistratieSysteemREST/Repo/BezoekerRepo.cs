using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class BezoekerRepo : IBezoekerRepository {
		public void BestaatBezoeker(Bezoeker bezoeker) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			throw new NotImplementedException();
		}

		public Bezoeker GeefBezoeker(uint id) {
			throw new NotImplementedException();
		}

		public Bezoeker GeefBezoekerOpNaam(string naam) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum) {
			throw new NotImplementedException();
		}

		public void VerwijderBezoeker(uint id) {
			throw new NotImplementedException();
		}

		public void VoegBezoekerToe(Bezoeker bezoeker) {
			throw new NotImplementedException();
		}

		public void WijzigBezoeker(uint id, Bezoeker bezoeker) {
			throw new NotImplementedException();
		}
	}
}