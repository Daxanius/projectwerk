using BezoekersRegistratieSysteemBL.Exceptions;
using System.Text.RegularExpressions;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Bedrijf
    {
        public uint Id { get; private set; }
        public string Naam { get; private set; }
        public string BTW { get; private set; }
        public string TelefoonNummer { get; private set; }
        public string Email { get; private set; }
        public string Adres { get; private set; }

        private List<Werknemer> _werknemers = new List<Werknemer>();

        public Bedrijf(uint id, string naam, string btw, string telefoonNummer, string email, string adres)
        {
            ZetId(id);
            ZetNaam(naam);
            ZetBTW(btw);
            ZetTelefoonNummer(telefoonNummer);
            ZetEmail(email);
            ZetAdres(adres);
        }

        public void ZetId(uint id)
        {
            Id = id;
        }

        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new BedrijfException("Naam mag niet leeg zijn");
            Naam = naam;
        }

        public void ZetBTW(string btw)
        {
            if (string.IsNullOrWhiteSpace(btw)) throw new BedrijfException("BTW mag niet leeg zijn");
            BTW = btw;
        }

        public void ZetTelefoonNummer(string telefoonNummer)
        {
            if (string.IsNullOrWhiteSpace(telefoonNummer)) throw new BedrijfException("Telefoonnummer mag niet leeg zijn");
            TelefoonNummer = telefoonNummer;
        }

        public void ZetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new BedrijfException("Email mag niet leeg zijn");
            //Email Authenticator
            string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            //
            if (regex.IsMatch(email.Trim())) Email = email.Trim();
            else throw new BedrijfException("Email is niet geldig");
        }

        public void ZetAdres(string adres)
        {
            if (string.IsNullOrWhiteSpace(adres)) throw new BedrijfException("Adres mag niet leeg zijn");
            Adres = adres;
        }

        public void VoegWerknemerToe(Werknemer werknemer)
        {
            if (werknemer == null) throw new BedrijfException("Werknemer mag niet leeg zijn");
            if (_werknemers.Contains(werknemer)) throw new BedrijfException("Werknemer bestaat al");
            _werknemers.Add(werknemer);
            if (werknemer.Bedrijf == null || werknemer.Bedrijf != this) werknemer.ZetBedrijf(this);
        }

        public void VerwijderWerknemer(Werknemer werknemer)
        {
            if (werknemer == null) throw new BedrijfException("Werknemer mag niet leeg zijn");
            if (!_werknemers.Contains(werknemer)) throw new BedrijfException("Werknemer bestaat niet");
            _werknemers.Remove(werknemer);
            werknemer.VerwijderBedrijf();
        }

        public IReadOnlyList<Werknemer> GeefWerknemers()
        {
            return _werknemers.AsReadOnly();
        }
    }
}
