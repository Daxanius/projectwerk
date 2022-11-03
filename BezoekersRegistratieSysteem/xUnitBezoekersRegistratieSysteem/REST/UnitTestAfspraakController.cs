using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
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
		private DateTime _st;
		private DateTime _et;

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
			_st = DateTime.Now;
			_et = _st.AddHours(2);

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

		#region UnitTest VerwijderAfspraak
		[Fact]
		public void VerwijderAfspraak_Invalid_AfspraakNegatief() {
			var result = _afspraakController.VerwijderAfspraak(-3);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}

		[Fact]
		public void VerwijderAfspraak_Invalid_AfspraakBestaatNiet() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(_a.NaarBusiness(_werknemerManger, _bedrijfManager))).Returns(false);
			var result = _afspraakController.VerwijderAfspraak(0);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}
		#endregion

		#region UnitTest BewerkAfspraak
		[Fact]
		public void BewerkAfspraak_Invalid_AfspraakLeeg() {
			var result = _afspraakController.BewerkAfspraak(-2, _a);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkAfspraak_Invalid_AfspraakBestaatNiet() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(_a.NaarBusiness(_werknemerManger, _bedrijfManager))).Returns(false);
			var result = _afspraakController.BewerkAfspraak(0, _a);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkAfspraak_Invalid_AfspraakNietGewijzigd() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(0)).Returns(true);
			_mockRepoAfspraak.Setup(x => x.GeefAfspraak(0)).Returns(_a.NaarBusiness(_werknemerManger, _bedrijfManager));
			var result = _afspraakController.BewerkAfspraak(0, _a);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest BeeindigAfspraakOpEmail
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void BeeindigAfspraakOpMail_Invalid_EmailLeeg(string email) {
			var result = _afspraakController.End(email);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}
		#endregion

		#region UnitTest GeefAfspraak
		[Fact]
		public void GeefAfspraak_Invalid_AfspraakBestaatNiet() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(0)).Returns(false);
			var result = _afspraakController.GeefAfspraak(0);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
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

		#region UnitTest GeefHuidigeAfsprakenPerBedrijf
		[Fact]
		public void GeefHuidigeAfsprakenPerBedrijf_Invalid_BedrijfNegatief() {
			var result = _afspraakController.GeefAfspraken(null, null, -3, true);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefHuidigeAfsprakenPerBedrijf_Invalid_GeenAfspraken() {
			_mockRepoAfspraak.Setup(x => x.GeefHuidigeAfsprakenPerBedrijf(0)).Returns(new List<Afspraak>());
			var result = _afspraakController.GeefAfspraken(null, null, 0, true);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefAfsprakenPerBedrijfOpDag
		[Fact]
		public void GeefAfsprakenPerBedrijfOpDag_Invalid_WerknemerNegatief() {
			var result = _afspraakController.GeefAfspraken(_st, null, -3, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAfsprakenPerBedrijfOpDag_Invalid_DatumInToekomst() {
			var result = _afspraakController.GeefAfspraken(_st.AddDays(1), null, -4, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAfsprakenPerBedrijfOpDag_Invalid_GeenAfspraken() {
			_mockRepoAfspraak.Setup(x => x.GeefAfsprakenPerBedrijfOpDag(0, _st)).Returns(new List<Afspraak>());
			var result = _afspraakController.GeefAfspraken(_st, null, 0, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefHuidigeAfsprakenPerWerknemerPerBedrijf
		[Fact]
		public void GeefHuidigeAfspraakPerWerknemer_Invalid_WerknemerNegatief() {
			var result = _afspraakController.GeefAfspraken(null, 0, -5, true);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefHuidigeAfspraakPerWerknemer_Invalid_BedrijfNegatief() {
			var result = _afspraakController.GeefAfspraken(null, -5, 0, true);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefHuidigeAfspraakPerWerknemer_Invalid_GeenAfspraak() {
			_mockRepoAfspraak.Setup(x => x.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(0, 0)).Returns(new List<Afspraak>());
			var result = _afspraakController.GeefAfspraken(null, 0, 0, true);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefAlleAfsprakenPerWerknemerPerBedrijf
		[Fact]
		public void GeefAlleAfsprakenPerWerknemerPerBedrijf_Invalid_WerknemerNegatief() {
			var result = _afspraakController.GeefAfspraken(null, -5, 0, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAlleAfsprakenPerWerknemerPerBedrijf_Invalid_BedrijfNegatief() {
			var result = _afspraakController.GeefAfspraken(null, 0, -5, true);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAlleAfsprakenPerWerknemerPerBedrijf_Invalid_GeenAfspraken() {
			_mockRepoAfspraak.Setup(x => x.GeefAlleAfsprakenPerWerknemerPerBedrijf(0, 0)).Returns(new List<Afspraak>());
			var result = _afspraakController.GeefAfspraken(null, 0, 0, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefAfsprakenPerWerknemerOpDagPerBedrijf
		[Fact]
		public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_WerknemerLeeg() {
			var result = _afspraakController.GeefAfspraken(_st, null, 0, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_DatumInToekomst() {
			var result = _afspraakController.GeefAfspraken(_st.AddDays(1), 0, 0, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_BedrijfLeeg() {
			var result = _afspraakController.GeefAfspraken(_st, 0, null, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_GeenAfspraken() {
			_mockRepoAfspraak.Setup(x => x.GeefAfsprakenPerWerknemerOpDagPerBedrijf(0, _st, 0)).Returns(new List<Afspraak>());
			var result = _afspraakController.GeefAfspraken(_st, 0, 0, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion
	}
}