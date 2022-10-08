using BezoekersRegistratieSysteemBL.Exceptions;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Afspraak
    {
        public uint Id { get; private set; }
        public DateTime Starttijd { get; private set; }
        public DateTime Eindtijd { get; private set; }
        public Bezoeker Bezoeker { get; private set; }
        public Werknemer Werknemer { get; private set; }

        public Afspraak(uint id, DateTime starttijd, DateTime eindtijd, Bezoeker bezoeker, Werknemer werknemer)
        {
            ZetId(id);
            ZetStarttijd(starttijd);
            ZetEindtijd(eindtijd);
            ZetBezoeker(bezoeker);
            ZetWerknemer(werknemer);
        }
        public void ZetId(uint id)
        {
            Id = id;
        }

        public void ZetStarttijd(DateTime starttijd)
        {
            if (starttijd == null) throw new AfspraakException("starttijd mag niet leeg zijn");
            Starttijd = starttijd;
        }

        public void ZetEindtijd(DateTime eindtijd)
        {
            if (eindtijd == null) throw new AfspraakException("eindtijd mag niet leeg zijn");
            Eindtijd = eindtijd;
        }

        public void ZetBezoeker(Bezoeker bezoeker)
        {
            if (bezoeker == null) throw new AfspraakException("bezoeker mag niet leeg zijn");
            Bezoeker = bezoeker;
        }

        public void ZetWerknemer(Werknemer werknemer)
        {
            if (werknemer == null) throw new AfspraakException("werknemer mag niet leeg zijn");
            Werknemer = werknemer;
        }
    }
}
