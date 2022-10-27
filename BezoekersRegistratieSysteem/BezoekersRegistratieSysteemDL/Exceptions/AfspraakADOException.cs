using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class AfspraakADOException : Exception {
        public AfspraakADOException(string? message) : base(message) {
        }

        public AfspraakADOException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
