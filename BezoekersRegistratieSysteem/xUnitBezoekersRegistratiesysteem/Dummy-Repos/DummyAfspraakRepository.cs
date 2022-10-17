using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace xUnitBezoekersRegistratiesysteem.DummyData.Repos
{
    public class DummyAfspraakRepository : IAfspraakRepository
    {
        private readonly Dictionary<uint, Afspraak> _afspraken = new();

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

        public DummyAfspraakRepository()
        {
            Bedrijf1 = new("aNaam", "BE0676747521", "0476687242", "a@gmail.com", "a 101 9000");
            Bedrijf2 = new("bNaam", "BE0676747521", "0476687246", "b@gmail.com", "b 101 9000");
            Bedrijf3 = new("cNaam", "BE0676747521", "0476687248", "c@gmail.com", "c 101 9000");

            Bedrijf1.ZetId(1);
            Bedrijf2.ZetId(2);
            Bedrijf3.ZetId(3);

            Werknemer1 = new("dVoorNaam", "dAchterNaam", "d@gmail.com", Bedrijf1, "dFuntie");
            Werknemer2 = new("eVoorNaam", "eAchterNaam", "e@gmail.com", Bedrijf2, "eFuntie");
            Werknemer3 = new("fVoorNaam", "fAchterNaam", "f@gmail.com", Bedrijf3, "fFuntie");

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

            _afspraken.Add(1, Afspraak1);
            _afspraken.Add(2, Afspraak2);
            _afspraken.Add(3, Afspraak3);
        }

        public void BeeindigAfspraak(uint werknemerId)
        {
            if (!_afspraken.ContainsKey(werknemerId))
                throw new AfspraakException("id does not exist");
            _afspraken[werknemerId].ZetEindtijd(DateTime.Now);
        }

        public void BewerkAfspraak(Afspraak afspraak)
        {
            if (!_afspraken.ContainsKey(afspraak.Id))
                throw new AfspraakException("id does not exist");
            _afspraken[afspraak.Id] = afspraak;

        }

        public Afspraak GeefAfspraakOpId(uint afspraakId)
        {
            if (!_afspraken.ContainsKey(afspraakId))
                throw new AfspraakException("id bestaat niet");
            return _afspraken[afspraakId];
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum)
        {
            return _afspraken.Where(a => a.Value.Starttijd.Date == datum.Date).Select(a => a.Value).ToList();
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum)
        {
            return _afspraken.Where(a => a.Value.Werknemer.Id == werknemerId && a.Value.Starttijd.Date == datum.Date).Select(a => a.Value).ToList();
        }

        public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId)
        {
            return _afspraken.Where(a => a.Value.Werknemer.Id == werknemerId).Select(a => a.Value).ToList();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfspraken()
        {
            return _afspraken.Where(a => a.Value.Eindtijd == null && a.Value.Starttijd < DateTime.Now).Select(a => a.Value).ToList();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId)
        {
            return _afspraken.Where(a => a.Value.Werknemer.Bedrijf != null && a.Value.Werknemer.Bedrijf.Id == bedrijfId && a.Value.Eindtijd == null && a.Value.Starttijd < DateTime.Now).Select(a => a.Value).ToList();
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint werknemerId)
        {
            return _afspraken.Where(a => a.Value.Werknemer.Id == werknemerId && a.Value.Eindtijd == null && a.Value.Starttijd < DateTime.Now).Select(a => a.Value).ToList();
        }

        public void VerwijderAfspraak(uint werknemerId)
        {
            if (!_afspraken.ContainsKey(werknemerId))
                throw new AfspraakException("id does not exist");
            _afspraken.Remove(werknemerId);
        }

        public void VoegAfspraakToe(Afspraak afspraak)
        {
            if (afspraak == null)
                throw new AfspraakException("afspraak is null");
            if (_afspraken.ContainsKey(afspraak.Id))
                throw new AfspraakException("id bestaat al");
            _afspraken.Add(afspraak.Id, afspraak);
        }
    }
}