using BezoekersRegistratieSysteemBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public abstract class Bezoeker : Persoon
    {
        public string Bedrijf { get; private set; }

        public Bezoeker(uint id, string voornaam, string achternaam, string email, string bedrijf) : base(id, voornaam, achternaam, email)
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
