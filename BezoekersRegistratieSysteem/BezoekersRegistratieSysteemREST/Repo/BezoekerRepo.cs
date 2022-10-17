using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class BezoekerRepo : IBezoekerRepository {
		private readonly Dictionary<uint, Bezoeker> _bezoekers = new();
		private uint _lastId = 0;

		public void BestaatBezoeker(Bezoeker bezoeker) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			throw new NotImplementedException();
		}

		public Bezoeker GeefBezoeker(uint id) {
			if (!_bezoekers.ContainsKey(id)) throw new Exception("Bezoeker bestaat niet");
			return _bezoekers[id];
		}

		public Bezoeker GeefBezoekerOpNaam(string naam) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum) {
			throw new NotImplementedException();
		}

		public void VerwijderBezoeker(uint id) {
			if (!_bezoekers.ContainsKey(id)) throw new Exception("Bezoeker bestaat niet");
			_bezoekers.Remove(id);
		}

		public void VoegBezoekerToe(Bezoeker bezoeker) {
			_bezoekers.Add(_lastId++, bezoeker);
		}

		public void WijzigBezoeker(uint id, Bezoeker bezoeker) {
			if (!_bezoekers.ContainsKey(id)) throw new Exception("Bezoeker bestaat niet");
			_bezoekers[id] = bezoeker;
		}
	}
}