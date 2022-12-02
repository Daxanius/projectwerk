namespace BezoekersRegistratieSysteemDL.Exceptions {
    public class ParkingContractMsServerException : Exception {
        public ParkingContractMsServerException(string? message) : base(message) {
        }

        public ParkingContractMsServerException(string? message, Exception? innerException) : base(message, innerException) {
        }
    }
}
