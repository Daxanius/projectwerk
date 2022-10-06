using BezoekersRegistratieSysteemBL.Domeinen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Managers
{
    public class AfspraakManager
    {
        public void MaakAfspraak(Afspraak afspraak)
        {
            
        }

        public void VerwijderAfspraak(Afspraak afspraak)
        {

        }

        public void BewerkAfspraak(Afspraak afspraak)
        {

        }
        
        public void BeeindigAfspraak(Afspraak afspraak)
        {
            
        }

        public IReadOnlyList<Afspraak> GeefHuidigeAfspraken(DateTime tijdstip)
        {
            return null;
        }

        public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemer(Werknemer werknemer)
        {
            return null;
        }
        
        public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum)
        {
            return null;
        }
    }
}
