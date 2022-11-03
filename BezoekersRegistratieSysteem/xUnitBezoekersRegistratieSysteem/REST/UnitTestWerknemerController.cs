using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Input;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.REST {
	public class UnitTestWerknemerController {
		#region MOQ
		// Moq repos
		private Mock<IWerknemerRepository> _mockRepoWerknemer;
		private Mock<IBedrijfRepository> _mockRepoBedrijf;

		// Managers
		private WerknemerManager _werknemerManager;
		private BedrijfManager _bedrijfManager;

		// Controllers
		private WerknemerController _werknemerController;
		#endregion

		#region Valid Info
		private WerknemerInputDTO _w;
		private BedrijfInputDTO _b;
		private WerknemerInfoInputDTO _wi;
		private string _f;
		#endregion

		#region Initialiseren
		public UnitTestWerknemerController() {
			// Moq repos
			_mockRepoWerknemer = new();
			_mockRepoBedrijf = new();

			// Managers
			_werknemerManager = new(_mockRepoWerknemer.Object);
			_bedrijfManager = new(_mockRepoBedrijf.Object);

			// Controllers
			_werknemerController = new(_werknemerManager, _bedrijfManager);

			// Data
			_w = new("werknemer", "werknemersen");
			_b = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			_f = "functie";

			_wi = new(0, "werknemer.werknemersen@email.com", new() { _f });
		}
		#endregion

		#region UnitTest VoegWerknemerToe
		[Fact]
		public void VoegWerknemerToe_Invalid_WerknemerLeeg() {
			var result = _werknemerController.VoegWerknemerToe(null);
			Assert.Null(result.Value);
		}

		[Fact]
		public void VoegWerknemerToe_Invalid_WerknemerBestaatAl() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(_w.NaarBusiness())).Returns(true);
			var result = _werknemerController.VoegWerknemerToe(_w);
			Assert.Null(result.Value);
		}
		#endregion
	}
}