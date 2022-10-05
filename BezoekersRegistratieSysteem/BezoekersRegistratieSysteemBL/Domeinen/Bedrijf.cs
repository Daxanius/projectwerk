using BezoekersRegistratieSysteemBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Bedrijf
    {
        public string Naam { get; private set; }
        public string BTW { get; private set; }
        public string TelefoonNummer { get; private set; }
        public string Email { get; private set; }
        public string Adres { get; private set; }

        public Bedrijf(string naam, string btw, string telefoonNummer, string email, string adres)
        {
            ZetNaam(naam);
            ZetBTW(btw);
            ZetTelefoonNummer(telefoonNummer);
            ZetEmail(email);
            ZetAdres(adres);
        }

        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam))
                throw new BedrijfException("Naam mag niet leeg zijn");
            Naam = naam;
        }

        public void ZetBTW(string btw)
        {
            if (string.IsNullOrWhiteSpace(btw))
                throw new BedrijfException("BTW mag niet leeg zijn");
            BTW = btw;
        }

        public void ZetTelefoonNummer(string telefoonNummer)
        {
            if (string.IsNullOrWhiteSpace(telefoonNummer))
                throw new BedrijfException("Telefoonnummer mag niet leeg zijn");
            TelefoonNummer = telefoonNummer;
        }

        public void ZetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new BedrijfException("Email mag niet leeg zijn");
            Email = email;
        }

        public void ZetAdres(string adres)
        {
            if (string.IsNullOrWhiteSpace(adres))
                throw new BedrijfException("Adres mag niet leeg zijn");
            Adres = adres;
        }
    }
}
