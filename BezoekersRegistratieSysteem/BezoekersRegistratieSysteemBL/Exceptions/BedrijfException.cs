using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions
{
    public class BedrijfException : Exception
    {
        public BedrijfException()
        {
        }

        public BedrijfException(string? message) : base(message)
        {
        }

        public BedrijfException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected BedrijfException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}