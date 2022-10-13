using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using xUnitBezoekersRegistratiesysteem.DummyRepo;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class AfspraakManagerTest {
		private readonly IAfspraakRepository _afspraakRepository;

		private readonly Bezoeker validBezoeker;
		private readonly DateTime validStarttijd;
		private readonly DateTime validEindtijd;
		private readonly Afspraak validAfspraak;
		private readonly Werknemer validWerknemer;
		private readonly Bedrijf validBedrijf;
		
		public AfspraakManagerTest()
		{
			validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
			validStarttijd = DateTime.Now;
			validEindtijd = DateTime.Now.AddHours(8);
			validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: validBedrijf, functie: "CEO");
			validAfspraak = new Afspraak(starttijd: validStarttijd, bezoeker: validBezoeker, werknemer: validWerknemer);

			_afspraakRepository = new DummyAfspraakRepository();
		}

		[Fact]
		public void MaakAfspraak() {

		}

		[Theory]
		public void VerwijderAfspraak(uint id) {
			
		}

		[Theory]
		public Afspraak GeefAfspraak(uint id) {
			
		}

		[Fact]
		public void BewerkAfspraak() {

		}

		[Fact]
		public void BeeindigAfspraak() {

		}

		[Fact]
		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {

		}

		[Fact]
		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf() {
			
		}

		[Fact]
		public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer() {

		}

		[Fact]
		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer() {

		}

		[Fact]
		public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDatum() {
			
		}

		[Fact]
		public IReadOnlyList<Afspraak> GeefAfsprakenPerDag() {

		}
	}
}