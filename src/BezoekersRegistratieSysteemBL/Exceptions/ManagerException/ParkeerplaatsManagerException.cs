using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.ManagerException {
	public class ParkeerplaatsManagerException : Exception {
		public ParkeerplaatsManagerException() {
		}

		public ParkeerplaatsManagerException(string? message) : base(message) {
		}

		public ParkeerplaatsManagerException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected ParkeerplaatsManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}