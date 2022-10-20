using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using Moq;
using xUnitBezoekersRegistratiesysteem.DummyData.Repos;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class BezoekerManagerTest
	{
		private readonly IBezoekerRepository _bezoekerRepository;
		private readonly BezoekerManager _bezoekerManager;
		private readonly IAfspraakRepository _afspraakRepository;
		private readonly AfspraakManager _afspraakManager;
		private readonly Bezoeker Bezoeker;
		private readonly DateTime Starttijd;
		private readonly DateTime Eindtijd;
		private readonly Afspraak Afspraak;
		private readonly Werknemer Werknemer;
		private readonly Bedrijf Bedrijf;

		public BezoekerManagerTest()
		{
			_bezoekerRepository = new DummyBezoekerRepository();
			_bezoekerManager = new(_bezoekerRepository);
			_afspraakRepository = new DummyAfspraakRepository();
			_afspraakManager = new(_afspraakRepository);
			Bezoeker = new Bezoeker(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
			Starttijd = DateTime.Now;
			Eindtijd = DateTime.Now.AddHours(8);
			Bedrijf = new Bedrijf(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			Werknemer = new Werknemer(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com");
			Afspraak = new Afspraak(starttijd: Starttijd, bezoeker: Bezoeker, werknemer: Werknemer);

			Afspraak.ZetId(1);
			Bedrijf.ZetId(2);
			Bezoeker.ZetId(3);
			Werknemer.ZetId(4);
		}

		[Theory]
		[InlineData("aVoornaam", "aAchternaam", "aEmail@gmail.com", "aBedrijf")]
		[InlineData("stan", "persoons", "stan.persoons@student.hogent.com", "Hogent")]
		public void BezoekerManagerTest_VoegBezoekerToe(string voornaam, string achternaam, string email, string bedrijf)
		{
			Bezoeker bezoeker = new Bezoeker(voornaam, achternaam, email, bedrijf);
			bezoeker.ZetId(1);

			Bezoeker result = _bezoekerManager.VoegBezoekerToe(bezoeker);

			Assert.Equal(bezoeker, result);
		}

		[Theory]
		[InlineData(10)]
		[InlineData(0)]
		public void BezoekerManagerTest_GeefBezoeker(uint id)
		{
			Bezoeker bezoeker = Bezoeker;
			bezoeker.ZetId(id);
			_bezoekerManager.VoegBezoekerToe(bezoeker);

			Assert.Equal(bezoeker, _bezoekerManager.GeefBezoeker(id));
		}

		[Fact]
		public void BezoekerManagerTest_WijzigBezoeker()
		{
			Bezoeker bezoeker = new();
			bezoeker.ZetVoornaam("a");
			bezoeker.ZetAchternaam("b");
			bezoeker.ZetBedrijf("c");
			bezoeker.ZetId(2);
			bezoeker.ZetEmail("d@gmail.com");

			_bezoekerManager.VoegBezoekerToe(bezoeker);

			Bezoeker bezoeker1 = new();
			bezoeker1.ZetVoornaam("a");
			bezoeker1.ZetAchternaam("b");
			bezoeker1.ZetBedrijf("c");
			bezoeker1.ZetId(2);
			bezoeker1.ZetEmail("a@gmail.com");

			_bezoekerManager.WijzigBezoeker(bezoeker1);

			Assert.Equal(bezoeker1.Voornaam, _bezoekerManager.GeefBezoeker(bezoeker.Id).Voornaam);
			Assert.Equal(bezoeker1.Achternaam, _bezoekerManager.GeefBezoeker(bezoeker.Id).Achternaam);
			Assert.Equal(bezoeker1.Bedrijf, _bezoekerManager.GeefBezoeker(bezoeker.Id).Bedrijf);
			Assert.Equal(bezoeker1.Email, _bezoekerManager.GeefBezoeker(bezoeker.Id).Email);
		}

		//[Fact]
		//public void BezoekerManagerTest_GeefAanwezigeBezoekers()
		//{
			
		//}

		//[Fact]
		//public void BezoekerManagerTest_GeefBezoekersOpDatum()
		//{

		//}

		//[Theory]
		//[InlineData()]
		//public Bezoeker BezoekerManagerTest_GeefBezoekerOpNaam(string naam)
		//{

		//}
	}
}