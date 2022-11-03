using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.REST {
	public class UnitTestAfspraakController {
		#region MOQ
		// Moq repos
		private Mock<IAfspraakRepository> _mockRepoAfspraak;
		private Mock<IBedrijfRepository> _mockRepoBedrijf;
		private Mock<IWerknemerRepository> _mockRepoWerknemer;

		// Managers
		private AfspraakManager _afspraakManager;
		private BedrijfManager _bedrijfManager;
		private WerknemerManager _werknemerManger;

		// Controllers
		private AfspraakController _afspraakController;
		#endregion

		#region Valid Info
		private BezoekerInputDTO _b;
		private WerknemerInputDTO _w;

		private BedrijfInputDTO _bd;

		private AfspraakInputDTO _a;
		#endregion

		#region Initialiseren
		public UnitTestAfspraakController() {
			// Moq repos
			_mockRepoAfspraak = new();
			_mockRepoBedrijf = new();
			_mockRepoWerknemer = new();

			// Managers
			_afspraakManager = new(_mockRepoAfspraak.Object);
			_bedrijfManager = new(_mockRepoBedrijf.Object);
			_werknemerManger = new(_mockRepoWerknemer.Object);

			// Controllers
			_afspraakController = new(_afspraakManager, _werknemerManger, _bedrijfManager);

			// Data
			_b = new("bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			_w = new("werknemer", "werknemersen");

			_bd = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			_a = new(_b, 0, 0);

			Bedrijf b = _bd.NaarBusiness();
			Werknemer w = _w.NaarBusiness();

			b.VoegWerknemerToeInBedrijf(w, "werknemer.werknemersen@bedrijf.com", "nietsen");

			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(true);
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(true);
			_mockRepoBedrijf.Setup(x => x.GeefBedrijf(0)).Returns(b);
			_mockRepoWerknemer.Setup(x => x.GeefWerknemer(0)).Returns(w);
		}
		#endregion

		#region UnitTest VoegAfspraakToe
		[Fact]
		public void VoegAfspraakToe_Invalid_AfspraakLeeg() {
			var result = _afspraakController.MaakAfspraak(null);
			Assert.Null(result.Value);
		}

		[Fact]
		public void VoegAfspraakToe_Invalid_AfspraakBestaatAl() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(_a.NaarBusiness(_werknemerManger, _bedrijfManager))).Returns(true);
			var result = _afspraakController.MaakAfspraak(_a);
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest BewerkAfspraak
		[Fact]
		public void BewerkAfspraak_Invalid_AfspraakLeeg() {
			var result = _afspraakController.BewerkAfspraak(-2, _a);
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkAfspraak_Invalid_AfspraakBestaatNiet() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(_a.NaarBusiness(_werknemerManger, _bedrijfManager))).Returns(false);
			var result = _afspraakController.BewerkAfspraak(0, _a);
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkAfspraak_Invalid_AfspraakNietGewijzigd() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(0)).Returns(true);
			_mockRepoAfspraak.Setup(x => x.GeefAfspraak(0)).Returns(_a.NaarBusiness(_werknemerManger, _bedrijfManager));
			var result = _afspraakController.BewerkAfspraak(0, _a);
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefAfspraak
		[Fact]
		public void GeefAfspraak_Invalid_AfspraakBestaatNiet() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(0)).Returns(false);
			var result = _afspraakController.GeefAfspraak(0);
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefHuidigeAfspraken
		[Fact]
		public void GeefHuidigeAfspraken_Invalid_GeenAfspraken() {
			_mockRepoAfspraak.Setup(x => x.GeefHuidigeAfspraken()).Returns(new List<Afspraak>());
			var result = _afspraakController.GeefAfspraken(null, null, null, true);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion
	}
}