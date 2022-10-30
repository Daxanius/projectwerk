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

        private Bedrijf _bd = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");

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

            //"AfspraakManager - BeeindigAfspraakBezoeker - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakBezoeker(null));
        }

        [Fact]
        public void BeeindigAfspraakBezoeker_Invalid_AfspraakReedsBeeindigd()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakBezoeker - afspraak bestaat al"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakBezoeker(_ia));
        }

        [Fact]
        public void BeeindigAfspraakBezoeker_Invalid_AfspraakBestaatNiet()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakBezoeker - afspraak bestaat al"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakBezoeker(_oa));
            Assert.Equal("AfspraakManager - BeeindigAfspraakBezoeker - afspraak is al beeindigd", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak beeindigen Systeem
        [Fact]
        public void BeeindigAfspraakSysteem_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakSysteem - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakSysteem(null));
        }

        [Fact]
        public void BeeindigAfspraakSysteem_Invalid_AfspraakReedsBeeindigd()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakSysteem - afspraak bestaat niet"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakSysteem(_ia));
        }

        [Fact]
        public void BeeindigAfspraakSysteem_Invalid_AfspraakBestaatNiet()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakSysteem - afspraak is al beeindigd"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakSysteem(_oa));
            Assert.Equal("AfspraakManager - BeeindigAfspraakSysteem - afspraak is al beeindigd", ex.Message);
        }
        #endregion

        #region UnitTest Afspraak beeindigen Email
        [Fact]
        public void BeeindigAfspraakOpMail_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakOpMail - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakOpEmail(null, _b.Email));
        }

        [Fact]
        public void BeeindigAfspraakOpMail_Invalid_EmailLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakOpMail - email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakOpEmail(_ia, null));
        }

        [Fact]
        public void BeeindigAfspraakOpEmail_Invalid_AfspraakReedsBeeindigd()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakSysteem - afspraak bestaat niet"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakOpEmail(_ia, _b.Email));
        }

        [Fact]
        public void BeeindigAfspraakOpEmail_Invalid_AfspraakBestaatNiet()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakOpEmail - afspraak is al beeindigd"
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakOpEmail(_oa, _b.Email));
            Assert.Equal("AfspraakManager - BeeindigAfspraakOpEmail - afspraak is al beeindigd", ex.Message);
        }
        #endregion

        #region UnitTest Bestaat lopende afspraak
        [Fact]
        public void BestaatLopendeAfspraak_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BestaatLopendeAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BestaatLopendeAfspraak(null));
        }
        #endregion

        #region UnitTest Afspraak opvragen
        [Fact]
        public void GeefAfspraak_Invalid_AfspraakBestaatNiet()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfspraak - afspraak bestaat niet"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfspraak(0));
        }
        #endregion

        #region UnitTest Huidige Afspraken
        [Fact]
        public void GeefHuidigeAfspraken_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraken - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefHuidigeAfspraken()).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefHuidigeAfspraken();
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Huidige Afspraken Per Bedrijf
        [Fact]
        public void GeefHuidigeAfsprakenPerBedrijf_Invalid_BedrijfNull()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfsprakenPerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefHuidigeAfsprakenPerBedrijf(null));
        }
        
        [Fact]
        public void GeefHuidigeAfsprakenPerBedrijf_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfsprakenPerBedrijf - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefHuidigeAfsprakenPerBedrijf(_bd.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefHuidigeAfsprakenPerBedrijf(_bd);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Afspraken Per Bedrijf Op Dag
        [Fact]
        public void GeefAfsprakenPerBedrijfOpDag_Invalid_WerknemerNull()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBedrijfOpDag - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBedrijfOpDag(null, _st));
        }

        [Fact]
        public void GeefAfsprakenPerBedrijfOpDag_Invalid_DatumInToekomst()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBedrijfOpDag - opvraag datum kan niet in de toekomst liggen"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBedrijfOpDag(_bd, _st.AddDays(1)));
        }

        [Fact]
        public void GeefAfsprakenPerBedrijfOpDag_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBedrijfOpDag - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerBedrijfOpDag(_bd.Id, _st)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerBedrijfOpDag(_bd, _st);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Huidige Afspraak voor Werknemer
        [Fact]
        public void GeefHuidigeAfspraakPerWerknemer_Invalid_WerknemerNull()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfsprakenPerWerknemer - werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefHuidigeAfspraakPerWerknemer(null));
        }

        [Fact]
        public void GeefHuidigeAfspraakPerWerknemer_Invalid_GeenAfspraak()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraakPerWerknemer - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefHuidigeAfspraakPerWerknemer(_w.Id)).Returns(new Afspraak());
            var ex = _afspraakManager.GeefHuidigeAfspraakPerWerknemer(_w);
            Assert.Null(ex.Werknemer);
        }
        #endregion
        
        #region UnitTest Alle Afspraken Per Werknemer
        [Fact]
        public void GeefAlleAfsprakenPerWerknemer_Invalid_WerknemerNull()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAlleAfsprakenPerWerknemer - werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAlleAfsprakenPerWerknemer(null));
        }

        [Fact]
        public void GeefAlleAfsprakenPerWerknemer_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAlleAfsprakenPerWerknemer - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAlleAfsprakenPerWerknemer(_w.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAlleAfsprakenPerWerknemer(_w);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Afspraken Per Werknemer Op Dag
        [Fact]
        public void GeefAfsprakenPerWerknemerOpDag_Invalid_WerknemerNull()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);
            
            //"AfspraakManager - GeefAfsprakenPerWerknemerOpDag - werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerWerknemerOpDag(null, _st));
        }
        
        [Fact]
        public void GeefAfsprakenPerWerknemerOpDag_Invalid_DatumInToekomst()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerWerknemerOpDag - opvraag datum kan niet in de toekomst liggen"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerWerknemerOpDag(_w, _st.AddDays(1)));
        }

        [Fact]
        public void GeefAfsprakenPerWerknemerOpDag_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAlleAfsprakenPerWerknemer - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerWerknemerOpDag(_w.Id, _st)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerWerknemerOpDag(_w, _st);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Afspraken Per Dag
        [Fact]
        public void GeefAfsprakenPerDag_Invalid_DatumInToekomst()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerDag - opvraag datum kan niet in de toekomst liggen"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerDag(_st.AddDays(1)));
        }

        [Fact]
        public void GeefAfsprakenPerDag_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerDag - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerDag(_st)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerDag(_st);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Afspraken Per Bezoeker Op Naam
        [Fact]
        public void GeefAfsprakenPerBezoekerOpNaam_Invalid_VoornaamLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfspraakPerBezoekerOpNaam - naam mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaam(null, "bezoekersen"));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpNaam_Invalid_AchternaamLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfspraakPerBezoekerOpNaam - naam mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaam("bezoeker", null));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpNaam_Invalid_NaamLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfspraakPerBezoekerOpNaam - naam mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaam(null, null));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpNaam_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaam - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerBezoekerOpNaam(_b.Voornaam, _b.Achternaam)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerBezoekerOpNaam(_b.Voornaam, _b.Achternaam);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Afspraken Per Bezoeker Op Email
        [Fact]
        public void GeefAfsprakenPerBezoekerOpEmail_Invalid_EmailLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpEmail - email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpEmail(null));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpEmail_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpEmail - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerBezoekerOpEmail(_b.Email)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerBezoekerOpEmail(_b.Email);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest Huidige Afspraak voor Bezoeker
        [Fact]
        public void GeefHuidigeAfspraakBezoeker_Invalid_EmailLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraakBezoeker - bezoeker mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefHuidigeAfspraakBezoeker(null));
        }

        [Fact]
        public void GeefHuidigeAfspraakBezoeker_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraakBezoeker - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefHuidigeAfspraakBezoeker(_b.Id)).Returns(new Afspraak());
            var ex = _afspraakManager.GeefHuidigeAfspraakBezoeker(_b);
            Assert.Null(ex.Bezoeker);
        }
        #endregion

        #region UnitTest Afspraken Per Werknemer Op Dag
        [Fact]
        public void GeefAfsprakenPerBezoekerOpDag_Invalid_WerknemerNull()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpDag - bezoeker mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpDag(null, _st));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpDag_Invalid_DatumInToekomst()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpDag - opvraag datum kan niet in de toekomst liggen"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpDag(_b, _st.AddDays(1)));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpDag_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAlleAfsprakenPerWerknemer - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerBezoekerOpDag(_b.Id, _st)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerBezoekerOpDag(_b, _st);
            Assert.Empty(ex);
        }
        #endregion
    }
}