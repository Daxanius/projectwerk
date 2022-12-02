namespace BezoekersRegistratieSysteemDL.Exceptions {
	public class ParkeerPlaatsMySQLException : Exception {
		public ParkeerPlaatsMySQLException(string? message) : base(message) {
		}

		public ParkeerPlaatsMySQLException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
