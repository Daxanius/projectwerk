using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class UnitTestBedrijfsManager {

		//AF

		#region MOQ
		private BedrijfManager _bedrijfManager;
		private Mock<IBedrijfRepository> _mockRepo;
		#endregion

		#region Valid Info
		private readonly Werknemer _w;
		private readonly Bedrijf _unb;
		private readonly Bedrijf _vb;
		#endregion

		#region Initialiseren
		public UnitTestBedrijfsManager() {
			_w = new(10, "werknemer", "werknemersen");
			_unb = new("bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			_vb = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
		}
		#endregion

		#region UnitTest VoegBedrijfToe
		[Fact]
		public void VoegBedrijfToe_Invalid_BedrijfLeeg() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);
			Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.VoegBedrijfToe(null));
		}

		[Fact]
		public void VoegBedrijfToe_Invalid_BedrijfBestaatAl() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);

			//"BedrijfManager - VoegBedrijfToe - bedrijf bestaat al"
			_mockRepo.Setup(x => x.BestaatBedrijf(_unb)).Returns(true);
			var ex = Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.VoegBedrijfToe(_unb));
			Assert.Equal("BedrijfManager - VoegBedrijfToe - bedrijf bestaat al", ex.Message);
		}
		#endregion

		#region UnitTest VerwijderBedrijf
		[Fact]
		public void VerwijderBedrijf_Invalid_BedrijfLeeg() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);
			Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.VerwijderBedrijf(null));
		}

		[Fact]
		public void VerwijderBedrijf_Invalid_BedrijfBestaatNiet() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);

			//"BedrijfManager - VerwijderBedrijf - bedrijf bestaat niet"
			_mockRepo.Setup(x => x.BestaatBedrijf(_vb)).Returns(false);
			var ex = Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.VerwijderBedrijf(_vb));
			Assert.Equal("BedrijfManager - VerwijderBedrijf - bedrijf bestaat niet", ex.Message);
		}
		#endregion

		#region UnitTest BewerkBedrijf
		[Fact]
		public void BewerkBedrijf_Invalid_BedrijfLeeg() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);
			Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.BewerkBedrijf(null));
		}

		[Fact]
		public void BewerkBedrijf_Invalid_BedrijfBestaatNiet() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);

			//"BedrijfManager - BewerkBedrijf - bedrijf bestaat niet"
			_mockRepo.Setup(x => x.BestaatBedrijf(_vb)).Returns(false);
			var ex = Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.BewerkBedrijf(_vb));
			Assert.Equal("BedrijfManager - BewerkBedrijf - bedrijf bestaat niet", ex.Message);
		}

		[Fact]
		public void BewerkBedrijf_Invalid_BedrijfNietGewijzigd() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);

			//"BedrijfManager - BewerkBedrijf - bedrijf is niet gewijzigd"
			_mockRepo.Setup(x => x.BestaatBedrijf(_vb)).Returns(true);
			_mockRepo.Setup(x => x.GeefBedrijf(_vb.Id)).Returns(_vb);
			var ex = Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.BewerkBedrijf(_vb));
			Assert.Equal("BedrijfManager - BewerkBedrijf - bedrijf is niet gewijzigd", ex.Message);
		}
		#endregion

		#region UnitTest GeefBedrijf [id]
		[Fact]
		public void GeefBedrijfOpId_Invalid_BedrijfBestaatNiet() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);

			//"BedrijfManager - GeefBedrijf - bedrijf bestaat niet"
			_mockRepo.Setup(x => x.BestaatBedrijf(_vb.Id)).Returns(false);
			var ex = Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.GeefBedrijf(_vb.Id));
			Assert.Equal("BedrijfManager - GeefBedrijf - bedrijf bestaat niet", ex.Message);
		}
		#endregion

		#region UnitTest GeefBedrijven
		[Fact]
		public void GeefBedrijven_Invalid_BedrijvenBestaanNiet() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);

			//"BedrijfManager - GeefBedrijven - er zijn geen bedrijven"
			_mockRepo.Setup(x => x.GeefBedrijven()).Returns(new List<Bedrijf>());
			var ex = _bedrijfManager.GeefBedrijven();
			Assert.Empty(ex);
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
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);
			Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.GeefBedrijf(bedrijfsnaam));
		}

		[Fact]
		public void GeefBedrijfOpNaam_Invalid_BedrijfBestaatNiet() {
			_mockRepo = new Mock<IBedrijfRepository>();
			_bedrijfManager = new BedrijfManager(_mockRepo.Object);

			//"BedrijfManager - GeefBedrijf - bedrijf bestaat niet"
			_mockRepo.Setup(x => x.BestaatBedrijf(_vb.Naam)).Returns(false);
			var ex = Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.GeefBedrijf(_vb.Naam));
			Assert.Equal("BedrijfManager - GeefBedrijf - bedrijf bestaat niet", ex.Message);
		}
		#endregion
	}
}