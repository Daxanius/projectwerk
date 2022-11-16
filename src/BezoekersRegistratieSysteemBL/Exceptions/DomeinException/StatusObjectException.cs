using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException
{
    public class StatusObjectException : Exception
    {
        public StatusObjectException()
        {
        }

        public StatusObjectException(string? message) : base(message)
        {
        }

        public StatusObjectException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected StatusObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}