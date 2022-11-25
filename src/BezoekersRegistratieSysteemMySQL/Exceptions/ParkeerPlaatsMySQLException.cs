using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class ParkeerPlaatsMySQLException : Exception {
        public ParkeerPlaatsMySQLException(string? message) : base(message) {
        }

        public ParkeerPlaatsMySQLException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
