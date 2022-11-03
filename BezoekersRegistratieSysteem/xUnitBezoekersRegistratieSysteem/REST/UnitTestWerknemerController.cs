using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model;
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
		private Werknemer _w;
		private Bedrijf _b;
		private WerknemerInfo _wi;
		private string _f;
		#endregion

		#region Initialiseren
		public UnitTestWerknemerController() {
			// Info
			_w = new(10, "werknemer", "werknemersen");
			_b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			_f = "functie";

			_wi = new(_b, "werknemer.werknemersen@email.com");

			// Moq repos
			_mockRepoWerknemer = new();
			_mockRepoBedrijf = new();

			// Managers
			_werknemerManager = new(_mockRepoWerknemer.Object);
			_bedrijfManager = new(_mockRepoBedrijf.Object);

			// Controllers
			_werknemerController = new(_werknemerManager, _bedrijfManager);
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
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(_w)).Returns(true);
			WerknemerInputDTO input = new(_w.Voornaam, _w.Achternaam);
			var result = _werknemerController.VoegWerknemerToe(input);
			Assert.Null(result.Value);
		}
		#endregion
	}
}