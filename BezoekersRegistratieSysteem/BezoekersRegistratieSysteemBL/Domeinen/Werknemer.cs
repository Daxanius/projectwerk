using BezoekersRegistratieSysteemBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public abstract class Werknemer : Persoon
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
            Bedrijf = bedrijf;
        }
        
        public void ZetFunctie(string functie)
        {
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerException("Functie mag niet leeg zijn");
            Functie = functie;
        }
    }
}
