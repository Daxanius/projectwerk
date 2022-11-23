using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.Managers {
	public class UnitTestParkingContractManager {
		#region MOQ
		private readonly ParkingContractManager _parkingContractManager;
		private readonly Mock<IParkingContractRepository> _mockRepo;
		#endregion

		#region Valid Info
		private readonly Werknemer _w;
		private readonly Bedrijf _b;
		#endregion

		#region Initialiseren
		public UnitTestParkingContractManager() {
			_mockRepo = new();

			_w = new(10, "werknemer", "werknemersen");
			_b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			_b.VoegWerknemerToeInBedrijf(_w, "werknemer.werknemerson@email.com", "werken");

			_parkingContractManager = new(_mockRepo.Object);
		}
		#endregion
	}
}
