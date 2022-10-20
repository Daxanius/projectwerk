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
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com");
			validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf, "CEO");
			validPersoon1 = validBezoeker;
			validPersoon2 = validBezoeker;

			validBedrijf.ZetId(1);
			validPersoon1.ZetId(1);
			validPersoon1.ZetId(2);

			validWerknemer.ZetId(1);

			validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf, "CEO");
		}

		#region InValid
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
		#endregion

		#region Valid
		[Fact]
		public void ZetVoornaam()
		{
			string voorNaam = "Dax";

			validPersoon1.ZetVoornaam(voorNaam);
			validPersoon2.ZetVoornaam(voorNaam);

			Assert.Equal(voorNaam, validPersoon1.Voornaam);
			Assert.Equal(voorNaam, validPersoon2.Voornaam);
		}

		[Fact]
		public void ZetAchternaamn()
		{
			string achterNaam = "Rammstein";

			validPersoon1.ZetAchternaam(achterNaam);
			validPersoon2.ZetAchternaam(achterNaam);

			Assert.Equal(achterNaam, validPersoon1.Achternaam);
			Assert.Equal(achterNaam, validPersoon2.Achternaam);
		}

		[Fact]
		public void ZetEmail()
		{
			string email = "Sabaton@gmail.com";

			validPersoon1.ZetEmail(email);
			validPersoon2.ZetEmail(email);

			Assert.Equal(email, validPersoon1.Email);
			Assert.Equal(email, validPersoon2.Email);
		}
		#endregion
	}
}