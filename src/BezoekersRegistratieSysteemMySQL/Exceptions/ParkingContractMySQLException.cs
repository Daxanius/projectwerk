namespace BezoekersRegistratieSysteemDL.Exceptions {
	public class ParkingContractMySQLException : Exception {
		public ParkingContractMySQLException(string? message) : base(message) {
		}

		public ParkingContractMySQLException(string? message, Exception? innerException) : base(message, innerException) {
		}
	}
}
