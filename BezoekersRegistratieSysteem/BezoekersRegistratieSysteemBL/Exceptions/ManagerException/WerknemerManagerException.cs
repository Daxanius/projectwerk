using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.ManagerException
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
