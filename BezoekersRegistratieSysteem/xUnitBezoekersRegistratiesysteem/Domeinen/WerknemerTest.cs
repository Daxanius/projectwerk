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
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan1@gmail.com");
			validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf1, "CEO");
		}

		#region InValid
		[Fact]
		public void ZetBedrijf_NullAndEquals_Exception()
		{
			Assert.Throws<WerknemerException>(() => validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(null, "Manager"));
		}

		[Fact]
		public void VerwijderBedrijf_Null_Exception()
		{
			validWerknemer.VerwijderBedrijfVanWerknemer(validBedrijf1);
			
			Assert.Equal(validBedrijf1.Naam, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().Naam);
			Assert.Equal(validBedrijf1.TelefoonNummer, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().TelefoonNummer);
			Assert.Equal(validBedrijf1.Adres, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().Adres);
			Assert.Equal(validBedrijf1.BTW, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().BTW);
			Assert.Equal(validBedrijf1.Email, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().Email);
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
			Bedrijf bedrijf = new();
			Assert.Throws<WerknemerException>(() => validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, functie));
		}
		#endregion

		#region Valid
		[Fact]
		public void ZetBedrijf()
		{
			validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf2, "CEO");

			Assert.Equal(validBedrijf2.Naam, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().Naam);
			Assert.Equal(validBedrijf2.TelefoonNummer, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().TelefoonNummer);
			Assert.Equal(validBedrijf2.Adres, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().Adres);
			Assert.Equal(validBedrijf2.BTW, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().BTW);
			Assert.Equal(validBedrijf2.Email, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.First().Email);
		}

		[Fact]
		public void VerwijderBedrijf()
		{
			KeyValuePair<Bedrijf, List<string>> bedrijfEnFunctie = validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().First();
			validWerknemer.VerwijderBedrijfVanWerknemer(bedrijfEnFunctie.Key);

			IReadOnlyDictionary<Bedrijf, List<string>> lijstMetBedrijven = validWerknemer.GeefBedrijfEnFunctiesPerWerknemer();

			Assert.Contains(bedrijfEnFunctie.Key, lijstMetBedrijven.Keys);
			Assert.Contains(bedrijfEnFunctie.Value, lijstMetBedrijven.Values);
		}
		#endregion
	}
}