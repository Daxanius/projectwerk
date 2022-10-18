using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace xUnitBezoekersRegistratiesysteem.DummyData.Repos
{
	public class DummyBedrijfsRepository : IBedrijfRepository
	{
		private readonly Dictionary<uint, Bedrijf> _bedrijven = new();

		public readonly Afspraak Afspraak1;
		public readonly Afspraak Afspraak2;
		public readonly Afspraak Afspraak3;

		public readonly DateTime StartTijd1;
		public readonly DateTime StartTijd2;
		public readonly DateTime StartTijd3;

		public readonly Bezoeker Bezoeker1;
		public readonly Bezoeker Bezoeker2;
		public readonly Bezoeker Bezoeker3;

		public readonly Werknemer Werknemer1;
		public readonly Werknemer Werknemer2;
		public readonly Werknemer Werknemer3;

		public readonly Bedrijf Bedrijf1;
		public readonly Bedrijf Bedrijf2;
		public readonly Bedrijf Bedrijf3;

		public DummyBedrijfsRepository()
		{
			Bedrijf1 = new("aNaam", "BE0676747521", "0476687242", "a@gmail.com", "a 101 9000");
			Bedrijf2 = new("bNaam", "BE0676747521", "0476687246", "b@gmail.com", "b 101 9000");
			Bedrijf3 = new("cNaam", "BE0676747521", "0476687248", "c@gmail.com", "c 101 9000");

			Bedrijf1.ZetId(1);
			Bedrijf2.ZetId(2);
			Bedrijf3.ZetId(3);

			Werknemer1 = new("dVoorNaam", "dAchterNaam", "d@gmail.com");
			Werknemer2 = new("eVoorNaam", "eAchterNaam", "e@gmail.com");
			Werknemer3 = new("fVoorNaam", "fAchterNaam", "f@gmail.com");

			Werknemer1.VoegBedrijfEnFunctieToeAanWerknemer(Bedrijf1, "dFuntie");
			Werknemer2.VoegBedrijfEnFunctieToeAanWerknemer(Bedrijf2, "eFuntie");
			Werknemer3.VoegBedrijfEnFunctieToeAanWerknemer(Bedrijf3, "fFuntie");

			Werknemer1.ZetId(1);
			Werknemer2.ZetId(2);
			Werknemer3.ZetId(3);

			Bezoeker1 = new("gVoorNaam", "gAchterNaam", "g@gmail.com", "gBedrijf");
			Bezoeker2 = new("hVoorNaam", "hAchterNaam", "h@gmail.com", "hBedrijf");
			Bezoeker3 = new("iVoorNaam", "iAchterNaam", "i@gmail.com", "iBedrijf");

			Bezoeker1.ZetId(1);
			Bezoeker2.ZetId(2);
			Bezoeker3.ZetId(3);

			StartTijd1 = DateTime.Now;
			StartTijd2 = DateTime.Now.AddHours(2);
			StartTijd3 = DateTime.Now.AddHours(3);

			Afspraak1 = new(StartTijd1, Bezoeker1, Werknemer1);
			Afspraak2 = new(StartTijd2, Bezoeker2, Werknemer2);
			Afspraak3 = new(StartTijd3, Bezoeker3, Werknemer3);

			Afspraak1.ZetId(1);
			Afspraak2.ZetId(2);
			Afspraak3.ZetId(3);

			_bedrijven.Add(1, Bedrijf1);
			_bedrijven.Add(2, Bedrijf2);
			_bedrijven.Add(3, Bedrijf3);
		}

		public void VerwijderBedrijf(uint id)
		{
			if (!_bedrijven.ContainsKey(id))
			{
				throw new BedrijfException("Bedrijf niet gevonden");
			}

			_bedrijven.Remove(id);
		}

		public void VoegBedrijfToe(Bedrijf bedrijf)
		{
			if (bedrijf == null)
			{
				throw new BedrijfException("Bedrijf is null");
			}

			if (_bedrijven.ContainsValue(bedrijf))
			{
				throw new BedrijfException("Bedrijf bestaat al");
			}

			_bedrijven.Add(bedrijf.Id, bedrijf);
		}

		public void WijzigBedrijf(uint id, Bedrijf bedrijf)
		{
			if (bedrijf == null)
			{
				throw new BedrijfException("Bedrijf is null");
			}

			if (!_bedrijven.ContainsKey(id))
			{
				throw new BedrijfException("Bedrijf niet gevonden");
			}

			_bedrijven[id] = bedrijf;
		}

		Bedrijf IBedrijfRepository.VoegBedrijfToe(Bedrijf bedrijf)
		{
			if (bedrijf == null)
				throw new BedrijfException("bedrijf is null");
			if (_bedrijven.ContainsValue(bedrijf))
				throw new BedrijfException("bedrijf bestaat al");
			_bedrijven.Add(bedrijf.Id, bedrijf);

			return GeefBedrijf(bedrijf.Id);
		}

		public void BewerkBedrijf(Bedrijf bedrijf)
		{
			if (bedrijf == null)
				throw new BedrijfException("bedrijf is null");
			if (!_bedrijven.ContainsKey(bedrijf.Id))
				throw new BedrijfException("bedrijf does not exist");
			_bedrijven[bedrijf.Id] = bedrijf;
		}

		public bool BestaatBedrijf(Bedrijf bedrijf)
		{
			if (bedrijf == null)
				throw new BedrijfException("bedrijf is null");
			return BestaatBedrijf(bedrijf.Id);
		}

		public bool BestaatBedrijf(uint bedrijf)
		{
			return _bedrijven.ContainsKey(bedrijf);
		}

		public bool BestaatBedrijf(string bedrijfsnaam)
		{
			return _bedrijven.Any(b => b.Value.Naam.ToLower() == bedrijfsnaam.ToLower());
		}

		public Bedrijf GeefBedrijf(uint id)
		{
			if (!_bedrijven.ContainsKey(id))
				throw new BedrijfException("bedrijf niet gevonden");
			return _bedrijven[id];
		}

		public IReadOnlyList<Bedrijf> Geefbedrijven()
		{
			return _bedrijven.Values.ToList();
		}

		public Bedrijf GeefBedrijf(string bedrijfsnaam)
		{
			if (!BestaatBedrijf(bedrijfsnaam))
				throw new BedrijfException("bedrijf niet gevonden");
			return _bedrijven.First(b => b.Value.Naam.ToLower() == bedrijfsnaam.ToLower()).Value;
		}
	}
}