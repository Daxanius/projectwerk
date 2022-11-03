using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.REST {
	public class UnitTestBedrijfController {
		#region MOQ
		// Moq repos
		private Mock<IBedrijfRepository> _mockRepoBedrijf;
		private Mock<IWerknemerRepository> _mockRepoWerknemer;

		// Managers
		private BedrijfManager _bedrijfManager;
		private WerknemerManager _werknemerManger;

		// Controllers
		private BedrijfController _bedrijfController;
		#endregion

		#region Valid Info
		private WerknemerInputDTO _w;
		private BedrijfInputDTO _b;
		#endregion

		#region Initialiseren
		public UnitTestBedrijfController() {
			// Moq repos
			_mockRepoBedrijf = new();
			_mockRepoWerknemer = new();

			// Managers
			_bedrijfManager = new(_mockRepoBedrijf.Object);
			_werknemerManger = new(_mockRepoWerknemer.Object);

			// Controllers
			_bedrijfController = new(_bedrijfManager, _werknemerManger);

			// Data
			_w = new("werknemer", "werknemersen");
			_b = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
		}
		#endregion

		#region UnitTest VoegBedrijfToe
		[Fact]
		public void VoegBedrijfToe_Invalid_BedrijfLeeg() {
			var result = _bedrijfController.VoegBedrijfToe(null);
			Assert.Null(result.Value);
		}

		[Fact]
		public void VoegBedrijfToe_Invalid_BedrijfBestaatAl() {
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(_b.NaarBusiness())).Returns(true);
			var result = _bedrijfController.VoegBedrijfToe(_b);
			Assert.Null(result.Value);
		}
		#endregion
	}
}
