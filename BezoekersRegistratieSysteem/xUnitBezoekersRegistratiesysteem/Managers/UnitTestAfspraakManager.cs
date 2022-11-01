using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class UnitTestAfspraakManagerTest
    {
        //AF
        
		#region MOQ
		private AfspraakManager _afspraakManager;
		private Mock<IAfspraakRepository> _mockRepo;
		#endregion

		#region Valid Info
		private static DateTime _st = DateTime.Now;
		private static DateTime _et = _st.AddHours(2);
		private static Bezoeker _b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
		private static Werknemer _w = new(10, "werknemer", "werknemersen");

        private Bedrijf _bd = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

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

        [Fact]
        public void BewerkAfspraak_Invalid_AfspraakNietGewijzigd()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BewerkAfspraak - afspraak is niet gewijzigd"
            _mockRepo.Setup(x => x.GeefAfspraak(_oa.Id)).Returns(_oa);
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BewerkAfspraak(_oa));
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
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
        public void BeeindigAfspraakOpMail_Invalid_EmailLeeg(string email)
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BeeindigAfspraakOpMail - email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BeeindigAfspraakOpEmail(email));
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
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa.Id)).Returns(false);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfspraak(_oa.Id));
            Assert.Equal("AfspraakManager - GeefAfspraak - afspraak bestaat niet", ex.Message);
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
            _mockRepo.Setup(x => x.GeefHuidigeAfspraakPerWerknemer(_w.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefHuidigeAfspraakPerWerknemer(_w);
            Assert.Empty(ex);
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

        #region UnitTest Afspraken Per Bezoeker Op Naam of Email
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
        public void GeefAfsprakenPerBezoekerOpNaamOfEmail_Invalid_VoornaamLeeg(string voornaam)
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmail - naam of email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmail(voornaam, "bezoekersen", "bezoeker.bezoekersen@email.com"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
        public void GeefAfsprakenPerBezoekerOpNaamOfEmail_Invalid_AchternaamLeeg(string achternaam)
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmail - naam of email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmail("bezoeker", achternaam, "bezoeker.bezoekersen@email.com"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
        public void GeefAfsprakenPerBezoekerOpNaamOfEmail_Invalid_EmailLeeg(string email)
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmail - email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmail("bezoeker", "bezoekersen", email));
        }

        [Theory]
        [InlineData(null, null, null)]

        [InlineData(null, "bezoekersen", "bezoeker.bezoekersen@email.com")]

        [InlineData(null, null, "bezoeker.bezoekersen@email.com")]
        [InlineData(null, "", "bezoeker.bezoekersen@email.com")]
        [InlineData(null, " ", "bezoeker.bezoekersen@email.com")]
        [InlineData(null, "\n", "bezoeker.bezoekersen@email.com")]
        [InlineData(null, "\r", "bezoeker.bezoekersen@email.com")]
        [InlineData(null, "\t", "bezoeker.bezoekersen@email.com")]
        [InlineData(null, "\v", "bezoeker.bezoekersen@email.com")]
        
        [InlineData(null, "bezoekersen", null)]
        [InlineData(null, "bezoekersen", "")]
        [InlineData(null, "bezoekersen", " ")]
        [InlineData(null, "bezoekersen", "\n")]
        [InlineData(null, "bezoekersen", "\r")]
        [InlineData(null, "bezoekersen", "\t")]
        [InlineData(null, "bezoekersen", "\v")]

        [InlineData("bezoeker", null, "bezoeker.bezoekersen@email.com")]

        [InlineData("", null, "bezoeker.bezoekersen@email.com")]
        [InlineData(" ", null, "bezoeker.bezoekersen@email.com")]
        [InlineData("\n", null, "bezoeker.bezoekersen@email.com")]
        [InlineData("\r", null, "bezoeker.bezoekersen@email.com")]
        [InlineData("\t", null, "bezoeker.bezoekersen@email.com")]
        [InlineData("\v", null, "bezoeker.bezoekersen@email.com")]

        [InlineData("bezoeker", null, null)]
        [InlineData("bezoeker", null, "")]
        [InlineData("bezoeker", null, " ")]
        [InlineData("bezoeker", null, "\n")]
        [InlineData("bezoeker", null, "\r")]
        [InlineData("bezoeker", null, "\t")]
        [InlineData("bezoeker", null, "\v")]

        [InlineData("bezoeker", "bezoekersen", null)]

        [InlineData("", "bezoekersen", null)]
        [InlineData(" ", "bezoekersen", null)]
        [InlineData("\n", "bezoekersen", null)]
        [InlineData("\r", "bezoekersen", null)]
        [InlineData("\t", "bezoekersen", null)]
        [InlineData("\v", "bezoekersen", null)]

        [InlineData("bezoeker", "", null)]
        [InlineData("bezoeker", " ", null)]
        [InlineData("bezoeker", "\n", null)]
        [InlineData("bezoeker", "\r", null)]
        [InlineData("bezoeker", "\t", null)]
        [InlineData("bezoeker", "\v", null)]

        [InlineData(null, "", "")]
        [InlineData(null, " ", "")]
        [InlineData(null, "\n", "")]
        [InlineData(null, "\r", "")]
        [InlineData(null, "\t", "")]
        [InlineData(null, "\v", "")]

        [InlineData(null, "", " ")]
        [InlineData(null, " ", " ")]
        [InlineData(null, "\n", " ")]
        [InlineData(null, "\r", " ")]
        [InlineData(null, "\t", " ")]
        [InlineData(null, "\v", " ")]

        [InlineData(null, "", "\n")]
        [InlineData(null, " ", "\n")]
        [InlineData(null, "\n", "\n")]
        [InlineData(null, "\r", "\n")]
        [InlineData(null, "\t", "\n")]
        [InlineData(null, "\v", "\n")]

        [InlineData(null, "", "\r")]
        [InlineData(null, " ", "\r")]
        [InlineData(null, "\n", "\r")]
        [InlineData(null, "\r", "\r")]
        [InlineData(null, "\t", "\r")]
        [InlineData(null, "\v", "\r")]

        [InlineData(null, "", "\t")]
        [InlineData(null, " ", "\t")]
        [InlineData(null, "\n", "\t")]
        [InlineData(null, "\r", "\t")]
        [InlineData(null, "\t", "\t")]
        [InlineData(null, "\v", "\t")]

        [InlineData(null, "", "\v")]
        [InlineData(null, " ", "\v")]
        [InlineData(null, "\n", "\v")]
        [InlineData(null, "\r", "\v")]
        [InlineData(null, "\t", "\v")]
        [InlineData(null, "\v", "\v")]

        [InlineData("", null, null)]
        [InlineData(" ", null, "")]
        [InlineData("\n", null, "")]
        [InlineData("\r", null, "")]
        [InlineData("\t", null, "")]
        [InlineData("\v", null, "")]

        [InlineData(" ", null, " ")]
        [InlineData("\n", null, " ")]
        [InlineData("\r", null, " ")]
        [InlineData("\t", null, " ")]
        [InlineData("\v", null, " ")]

        [InlineData(" ", null, "\n")]
        [InlineData("\n", null, "\n")]
        [InlineData("\r", null, "\n")]
        [InlineData("\t", null, "\n")]
        [InlineData("\v", null, "\n")]

        [InlineData(" ", null, "\r")]
        [InlineData("\n", null, "\r")]
        [InlineData("\r", null, "\r")]
        [InlineData("\t", null, "\r")]
        [InlineData("\v", null, "\r")]

        [InlineData(" ", null, "\t")]
        [InlineData("\n", null, "\t")]
        [InlineData("\r", null, "\t")]
        [InlineData("\t", null, "\t")]
        [InlineData("\v", null, "\t")]

        [InlineData(" ", null, "\v")]
        [InlineData("\n", null, "\v")]
        [InlineData("\r", null, "\v")]
        [InlineData("\t", null, "\v")]
        [InlineData("\v", null, "\v")]

        [InlineData("", "", null)]
        [InlineData(" ", "", null)]
        [InlineData("\n", "", null)]
        [InlineData("\r", "", null)]
        [InlineData("\t", "", null)]
        [InlineData("\v", "", null)]

        [InlineData("", " ", null)]
        [InlineData(" ", " ", null)]
        [InlineData("\n", " ", null)]
        [InlineData("\r", " ", null)]
        [InlineData("\t", " ", null)]
        [InlineData("\v", " ", null)]

        [InlineData("", "\n", null)]
        [InlineData(" ", "\n", null)]
        [InlineData("\n", "\n", null)]
        [InlineData("\r", "\n", null)]
        [InlineData("\t", "\n", null)]
        [InlineData("\v", "\n", null)]

        [InlineData("", "\r", null)]
        [InlineData(" ", "\r", null)]
        [InlineData("\n", "\r", null)]
        [InlineData("\r", "\r", null)]
        [InlineData("\t", "\r", null)]
        [InlineData("\v", "\r", null)]

        [InlineData("", "\t", null)]
        [InlineData(" ", "\t", null)]
        [InlineData("\n", "\t", null)]
        [InlineData("\r", "\t", null)]
        [InlineData("\t", "\t", null)]
        [InlineData("\v", "\t", null)]

        [InlineData("", "\v", null)]
        [InlineData(" ", "\v", null)]
        [InlineData("\n", "\v", null)]
        [InlineData("\r", "\v", null)]
        [InlineData("\t", "\v", null)]
        [InlineData("\v", "\v", null)]
        public void GeefAfsprakenPerBezoekerOpNaamOfEmail_Invalid_NaamOfEmailLeeg(string voornaam, string achternaam, string email)
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfspraakPerBezoekerOpNaam - naam of email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmail(voornaam, achternaam, email));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpNaamOfEmail_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaam - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerBezoekerOpNaamOfEmail(_b.Voornaam, _b.Achternaam, _b.Email)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmail(_b.Voornaam, _b.Achternaam, _b.Email);
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

            //"AfspraakManager - GeefHuidigeAfspraakBezoeker - er is geen afspraak"
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