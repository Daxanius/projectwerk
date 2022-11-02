using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.ManagerException {

	public class ParkeerManagerException : Exception {

		public ParkeerManagerException() {
		}

		public ParkeerManagerException(string? message) : base(message) {
		}

		public ParkeerManagerException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected ParkeerManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}