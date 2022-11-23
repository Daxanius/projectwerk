using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model.Input;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.REST {
	public class UnitTestParkingContractController {
		#region MOQ
		// Moq repos
		private Mock<IParkingContractRepository> _mockRepoParkingContract;
		private Mock<IBedrijfRepository> _mockRepoBedrijf;
		private Mock<IAfspraakRepository> _mockAfspraakRepository;

		// Managers
		private ParkingContractManager _parkingContractManager;
		private BedrijfManager _bedrijfManager;

		// Controllers
		private ParkingContractController _parkingContractController;
		#endregion

		#region Valid Info
		private string _n;
		private DateTime _st;
		private DateTime _et;
		private ParkeerplaatsInputDTO _p;
		#endregion

		#region Initialiseren
		public UnitTestParkingContractController() {
			// Moq repos
			_mockRepoParkingContract = new();
			_mockAfspraakRepository = new();
			_mockRepoBedrijf = new();

			// Managers
			_parkingContractManager = new(_mockRepoParkingContract.Object);
			_bedrijfManager = new(_mockRepoBedrijf.Object, _mockAfspraakRepository.Object);

			_n = "0-KKK-000";

			_st = DateTime.Now;
			_et = _st.AddHours(3);

			Bedrijf b = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			// Controllers
			_parkingContractController = new(_parkingContractManager, _bedrijfManager);

			// Data
			_p = new(0, _st, null, _n);

			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(true);
			_mockRepoBedrijf.Setup(x => x.GeefBedrijf(0)).Returns(b);
		}
		#endregion
	}
}
