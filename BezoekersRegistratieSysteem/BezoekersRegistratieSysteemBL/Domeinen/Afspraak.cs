using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
    /// <summary>
    /// Een klasse die de aanwezigheid van bezoekers
    /// bijhoudt
    /// </summary>
    public class Afspraak {
        public uint Id { get; private set; }
        public DateTime Starttijd { get; private set; }
        public DateTime? Eindtijd { get; private set; }
        public Bezoeker Bezoeker { get; private set; }
        public Werknemer Werknemer { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="starttijd"></param>
        /// <param name="bezoeker"></param>
        /// <param name="werknemer"></param>
        public Afspraak(DateTime starttijd, Bezoeker bezoeker, Werknemer werknemer) {
            ZetStarttijd(starttijd);
            ZetBezoeker(bezoeker);
            ZetWerknemer(werknemer);
        }

        /// <summary>
        /// Past de ID aan
        /// </summary>
        /// <param name="id"></param>
        public void ZetId(uint id) {
            Id = id;
        }

        /// <summary>
        /// Past de starttijd aan
        /// </summary>
        /// <param name="starttijd"></param>
        /// <exception cref="AfspraakException"></exception>
        public void ZetStarttijd(DateTime starttijd) {
            if (Eindtijd is not null) throw new AfspraakException("Afspraak - ZetStarttijd - Afspraak is al afgelopen");
            Starttijd = starttijd;
        }

        /// <summary>
        /// Past de eindtijd aan
        /// </summary>
        /// <param name="eindtijd"></param>
        /// <exception cref="AfspraakException"></exception>
        public void ZetEindtijd(DateTime eindtijd) {
            if (eindtijd <= Starttijd) throw new AfspraakException("Afspraak - ZetEindtijd - Eindtijd moet na starttijd liggen");
            Eindtijd = eindtijd;
        }

        /// <summary>
        /// Past de bezoeker aan
        /// </summary>
        /// <param name="bezoeker"></param>
        /// <exception cref="AfspraakException"></exception>
        public void ZetBezoeker(Bezoeker bezoeker) {
            Bezoeker = bezoeker ?? throw new AfspraakException("Afspraak - ZetBezoeker - Bezoeker mag niet leeg zijn");
        }

        /// <summary>
        /// Past de werknemer aan
        /// </summary>
        /// <param name="werknemer"></param>
        /// <exception cref="AfspraakException"></exception>
        public void ZetWerknemer(Werknemer werknemer) {
            Werknemer = werknemer ?? throw new AfspraakException("Afspraak - ZetWerknemer - Werknemer mag niet leeg zijn");
        }
    }
}