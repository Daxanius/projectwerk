using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace xUnitBezoekersRegistratiesysteem.DummyData.Repos
{
	public class DummyWerknemerRepository : IWerknemerRepository
	{
		private readonly Dictionary<uint, Werknemer> _werknemers = new();
		private readonly DummyBedrijfsRepository _bedrijfsRepo = new();

		public bool BestaatWerknemer(Werknemer werknemer)
		{
			if (werknemer == null)
				throw new WerknemerException("werknemer is null");
			return BestaatWerknemer(werknemer.Id);
		}

		public bool BestaatWerknemer(uint werknemer)
		{
			return _werknemers.ContainsKey(werknemer);
		}

		public Werknemer GeefWerknemer(uint id)
		{
			if (!_werknemers.ContainsKey(id))
				throw new Exception("Werknemer bestaat niet");

			return _werknemers[id];
		}

		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string naam, string achternaam)
		{
			return _werknemers.Select(w => w.Value).Where(w => w.Voornaam.ToLower() == naam.ToLower() && w.Achternaam.ToLower() == achternaam.ToLower()).ToList();
		}

		public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint bedrijfsId)
		{
			var bedrijven = _bedrijfsRepo.Geefbedrijven();

			return bedrijven.Where(b => b.Id == bedrijfsId).Select(b => b.GeefWerknemers()).Select(w => w[0]).ToList();
		}

		public void VerwijderWerknemer(uint id)
		{
			if (!_werknemers.ContainsKey(id))
				throw new WerknemerException("Werknemer bestaat niet");

			_werknemers.Remove(id);
		}

		public Werknemer VoegWerknemerToe(Werknemer werknemer)
		{
			_werknemers.Add(werknemer.Id, werknemer);
			return _werknemers[werknemer.Id];
		}

		public void WijzigWerknemer(Werknemer werknemer)
		{
			if (!_werknemers.ContainsKey(werknemer.Id))
				throw new WerknemerException("Werknemer bestaat niet");
			_werknemers[werknemer.Id] = werknemer;
		}
	}
}