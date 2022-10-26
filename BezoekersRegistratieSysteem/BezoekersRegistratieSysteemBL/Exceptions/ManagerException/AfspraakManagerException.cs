using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.ManagerException {
	public class AfspraakManagerException : Exception {
		public AfspraakManagerException() { }

		public AfspraakManagerException(string? message) : base(message) { }

		public AfspraakManagerException(string? message, Exception? innerException) : base(message, innerException) { }

		protected AfspraakManagerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}