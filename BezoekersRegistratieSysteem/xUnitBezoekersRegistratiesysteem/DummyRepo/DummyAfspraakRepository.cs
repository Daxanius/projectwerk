using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace xUnitBezoekersRegistratiesysteem.DummyRepo
{
    public class DummyAfspraakRepository : IAfspraakRepository
    {
        private Dictionary<uint, Afspraak> _afspraken = new();

        public void BeeindigAfspraak(uint id)
        {
            if (!_afspraken.ContainsKey(id))
                throw new Exception("id does not exist");
			_afspraken.Remove(id);
		}

        public void BewerkAfspraak(Afspraak afspraak)
        {
            if (!_afspraken.ContainsKey(afspraak.Id))
                throw new Exception("id does not exist");
            _afspraken[afspraak.Id] = afspraak;

		}

        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum)
        {
			return _afspraken.Where(a => a.Value.Starttijd.Date == datum.Date).Select(a => a.Value).ToList();
		}

        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDatum(uint id, DateTime datum)
        {
			return _afspraken.Where(a => a.Value.Werknemer.Id == id && a.Value.Starttijd.Date == datum.Date).Select(a => a.Value).ToList();
		}

        public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint id)
        {
			return _afspraken.Where(a => a.Value.Werknemer.Id == id).Select(a => a.Value).ToList();
		}

        public IReadOnlyList<Afspraak> GeefHuidigeAfspraken()
        {
			return _afspraken.Where(a => a.Value.Eindtijd == null && a.Value.Starttijd < DateTime.Now).Select(a => a.Value).ToList();
		}

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint id)
        {
			return _afspraken.Where(a => a.Value.Werknemer.Bedrijf != null && a.Value.Werknemer.Bedrijf.Id == id && a.Value.Eindtijd == null && a.Value.Starttijd < DateTime.Now).Select(a => a.Value).ToList();
		}

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint id)
        {
			return _afspraken.Where(a => a.Value.Werknemer.Id == id && a.Value.Eindtijd == null && a.Value.Starttijd < DateTime.Now).Select(a => a.Value).ToList();
		}

        public void VerwijderAfspraak(uint id)
        {
			if (!_afspraken.ContainsKey(id))
				throw new Exception("id does not exist");
			_afspraken.Remove(id);
		}

        public void VoegAfspraakToe(Afspraak afspraak)
        {
            if (afspraak == null)
                throw new Exception("afspraak is null");
			if (_afspraken.ContainsKey(afspraak.Id))
                throw new Exception("id bestaat al");
			_afspraken.Add(afspraak.Id, afspraak);
		}
    }
}