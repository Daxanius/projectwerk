namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class BezoekerADOException : Exception {

		public BezoekerADOException(string? message) : base(message) {
		}

		public BezoekerADOException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}