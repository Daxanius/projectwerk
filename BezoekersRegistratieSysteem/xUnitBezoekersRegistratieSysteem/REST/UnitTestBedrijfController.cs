using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
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
		private Werknemer _w;
		private Bedrijf _unb;
		private Bedrijf _vb;
		#endregion

		#region Initialiseren
		public UnitTestBedrijfController() {
			_w = new(10, "werknemer", "werknemersen");
			_unb = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			_vb = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			// Moq repos
			_mockRepoBedrijf = new();
			_mockRepoWerknemer = new();

			// Managers
			_bedrijfManager = new(_mockRepoBedrijf.Object);
			_werknemerManger = new(_mockRepoWerknemer.Object);

			// Controllers
			_bedrijfController = new(_bedrijfManager, _werknemerManger);
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
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(_unb)).Returns(true);
			BedrijfInputDTO input = new(_unb.Naam, _unb.BTW, _unb.TelefoonNummer, _unb.Email, _unb.Adres);
			var result = _bedrijfController.VoegBedrijfToe(input);
			Assert.Null(result.Value);
		}
		#endregion
	}
}
