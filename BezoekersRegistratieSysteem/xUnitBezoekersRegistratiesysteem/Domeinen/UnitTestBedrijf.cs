using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen
{
	public class UnitTestBedrijf
	{
		//AF

		#region Valid Info
		private Werknemer _w = new(10, "werknemer", "werknemersen");
		private string _f1 = "functie1";
		private string _f2 = "functie2";
		private string _e = "werknemer.werknemersen@email.com";
		#endregion

		#region UnitTest Id
		[Fact]
		public void ZetId_Valid()
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.ZetId(10);
			Assert.Equal((long)10, b.Id);
		}

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ZetId_Invalid(long id)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Assert.Throws<BedrijfException>(() => b.ZetId(id));
		}
		#endregion

		#region UnitTest Naam
		[Theory]
		[InlineData("bedrijf", "bedrijf")]
		[InlineData("     bedrijf", "bedrijf")]
		[InlineData("bedrijf     ", "bedrijf")]
		public void ZetNaam_Valid(string naamIn, string naamUit)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.ZetNaam(naamIn);
			Assert.Equal(naamUit, b.Naam);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void ZetNaam_Invalid(string naam)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Assert.Throws<BedrijfException>(() => b.ZetNaam(naam));
		}
		#endregion

		#region UnitTest BTW
		[Theory]
		[InlineData("BE0676747521", "BE0676747521")]
		[InlineData("     BE0676747521", "BE0676747521")]
		[InlineData("BE0676747521     ", "BE0676747521")]
		public void ZetBTW_Valid(string btwIn, string btwUit)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.ZetBTW(btwIn, true);
			Assert.Equal(btwUit, b.BTW);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void ZetBTW_Invalid(string btw)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Assert.Throws<BedrijfException>(() => b.ZetBTW(btw, true));
		}
		#endregion

		#region UnitTest BTWControle
		[Theory]
		[InlineData("BE0676747521", "BE0676747521")]
		[InlineData("     BE0676747521", "BE0676747521")]
		[InlineData("BE0676747521     ", "BE0676747521")]
		public void ZetBTWControle_Valid(string btwIn, string btwUit)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.ZetBTWControle(btwIn);
			Assert.Equal(btwUit, b.BTW);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		[InlineData("BE123456789")] //nep BTWnummer
		public void ZetBTWControle_Invalid(string btw)
		{
			Assert.Throws<BedrijfException>(() => new Bedrijf("bedrijf", btw, "012345678", "bedrijf@email.com", "bedrijfstraat 10"));
		}
		#endregion

		#region UnitTest Telefoonnummer
		[Theory]
		[InlineData("012345678", "012345678")]
		[InlineData("     012345678", "012345678")]
		[InlineData("012345678     ", "012345678")]
		public void ZetTelefoonnummer_Valid(string telefoonnummerIn, string telefoonnummerUit)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.ZetTelefoonNummer(telefoonnummerIn);
			Assert.Equal(telefoonnummerUit, b.TelefoonNummer);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		[InlineData("0123456789101112")]
		public void ZetTelefoonnummer_Invalid(string telefoonnummer)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Assert.Throws<BedrijfException>(() => b.ZetTelefoonNummer(telefoonnummer));
		}
		#endregion

		#region UnitTest Email
		[Theory]
		[InlineData("bedrijf@email.com", "bedrijf@email.com")]
		[InlineData("     bedrijf@email.com", "bedrijf@email.com")]
		[InlineData("bedrijf@email.com     ", "bedrijf@email.com")]
		public void ZetEmail_Valid(string emailIn, string emailUit)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.ZetEmail(emailIn);
			Assert.Equal(emailUit, b.Email);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		[InlineData("@email.com")]
		[InlineData("bedrijf@email.")]
		[InlineData("bedrijf@.com")]
		[InlineData("bedrijf@email")]
		[InlineData("bedrijf@")]
		[InlineData("bedrijf")]
		[InlineData("bedrijf@.")]
		[InlineData("bedrijf.com")]
		public void ZetEmail_Invalid(string email)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Assert.Throws<BedrijfException>(() => b.ZetEmail(email));
		}
		#endregion

		#region UnitTest Adres
		[Theory]
		[InlineData("bedrijfstraat 10", "bedrijfstraat 10")]
		[InlineData("     bedrijfstraat 10", "bedrijfstraat 10")]
		[InlineData("bedrijfstraat 10     ", "bedrijfstraat 10")]
		public void ZetAdres_Valid(string adresIn, string adresUit)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.ZetAdres(adresIn);
			Assert.Equal(adresUit, b.Adres);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void ZetAdres_Invalid(string adres)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Assert.Throws<BedrijfException>(() => b.ZetAdres(adres));

		}
		#endregion

		#region UnitTest Voeg Werknemer Toe
		[Fact]
		public void VoegWerknemerToeInBedrijf_Valid()
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.VoegWerknemerToeInBedrijf(_w, _e, _f1);
			Assert.Contains(_w, b.GeefWerknemers());
			//Check: Meerdere functies per werknemer bij 1 bedrijf
			b.VoegWerknemerToeInBedrijf(_w, _e, _f2);
			Assert.Contains(_w, b.GeefWerknemers());
		}

		[Theory]
		[InlineData("@email.com", "functie")]
		[InlineData("werknemer.werknemersen@email.", "functie")]
		[InlineData("werknemer.werknemersen@.com", "functie")]
		[InlineData("werknemer.werknemersen@email", "functie")]
		[InlineData("werknemer.werknemersen@", "functie")]
		[InlineData("werknemer.werknemersen", "functie")]
		[InlineData("werknemer.werknemersen@.", "functie")]
		[InlineData("werknemer.werknemersen.com", "functie")]
		[InlineData("werknemer.werknemersen@email.com", null)]
		[InlineData("werknemer.werknemersen@email.com", "")]
		[InlineData("werknemer.werknemersen@email.com", " ")]
		[InlineData("werknemer.werknemersen@email.com", "\n")]
		[InlineData("werknemer.werknemersen@email.com", "\r")]
		[InlineData("werknemer.werknemersen@email.com", "\t")]
		[InlineData("werknemer.werknemersen@email.com", "\v")]
		[InlineData(null, "functie")]
		[InlineData("", "functie")]
		[InlineData(" ", "functie")]
		[InlineData("\n", "functie")]
		[InlineData("\r", "functie")]
		[InlineData("\t", "functie")]
		[InlineData("\v", "functie")]
		public void VoegWerknemerToeInBedrijf_Invalid(string email, string functie)
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(null, _e, _f1));
			Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(null, null, null));
			if (string.IsNullOrWhiteSpace(functie))
				Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, email, functie));
			else if (string.IsNullOrWhiteSpace(email))
				Assert.Throws<WerknemerException>(() => b.VoegWerknemerToeInBedrijf(_w, email, functie));
			else
				Assert.Throws<WerknemerInfoException>(() => b.VoegWerknemerToeInBedrijf(_w, email, functie));
			//"Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - werknemer is in dit bedrijf al werkzaam onder deze functie"
			b.VoegWerknemerToeInBedrijf(_w, _e, _f1);
			Assert.Throws<WerknemerException>(() => b.VoegWerknemerToeInBedrijf(_w, _e, _f1));
		}
		#endregion

		#region UnitTest Verwijder Werknemer
		[Fact]
		public void VerwijderWerknemerUitBedrijf_Valid()
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.VoegWerknemerToeInBedrijf(_w, _e, _f1);
			b.VerwijderWerknemerUitBedrijf(_w);
			Assert.DoesNotContain(_w, b.GeefWerknemers());
		}

		[Fact]
		public void VerwijderWerknemerUitBedrijf_Invalid()
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			Assert.Throws<BedrijfException>(() => b.VerwijderWerknemerUitBedrijf(null));
			Assert.Throws<BedrijfException>(() => b.VerwijderWerknemerUitBedrijf(_w));
		}
		#endregion

		#region UnitTest Geef Werknemers
		[Fact]
		public void GeefWerknemers_Valid()
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			b.VoegWerknemerToeInBedrijf(_w, _e, _f1);
			Assert.Equal(_w, b.GeefWerknemers()[0]);
		}
		#endregion

		#region UnitTest Bedrijf is gelijk
		[Fact]
		public void BedrijfIsGelijk_Valid()
		{
			Bedrijf b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Assert.True(b.BedrijfIsGelijk(b));
		}

		[Theory]
		[InlineData(1, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "fjirdeb", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0724540609", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "876543210", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "anderbedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "anderebedrijfstraat 10")]
		public void BedrijfIsGelijk_Invalid(long id, string naam, string btwNummer, string telefoonNummer, string email, string adres)
		{
			Bedrijf b1 = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			Bedrijf b2 = new(id, naam, btwNummer, true, telefoonNummer, email, adres);
			Assert.False(b1.BedrijfIsGelijk(b2));
		}
		#endregion

		#region UnitTest Bedrijf Constructor
		[Theory]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "     bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf     ", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", "     BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521     ", "012345678", "bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", "BE0676747521", "     012345678", "bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678     ", "bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "     bedrijf@email.com", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com     ", "bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "     bedrijfstraat 10", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10     ", 10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		public void ctor_Valid(long idIn, string naamIn, string btwNummerIn, string telefoonNummerIn, string emailIn, string adresIn, long idUit, string naamUit, string btwNummerUit, string telefoonNummerUit, string emailUit, string adresUit)
		{
			Bedrijf b = new(idIn, naamIn, btwNummerIn, true, telefoonNummerIn, emailIn, adresIn);
			Assert.Equal(idUit, b.Id);
			Assert.Equal(naamUit, b.Naam);
			Assert.Equal(btwNummerUit, b.BTW);
			Assert.Equal(emailUit, b.Email);
			Assert.Equal(telefoonNummerUit, b.TelefoonNummer);
			Assert.Equal(adresUit, b.Adres);
		}

		[Theory]
		[InlineData(0, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, null, "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, " ", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "\n", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "\r", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "\t", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "\v", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", null, "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", " ", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "\n", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "\r", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "\t", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "\v", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE123456789", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", "BE0676747521", null, "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", " ", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "\n", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "\r", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "\t", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "\v", "bedrijf@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "0123456789101112", "bedrijf@email.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", "BE0676747521", "012345678", null, "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", " ", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "\n", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "\r", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "\t", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "\v", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "@email.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@.com", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@.", "bedrijfstraat 10")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf.com", "bedrijfstraat 10")]

		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", null)]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", " ")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "\n")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "\r")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "\t")]
		[InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "\v")]

		[InlineData(10, null, null, null, null, null)]
		public void ctor_Invalid(long id, string naam, string btwNummer, string telefoonNummer, string email, string adres)
		{
			Assert.Throws<BedrijfException>(() => {
				Bedrijf b = new Bedrijf(id, naam, btwNummer, true, telefoonNummer, email, adres);
				b.ZetBTWControle(btwNummer);
			});

		}
		#endregion
	}
}
