using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class ParkingContractTest
	{
		private readonly Bezoeker validBezoeker;
		private readonly DateTime validStarttijd;
		private readonly DateTime validEindtijd;
		private readonly Werknemer validWerknemer;
		private readonly Bedrijf validBedrijf;

		private const int inValidAantalPlaatsen = -1;
		private const int validAantalPlaatsen = 1;

		public ParkingContractTest()
		{
			validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
			validStarttijd = DateTime.Now;
			validEindtijd = DateTime.Now.AddHours(8);
			validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: validBedrijf, functie: "CEO");
		}

		[Fact]
		public void ZetAantalPlaatsen_Negatief_Exception()
		{
			Assert.Throws<ParkingContractException>(() => new ParkingContract(starttijd: validStarttijd, aantalPlaatsen: inValidAantalPlaatsen));
		}

		[Fact]
		public void ZetEindtijd_VoorStartijd_Exception()
		{
			ParkingContract parkingContract = new ParkingContract(starttijd: validEindtijd, aantalPlaatsen: validAantalPlaatsen);
			Assert.Throws<ParkingContractException>(() => parkingContract.ZetEindtijd(new()));
		}
	}
}