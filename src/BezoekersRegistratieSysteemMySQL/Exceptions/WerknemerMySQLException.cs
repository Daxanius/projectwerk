namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class WerknemerMySQLException : Exception {

		public WerknemerMySQLException(string? message) : base(message) {
		}

		public WerknemerMySQLException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}