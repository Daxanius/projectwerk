using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
    public class Grafiek
    {
        public string Ywaarde { get; set; }
        public int XwaardeGeparkeerdenTotaalPerDag { get; set; }
        public int XwaardeGeparkeerdenUurlijksPerDag { get; set; }

        public int XwaardeGeparkeerdenTotaalPerWeek { get; set; }

        public Grafiek(string ywaarde, int xwaardeGeparkeerdenTotaalPerDag, int xwaardeGeparkeerdenUurlijksPerDag)
        {
            Ywaarde = ywaarde;
            XwaardeGeparkeerdenTotaalPerDag = xwaardeGeparkeerdenTotaalPerDag;
            XwaardeGeparkeerdenUurlijksPerDag = xwaardeGeparkeerdenUurlijksPerDag;
        }

        public Grafiek(string ywaarde, int xwaardeGeparkeerdenTotaalPerWeek)
        {
            Ywaarde = ywaarde;
            XwaardeGeparkeerdenTotaalPerWeek = xwaardeGeparkeerdenTotaalPerWeek;
        }
    }
}
