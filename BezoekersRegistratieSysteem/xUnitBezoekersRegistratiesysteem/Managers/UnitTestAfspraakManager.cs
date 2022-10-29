using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class AfspraakManagerTest {
		#region MOQ
		private AfspraakManager _afspraakManager;
		private Mock<IAfspraakRepository> _mockRepo;
		#endregion

		#region Valid Info
		private static DateTime _st = DateTime.Now;
		private static DateTime _et = _st.AddHours(2);
		private static Bezoeker _b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
		private static Werknemer _w = new(10, "werknemer", "werknemersen");

        private Afspraak _ia = new(_st, _b, _w);
        private Afspraak _oa = new (10, _st, _et, _b, _w);
        #endregion

        #region UnitTest Afspraak toevoegen
        [Fact]
        public void VoegAfspraakToe_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - VoegAfspraakToe - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VoegAfspraakToe(null));
        }

        [Fact]
		public void VoegAfspraakToe_Invalid_AfspraakBestaatAl()
		{
			_mockRepo = new Mock<IAfspraakRepository>();
			_afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - VoegAfspraakToe - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_ia)).Returns(true);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VoegAfspraakToe(_ia));
            Assert.Equal("AfspraakManager - VoegAfspraakToe - afspraak bestaat al", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak verwijderen
        [Fact]
        public void VerwijderAfspraak_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - VerwijderAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VerwijderAfspraak(null));
        }
        
        [Fact]
		public void VerwijderAfspraak_Invalid_AfspraakBestaatNiet()
		{
			_mockRepo = new Mock<IAfspraakRepository>();
			_afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - VerwijderAfspraak - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VerwijderAfspraak(_oa));
            Assert.Equal("AfspraakManager - VerwijderAfspraak - afspraak bestaat niet", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak bewerken
        [Fact]
        public void BewerkAfspraak_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BewerkAfspraak(null));
        }
        
        [Fact]
        public void BewerkAfspraak_Invalid_AfspraakBestaatNiet()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BewerkAfspraak(_oa));
            Assert.Equal("AfspraakManager - BewerkAfspraak - afspraak bestaat niet", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak beeindigen Bezoeker
        [Fact]
        public void BeeindigAfspraakBezoeker_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakBezoeker(null));
        }

        [Fact]
        public void BeeindigAfspraakBezoeker_Invalid_AfspraakBestaatNiet()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakBezoeker(_oa));
            Assert.Equal("AfspraakManager - BeeindigAfspraak - afspraak is al beeindigd", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak beeindigen Systeem
        [Fact]
        public void BeeindigAfspraakSysteem_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakSysteem(null));
        }

        [Fact]
        public void BeeindigAfspraakSysteem_Invalid_AfspraakReedsBeeindigd()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak bestaat al"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakSysteem(_ia));
        }

        [Fact]
        public void BeeindigAfspraakSysteem_Invalid_AfspraakBestaatNiet()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakSysteem(_oa));
            Assert.Equal("AfspraakManager - BeeindigAfspraak - afspraak is al beeindigd", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak opvragen
        
        #endregion
    }
}