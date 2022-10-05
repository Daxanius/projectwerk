using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Exceptions
{
    public class PersoonManagerException : Exception
    {
        public PersoonManagerException(string message) : base(message)
        {
        }
        
        public PersoonManagerException()
        {
        }

        public PersoonManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected PersoonManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
