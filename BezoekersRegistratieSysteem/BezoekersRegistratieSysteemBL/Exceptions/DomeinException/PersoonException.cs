using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {
	public class PersoonException : Exception {
		public PersoonException() {}

		public PersoonException(string? message) : base(message) {}

		public PersoonException(string? message, Exception? innerException) : base(message, innerException) {}

		protected PersoonException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}