using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class WerknemerInfo
    {
        public Bedrijf Bedrijf { get; private set; }
        public string Email { get; private set; }
        
        private List<string> Functies = new();

        public WerknemerInfo() {}

        public WerknemerInfo(Bedrijf bedrijf, string email)
        {
            ZetBedrijf(bedrijf);
            ZetEmail(email);
        }
        
        /// <summary>
		/// Zet Bedrijf van werknemer.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <exception cref="WerknemerInfoException"></exception>
        public void ZetBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new WerknemerInfoException("WerknemerInfo - ZetBedrijf - bedrijf mag niet leeg zijn");
            Bedrijf = bedrijf;
        }

        /// <summary>
		/// Roept email controle op uit Nutsvoorziening & zet email.
		/// </summary>
		/// <param name="email"></param>
		/// <exception cref="WerknemerInfoException"></exception>
		public void ZetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new WerknemerInfoException("WerknemerInfo - ZetEmail - email mag niet leeg zijn");
            //Checkt of email geldig is
            if (Nutsvoorziening.IsEmailGeldig(email.Trim())) Email = email.Trim();
            else throw new WerknemerInfoException("WerknemerInfo - ZetEmail - email is niet geldig");
        }

        public IReadOnlyList<string> GeefWerknemerFuncties()
        {
            return Functies.AsReadOnly();
        }

        public void VoegWerknemerFunctieToe(string functie)
        {
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerInfoException("WerknemerInfo - VoegWerknemerFunctieToe - functie mag niet leeg zijn");
            if (Functies.Contains(functie)) throw new WerknemerInfoException("WerknemerInfo - VoegWerknemerFunctieToe - werknemer heeft deze functie al");
            Functies.Add(functie);
        }

        public void WijzigWerknemerFunctie(string oudefunctie, string nieuwefunctie)
        {
            if (string.IsNullOrWhiteSpace(oudefunctie)) throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - functie mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(nieuwefunctie)) throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - functie mag niet leeg zijn");
            if (!Functies.Contains(oudefunctie)) throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie niet");
            if (Functies.Contains(nieuwefunctie)) throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie al");
            Functies.Remove(oudefunctie);
            Functies.Add(nieuwefunctie);
        }

        public void VerwijderWerknemerFunctie(string functie)
        {
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - functie mag niet leeg zijn");
            if (!Functies.Contains(functie)) throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer heeft deze functie niet");
            if (Functies.Count == 1) throw new WerknemerInfoException("WerknemerInfo - VerwijderWerknemerFunctie - werknemer moet minstens 1 functie hebben");
            Functies.Remove(functie);
        }

        public bool WerknemerInfoIsGelijk(WerknemerInfo werknemerinfo)
        {
            if (werknemerinfo is null) return false;
            if (werknemerinfo.Bedrijf != Bedrijf) return false;
            if (werknemerinfo.Email != Email) return false;
            foreach (string functie in Functies)
            {
                if (!werknemerinfo.Functies.Contains(functie)) return false;
            }
            return true;
        }  
    }
}
