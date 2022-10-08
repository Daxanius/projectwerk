using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Werknemer : Persoon
    {
        public Bedrijf Bedrijf { get; private set; }
        public string Functie { get; private set; }

        public Werknemer(uint id, string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie) : base(id, voornaam, achternaam, email)
        {
            ZetBedrijf(bedrijf);
            ZetFunctie(functie);
        }

        public void ZetBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new WerknemerException("Bedrijf mag niet leeg zijn");
            Bedrijf = bedrijf;
        }

        public void VerwijderBedrijf()
        {
            if (Bedrijf != null)
                if (Bedrijf.GeefWerknemers().Contains(this))
                    Bedrijf.VerwijderWerknemer(this);
            Bedrijf = null;
        }

        public void ZetFunctie(string functie)
        {
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Functie mag niet leeg zijn");
            Functie = functie;
        }
    }
}
