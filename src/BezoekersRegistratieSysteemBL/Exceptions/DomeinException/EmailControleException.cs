using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {

	public class EmailControleException : Exception {

		public EmailControleException() {
		}

		public EmailControleException(string? message) : base(message) {
		}

		public EmailControleException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected EmailControleException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}