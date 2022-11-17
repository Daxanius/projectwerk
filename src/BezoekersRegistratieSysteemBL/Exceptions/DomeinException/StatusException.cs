using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException
{
    public class StatusException : Exception
    {
        public StatusException()
        {
        }

        public StatusException(string? message) : base(message)
        {
        }

        public StatusException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected StatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}