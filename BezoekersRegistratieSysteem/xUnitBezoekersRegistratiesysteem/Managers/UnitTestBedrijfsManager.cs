using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class UnitTestBedrijfsManagerTest {
        #region MOQ
        private BedrijfManager _bedrijfManager;
        private Mock<IBedrijfRepository> _mockRepo;
        #endregion

        #region Valid Info
        private static Werknemer _w = new(10, "werknemer", "werknemersen");
        private Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
        #endregion

        #region Bedrijf Toevoegen
        [Fact]
        public void VoegBedrijfToe_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IBedrijfRepository>();
            _bedrijfManager = new BedrijfManager(_mockRepo.Object);
            Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.VoegBedrijfToe(null));
        }

        [Fact]
        public void VoegBedrijfToe_Invalid_BedrijfBestaatAl()
        {
            _mockRepo = new Mock<IBedrijfRepository>();
            _bedrijfManager = new BedrijfManager(_mockRepo.Object);

            //"BedrijfManager - VoegBedrijfToe - bedrijf bestaat al"
            _mockRepo.Setup(x => x.BestaatBedrijf(_b)).Returns(true);
            var ex = Assert.Throws<BedrijfManagerException>(() => _bedrijfManager.VoegBedrijfToe(_b));
            Assert.Equal("BedrijfManager - VoegBedrijfToe - bedrijf bestaat al", ex.Message);
        }

        [Fact]
        public void VoegBedrijfToe_Invalid_GeenBedrijf()
        {
            _mockRepo = new Mock<IBedrijfRepository>();
            _bedrijfManager = new BedrijfManager(_mockRepo.Object);

            //"BedrijfManager - VoegBedrijfToe - er zijn geen afspraken"
            _mockRepo.Setup(x => x.VoegBedrijfToe(_b)).Returns(new Bedrijf());
            var ex = _bedrijfManager.VoegBedrijfToe(_b);
            Assert.Null(ex);
        }
        #endregion
    }
}