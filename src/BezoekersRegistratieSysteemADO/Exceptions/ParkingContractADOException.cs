namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class ParkingContractADOException : Exception {
        public ParkingContractADOException(string? message) : base(message) {
        }

        public ParkingContractADOException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
