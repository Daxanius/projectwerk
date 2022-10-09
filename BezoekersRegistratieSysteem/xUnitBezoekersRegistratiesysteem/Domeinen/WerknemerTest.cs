using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class WerknemerTest
	{
		private readonly Werknemer validWerknemer;
		private readonly Bedrijf validBedrijf;
		public WerknemerTest()
		{
			validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan1@gmail.com", bedrijf: validBedrijf, functie: "CEO");
		}

		[Fact]
		public void ZetBedrijf_NullAndEquals_Exception()
		{
			Assert.Throws<WerknemerException>(() => validWerknemer.ZetBedrijf(null));
		}

		[Fact]
		public void VerwijderBedrijf_Null_Exception()
		{
			validWerknemer.VerwijderBedrijf();
			Assert.Null(validWerknemer.Bedrijf);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetFunctie_NullAndWhiteSpace_Exception(string functie)
		{
			Assert.Throws<WerknemerException>(() => validWerknemer.ZetFunctie(functie));
		}
	}
}