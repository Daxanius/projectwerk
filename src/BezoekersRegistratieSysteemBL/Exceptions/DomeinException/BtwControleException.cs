using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {

	public class BtwControleException : Exception {

		public BtwControleException() {
		}

		public BtwControleException(string? message) : base(message) {
		}

		public BtwControleException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected BtwControleException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}