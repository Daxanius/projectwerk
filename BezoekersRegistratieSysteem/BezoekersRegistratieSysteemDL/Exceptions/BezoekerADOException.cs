using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class BezoekerADOException : Exception {
        public BezoekerADOException(string? message) : base(message) {
        }

        public BezoekerADOException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
