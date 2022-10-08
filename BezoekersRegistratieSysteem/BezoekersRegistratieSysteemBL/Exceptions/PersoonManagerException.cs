using System.Runtime.Serialization;

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
