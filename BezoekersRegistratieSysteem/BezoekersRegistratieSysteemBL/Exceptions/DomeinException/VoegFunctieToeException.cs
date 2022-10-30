using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException
{
    public class VoegFunctieToeException : Exception
    {
        public VoegFunctieToeException()
        {
        }

        public VoegFunctieToeException(string? message) : base(message)
        {
        }

        public VoegFunctieToeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected VoegFunctieToeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}