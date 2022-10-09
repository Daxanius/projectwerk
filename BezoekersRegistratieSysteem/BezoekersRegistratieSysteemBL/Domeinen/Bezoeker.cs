using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Bezoeker : Persoon
    {
        public string Bedrijf { get; private set; }

        public Bezoeker(string voornaam, string achternaam, string email, string bedrijf) : base(voornaam, achternaam, email)
        {
            ZetBedrijf(bedrijf);
        }

        public void ZetBedrijf(string bedrijf)
        {
            if (string.IsNullOrWhiteSpace(bedrijf)) throw new BezoekerException("Bedrijf mag niet leeg zijn");
            Bedrijf = bedrijf;
        }
    }
}
