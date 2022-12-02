namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class BezoekerMsServerException : Exception {

		public BezoekerMsServerException(string? message) : base(message) {
		}

		public BezoekerMsServerException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}