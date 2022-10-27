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
		public void VoegAfspraakToe_Invalid()
		{
			_mockRepo = new Mock<IAfspraakRepository>();
			_afspraakManager = new AfspraakManager(_mockRepo.Object);

			//"AfspraakManager - VoegAfspraakToe - afspraak mag niet leeg zijn"
			Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VoegAfspraakToe(null));

            //"AfspraakManager - VoegAfspraakToe - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_ia)).Returns(true);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VoegAfspraakToe(_ia));
            Assert.Equal("AfspraakManager - VoegAfspraakToe - afspraak bestaat al", ex.Message);
        }
		#endregion

		#region UnitTest Afspraak verwijderen
		[Fact]
		public void VerwijderAfspraak_Invalid()
		{
			_mockRepo = new Mock<IAfspraakRepository>();
			_afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - VerwijderAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VerwijderAfspraak(null));

            //"AfspraakManager - VerwijderAfspraak - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VerwijderAfspraak(_oa));
            Assert.Equal("AfspraakManager - VerwijderAfspraak - afspraak bestaat niet", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak bewerken
        [Fact]
        public void BewerkAfspraak_Invalid()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BewerkAfspraak(null));

            //"AfspraakManager - BewerkAfspraak - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BewerkAfspraak(_oa));
            Assert.Equal("AfspraakManager - BewerkAfspraak - afspraak bestaat niet", ex.Message);
        }
        #endregion
    }
}