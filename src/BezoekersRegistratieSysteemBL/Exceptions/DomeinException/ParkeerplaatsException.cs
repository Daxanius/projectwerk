using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {
	public class ParkeerplaatsException : Exception {
		public ParkeerplaatsException() {
		}

		public ParkeerplaatsException(string? message) : base(message) {
		}

		public ParkeerplaatsException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected ParkeerplaatsException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}