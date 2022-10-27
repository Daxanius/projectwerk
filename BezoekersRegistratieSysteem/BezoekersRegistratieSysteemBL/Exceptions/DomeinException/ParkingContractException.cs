using System.Runtime.Serialization;

namespace BezoekersRegistratieSysteemBL.Exceptions.DomeinException {
	public class ParkingContractException : Exception {
		public ParkingContractException() {}

		public ParkingContractException(string? message) : base(message) {}

		public ParkingContractException(string? message, Exception? innerException) : base(message, innerException) {}

		protected ParkingContractException(SerializationInfo info, StreamingContext context) : base(info, context) {}
	}
}