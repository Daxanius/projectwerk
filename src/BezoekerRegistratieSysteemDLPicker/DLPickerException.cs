using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekerRegistratieSysteemDLPicker {
    public class DLPickerException : Exception {
        public DLPickerException(string? message) : base(message) {
        }

        public DLPickerException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
