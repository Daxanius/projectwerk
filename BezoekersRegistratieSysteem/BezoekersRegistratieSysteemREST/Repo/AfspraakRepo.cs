using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemREST.Repo {
	// TIJDELIJK
	public class AfspraakRepo : IAfspraakRepository {
		private readonly Dictionary<uint, Afspraak> _afspraken = new();
		private uint _lastId = 0;

		public void BeeindigAfspraakBezoeker(uint id) {
			if (!_afspraken.ContainsKey(id)) throw new Exception("afspraak bestaat niet");

			// Het klopt niet dat ik eindtijd hier op datetime.now moet zetten
			_afspraken[id].ZetEindtijd(DateTime.Now);
		}

		public void BeeindigAfspraakSysteem(uint id) {
			if (!_afspraken.ContainsKey(id)) throw new Exception("afspraak bestaat niet");

			// Het klopt niet dat ik eindtijd hier op datetime.now moet zetten
			_afspraken[id].ZetEindtijd(DateTime.Now);
		}


		public bool BestaatAfspraak(Afspraak afspraak) {
			// Bruh
			throw new NotImplementedException();
		}

		public bool BestaatAfspraak(uint afspraak) {
			// Bruh
			throw new NotImplementedException();
		}

		public void BewerkAfspraak(Afspraak afspraak) {
			if (!_afspraken.ContainsKey(afspraak.Id)) throw new Exception("afspraak bestaat niet");
			_afspraken[afspraak.Id] = afspraak;
		}

		public Afspraak GeefAfspraak(uint afspraakid) {
			if (!_afspraken.ContainsKey(afspraakid)) throw new Exception("afspraak bestaat niet");
			return _afspraken[afspraakid];
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
			if (!_afspraken.ContainsKey(afspraakId)) throw new Exception("Afspraak bestaat niet");
			_afspraken.Remove(afspraakId);
		}

		// Voegt een afspraak toe en increment de last ID
		public Afspraak VoegAfspraakToe(Afspraak afspraak) {
			_afspraken.Add(_lastId, afspraak);
			_afspraken[_lastId].ZetId(_lastId);
			return _afspraken[_lastId++];
		}
	}
}