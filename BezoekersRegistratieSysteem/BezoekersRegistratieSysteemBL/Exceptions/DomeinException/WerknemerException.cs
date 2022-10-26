using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {
	public class WerknemerException : Exception {
		public WerknemerException() { }

		public WerknemerException(string? message) : base(message) { }

		public WerknemerException(string? message, Exception? innerException) : base(message, innerException) { }

		protected WerknemerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}