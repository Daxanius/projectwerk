using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class AfspraakTest
	{
		private readonly Bezoeker validBezoeker;
		private readonly DateTime validStarttijd;
		private readonly DateTime validEindtijd;
		private readonly Afspraak validAfspraak;
		private readonly Werknemer validWerknemer;
		private readonly Bedrijf validBedrijf;
		public AfspraakTest()
		{
			validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
			validStarttijd = DateTime.Now;
			validEindtijd = DateTime.Now.AddHours(8);
			validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: validBedrijf, functie: "CEO");
			validAfspraak = new Afspraak(starttijd: validStarttijd, bezoeker: validBezoeker, werknemer: validWerknemer);
		}

		#region InValid
		[Fact]
		public void ZetBezoeker_Null_Exception()
		{
			Assert.Throws<AfspraakException>(() => new Afspraak(validStarttijd, null, validWerknemer));
		}

		[Fact]
		public void ZetWerknemer_Null_Exception()
		{
			Assert.Throws<AfspraakException>(() => new Afspraak(validStarttijd, validBezoeker, null));
		}

		[Fact]
		public void ZetEindtijd_VoorStartijd_Exception()
		{
			Assert.Throws<AfspraakException>(() => validAfspraak.ZetEindtijd(new()));
		}
		#endregion

		#region Valid
		[Fact]
		public void ZetBezoeker_NotNull_Valid()
		{
			string voornaam, achternaam, email, bedrijf;

			voornaam = "stan1";
			achternaam = "persoons1";
			email = "stan.mail@gmail.com";
			bedrijf = "EA";

			Bezoeker bezoeker1 = new(voornaam: voornaam, achternaam: achternaam, email: email, bedrijf: bedrijf);

			validAfspraak.ZetBezoeker(bezoeker1);

			Assert.Equal(validAfspraak.Bezoeker.Voornaam, voornaam);
			Assert.Equal(validAfspraak.Bezoeker.Achternaam, achternaam);
			Assert.Equal(validAfspraak.Bezoeker.Email, email);
			Assert.Equal(validAfspraak.Bezoeker.Bedrijf, bedrijf);
		}

		[Fact]
		public void ZetWerknemer_NotNull_Valid()
		{
			validAfspraak.ZetWerknemer(validWerknemer);
			Assert.Equal(validAfspraak.Werknemer, validWerknemer); //Verder doen
		}

		[Fact]
		public void ZetEindtijd_NotVoorStartijd_Valid()
		{

		}
		#endregion
	}
}