using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public abstract class Werknemer : IPersoon
    {
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public Bedrijf bedrijf { get; set; }
        public string Functie { get; set; }
        public Werknemer(string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie) : base(voornaam, achternaam, email, bedrijf)
        {
            ZetFunctie(functie);
        }
        
        public void ZetFunctie(string functie)
        {
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Functie mag niet leeg zijn");
            Functie = functie;
        }
    }
}
