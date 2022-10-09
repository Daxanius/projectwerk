using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class BedrijfTest
	{
		private readonly Werknemer validWerknemer;
		private readonly Bedrijf validBedrijf;
		private readonly Bedrijf validBedrijf2;
		public BedrijfTest()
		{
			validBedrijf = new(naam: "HoGent", btw: "BE0475730461", telefoonNummer: "0476687242", email: "mail@hogent.be", adres: "Kerkstraat snorkelland 9000 101");
			validBedrijf2 = new(naam: "Odicee", btw: "BE0475730461", telefoonNummer: "04766872462", email: "mail@odice.be", adres: "Kerkstraat snorkelland 9000 104");
			validWerknemer = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: validBedrijf, functie: "CEO");
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetNaam_NullAndWhiteSpace_Exception(string naam)
		{
			Assert.Throws<BedrijfException>(() => validBedrijf.ZetNaam(naam));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetBTW_NullAndWhiteSpace_Exception(string btw)
		{
			Assert.Throws<BedrijfException>(() => validBedrijf.ZetBTW(btw));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetTelefoonNummer_NullAndWhiteSpace_Exception(string telefoonNummer)
		{
			Assert.Throws<BedrijfException>(() => validBedrijf.ZetTelefoonNummer(telefoonNummer));
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
		public void ZetAdres_NullAndWhiteSpace_Exception(string email)
		{
			Assert.Throws<BedrijfException>(() => validBedrijf.ZetEmail(email));
		}

		[Fact]
		public void VoegWerknemerToe_Null_Exception()
		{
			Bedrijf inValidBedrijf = validBedrijf;

			Assert.Throws<BedrijfException>(() => inValidBedrijf.VoegWerknemerToe(null));
		}

		[Fact]
		public void VoegWerknemerToe_ContainsWernemer_Exception()
		{
			Assert.Throws<BedrijfException>(() => validBedrijf.VoegWerknemerToe(validWerknemer));
		}

		[Fact]
		public void VerwijderWerknemer_Null_Exception()
		{
			Assert.Throws<BedrijfException>(() => validBedrijf.VerwijderWerknemer(null));
		}

		[Fact]
		public void VerwijderWerknemer_ContainsWernemer_Exception()
		{
			Assert.Throws<BedrijfException>(() => validBedrijf2.VerwijderWerknemer(validWerknemer));
		}

		[Fact]
		public void GeefWerknemers_Null_Exception()
		{
			Assert.NotNull(validBedrijf.GeefWerknemers());
		}
	}
}
