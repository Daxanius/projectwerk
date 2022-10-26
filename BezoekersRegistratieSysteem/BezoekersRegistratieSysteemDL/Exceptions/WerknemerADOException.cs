using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class WerknemerADOException : Exception {
        public WerknemerADOException(string? message) : base(message) {
        }

        public WerknemerADOException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
