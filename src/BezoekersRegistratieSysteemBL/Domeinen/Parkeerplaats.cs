using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Parkeerplaats
    {
        public Bedrijf Bedrijf { get; private set; }
        public DateTime Starttijd { get; private set; }
        public DateTime? Eindtijd { get; private set; }
        public string Nummerplaat { get; private set; }

        public Parkeerplaats()
        {
        }

        public Parkeerplaats(Bedrijf bedrijf, DateTime starttijd, string nummerplaat)
        {
            Bedrijf = bedrijf;
            Starttijd = starttijd;
            Nummerplaat = nummerplaat;
        }

        public Parkeerplaats(Bedrijf bedrijf, DateTime starttijd, DateTime? eindtijd, string nummerplaat)
        {
            Bedrijf = bedrijf;
            Starttijd = starttijd;
            Nummerplaat = nummerplaat;
        }

        public void ZetBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null)
                throw new ParkeerplaatsException("Parkeerplaats - ZetBedrijf - Bedrijf mag niet leeg zijn");
            Bedrijf = bedrijf;
        }

        public void ZetStarttijd(DateTime starttijd)
        {
            if (Eindtijd is not null)
                throw new ParkeerplaatsException("Parkeerplaats - ZetStarttijd - Parking is al afgelopen");
            Starttijd = starttijd;
        }

        public void ZetEindtijd(DateTime eindtijd)
        {
            if (eindtijd < Starttijd)
                throw new ParkeerplaatsException("Parkeerplaats - ZetEindtijd - Parking is al afgelopen");
            Eindtijd = eindtijd;
        }

        public void ZetNummerplaat(string nummerplaat)
        {
            if (string.IsNullOrWhiteSpace(nummerplaat))
                throw new ParkeerplaatsException("Parkeerplaats - ZetNummerplaat - Nummerplaat mag niet leeg zijn");
            //if (Nutsvoorziening.IsNummerplaatGeldig(nummerplaat))
            //throw new ParkeerplaatsException("Parkeerplaats - ZetNummerplaat - Nummerplaat is niet geldig");
            if (nummerplaat.Length > 9)
                throw new ParkeerplaatsException("Parkeerplaats - ZetNummerplaat - Nummerplaat mag niet langer zijn dan 9 karakters");
            Nummerplaat = nummerplaat.Trim();
        }
    }
}
