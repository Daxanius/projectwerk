//using BezoekersRegistratieSysteemBL.Domeinen;
//using BezoekersRegistratieSysteemBL.Interfaces;

//namespace BezoekersRegistratieSysteemBL.Managers
//{
//	public class BezoekerManagerTest
//	{
//		private readonly IBezoekerRepository _bezoekerRepository;
//		private readonly Bezoeker _validBezoeker;
//		private readonly DateTime _validStarttijd;
//		private readonly DateTime _validEindtijd;
//		private readonly Afspraak _validAfspraak;
//		private readonly Werknemer _validWerknemer;
//		private readonly Bedrijf _validBedrijf;

//		public BezoekerManagerTest(IBezoekerRepository bezoekerRepository)
//		{
//			this._bezoekerRepository = bezoekerRepository;

//			_validBezoeker = new Bezoeker(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
//			_validStarttijd = DateTime.Now;
//			_validEindtijd = DateTime.Now.AddHours(8);
//			_validBedrijf = new Bedrijf(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
//			_validWerknemer = new Werknemer(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com");
//			_validAfspraak = new Afspraak(starttijd: _validStarttijd, bezoeker: _validBezoeker, werknemer: _validWerknemer);
//		}

//		#region Valid

//		[Theory]
//		[InlineData()]
//		public void BezoekerManagerTest_VoegBezoekerToe_Valid(string voornaam, string achternaam, string email, string bedrijf)
//		{

//		}

//		[Theory]
//		[InlineData()]
//		public void BezoekerManagerTest_VerwijderBezoeker_Valid(uint id)
//		{

//		}

//		[Theory]
//		[InlineData()]
//		public Bezoeker BezoekerManagerTest_GeefBezoeker_Valid(uint id)
//		{

//		}

//		[Fact]
//		public void BezoekerManagerTest_WijzigBezoeker_Valid()
//		{

//		}

//		[Fact]
//		public IReadOnlyList<Bezoeker> BezoekerManagerTest_GeefAanwezigeBezoekers_Valid()
//		{

//		}

//		[Fact]
//		public IReadOnlyList<Bezoeker> BezoekerManagerTest_GeefBezoekersOpDatum_Valid()
//		{

//		}

//		[Theory]
//		[InlineData()]
//		public Bezoeker BezoekerManagerTest_GeefBezoekerOpNaam_Valid(string naam)
//		{

//		}

//		#endregion

//		#region InValid

//		[Theory]
//		[InlineData()]
//		public void BezoekerManagerTest_VoegBezoekerToe_InValid(string voornaam, string achternaam, string email, string bedrijf)
//		{

//		}

//		[Theory]
//		[InlineData()]
//		public void BezoekerManagerTest_VerwijderBezoeker_InValid(uint id)
//		{

//		}

//		[Theory]
//		[InlineData()]
//		public Bezoeker BezoekerManagerTest_GeefBezoeker_InValid(uint id)
//		{

//		}

//		[Fact]
//		public void BezoekerManagerTest_WijzigBezoeker_InValid()
//		{

//		}

//		[Fact]
//		public IReadOnlyList<Bezoeker> BezoekerManagerTest_GeefAanwezigeBezoekers_InValid()
//		{

//		}

//		[Fact]
//		public IReadOnlyList<Bezoeker> BezoekerManagerTest_GeefBezoekersOpDatum_InValid()
//		{

//		}

//		[Theory]
//		[InlineData()]
//		public Bezoeker BezoekerManagerTest_GeefBezoekerOpNaam_InValid(string naam)
//		{

//		}

//		#endregion
//	}
//}