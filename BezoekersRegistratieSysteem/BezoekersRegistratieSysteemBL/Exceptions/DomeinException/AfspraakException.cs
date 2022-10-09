using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {
	public class AfspraakException : Exception {
		public AfspraakException() {}

		public AfspraakException(string? message) : base(message) {}

		public AfspraakException(string? message, Exception? innerException) : base(message, innerException) {}

		protected AfspraakException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}