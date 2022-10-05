using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions
{
    public class BedrijfManagerException : Exception
    {
        public BedrijfManagerException()
        {
        }

        public BedrijfManagerException(string? message) : base(message)
        {
        }

        public BedrijfManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BedrijfManagerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}