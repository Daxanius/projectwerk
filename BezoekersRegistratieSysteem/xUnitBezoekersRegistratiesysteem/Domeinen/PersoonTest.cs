using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class PersoonTest
	{
		private readonly Persoon validPersoon1;
		private readonly Persoon validPersoon2;
		private readonly Bezoeker validBezoeker;
		private readonly Werknemer validWerknemer;
		private readonly Bedrijf validBedrijf;
		public PersoonTest()
		{
			validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
			validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: validBedrijf, functie: "CEO");
			validPersoon1 = validBezoeker;
			validPersoon2 = validBezoeker;
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetVoornaam_Null_WhiteSpace_Exception(string voornaam)
		{
			Assert.Throws<PersoonException>(() => validPersoon1.ZetVoornaam(voornaam));
			Assert.Throws<PersoonException>(() => validPersoon2.ZetVoornaam(voornaam));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetAchternaam_Null_WhiteSpace_Exception(string achternaam)
		{
			Assert.Throws<PersoonException>(() => validPersoon1.ZetAchternaam(achternaam));
			Assert.Throws<PersoonException>(() => validPersoon2.ZetAchternaam(achternaam));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		[InlineData("@email.com")]
		[InlineData("Persoon.Persoons@email.")]
		[InlineData("Persoon.Persoons@.com")]
		[InlineData("Persoon.Persoons@email")]
		[InlineData("Persoon.Persoons@")]
		[InlineData("Persoon.Persoons")]
		[InlineData("Persoon.Persoons.com")]
		public void ZetEmail_Null_WhiteSpace_Exception(string email)
		{
			Assert.Throws<PersoonException>(() => validPersoon1.ZetEmail(email));
			Assert.Throws<PersoonException>(() => validPersoon2.ZetEmail(email));
		}
	}
}