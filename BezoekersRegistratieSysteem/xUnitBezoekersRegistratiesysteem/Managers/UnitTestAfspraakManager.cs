using BezoekersRegistratieSysteemBL.Domeinen;
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
		private Bezoeker _b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
		private Werknemer _w = new(10, "werknemer", "werknemersen");
		#endregion

		#region UnitTest Afspraak toevoegen
		[Fact]
		public void VoegAfspraakToe_Valid() {
			_mockRepo = new Mock<IAfspraakRepository>();
			_afspraakManager = new AfspraakManager(_mockRepo.Object);
			Afspraak a = new(_st, _b, _w);

			_afspraakManager.VoegAfspraakToe(a);

			_mockRepo.Verify(x => x.VoegAfspraakToe(a), Times.Once);
		}

		//[Fact]
		//public void VoegAfspraakToe_Invalid()
		//{
		//    _mockRepo = new Mock<IAfspraakRepository>();
		//    _afspraakManager = new AfspraakManager(_mockRepo.Object);

		//    //"AfspraakManager - VoegAfspraakToe - afspraak mag niet leeg zijn"
		//    Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VoegAfspraakToe(null));
		//    //"AfspraakManager - VoegAfspraakToe - Eindtijd mag nog niet ingevuld zijn"
		//    Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VoegAfspraakToe(new Afspraak(_st, _b, _w)));

		//    Assert.Throws<AfspraakException>(() => _afspraakManager.VoegAfspraakToe(new Afspraak(_st, _b, null)));
		//    Assert.Throws<AfspraakException>(() => _afspraakManager.VoegAfspraakToe(new Afspraak(_st, null, _w)));

		//    //"AfspraakManager - VoegAfspraakToe - afspraak bestaat al"
		//    _afspraakManager.VoegAfspraakToe(new Afspraak(_st, _b, _w));
		//    Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VoegAfspraakToe(new Afspraak(_st, _b, _w)));

		//}
		#endregion

		#region UnitTest Afspraak verwijderen
		//[Fact]
		//public void VerwijderAfspraak_Valid()
		//{
		//    _mockRepo = new Mock<IAfspraakRepository>();
		//    _afspraakManager = new AfspraakManager(_mockRepo.Object);
		//    Afspraak a = new(10, _st, _et, _b, _w);

		//    _afspraakManager.VerwijderAfspraak(a);

		//    _mockRepo.Verify(x => x.VerwijderAfspraak(a.Id), Times.Once);
		//}

		//[Fact]
		//public void VerwijderAfspraak_Invalid()
		//{
		//    _mockRepo = new Mock<IAfspraakRepository>();
		//    _afspraakManager = new AfspraakManager(_mockRepo.Object);

		//    //"AfspraakManager - VoegAfspraakToe - afspraak mag niet leeg zijn"
		//    Assert.Throws<AfspraakManagerException>(() => _afspraakManager.VerwijderAfspraak(null));

		//}
		#endregion
	}
}