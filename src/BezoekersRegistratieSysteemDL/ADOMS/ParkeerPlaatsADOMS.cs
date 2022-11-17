using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.ADOMS {
    internal class ParkeerPlaatsADOMS : IParkeerplaatsRepository {
        public bool BestaatNummerplaat(string nummerplaat) {
            throw new NotImplementedException();
        }

        public void CheckNummerplaatIn(Parkeerplaats parkeerplaats) {
            throw new NotImplementedException();
        }

        public void CheckNummerplaatUit(string nummerplaat) {
            throw new NotImplementedException();
        }

        public IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf) {
            throw new NotImplementedException();
        }
    }
}
