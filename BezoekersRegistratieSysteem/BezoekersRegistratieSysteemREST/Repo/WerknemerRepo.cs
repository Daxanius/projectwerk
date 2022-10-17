using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class WerknemerRepo : IWerknemerRepository {
		private readonly Dictionary<uint, Werknemer> _werknemers = new();
		private uint _lastId = 0;

		public void BestaatWerknemer(Werknemer werknemer) {
			throw new NotImplementedException();
		}

		public Werknemer GeefWerknemer(uint id) {
			if (!_werknemers.ContainsKey(id)) throw new Exception("Werknemer bestaat niet");

			return _werknemers[id];
		}

		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string naam) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id) {
			throw new NotImplementedException();
		}

		public void VerwijderWerknemer(uint id) {
			if (!_werknemers.ContainsKey(id)) throw new Exception("Werknemer bestaat niet");

			_werknemers.Remove(id);
		}

		public void VoegWerknemerToe(Werknemer werknemer) {
			_werknemers.Add(_lastId++, werknemer);
		}

		public void WijzigWerknemer(uint id, Werknemer werknemer) {
			if (!_werknemers.ContainsKey(id)) throw new Exception("Werknemer bestaat niet");

			_werknemers[id] = werknemer;
		}
	}
}