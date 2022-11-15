using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model.Input;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.REST {
	public class UnitTestBedrijfController {
		#region MOQ
		// Moq repos
		private readonly Mock<IBedrijfRepository> _mockRepoBedrijf;
		private readonly Mock<IWerknemerRepository> _mockRepoWerknemer;

		// Managers
		private readonly BedrijfManager _bedrijfManager;
		private readonly WerknemerManager _werknemerManger;

		// Controllers
		private readonly BedrijfController _bedrijfController;
		#endregion

		#region Valid Info
		private readonly WerknemerInputDTO _w;
		private readonly BedrijfInputDTO _b;
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
			_b = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			_w = new("werknemer", "werknemersen", new List<WerknemerInfoInputDTO>());

			Bedrijf b = _b.NaarBusiness();
			Werknemer w = _w.NaarBusiness(_bedrijfManager);

			b.VoegWerknemerToeInBedrijf(w, "werknemer.werknemersen@bedrijf.com", "nietsen");

			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(true);
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(true);
			_mockRepoBedrijf.Setup(x => x.GeefBedrijf(0)).Returns(b);
			_mockRepoWerknemer.Setup(x => x.GeefWerknemer(0)).Returns(w);
		}
		#endregion

		#region UnitTest VoegBedrijfToe
		[Fact]
		public void VoegBedrijfToe_Invalid_BedrijfLeeg() {
			var result = _bedrijfController.VoegBedrijfToe(null);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void VoegBedrijfToe_Invalid_BedrijfBestaatAl() {
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(_b.NaarBusiness())).Returns(true);
			var result = _bedrijfController.VoegBedrijfToe(_b);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest VerwijderBedrijf
		[Fact]
		public void VerwijderBedrijf_Invalid_BedrijfNegatief() {
			var result = _bedrijfController.VerwijderBedrijf(-5);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}

		[Fact]
		public void VerwijderBedrijf_Invalid_BedrijfBestaatNiet() {
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(false);
			var result = _bedrijfController.VerwijderBedrijf(0);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}
		#endregion

		#region UnitTest BewerkBedrijf
		[Fact]
		public void BewerkBedrijf_Invalid_BedrijfNegatief() {
			var result = _bedrijfController.BewerkBedrijf(-3, _b);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkBedrijf_Invalid_BedrijfBestaatNiet() {
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(false);
			var result = _bedrijfController.BewerkBedrijf(0, _b);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkBedrijf_Invalid_BedrijfNietGewijzigd() {
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(true);
			_mockRepoBedrijf.Setup(x => x.GeefBedrijf(0)).Returns(_b.NaarBusiness());
			var result = _bedrijfController.BewerkBedrijf(0, _b);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefBedrijf [id]
		[Fact]
		public void GeefBedrijfOpId_Invalid_BedrijfBestaatNiet() {
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(false);
			var result = _bedrijfController.GeefBedrijfOpId(0);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefBedrijven
		[Fact]
		public void GeefBedrijven_Invalid_BedrijvenBestaanNiet() {
			_mockRepoBedrijf.Setup(x => x.GeefBedrijven()).Returns(new List<Bedrijf>());
			var result = _bedrijfController.GeefAlleBedrijven();
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefBedrijf [naam]
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void GeefBedrijfOpNaam_Invalid_BedrijfLeeg(string bedrijfsnaam) {
			var result = _bedrijfController.GeefBedrijfOpNaam(bedrijfsnaam);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefBedrijfOpNaam_Invalid_BedrijfBestaatNiet() {
			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(_b.Naam)).Returns(false);
			var result = _bedrijfController.GeefBedrijfOpNaam(_b.Naam);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion
	}
}
