using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.ADOMS {
    internal class ParkeerPlaatsADOMS : IParkeerplaatsRepository {
        /// <summary>
		/// Private lokale variabele connectiestring
		/// </summary>
		private string _connectieString;

        /// <summary>
        /// ParkeerPlaatsADOMS constructor krijgt connectie string als parameter.
        /// </summary>
        /// <param name="connectieString">Connectie string database</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
        public ParkeerPlaatsADOMS(string connectieString) {
            _connectieString = connectieString;
        }
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
