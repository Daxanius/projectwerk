using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System.Text.RegularExpressions;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public abstract class Persoon
    {
        public uint Id { get; private set; }
        public string Voornaam { get; private set; }
        public string Achternaam { get; private set; }
        public string Email { get; private set; }

        public Persoon(uint id, string voornaam, string achternaam, string email)
        {
            ZetId(id);
            ZetVoornaam(voornaam);
            ZetAchternaam(achternaam);
            ZetEmail(email);
        }

        public void ZetId(uint id)
        {
            Id = id;
        }
        public void ZetVoornaam(string voornaam)
        {
            if (string.IsNullOrWhiteSpace(voornaam)) throw new PersoonException("Voornaam mag niet leeg zijn");
            Voornaam = voornaam;
        }
        public void ZetAchternaam(string achternaam)
        {
            if (string.IsNullOrWhiteSpace(achternaam)) throw new PersoonException("Achternaam mag niet leeg zijn");
            Achternaam = achternaam;
        }
        public void ZetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new PersoonException("Email mag niet leeg zijn");
            //Email Authenticator
            string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            //
            if (regex.IsMatch(email.Trim())) Email = email.Trim();
            else throw new PersoonException("Email is niet geldig");
        }
    }
}
