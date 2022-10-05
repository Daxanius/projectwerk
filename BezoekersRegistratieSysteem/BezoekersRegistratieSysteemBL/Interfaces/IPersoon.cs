using BezoekersRegistratieSysteemBL.Domeinen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
    public interface IPersoon
    {
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Email { get; set; }
        public Bedrijf bedrijf { get; set; }
    }
}
