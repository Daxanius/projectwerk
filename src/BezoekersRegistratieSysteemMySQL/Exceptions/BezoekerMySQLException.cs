namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class BezoekerMySQLException : Exception {

		public BezoekerMySQLException(string? message) : base(message) {
		}

		public BezoekerMySQLException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}