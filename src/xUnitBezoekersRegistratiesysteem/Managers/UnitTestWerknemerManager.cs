using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class UnitTestWerknemerManager {

		//TODO "WerknemerManager - VoegWerknemerFunctieToe - werknemer heeft deze functie al bij dit bedrijf"

		#region MOQ
		private WerknemerManager _werknemerManager;
		private Mock<IWerknemerRepository> _mockRepo;
		#endregion

		#region Valid Info
		private Werknemer _w;
		private Bedrijf _b;
		private WerknemerInfo _wi;
		private string _f;
		#endregion

        #region Initialiseren
        public UnitTestWerknemerManager()
		{
            _w = new(10, "werknemer", "werknemersen");
            _b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            _f = "functie";

			_wi = new(_b, "werknemer.werknemersen@email.com");
        }
        #endregion

        #region UnitTest VoegWerknemerToe
        [Fact]
		public void VoegWerknemerToe_Invalid_WerknemerLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegWerknemerToe(null));
		}

        #endregion

        #region UnitTest VerwijderWerknemer
        [Fact]
		public void VerwijderWerknemer_Invalid_WerknemerLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(null, _b));
		}

		[Fact]
		public void VerwijderWerknemer_Invalid_BedrijfLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(_w, null));
		}

		[Fact]
		public void VerwijderWerknemer_Invalid_WerknemerBestaatNiet() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"BedrijfManager - VoegWerknemerToe - werknemer bestaat niet"
			_mockRepo.Setup(x => x.BestaatWerknemer(_w)).Returns(false);
			var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(_w, _b));
			Assert.Equal("WerknemerManager - VerwijderWerknemer - werknemer bestaat niet", ex.Message);
		}
        #endregion

        #region UnitTest VoegWerknemerFunctieToe
        [Fact]
		public void VoegWerknemerFunctieToe_Invalid_WerknemerLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegWerknemerFunctieToe(null, _wi));
		}

		[Fact]
		public void VoegWerknemerFunctieToe_Invalid_WerknemerInfoLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegWerknemerFunctieToe(_w, null));
		}

		[Fact]
		public void VoegWerknemerFunctieToe_Invalid_WerknemerBestaatNiet() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"BedrijfManager - VoegWerknemerFunctieToe - werknemer bestaat niet"
			_mockRepo.Setup(x => x.BestaatWerknemer(_w)).Returns(false);
			var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegWerknemerFunctieToe(_w, _wi));
			Assert.Equal("WerknemerManager - VoegWerknemerFunctieToe - werknemer bestaat niet", ex.Message);
		}

		[Fact]
		public void VoegWerknemerFunctieToe_Invalid_WerknemerNietBijBedrijf() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"BedrijfManager - VoegWerknemerFunctieToe - bedrijf bestaat niet"

			_mockRepo.Setup(x => x.GeefWerknemer(_w.Id)).Returns(_w);
			Assert.DoesNotContain(_b, _w.GeefBedrijvenEnFunctiesPerWerknemer().Keys);
		}
        #endregion

        #region UnitTest VerwijderWerknemerFunctie
        [Fact]
		public void VerwijderWerknemerFunctie_Invalid_WerknemerLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemerFunctie(null, _b, _f));
		}

		[Fact]
		public void VerwijderWerknemerFunctie_Invalid_BedrijfLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemerFunctie(_w, null, _f));
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
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemerFunctie(_w, _b, functie));
		}

		[Fact]
		public void VerwijderWerknemerFunctie_Invalid_WerknemerBestaatNiet() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"BedrijfManager - VoegWerknemerFunctieToe - werknemer bestaat niet"
			_mockRepo.Setup(x => x.BestaatWerknemer(_w)).Returns(false);
			var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemerFunctie(_w, _b, _f));
			Assert.Equal("WerknemerManager - VerwijderWerknemerFunctie - werknemer bestaat niet", ex.Message);
		}

		[Fact]
		public void VerwijderWerknemerFunctie_Invalid_WerknemerNietBijBedrijf() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"BedrijfManager - VerwijderWerknemerFunctie - werknemer niet werkzaam bij dit bedrijf"

			_mockRepo.Setup(x => x.GeefWerknemer(_w.Id)).Returns(_w);
			Assert.DoesNotContain(_b, _w.GeefBedrijvenEnFunctiesPerWerknemer().Keys);
		}

		[Fact]
		public void VerwijderWerknemerFunctie_Invalid_GeenFunctiesBijBedrijf() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"WerknemerManager - VerwijderWerknemerFunctie - werknemer heeft geen functie bij dit bedrijf"

			_mockRepo.Setup(x => x.GeefWerknemer(_w.Id)).Returns(_w);
			Assert.Empty(_w.GeefBedrijvenEnFunctiesPerWerknemer().Values);
		}

		[Fact]
		public void VerwijderWerknemerFunctie_Invalid_WerknemerMinstens1FunctieBijBedrijf() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"WerknemerManager - VerwijderWerknemerFunctie - werknemer moet minstens 1 functie hebben"

			_mockRepo.Setup(x => x.GeefWerknemer(_w.Id)).Returns(_w);
			Assert.True(_w.GeefBedrijvenEnFunctiesPerWerknemer().Values.Count() < 1);
		}
        #endregion

        #region UnitTest BewerkWerknemer
        [Fact]
		public void BewerkWerknemer_Invalid_WerknemerLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.BewerkWerknemer(null, _b));
		}

		[Fact]
		public void BewerkWerknemer_Invalid_BedrijfLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.BewerkWerknemer(_w, null));
		}

		[Fact]
		public void BewerkWerknemer_Invalid_WerknemerBestaatAl() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"BedrijfManager - VoegWerknemerToe - werknemer bestaat niet"
			_mockRepo.Setup(x => x.BestaatWerknemer(_w)).Returns(false);
			var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.BewerkWerknemer(_w, _b));
			Assert.Equal("WerknemerManager - BewerkWerknemer - werknemer bestaat niet", ex.Message);
		}

		[Fact]
		public void BewerkWerknemer_Invalid_WerknemerNietGewijzigd() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - BewerkWerknemer - werknemer is niet gewijzigd"
            _mockRepo.Setup(x => x.BestaatWerknemer(_w.Id)).Returns(true);
            _mockRepo.Setup(x => x.GeefWerknemer(_w.Id)).Returns(_w);
			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.BewerkWerknemer(_w, _b));
        }
        #endregion

        #region UnitTest GeefWerknemer
        [Fact]
		public void GeefWerknemer_Invalid_WerknemerBestaatNiet() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"WerknemerManager - GeefWerknemer - werknemer bestaat niet"
			_mockRepo.Setup(x => x.BestaatWerknemer(_w.Id)).Returns(false);
			var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemer(_w.Id));
			Assert.Equal("WerknemerManager - GeefWerknemer - werknemer bestaat niet", ex.Message);
		}
        #endregion

        #region UnitTest GeefWerknemersOpNaamPerBedrijf
        [Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void GeefWerknemersOpNaam_Invalid_VoornaamLeeg(string voornaam) {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefWerknemersOpNaamPerBedrijf - naam mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemersOpNaamPerBedrijf(voornaam, "werknemersen", _b));
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
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefWerknemersOpNaamPerBedrijf - naam mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemersOpNaamPerBedrijf("werknemer", achternaam, _b));
		}

        [Fact]
        public void GeefWerknemersOpNaam_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefWerknemersOpNaamPerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemersOpNaamPerBedrijf("werknemer", "werknemersen", null));
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
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"WerknemerManager - GeefWerknemersOpNaam - naam mag niet leeg zijn"
			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemersOpNaamPerBedrijf(voornaam, achternaam, _b));
		}

		[Fact]
		public void GeefWerknemersOpNaam_Invalid_GeenWerknemers() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"WerknemerManager - GeefWerknemersOpNaam - er zijn geen werknemers"
			_mockRepo.Setup(x => x.GeefWerknemersOpNaamPerBedrijf(_w.Voornaam, _w.Achternaam, _b.Id)).Returns(new List<Werknemer>());
			var ex = _werknemerManager.GeefWerknemersOpNaamPerBedrijf(_w.Voornaam, _w.Achternaam, _b);
			Assert.Empty(ex);
		}
        #endregion

        #region UnitTest GeefWerknemersOpFunctiePerBedrijf
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
        public void GeefWerknemersOpFunctie_Invalid_FunctieLeeg(string functie)
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefWerknemersOpFunctiePerBedrijf - functie mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemersOpFunctiePerBedrijf(functie, _b));
        }

        [Fact]
        public void GeefWerknemersOpFunctie_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);
            
            //"WerknemerManager - GeefWerknemersOpFunctiePerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemersOpFunctiePerBedrijf("functie", null));
        }

        [Fact]
        public void GeefWerknemersOpFunctie_Invalid_GeenWerknemers()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefWerknemersOpFunctiePerBedrijf - er zijn geen werknemers"
            _mockRepo.Setup(x => x.GeefWerknemersOpFunctiePerBedrijf("functie", _b.Id)).Returns(new List<Werknemer>());
            var ex = _werknemerManager.GeefWerknemersOpFunctiePerBedrijf("functie", _b);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest GeefWerknemersPerBedrijf
        [Fact]
		public void GeefWerknemersPerBedrijf_Invalid_BedrijfLeeg() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"WerknemerManager - GeefWerknemersPerBedrijf - bedrijf mag niet leeg zijn"
			Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefWerknemersPerBedrijf(null));
		}

		[Fact]
		public void GeefWerknemersPerBedrijf_Invalid_GeenAfspraken() {
			_mockRepo = new Mock<IWerknemerRepository>();
			_werknemerManager = new WerknemerManager(_mockRepo.Object);

			//"WerknemerManager - GeefWerknemersPerBedrijf - er zijn geen werknemers"
			_mockRepo.Setup(x => x.GeefWerknemersPerBedrijf(_b.Id)).Returns(new List<Werknemer>());
			var ex = _werknemerManager.GeefWerknemersPerBedrijf(_b);
			Assert.Empty(ex);
		}
        #endregion

        #region UnitTest VoegFunctieToe
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
        public void VoegFunctieToe_Invalid_FunctieLeeg(string functie)
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - VoegFunctieToe - functie mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegFunctieToe(functie));
        }

        [Fact]
        public void VoegFunctieToe_Invalid_FunctieBestaat()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - VoegFunctieToe - functie bestaat al"
            _mockRepo.Setup(x => x.BestaatFunctie("functie")).Returns(true);
            var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegFunctieToe("functie"));
            Assert.Equal("WerknemerManager - VoegFunctieToe - functie bestaat al", ex.Message);
        }
        #endregion

        #region GeefVrijeWerknemersOpDitMomentVoorBedrijf
        [Fact]
        public void GeefVrijeWerknemersOpDitMomentVoorBedrijf_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefVrijeWerknemersOpDitMomentVoorBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefVrijeWerknemersOpDitMomentVoorBedrijf(null));
        }

        [Fact]
        public void GeefVrijeWerknemersOpDitMomentVoorBedrijf_Invalid_GeenWerknemers()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefVrijeWerknemersOpDitMomentVoorBedrijf - er zijn geen werknemers"
            _mockRepo.Setup(x => x.GeefVrijeWerknemersOpDitMomentVoorBedrijf(_b.Id)).Returns(new List<Werknemer>());
            var ex = _werknemerManager.GeefVrijeWerknemersOpDitMomentVoorBedrijf(_b);
            Assert.Empty(ex);
        }
        #endregion

        #region GeefBezetteWerknemersOpDitMomentVoorBedrijf
        [Fact]
        public void GeefBezetteWerknemersOpDitMomentVoorBedrijf_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefBezetteWerknemersOpDitMomentVoorBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.GeefBezetteWerknemersOpDitMomentVoorBedrijf(null));
        }

        [Fact]
        public void GeefBezetteWerknemersOpDitMomentVoorBedrijf_Invalid_GeenWerknemers()
        {
            _mockRepo = new Mock<IWerknemerRepository>();
            _werknemerManager = new WerknemerManager(_mockRepo.Object);

            //"WerknemerManager - GeefBezetteWerknemersOpDitMomentVoorBedrijf - er zijn geen werknemers"
            _mockRepo.Setup(x => x.GeefBezetteWerknemersOpDitMomentVoorBedrijf(_b.Id)).Returns(new List<Werknemer>());
            var ex = _werknemerManager.GeefBezetteWerknemersOpDitMomentVoorBedrijf(_b);
            Assert.Empty(ex);
        }
        #endregion
    }
}