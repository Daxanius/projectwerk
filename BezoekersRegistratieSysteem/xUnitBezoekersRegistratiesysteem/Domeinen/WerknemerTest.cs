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

			validWerknemer.ZetId(1);

			validBedrijf1.ZetId(1);
			validBedrijf2.ZetId(2);
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
			validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf1, "CEO");
			
			validWerknemer.VerwijderBedrijfVanWerknemer(validBedrijf1);
			Assert.Empty(validWerknemer.GeefBedrijfEnFunctiesPerWerknemer());

			validBedrijf1.VoegWerknemerToeInBedrijf(validWerknemer, "CEO");

			Assert.Equal(validBedrijf1.Id, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().First().Key.Id);

			validWerknemer.VerwijderBedrijfVanWerknemer(validBedrijf1);

			Assert.Empty(validWerknemer.GeefBedrijfEnFunctiesPerWerknemer());
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
			validWerknemer.VoegBedrijfEnFunctieToeAanWerknemer(validBedrijf2, "Technisch Medewerker");

			var listMetData = validWerknemer.GeefBedrijfEnFunctiesPerWerknemer();

			Assert.Equal(validBedrijf2.Naam, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.ToList()[1].Naam);
			Assert.Equal(validBedrijf2.TelefoonNummer, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.ToList()[1].TelefoonNummer);
			Assert.Equal(validBedrijf2.Adres, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.ToList()[1].Adres);
			Assert.Equal(validBedrijf2.BTW, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.ToList()[1].BTW);
			Assert.Equal(validBedrijf2.Email, validWerknemer.GeefBedrijfEnFunctiesPerWerknemer().Keys.ToList()[1].Email);
		}

		[Fact]
		public void VerwijderBedrijf()
		{		
			var listVanBedrijvenEnFuncties = validWerknemer.GeefBedrijfEnFunctiesPerWerknemer();

			Assert.Equal(1, listVanBedrijvenEnFuncties.Count);

			KeyValuePair<Bedrijf, List<string>> bedrijfEnFunctie = listVanBedrijvenEnFuncties.First();
			
			validWerknemer.VerwijderBedrijfVanWerknemer(bedrijfEnFunctie.Key);
			listVanBedrijvenEnFuncties = validWerknemer.GeefBedrijfEnFunctiesPerWerknemer();

			Assert.Equal(0, listVanBedrijvenEnFuncties.Count);
		}
		#endregion
	}
}