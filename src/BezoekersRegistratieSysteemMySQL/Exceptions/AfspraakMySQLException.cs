namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class AfspraakMySQLException : Exception {

		public AfspraakMySQLException(string? message) : base(message) {
		}

		public AfspraakMySQLException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}