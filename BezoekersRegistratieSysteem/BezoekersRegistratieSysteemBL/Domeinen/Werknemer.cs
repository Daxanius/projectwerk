using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Werknemer : Persoon
    {
        public Bedrijf Bedrijf { get; private set; }
        public string Functie { get; private set; }

        public Werknemer(string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie) : base(voornaam, achternaam, email)
        {
            ZetBedrijf(bedrijf);
            ZetFunctie(functie);
        }

        public void ZetBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new WerknemerException("Bedrijf mag niet leeg zijn");
            if (bedrijf.Equals(Bedrijf)) throw new WerknemerException("Bedrijf mag niet leeg zijn");
            if (Bedrijf != null) Bedrijf.VerwijderWerknemer(this);
            Bedrijf = bedrijf;
            if (!bedrijf.GeefWerknemers().Contains(this)) bedrijf.VoegWerknemerToe(this);
        }

        public void VerwijderBedrijf()
        {
            if (Bedrijf == null) throw new WerknemerException("Bedrijf is al leeg");
            if (Bedrijf.GeefWerknemers().Contains(this)) Bedrijf.VerwijderWerknemer(this);
            Bedrijf = null;
        }

        public void ZetFunctie(string functie)
        {
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Functie mag niet leeg zijn");
            Functie = functie;
        }
    }
}
