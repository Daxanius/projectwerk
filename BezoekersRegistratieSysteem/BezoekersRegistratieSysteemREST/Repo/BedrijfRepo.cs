using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class BedrijfRepo : IBedrijfRepository {
		private readonly Dictionary<uint, Bedrijf> _bedrijven = new();
		private uint _lastId = 0;

		public void BestaatBedrijf(Bedrijf bedrijf) {
			// Returnt void klopt niet
			throw new NotImplementedException();
		}

		public void BewerkBedrijf(uint id, Bedrijf bedrijf) {
			// Waarom heeft bewerk bedrijf ID nog appart nodig?

			if (!_bedrijven.ContainsKey(id)) throw new Exception("Bedrijf bestaat niet");
			_bedrijven[id] = bedrijf;
		}

		public Bedrijf GeefBedrijf(string bedrijfsnaam) {
			throw new NotImplementedException();
		}

		public IReadOnlyList<Bedrijf> Geefbedrijven() {
			return (IReadOnlyList<Bedrijf>)_bedrijven.Values.AsEnumerable();
		}

		public Bedrijf GetBedrijf(uint id) {
			if (!_bedrijven.ContainsKey(id)) throw new Exception("Bedrijf bestaat niet");

			return _bedrijven[id];
		}

		public void VerwijderBedrijf(uint id) {
			if (!_bedrijven.ContainsKey(id)) throw new Exception("Bedrijf bestaat niet");

			_bedrijven.Remove(id);
		}

		public void VoegBedrijfToe(Bedrijf bedrijf) {
			_bedrijven.Add(_lastId++, bedrijf);
		}
	}
}