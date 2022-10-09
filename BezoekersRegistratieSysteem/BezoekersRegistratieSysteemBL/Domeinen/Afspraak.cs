using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
    public class Afspraak {
        public uint Id { get; private set; }
        public DateTime Starttijd { get; private set; }
        public DateTime? Eindtijd { get; private set; }
        public Bezoeker Bezoeker { get; private set; }
        public Werknemer Werknemer { get; private set; }

        public Afspraak(DateTime starttijd, Bezoeker bezoeker, Werknemer werknemer) {
            ZetStarttijd(starttijd);
            ZetBezoeker(bezoeker);
            ZetWerknemer(werknemer);
        }

        public void ZetId(uint id) {
            Id = id;
        }

        public void ZetStarttijd(DateTime starttijd) {
            if (Eindtijd is not null) throw new AfspraakException("afspraak reeds beëindigd");
            Starttijd = starttijd;
        }

        public void ZetEindtijd(DateTime eindtijd) {
            if (eindtijd <= Starttijd) throw new AfspraakException("eindtijd moet na starttijd liggen");
            Eindtijd = eindtijd;
        }

        public void ZetBezoeker(Bezoeker bezoeker) {
            Bezoeker = bezoeker ?? throw new AfspraakException("bezoeker mag niet leeg zijn");
        }

        public void ZetWerknemer(Werknemer werknemer) {
            Werknemer = werknemer ?? throw new AfspraakException("werknemer mag niet leeg zijn");
        }
    }
}