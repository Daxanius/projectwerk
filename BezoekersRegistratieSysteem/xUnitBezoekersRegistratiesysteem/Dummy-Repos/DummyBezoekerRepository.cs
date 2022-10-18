using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace xUnitBezoekersRegistratiesysteem.DummyData.Repos
{
	public class DummyBezoekerRepository : IBezoekerRepository
	{
		private readonly Dictionary<uint, Bezoeker> _bezoekers = new();
		private readonly DummyAfspraakRepository _afspraakRepository = new();
		private uint _lastId = 0;

		public bool BestaatBezoeker(Bezoeker bezoeker)
		{
			return BestaatBezoeker(bezoeker.Id);
		}

		public bool BestaatBezoeker(uint bezoeker)
		{
			return _bezoekers.ContainsKey(bezoeker);
		}

		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers()
		{
			var huidigeAfspraken = _afspraakRepository.GeefHuidigeAfspraken();

			return huidigeAfspraken.Where(a => a.Eindtijd == null).Select(a => a.Bezoeker).ToList();
		}

		public Bezoeker GeefBezoeker(uint id)
		{
			if (!_bezoekers.ContainsKey(id))
				throw new Exception("Bezoeker bestaat niet");
			return _bezoekers[id];
		}

		public IReadOnlyList<Bezoeker> GeefBezoekerOpNaam(string naam, string achternaam)
		{
			return _bezoekers.Select(b => b.Value).Where(b => b.Voornaam.ToLower() == naam.ToLower() && b.Achternaam.ToLower() == achternaam.ToLower()).ToList();
		}

		public IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum)
		{
			var huidigeAfspraken = _afspraakRepository.GeefHuidigeAfspraken();
			return huidigeAfspraken.Where(a => a.Starttijd.Date == datum.Date).Select(a => a.Bezoeker).ToList();
		}

		public void VerwijderBezoeker(uint id)
		{
			if (!_bezoekers.ContainsKey(id))
				throw new Exception("Bezoeker bestaat niet");
			_bezoekers.Remove(id);
		}

		public void VoegBezoekerToe(Bezoeker bezoeker)
		{
			if (bezoeker == null)
				throw new BezoekerException("bezoeker is null");
			_bezoekers.Add(_lastId++, bezoeker);
		}

		public void WijzigBezoeker(Bezoeker bezoeker)
		{
			if (!_bezoekers.ContainsKey(bezoeker.Id))
				throw new Exception("Bezoeker bestaat niet");
			_bezoekers[bezoeker.Id] = bezoeker;
		}

		Bezoeker IBezoekerRepository.VoegBezoekerToe(Bezoeker bezoeker)
		{
			if (bezoeker == null)
				throw new BezoekerException("bezoeker is null");
			_bezoekers.Add(_lastId++, bezoeker);
			return _bezoekers[_lastId];
		}
	}
}