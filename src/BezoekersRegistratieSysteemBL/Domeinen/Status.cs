using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Domeinen {
    public abstract class Status {
        protected Status() {
        }
        protected Status(string statusNaam) {
            ZetStatusNaam(statusNaam);
        }
        public string StatusNaam { get; private set; }
        public void ZetStatusNaam(string statusNaam) {
            if (String.IsNullOrWhiteSpace(statusNaam)) {
                throw new StatusException("Status naam is leeg");
            }
            StatusNaam = Nutsvoorziening.NaamOpmaak(statusNaam);
        }
    }
}
