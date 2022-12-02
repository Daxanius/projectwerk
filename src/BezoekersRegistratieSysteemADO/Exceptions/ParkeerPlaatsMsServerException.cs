using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class ParkeerPlaatsMsServerException : Exception {
        public ParkeerPlaatsMsServerException(string? message) : base(message) {
        }

        public ParkeerPlaatsMsServerException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
