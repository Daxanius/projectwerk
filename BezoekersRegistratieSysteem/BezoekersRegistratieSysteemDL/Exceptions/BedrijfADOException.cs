using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class BedrijfADOException : Exception {
        public BedrijfADOException(string? message) : base(message) {
        }

        public BedrijfADOException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
