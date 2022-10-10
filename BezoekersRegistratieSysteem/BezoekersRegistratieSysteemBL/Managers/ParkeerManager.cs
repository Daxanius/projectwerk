using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class ParkeerManager {
		private readonly Dictionary<Bedrijf, List<string>> _nummerplaten = new();

		public void VoegNummerplaatToe(Bedrijf bedrijf, string parkeerplaats) {
			if (bedrijf == null) throw new ParkeerManagerException("Bedrijf mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(parkeerplaats)) throw new ParkeerManagerException("Nummerplaat mag niet leeg zijn");
			if (!_nummerplaten.ContainsKey(bedrijf)) throw new ParkeerManagerException("Bedrijf bestaat niet");
			_nummerplaten[bedrijf].Add(parkeerplaats);
		}

		public void VerwijderNummerplaat(Bedrijf bedrijf, string parkeerplaats) {
			if (bedrijf == null) throw new ParkeerManagerException("Bedrijf mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(parkeerplaats)) throw new ParkeerManagerException("Nummerplaat mag niet leeg zijn");
			if (!_nummerplaten.ContainsKey(bedrijf)) throw new ParkeerManagerException("Bedrijf bestaat niet");
			_nummerplaten[bedrijf].Remove(parkeerplaats);
		}

		public IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null) throw new ParkeerManagerException("Bedrijf mag niet leeg zijn");
			if (!_nummerplaten.ContainsKey(bedrijf)) throw new ParkeerManagerException("Bedrijf bestaat niet");
			return _nummerplaten[bedrijf].AsReadOnly();
		}

		public Bedrijf? GeefBedrijfPerNummerplaat(string nummerplaat) {
			if (string.IsNullOrWhiteSpace(nummerplaat)) throw new ParkeerManagerException("Nummerplaat mag niet leeg zijn");
			foreach (KeyValuePair<Bedrijf, List<string>> nummerplaatPerBedrijf in _nummerplaten) {
				if (nummerplaatPerBedrijf.Value.Contains(nummerplaat)) return nummerplaatPerBedrijf.Key;
			}
			return null;
		}
	}
}