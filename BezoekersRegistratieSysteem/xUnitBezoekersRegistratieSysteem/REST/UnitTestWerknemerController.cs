﻿using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Controllers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Input;
using Microsoft.AspNetCore.Mvc;
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

			Bedrijf b = _b.NaarBusiness();
			Werknemer w = _w.NaarBusiness();

			b.VoegWerknemerToeInBedrijf(w, "werknemer.werknemerson@bedrijf.com", _f);

			_mockRepoBedrijf.Setup(x => x.BestaatBedrijf(0)).Returns(true);
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(true);
			_mockRepoBedrijf.Setup(x => x.GeefBedrijf(0)).Returns(b);
			_mockRepoWerknemer.Setup(x => x.GeefWerknemer(0)).Returns(w);
		}
		#endregion

		#region UnitTest VoegWerknemerToe
		[Fact]
		public void VoegWerknemerToe_Invalid_WerknemerLeeg() {
			var result = _werknemerController.VoegWerknemerToe(null);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void VoegWerknemerToe_Invalid_WerknemerBestaatAl() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(_w.NaarBusiness())).Returns(true);
			var result = _werknemerController.VoegWerknemerToe(_w);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest VerwijderWerknemer
		[Theory]
		[InlineData(0, -1)]
		[InlineData(-1, 0)]
		[InlineData(-1, -1)]
		public void VerwijderWerknemer_Invalid(long bedrijfId, long werknemerId) {
			var result = _werknemerController.VerwijderWerknemer(bedrijfId, werknemerId);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}

		[Fact]
		public void VerwijderWerknemer_Invalid_WerknemerBestaatNiet() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(false);
			var result = _werknemerController.VerwijderWerknemer(0, 0);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}
		#endregion

		#region UnitTest VoegWerknemerFunctieToe
		[Fact]
		public void VoegWerknemerFunctieToe_Invalid_WerknemerNegatief() {
			var result = _werknemerController.VoegWerknemerFunctieToe(-2, 0, _f);
			Assert.NotNull(result);
			Assert.Equal(typeof(BadRequestObjectResult), result.GetType());
		}

		[Fact]
		public void VoegWerknemerFunctieToe_Invalid_BedrijfNegatief() {
			var result = _werknemerController.VoegWerknemerFunctieToe(0, -2, _f);
			Assert.NotNull(result);
			Assert.Equal(typeof(BadRequestObjectResult), result.GetType());
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void VoegWerknemerFunctieToe_Invalid_functieLeeg(string functie) {
			var result = _werknemerController.VoegWerknemerFunctieToe(0, 0, functie);
			Assert.NotNull(result);
			Assert.Equal(typeof(BadRequestObjectResult), result.GetType());
		}

		[Fact]
		public void VoegWerknemerFunctieToe_Invalid_WerknemerBestaatNiet() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(false);
			var result = _werknemerController.VoegWerknemerFunctieToe(0, 0, _f);
			Assert.NotNull(result);
			Assert.Equal(typeof(BadRequestObjectResult), result.GetType());
		}
		#endregion

		#region UnitTest VerwijderWerknemerFunctie
		[Theory]
		[InlineData(0, -2)]
		[InlineData(-2, 0)]
		[InlineData(-2, -2)]
		public void VerwijderWerknemerFunctie_Invalid(long werknemerId, long bedrijfId) {
			var result = _werknemerController.VerwijderWerknemerFunctie(werknemerId, bedrijfId, _f);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void VerwijderWerknemerFunctie_Invalid_functieLeeg(string functie) {
			var result = _werknemerController.VerwijderWerknemerFunctie(0, 0, functie);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}

		[Fact]
		public void VerwijderWerknemerFunctie_Invalid_WerknemerBestaatNiet() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(false);
			var result = _werknemerController.VerwijderWerknemerFunctie(0, 0, _f);
			Assert.NotNull(result);
			Assert.Equal(typeof(NotFoundObjectResult), result.GetType());
		}
		#endregion

		#region UnitTest BewerkWerknemer
		[Fact]
		public void BewerkWerknemer_Invalid_WerknemerNegatief() {
			var result = _werknemerController.BewerkWerknemer(-2, 0, _w);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkWerknemer_Invalid_BedrijfNegatief() {
			var result = _werknemerController.BewerkWerknemer(0, -2, _w);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkWerknemer_Invalid_WerknemerBestaatAl() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(false);
			var result = _werknemerController.BewerkWerknemer(0, 0, _w);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void BewerkWerknemer_Invalid_WerknemerNietGewijzigd() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(true);
			_mockRepoWerknemer.Setup(x => x.GeefWerknemer(0)).Returns(_w.NaarBusiness());
			var result = _werknemerController.BewerkWerknemer(0, 0, _w);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(BadRequestObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefWerknemer
		[Fact]
		public void GeefWerknemer_Invalid_WerknemerBestaatNiet() {
			_mockRepoWerknemer.Setup(x => x.BestaatWerknemer(0)).Returns(false);
			var result = _werknemerController.GeefWerknemerOpId(0);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefWerknemersOpNaam
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void GeefWerknemersOpNaam_Invalid_VoornaamLeeg(string voornaam) {
			var result = _werknemerController.GeefWerknemersOpNaam(voornaam, "werknemersen");
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void GeefWerknemersOpNaam_Invalid_AchternaamLeeg(string achternaam) {
			var result = _werknemerController.GeefWerknemersOpNaam("werknemer", achternaam);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Theory]
		[InlineData(null, null)]

		[InlineData(null, "")]
		[InlineData(null, " ")]
		[InlineData(null, "\n")]
		[InlineData(null, "\r")]
		[InlineData(null, "\t")]
		[InlineData(null, "\v")]

		[InlineData("", "")]
		[InlineData("", " ")]
		[InlineData("", "\n")]
		[InlineData("", "\r")]
		[InlineData("", "\t")]
		[InlineData("", "\v")]

		[InlineData(" ", "")]
		[InlineData(" ", " ")]
		[InlineData(" ", "\n")]
		[InlineData(" ", "\r")]
		[InlineData(" ", "\t")]
		[InlineData(" ", "\v")]

		[InlineData("\n", "")]
		[InlineData("\n", " ")]
		[InlineData("\n", "\n")]
		[InlineData("\n", "\r")]
		[InlineData("\n", "\t")]
		[InlineData("\n", "\v")]

		[InlineData("\r", "")]
		[InlineData("\r", " ")]
		[InlineData("\r", "\n")]
		[InlineData("\r", "\r")]
		[InlineData("\r", "\t")]
		[InlineData("\r", "\v")]

		[InlineData("\t", "")]
		[InlineData("\t", " ")]
		[InlineData("\t", "\n")]
		[InlineData("\t", "\r")]
		[InlineData("\t", "\t")]
		[InlineData("\t", "\v")]

		[InlineData("\v", "")]
		[InlineData("\v", " ")]
		[InlineData("\v", "\n")]
		[InlineData("\v", "\r")]
		[InlineData("\v", "\t")]
		[InlineData("\v", "\v")]

		[InlineData("", null)]
		[InlineData(" ", null)]
		[InlineData("\n", null)]
		[InlineData("\r", null)]
		[InlineData("\t", null)]
		[InlineData("\v", null)]
		public void GeefWerknemersOpNaam_Invalid_NaamLeeg(string voornaam, string achternaam) {
			var result = _werknemerController.GeefWerknemersOpNaam(voornaam, achternaam);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefWerknemersOpNaam_Invalid_GeenWerknemers() {
			_mockRepoWerknemer.Setup(x => x.GeefWerknemersOpNaam(_w.Voornaam, _w.Achternaam)).Returns(new List<Werknemer>());
			var result = _werknemerController.GeefWerknemersOpNaam(_w.Voornaam, _w.Achternaam);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion

		#region UnitTest GeefWerknemersOpFunctie
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void GeefWerknemersOpFunctie_Invalid_FunctieLeeg(string functie) {
			var result = _werknemerController.GeefWerknemersOpFunctie(functie);
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(NotFoundObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}

		[Fact]
		public void GeefWerknemersOpFunctie_Invalid_GeenWerknemers() {
			_mockRepoWerknemer.Setup(x => x.GeefWerknemersOpFunctie("functie")).Returns(new List<Werknemer>());
			var result = _werknemerController.GeefWerknemersOpFunctie("functie");
			Assert.NotNull(result.Result);
			Assert.Equal(typeof(OkObjectResult), result.Result.GetType());
			Assert.Null(result.Value);
		}
		#endregion
	}
}