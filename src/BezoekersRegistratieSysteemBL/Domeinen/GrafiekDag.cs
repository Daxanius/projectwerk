using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen {
    public class GrafiekDag {
        private Dictionary<string, int> XwaardeCheckInsPerUur { get; set; } = new Dictionary<string, int>();
        private Dictionary<string, int> XwaardeGeparkeerdenTotaalPerUur { get; set; } = new Dictionary<string, int>();
        public void VoegWaardesToe(string xwaarde, int CheckInsPerUur, int GeparkeerdenTotaalPerUur) {
            if (XwaardeCheckInsPerUur.ContainsKey(xwaarde)) {
                throw new Exception("Bevat al deze waarde");
            }
            XwaardeCheckInsPerUur.Add(xwaarde, CheckInsPerUur);
            XwaardeGeparkeerdenTotaalPerUur.Add(xwaarde, GeparkeerdenTotaalPerUur);
        }
        public IReadOnlyDictionary<string, int> GeefXwaardeCheckInsPerUur() {
            return XwaardeCheckInsPerUur;
        }
        public IReadOnlyDictionary<string, int> GeefXwaardeGeparkeerdenTotaalPerUur() {
            return XwaardeGeparkeerdenTotaalPerUur;
        }
    }
}
