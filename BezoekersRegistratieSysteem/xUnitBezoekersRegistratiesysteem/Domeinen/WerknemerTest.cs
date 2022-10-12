using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class WerknemerTest
	{
		private readonly Werknemer validWerknemer;
		private readonly Bedrijf validBedrijf1;
		private readonly Bedrijf validBedrijf2;
		public WerknemerTest()
		{
			validBedrijf1 = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			validBedrijf2 = new(naam: "Artevelde", btw: "BE0475730464", telefoonNummer: "0476687244", email: "mail.me@artevelde.be", adres: "Kerkstraat snorkelland 9006 101");
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan1@gmail.com", bedrijf: validBedrijf1, functie: "CEO");
		}

		#region InValid
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
		#endregion

		#region Valid
		[Fact]
		public void ZetBedrijf()
		{
			validWerknemer.ZetBedrijf(validBedrijf2);

			Assert.Equal(validBedrijf2.Naam, validWerknemer.Bedrijf.Naam);
		}

		[Fact]
		public void VerwijderBedrijf()
		{
			validWerknemer.VerwijderBedrijf();

			Assert.Null(validWerknemer.Bedrijf);
		}

		[Fact]
		public void ZetFunctie()
		{
			string functie = "CIA";

			validWerknemer.ZetFunctie(functie);

			Assert.Equal(functie, validWerknemer.Functie);
		}
		#endregion
	}
}