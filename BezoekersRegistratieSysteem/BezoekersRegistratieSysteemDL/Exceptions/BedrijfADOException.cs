namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class BedrijfADOException : Exception {

		public BedrijfADOException(string? message) : base(message) {
		}

		public BedrijfADOException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}