using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.ManagerException {

	public class BezoekerManagerException : Exception {

		public BezoekerManagerException() {
		}

		public BezoekerManagerException(string message) : base(message) {
		}

		public BezoekerManagerException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected BezoekerManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}