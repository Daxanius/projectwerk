namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class BedrijfMsServerException : Exception {

		public BedrijfMsServerException(string? message) : base(message) {
		}

		public BedrijfMsServerException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}