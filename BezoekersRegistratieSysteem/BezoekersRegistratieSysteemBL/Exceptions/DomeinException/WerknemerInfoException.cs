using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {
	public class WerknemerInfoException : Exception {
		public WerknemerInfoException() {
		}

		public WerknemerInfoException(string? message) : base(message) {
		}

		public WerknemerInfoException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected WerknemerInfoException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}