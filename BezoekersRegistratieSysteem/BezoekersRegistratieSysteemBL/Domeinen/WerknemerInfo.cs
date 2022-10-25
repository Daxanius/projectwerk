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
        
        public List<string> Functies = new List<string>();

        public WerknemerInfo() {}

        public WerknemerInfo(Bedrijf bedrijf, string email)
        {
            ZetBedrijf(bedrijf);
            ZetEmail(email);
        }
        
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
    }
}
