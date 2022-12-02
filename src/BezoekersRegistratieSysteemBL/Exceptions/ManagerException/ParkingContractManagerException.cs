using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.ManagerException {
	public class ParkingContractManagerException : Exception {
		public ParkingContractManagerException() {
		}

		public ParkingContractManagerException(string? message) : base(message) {
		}

		public ParkingContractManagerException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected ParkingContractManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}