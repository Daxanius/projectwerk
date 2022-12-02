namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class WerknemerMsServerException : Exception {

		public WerknemerMsServerException(string? message) : base(message) {
		}

		public WerknemerMsServerException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}