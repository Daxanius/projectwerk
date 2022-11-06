using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class UnitTestAfspraakManager
    {
        //AF
        
		#region MOQ
		private AfspraakManager _afspraakManager;
		private Mock<IAfspraakRepository> _mockRepo;
		#endregion

		#region Valid Info
		private  DateTime _st;
		private  DateTime _et;
		private  Bezoeker _b;
        private Werknemer _w;

        private Bedrijf _bd;

        private Afspraak _ia;
        private Afspraak _oa;
        #endregion

        #region Initialiseren
        public UnitTestAfspraakManager()
        {
            _st = DateTime.Now;
            _et = _st.AddHours(2);

            _b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
            _w = new(10, "werknemer", "werknemersen");

            _bd = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

            _bd.VoegWerknemerToeInBedrijf(_w, "werknemer.werknemersen@email.com", "functie");
            
            _ia = new(_st, _bd, _b, _w);
            _oa = new(10, _st, _et, _bd, _b, _w);
        }
        #endregion

        #region UnitTest VoegAfspraakToe
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
            Assert.Equal("AfspraakManager - VoegAfspraakToe - er is nog een lopende afspraak voor dit email adres", ex.Message);
        }
        #endregion

        #region UnitTest VerwijderAfspraak
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

        #region UnitTest BewerkAfspraak
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

            //"AfspraakManager - BewerkAfspraak - afspraak bestaat niet"
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
            _mockRepo.Setup(x => x.BestaatAfspraak(_oa)).Returns(true);
            _mockRepo.Setup(x => x.GeefAfspraak(_oa.Id)).Returns(_oa);
            var ex = Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BewerkAfspraak(_oa));
            Assert.Equal("AfspraakManager - BewerkAfspraak - afspraak is niet gewijzigd", ex.Message);
        }
        #endregion

        #region UnitTest BeeindigAfspraakBezoeker
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

        #region UnitTest BeeindigAfspraakSysteem
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

        #region UnitTest BeeindigAfspraakOpEmail
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

        #region UnitTest BestaatLopendeAfspraak
        [Fact]
        public void BestaatLopendeAfspraak_Invalid_AfspraakLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - BestaatLopendeAfspraak - afspraak mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.BestaatLopendeAfspraak(null));
        }
        #endregion

        #region UnitTest GeefAfspraak
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

        #region UnitTest GeefHuidigeAfspraken
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

        #region UnitTest GeefHuidigeAfsprakenPerBedrijf
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

		#region UnitTest GeefAfsprakenPerBedrijfOpDag
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

        #region UnitTest GeefHuidigeAfsprakenPerWerknemerPerBedrijf
        [Fact]
        public void GeefHuidigeAfspraakPerWerknemer_Invalid_WerknemerLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfsprakenPerWerknemer - werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(null, _bd));
        }

        [Fact]
        public void GeefHuidigeAfspraakPerWerknemer_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfsprakenPerWerknemer - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(_w, null));
        }

        [Fact]
        public void GeefHuidigeAfspraakPerWerknemer_Invalid_GeenAfspraak()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraakPerWerknemer - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(_w.Id, _bd.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(_w,_bd);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest GeefAlleAfsprakenPerWerknemerPerBedrijf
        [Fact]
        public void GeefAlleAfsprakenPerWerknemerPerBedrijf_Invalid_WerknemerLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAlleAfsprakenPerWerknemer - werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAlleAfsprakenPerWerknemerPerBedrijf(null, _bd));
        }

        [Fact]
        public void GeefAlleAfsprakenPerWerknemerPerBedrijf_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAlleAfsprakenPerWerknemerPerBedrijf - werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAlleAfsprakenPerWerknemerPerBedrijf(_w, null));
        }

        [Fact]
        public void GeefAlleAfsprakenPerWerknemerPerBedrijf_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAlleAfsprakenPerWerknemerPerBedrijf - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAlleAfsprakenPerWerknemerPerBedrijf(_w.Id, _bd.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAlleAfsprakenPerWerknemerPerBedrijf(_w,_bd);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest GeefAfsprakenPerWerknemerOpDagPerBedrijf
        [Fact]
        public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_WerknemerLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - werknemer mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerWerknemerOpDagPerBedrijf(null, _st, _bd));
        }
        
        [Fact]
        public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_DatumInToekomst()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - opvraag datum kan niet in de toekomst liggen"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerWerknemerOpDagPerBedrijf(_w, _st.AddDays(1), _bd));
        }

        [Fact]
        public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerWerknemerOpDagPerBedrijf(_w, _st, null));
        }

        [Fact]
        public void GeefAfsprakenPerWerknemerOpDagPerBedrijf_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerWerknemerOpDagPerBedrijf - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerWerknemerOpDagPerBedrijf(_w.Id, _st, _bd.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerWerknemerOpDagPerBedrijf(_w, _st, _bd);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf
        [Fact]
        public void GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf("bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", null));
        }

        [Theory]
        [InlineData(null, null, null)]

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
        public void GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf_Invalid_NaamOfEmailLeeg(string voornaam, string achternaam, string email)
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - naam of email mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(voornaam, achternaam, email, _bd));
            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(voornaam, achternaam, email, null));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpNaamOfEmail_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(_b.Voornaam, _b.Achternaam, _b.Email, _bd.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(_b.Voornaam, _b.Achternaam, _b.Email, _bd);
            Assert.Empty(ex);
        }
        #endregion

        #region UnitTest GeefAfsprakenPerBezoekerOpDagPerBedrijf
        [Fact]
        public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_WerknemerLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - bezoeker mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpDagPerBedrijf(null, _st, _bd));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_DatumInToekomst()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - opvraag datum kan niet in de toekomst liggen"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpDagPerBedrijf(_b, _st.AddDays(1), _bd));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefAfsprakenPerBezoekerOpDagPerBedrijf(_b, _st, null));
        }

        [Fact]
        public void GeefAfsprakenPerBezoekerOpDagPerBedrijf_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAfsprakenPerBezoekerOpDagPerBedrijf - er zijn geen afspraken"
            _mockRepo.Setup(x => x.GeefAfsprakenPerBezoekerOpDagPerBedrijf(_b.Id, _st, _bd.Id)).Returns(new List<Afspraak>());
            var ex = _afspraakManager.GeefAfsprakenPerBezoekerOpDagPerBedrijf(_b, _st, _bd);
            Assert.Empty(ex);
        }
        #endregion
        
        #region UnitTest GeefHuidigeAfspraakBezoekerPerBedrijf
        [Fact]
        public void GeefHuidigeAfspraakBezoekerPerBedrijf_Invalid_EmailLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraakBezoekerPerBedrijf - bezoeker mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefHuidigeAfspraakBezoekerPerBedrijf(null, _bd));
        }

        [Fact]
        public void GeefHuidigeAfspraakBezoekerPerBedrijf_Invalid_BedrijfLeeg()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraakBezoekerPerBedrijf - bedrijf mag niet leeg zijn"
            Assert.Throws<AfspraakManagerException>(() => _afspraakManager.GeefHuidigeAfspraakBezoekerPerBedrijf(_b, null));
        }

        [Fact]
        public void GeefHuidigeAfspraakBezoekerPerBedrijf_Invalid_GeenAfspraken()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefHuidigeAfspraakBezoekerPerBedrijf - er is geen afspraak"
            _mockRepo.Setup(x => x.GeefHuidigeAfspraakBezoekerPerBerijf(_b.Id, _bd.Id)).Returns(new Afspraak());
            var ex = _afspraakManager.GeefHuidigeAfspraakBezoekerPerBedrijf(_b, _bd);
            Assert.Null(ex.Bezoeker);
        }
        #endregion

        #region UnitTest GeefAfsprakenPerDag
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

        #region UnitTest GeefAanwezigeBezoekers
        [Fact]
        public void GeefAanwezigeBezoekers_Invalid_GeenAanwezigeBezoekers()
        {
            _mockRepo = new Mock<IAfspraakRepository>();
            _afspraakManager = new AfspraakManager(_mockRepo.Object);

            //"AfspraakManager - GeefAanwezigeBezoekers - geen aanwezige bezoekers"
            _mockRepo.Setup(x => x.GeefAanwezigeBezoekers()).Returns(new List<Bezoeker>());
            var ex =_afspraakManager.GeefAanwezigeBezoekers();
            Assert.Empty(ex);
        }
        #endregion
    }
}