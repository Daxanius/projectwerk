using BezoekersRegistratieSysteemBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public abstract class Persoon
    {
        public string Voornaam { get; private set; }
        public string Achternaam { get; private set; }
        public string Email { get; private set; }
        
        public Persoon(string voornaam, string achternaam, string email)
        {
            ZetVoornaam(voornaam);
            ZetAchternaam(achternaam);
            ZetEmail(email);
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
            Email = email;
        }
    }
}
