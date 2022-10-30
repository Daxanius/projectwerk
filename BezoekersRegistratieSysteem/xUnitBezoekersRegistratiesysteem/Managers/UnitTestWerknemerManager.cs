using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class UnitTestWerknemerManager {

        #region MOQ
        private WerknemerManager _werknemerManager;
        private Mock<IWerknemerRepository> _mockRepoWerknemer;
        private Mock<IBedrijfRepository> _mockRepoBedrijf;
        #endregion

        #region Valid Info
        private static Werknemer _w = new(10, "werknemer", "werknemersen");
        private Bedrijf _b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
        #endregion

        #region Werknemer Toevoegen
        [Fact]
        public void VoegWerknemerToe_Invalid_WerknemerLeeg()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);
            
            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegWerknemerToe(null));
        }

        [Fact]
        public void VoegWerknemerToe_Invalid_WerknemerBestaatAl()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);

            //"BedrijfManager - VoegWerknemerToe - werknemer bestaat al"
            _mockRepoWerknemer.Setup(x => x.BestaatWerknemer(_w)).Returns(true);
            var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VoegWerknemerToe(_w));
            Assert.Equal("WerknemerManager - VoegWerknemerToe - werknemer bestaat al", ex.Message);
        }
        #endregion

        #region Werknemer Verwijderen
        [Fact]
        public void VerwijderWerknemer_Invalid_WerknemerLeeg()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);

            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(null, _b));
        }

        [Fact]
        public void VerwijderWerknemer_Invalid_BedrijfLeeg()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);

            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(_w, null));
        }

        [Fact]
        public void VerwijderWerknemer_Invalid_WerknemerBestaatAl()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);

            //"BedrijfManager - VoegWerknemerToe - werknemer bestaat niet"
            _mockRepoWerknemer.Setup(x => x.BestaatWerknemer(_w)).Returns(false);
            var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(_w, _b));
            Assert.Equal("WerknemerManager - VerwijderWerknemer - werknemer bestaat niet", ex.Message);
        }
        #endregion

        #region Werknemer Verwijderen
        [Fact]
        public void VerwijderWerknemer_Invalid_WerknemerLeeg()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);

            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(null, _b));
        }

        [Fact]
        public void VerwijderWerknemer_Invalid_BedrijfLeeg()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);

            Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(_w, null));
        }

        [Fact]
        public void VerwijderWerknemer_Invalid_WerknemerBestaatAl()
        {
            _mockRepoWerknemer = new Mock<IWerknemerRepository>();
            _mockRepoBedrijf = new Mock<IBedrijfRepository>();
            _werknemerManager = new WerknemerManager(_mockRepoWerknemer.Object, _mockRepoBedrijf.Object);

            //"BedrijfManager - VoegWerknemerToe - werknemer bestaat niet"
            _mockRepoWerknemer.Setup(x => x.BestaatWerknemer(_w)).Returns(false);
            var ex = Assert.Throws<WerknemerManagerException>(() => _werknemerManager.VerwijderWerknemer(_w, _b));
            Assert.Equal("WerknemerManager - VerwijderWerknemer - werknemer bestaat niet", ex.Message);
        }
        #endregion
    }
}