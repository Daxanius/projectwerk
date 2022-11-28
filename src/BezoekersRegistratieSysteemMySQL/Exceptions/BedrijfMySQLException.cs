namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class BedrijfMySQLException : Exception {

		public BedrijfMySQLException(string? message) : base(message) {
		}

		public BedrijfMySQLException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}