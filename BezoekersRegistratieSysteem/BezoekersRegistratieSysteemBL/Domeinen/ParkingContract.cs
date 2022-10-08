using BezoekersRegistratieSysteemBL.Exceptions;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class ParkingContract
    {
        public DateTime Starttijd { get; private set; }
        public DateTime Eindtijd { get; private set; }
        public int AantalPlaatsen { get; private set; }

        public ParkingContract(DateTime starttijd, DateTime eindtijd, int aantalPlaatsen)
        {
            ZetStarttijd(starttijd);
            ZetEindtijd(eindtijd);
            ZetAantalPlaatsen(aantalPlaatsen);
        }

        public void ZetStarttijd(DateTime starttijd)
        {
            if (starttijd == null)
                throw new ParkingContractException("Starttijd mag niet leeg zijn");
            Starttijd = starttijd;
        }

        public void ZetEindtijd(DateTime eindtijd)
        {
            if (eindtijd == null)
                throw new ParkingContractException("Eindtijd mag niet leeg zijn");
            Eindtijd = eindtijd;
        }

        public void ZetAantalPlaatsen(int aantalPlaatsen)
        {
            if (aantalPlaatsen < 0)
                throw new ParkingContractException("Aantal plaatsen mag niet leeg zijn");
            AantalPlaatsen = aantalPlaatsen;
        }
    }
}
