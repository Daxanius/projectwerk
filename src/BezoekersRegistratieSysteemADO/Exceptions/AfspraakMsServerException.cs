namespace BezoekersRegistratieSysteemDL.Exceptions {

	public class AfspraakMsServerException : Exception {

		public AfspraakMsServerException(string? message) : base(message) {
		}

		public AfspraakMsServerException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}