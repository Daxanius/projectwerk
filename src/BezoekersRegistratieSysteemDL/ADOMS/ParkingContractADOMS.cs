using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.ADOMS {
    public class ParkingContractADOMS : IParkingContractRepository {
        /// <summary>
        /// Private lokale variabele connectiestring
        /// </summary>
        private string _connectieString;

        /// <summary>
        /// ParkingContractADOMS constructor krijgt connectie string als parameter.
        /// </summary>
        /// <param name="connectieString">Connectie string database</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_connectieString] gelijk aan de connectie string parameter.</remarks>
        public ParkingContractADOMS(string connectieString) {
            _connectieString = connectieString;
        }

        /// <summary>
        /// Zet SQL connectie op met desbetreffende database adv de lokale variabele [_connectieString].
        /// </summary>
        /// <returns>SQL connectie</returns>
        private SqlConnection GetConnection() {
            return new SqlConnection(_connectieString);
        }
        public bool BestaatParkingContract(ParkingContract parkingContract) {
            throw new NotImplementedException();
        }

        public void BewerkParkingContract(ParkingContract parkingContract) {
            throw new NotImplementedException();
        }

        public ParkingContract GeefParkingContract(long bedrijfId) {
            throw new NotImplementedException();
        }

        public void VerwijderParkingContract(ParkingContract parkingContract) {
            throw new NotImplementedException();
        }

        public void VoegParkingContractToe(ParkingContract parkingContract) {
            throw new NotImplementedException();
        }
    }
}
