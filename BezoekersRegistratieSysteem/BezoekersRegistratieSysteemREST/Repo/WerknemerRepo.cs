using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class WerknemerRepo : IWerknemerRepository {
		private readonly Dictionary<uint, Werknemer> _werknemers = new();
		private uint _lastId = 0;

		public bool BestaatWerknemer(Werknemer werknemer) {
			throw new NotImplementedException();
		}

		public bool BestaatWerknemer(uint werknemer) {
			throw new NotImplementedException();
		}

		public Werknemer GeefWerknemer(uint id) {
			if (!_werknemers.ContainsKey(id)) throw new Exception("Werknemer bestaat niet");

			return _werknemers[id];
		}

		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string naam, string achternaam) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id) {
			throw new NotImplementedException();
		}

		public void VerwijderWerknemer(uint id) {
			if (!_werknemers.ContainsKey(id)) throw new Exception("Werknemer bestaat niet");

			_werknemers.Remove(id);
		}

		public Werknemer VoegWerknemerToe(Werknemer werknemer) {
			_werknemers.Add(_lastId, werknemer);
			_werknemers[_lastId].ZetId(_lastId);
			return _werknemers[_lastId++];
		}

		public void WijzigWerknemer(Werknemer werknemer) {
			if (!_werknemers.ContainsKey(werknemer.Id)) throw new Exception("Werknemer bestaat niet");
			_werknemers[werknemer.Id] = werknemer;
		}
	}
}