using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model.Input;
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
			_bedrijfManager = new(_mockRepoBedrijf.Object, _mockRepoAfspraak.Object);
			_werknemerManger = new(_mockRepoWerknemer.Object);

			// Controllers
			_afspraakController = new(_afspraakManager, _werknemerManger, _bedrijfManager);

			// Data
			_st = DateTime.Now;
			_et = _st.AddHours(2);

			_b = new("bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			_w = new("werknemer", "werknemersen", new List<WerknemerInfoInputDTO>());

			_bd = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			_a = new(_b, null, null, 0, 0);

			Bedrijf b = _bd.NaarBusiness();
			Werknemer w = _w.NaarBusiness(_bedrijfManager);

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
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void VoegAfspraakToe_Invalid_AfspraakBestaatAl() {
			_mockRepoAfspraak.Setup(x => x.BestaatAfspraak(_a.NaarBusiness(_werknemerManger, _bedrijfManager))).Returns(true);
			var result = _afspraakController.MaakAfspraak(_a);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
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

		#region UnitTest GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf
		//[Fact]
		//public void GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf_Invalid_BedrijfLeeg() {
		//	var result = _afspraakController.GeefAfsprakenOpBezoeker(0, _b, null);
		//	Assert.NotNull(result.Result);
		//	Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
		//	Assert.Null(result.Value);
		//}

		//[Theory]
		//[InlineData(null, null, null)]

		//[InlineData(null, "", "")]
		//[InlineData(null, " ", "")]
		//[InlineData(null, "\n", "")]
		//[InlineData(null, "\r", "")]
		//[InlineData(null, "\t", "")]
		//[InlineData(null, "\v", "")]

		//[InlineData(null, "", " ")]
		//[InlineData(null, " ", " ")]
		//[InlineData(null, "\n", " ")]
		//[InlineData(null, "\r", " ")]
		//[InlineData(null, "\t", " ")]
		//[InlineData(null, "\v", " ")]

		//[InlineData(null, "", "\n")]
		//[InlineData(null, " ", "\n")]
		//[InlineData(null, "\n", "\n")]
		//[InlineData(null, "\r", "\n")]
		//[InlineData(null, "\t", "\n")]
		//[InlineData(null, "\v", "\n")]

		//[InlineData(null, "", "\r")]
		//[InlineData(null, " ", "\r")]
		//[InlineData(null, "\n", "\r")]
		//[InlineData(null, "\r", "\r")]
		//[InlineData(null, "\t", "\r")]
		//[InlineData(null, "\v", "\r")]

		//[InlineData(null, "", "\t")]
		//[InlineData(null, " ", "\t")]
		//[InlineData(null, "\n", "\t")]
		//[InlineData(null, "\r", "\t")]
		//[InlineData(null, "\t", "\t")]
		//[InlineData(null, "\v", "\t")]

		//[InlineData(null, "", "\v")]
		//[InlineData(null, " ", "\v")]
		//[InlineData(null, "\n", "\v")]
		//[InlineData(null, "\r", "\v")]
		//[InlineData(null, "\t", "\v")]
		//[InlineData(null, "\v", "\v")]

		//[InlineData("", null, null)]
		//[InlineData(" ", null, "")]
		//[InlineData("\n", null, "")]
		//[InlineData("\r", null, "")]
		//[InlineData("\t", null, "")]
		//[InlineData("\v", null, "")]

		//[InlineData(" ", null, " ")]
		//[InlineData("\n", null, " ")]
		//[InlineData("\r", null, " ")]
		//[InlineData("\t", null, " ")]
		//[InlineData("\v", null, " ")]

		//[InlineData(" ", null, "\n")]
		//[InlineData("\n", null, "\n")]
		//[InlineData("\r", null, "\n")]
		//[InlineData("\t", null, "\n")]
		//[InlineData("\v", null, "\n")]

		//[InlineData(" ", null, "\r")]
		//[InlineData("\n", null, "\r")]
		//[InlineData("\r", null, "\r")]
		//[InlineData("\t", null, "\r")]
		//[InlineData("\v", null, "\r")]

		//[InlineData(" ", null, "\t")]
		//[InlineData("\n", null, "\t")]
		//[InlineData("\r", null, "\t")]
		//[InlineData("\t", null, "\t")]
		//[InlineData("\v", null, "\t")]

		//[InlineData(" ", null, "\v")]
		//[InlineData("\n", null, "\v")]
		//[InlineData("\r", null, "\v")]
		//[InlineData("\t", null, "\v")]
		//[InlineData("\v", null, "\v")]

		//[InlineData("", "", null)]
		//[InlineData(" ", "", null)]
		//[InlineData("\n", "", null)]
		//[InlineData("\r", "", null)]
		//[InlineData("\t", "", null)]
		//[InlineData("\v", "", null)]

		//[InlineData("", " ", null)]
		//[InlineData(" ", " ", null)]
		//[InlineData("\n", " ", null)]
		//[InlineData("\r", " ", null)]
		//[InlineData("\t", " ", null)]
		//[InlineData("\v", " ", null)]

		//[InlineData("", "\n", null)]
		//[InlineData(" ", "\n", null)]
		//[InlineData("\n", "\n", null)]
		//[InlineData("\r", "\n", null)]
		//[InlineData("\t", "\n", null)]
		//[InlineData("\v", "\n", null)]

		//[InlineData("", "\r", null)]
		//[InlineData(" ", "\r", null)]
		//[InlineData("\n", "\r", null)]
		//[InlineData("\r", "\r", null)]
		//[InlineData("\t", "\r", null)]
		//[InlineData("\v", "\r", null)]

		//[InlineData("", "\t", null)]
		//[InlineData(" ", "\t", null)]
		//[InlineData("\n", "\t", null)]
		//[InlineData("\r", "\t", null)]
		//[InlineData("\t", "\t", null)]
		//[InlineData("\v", "\t", null)]

		//[InlineData("", "\v", null)]
		//[InlineData(" ", "\v", null)]
		//[InlineData("\n", "\v", null)]
		//[InlineData("\r", "\v", null)]
		//[InlineData("\t", "\v", null)]
		//[InlineData("\v", "\v", null)]
		//public void GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf_Invalid_NaamOfEmailLeeg(string voornaam, string achternaam, string email) {
		//	BezoekerInputDTO b = new(voornaam, achternaam, email, "");
		//	var result1 = _afspraakController.GeefAfsprakenOpBezoeker(0, b, null);
		//	var result2 = _afspraakController.GeefAfsprakenOpBezoeker(-2, b, null);

		//	Assert.NotNull(result1.Result);
		//	Assert.Equal(typeof(NotFoundObjectResult), result1.Result.GetType());
		//	Assert.Null(result1.Value);

		//	Assert.NotNull(result1.Result);
		//	Assert.Equal(typeof(NotFoundObjectResult), result1.Result.GetType());
		//	Assert.Null(result1.Value);
		//}

		//[Fact]
		//public void GeefAfsprakenPerBezoekerOpNaamOfEmail_Invalid_GeenAfspraken() {
		//	_mockRepoAfspraak.Setup(x => x.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(_b.Voornaam, _b.Achternaam, _b.Email, 0)).Returns(new List<Afspraak>());
		//	var result = _afspraakController.GeefAfsprakenOpBezoeker(0, _b, null);
		//	Assert.NotNull(result.Result);
		//	Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
		//	Assert.Null(result.Value);
		//}
		//#endregion

		//#region UnitTest GeefAfsprakenPerBezoekerOpDagPerBedrijf
		//[Fact]
		//public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_WerknemerLeeg() {
		//	var result = _afspraakController.GeefAfsprakenOpBezoeker(0, null, _st);
		//	Assert.NotNull(result.Result);
		//	Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
		//	Assert.Null(result.Value);
		//}

		//[Fact]
		//public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_DatumInToekomst() {
		//	var result = _afspraakController.GeefAfsprakenOpBezoeker(0, _b, _st.AddDays(1));
		//	Assert.NotNull(result.Result);
		//	Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
		//	Assert.Null(result.Value);
		//}

		//[Fact]
		//public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_BedrijfNegatief() {
		//	var result = _afspraakController.GeefAfsprakenOpBezoeker(-23, _b, _st.AddDays(1));
		//	Assert.NotNull(result.Result);
		//	Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
		//	Assert.Null(result.Value);
		//}

		//[Fact]
		//public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_GeenAfspraken() {
		//	_mockRepoAfspraak.Setup(x => x.GeefAfsprakenPerBezoekerOpDagPerBedrijf(0, _st, 0)).Returns(new List<Afspraak>());
		//	var result = _afspraakController.GeefAfsprakenOpBezoeker(0, _b, _st);
		//	Assert.NotNull(result.Result);
		//	Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
		//	Assert.Null(result.Value);
		//}
		#endregion

		#region UnitTest GeefHuidigeAfspraakBezoekerPerBedrijf
		[Fact]
		public void GeefHuidigeAfspraakBezoekerPerBedrijf_Invalid_EmailLeeg() {
			var result = _afspraakController.GeefAfspraakOpBezoeker(-34, 1);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefHuidigeAfspraakBezoekerPerBedrijf_Invalid_GeenAfspraken() {
			_mockRepoAfspraak.Setup(x => x.GeefHuidigeAfspraakBezoekerPerBerijf(0, 0)).Returns(_a.NaarBusiness(_werknemerManger, _bedrijfManager));
			var result = _afspraakController.GeefAfspraakOpBezoeker(0, 0);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefAfsprakenPerDag
		[Fact]
		public void GeefAfsprakenPerDag_Invalid_DatumInToekomst() {
			var result = _afspraakController.GeefAfspraken(_st.AddDays(1), null, null, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefAfsprakenPerDag_Invalid_GeenAfspraken() {
			_mockRepoAfspraak.Setup(x => x.GeefAfsprakenPerDag(_st)).Returns(new List<Afspraak>());
			var result = _afspraakController.GeefAfspraken(_st, null, null, false);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefAanwezigeBezoekers
		[Fact]
		public void GeefAanwezigeBezoekers_Invalid_GeenAanwezigeBezoekers() {
			_mockRepoAfspraak.Setup(x => x.GeefAanwezigeBezoekers()).Returns(new List<Bezoeker>());
			var result = _afspraakController.GeefAanwezigeBezoekers();
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion
	}
}