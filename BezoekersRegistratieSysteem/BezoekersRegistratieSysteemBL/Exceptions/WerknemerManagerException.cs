using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Exceptions
{
    public class WerknemerManagerException : Exception
    {
        public WerknemerManagerException()
        {
        }

        public WerknemerManagerException(string message) : base(message)
        {

        }

        public WerknemerManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected WerknemerManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
