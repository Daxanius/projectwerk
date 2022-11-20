using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class ParkeerPlaatsADOException : Exception {
        public ParkeerPlaatsADOException(string? message) : base(message) {
        }

        public ParkeerPlaatsADOException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
