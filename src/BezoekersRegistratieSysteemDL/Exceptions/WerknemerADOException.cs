namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class WerknemerADOException : Exception {

		public WerknemerADOException(string? message) : base(message) {
		}

		public WerknemerADOException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}