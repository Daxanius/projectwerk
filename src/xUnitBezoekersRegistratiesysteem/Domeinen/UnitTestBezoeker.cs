using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen
{
	public class UnitTestBezoeker
	{
		//AF

		#region UnitTest Id
		[Fact]
		public void ZetId_Valid()
		{
			Bezoeker b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			b.ZetId(10);
			Assert.Equal((long)10, b.Id);
		}

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ZetId_Invalid(long id)
		{
			Bezoeker b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			//"Werknemer - ZetId - Id moet groter zijn dan 0"
			Assert.Throws<BezoekerException>(() => b.ZetId(id));
		}
		#endregion

		#region UnitTest Voornaam
		[Fact]
		public void ZetVoornaam_Valid()
		{
			Bezoeker b = new(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			b.ZetVoornaam("Bezoeker");
			Assert.Equal("Bezoeker", b.Voornaam);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void ZetVoornaam_Invalid(string voornaam)
		{
			Bezoeker b = new(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			Assert.Throws<BezoekerException>(() => b.ZetVoornaam(voornaam));
		}
		#endregion

		#region UnitTest Achternaam
		[Fact]
		public void ZetAchternaam_Valid()
		{
			Bezoeker b = new(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			b.ZetAchternaam("Bezoekersen");
			Assert.Equal("Bezoekersen", b.Achternaam);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void ZetAchternaam_Invalid(string achternaam)
		{
			Bezoeker b = new(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			Assert.Throws<BezoekerException>(() => b.ZetAchternaam(achternaam));
		}
		#endregion

		#region UnitTest Email
		[Fact]
		public void ZetEmail_Valid()
		{
			Bezoeker b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			b.ZetEmail("bezoeker.bezoekersen@email.com");
			Assert.Equal("bezoeker.bezoekersen@email.com", b.Email);
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
		[InlineData("bezoeker.bezoekersen@email.")]
		[InlineData("bezoeker.bezoekersen@.com")]
		[InlineData("bezoeker.bezoekersen@email")]
		[InlineData("bezoeker.bezoekersen@")]
		[InlineData("bezoeker.bezoekersen")]
		[InlineData("bezoeker.bezoekersen@.")]
		[InlineData("bezoeker.bezoekersen.com")]
		public void ZetEmail_Invalid(string email)
		{
			Bezoeker b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			Assert.Throws<BezoekerException>(() => b.ZetEmail(email));
		}
		#endregion

		#region UnitTest Bedrijf
		[Theory]
		[InlineData("bezoekerbedrijf", "bezoekerbedrijf")]
		[InlineData("     bezoekerbedrijf", "bezoekerbedrijf")]
		[InlineData("bezoekerbedrijf     ", "bezoekerbedrijf")]
		public void ZetBedrijf(string bedrijfIn, string bedrijfUit)
		{
			Bezoeker b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			b.ZetBedrijf(bedrijfIn);
			Assert.Equal(bedrijfUit, b.Bedrijf);
		}
		#endregion

		#region UnitTest Bezoeker is gelijk
		[Fact]
		public void BezoekerIsGelijk_Valid()
		{
			Bezoeker b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			Assert.True(b.BezoekerIsGelijk(b));
		}

		[Theory]
		[InlineData(1, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "anderebezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "bezoeker", "anderebezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "bezoeker", "bezoekersen", "anderebezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "anderbezoekerbedrijf")]
		public void BezoekerIsGelijk_Invalid(long id, string voornaam, string achternaam, string email, string bedrijf)
		{
			Bezoeker b1 = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
			Bezoeker b2 = new(id, voornaam, achternaam, email, bedrijf);
			Assert.False(b1.BezoekerIsGelijk(b2));
		}
		#endregion

		#region UnitTest Bezoeker Constructor
		[Theory]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

		[InlineData(10, "     Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker     ", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

		[InlineData(10, "Bezoeker", "     Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen     ", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

		[InlineData(10, "Bezoeker", "Bezoekersen", "     bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com     ", "bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "     bezoekerbedrijf", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf     ", 10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		public void ctor_Valid(long idIn, string voornaamIn, string achternaamIn, string emailIn, string bedrijfIn, long idUit, string voornaamUit, string achternaamUit, string emailUit, string bedrijfUit)
		{
			Bezoeker b = new(idIn, voornaamIn, achternaamIn, emailIn, bedrijfIn);
			Assert.Equal(idUit, b.Id);
			Assert.Equal(voornaamUit, b.Voornaam);
			Assert.Equal(achternaamUit, b.Achternaam);
			Assert.Equal(emailUit, b.Email);
			Assert.Equal(bedrijfUit, b.Bedrijf);
		}

		[Theory]
		[InlineData(0, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(-1, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

        [InlineData(10, null, "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, " ", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "\n", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "\r", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "\t", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "\v", "Bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

		[InlineData(10, "Bezoeker", null, "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", " ", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "\n", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "\r", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "\t", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "\v", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

		[InlineData(10, "Bezoeker", "Bezoekersen", null, "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", " ", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "\n", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "\r", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "\t", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "\v", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "@email.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email.", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@.com", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@email", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen@.", "bezoekerbedrijf")]
		[InlineData(10, "Bezoeker", "Bezoekersen", "bezoeker.bezoekersen.com", "bezoekerbedrijf")]
		public void ctor_Invalid(long id, string voornaam, string achternaam, string email, string bedrijf)
		{
			Assert.Throws<BezoekerException>(() => new Bezoeker(id, voornaam, achternaam, email, bedrijf));
		}
		#endregion
	}
}