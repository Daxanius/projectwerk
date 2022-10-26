using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System.Security;

namespace xUnitBezoekersRegistratiesysteem.Domeinen
{
	public class UnitTestBezoeker
	{
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

        [Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetBedrijf_Invalid(string bedrijf)
		{
            Bezoeker b = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
            Assert.Throws<BezoekerException>(() => b.ZetBedrijf(bedrijf));
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
        public void BezoekerIsGelijk_Invalid(uint id, string voornaam, string achternaam, string email, string bedrijf)
        {
            Bezoeker b1 = new(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf");
            Bezoeker b2 = new(id, voornaam, achternaam, email, bedrijf);
            Assert.False(b1.BezoekerIsGelijk(b2));
        }
        #endregion

        #region UnitTest Bedrijf Constructor
        [Theory]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

        [InlineData(10, "     bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker     ", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

        [InlineData(10, "bezoeker", "     bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen     ", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

        [InlineData(10, "bezoeker", "bezoekersen", "     bezoeker.bezoekersen@email.com", "bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com     ", "bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "     bezoekerbedrijf", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf     ", 10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        public void ctor_Valid(uint idIn, string voornaamIn, string achternaamIn, string emailIn, string bedrijfIn, uint idUit, string voornaamUit, string achternaamUit, string emailUit, string bedrijfUit)
        {
            Bezoeker b = new(idIn, voornaamIn, achternaamIn, emailIn, bedrijfIn);
            Assert.Equal(idUit, b.Id);
            Assert.Equal(voornaamUit, b.Voornaam);
            Assert.Equal(achternaamUit, b.Achternaam);
            Assert.Equal(emailUit, b.Email);
            Assert.Equal(bedrijfUit, b.Bedrijf);
        }

        [Theory]
        [InlineData(0, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

        [InlineData(10, null, "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, " ", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "\n", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "\r", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "\t", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "\v", "bezoekersen", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

        [InlineData(10, "bezoeker", null, "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", " ", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "\n", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "\r", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "\t", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "\v", "bezoeker.bezoekersen@email.com", "bezoekerbedrijf")]

        [InlineData(10, "bezoeker", "bezoekersen", null, "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", " ", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "\n", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "\r", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "\t", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "\v", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "@email.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@.com", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@.", "bezoekerbedrijf")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen.com", "bezoekerbedrijf")]

        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", null)]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", " ")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\n")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\r")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\t")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\v")]
        public void ctor_Invalid_PersoonException(uint id, string voornaam, string achternaam, string email, string bedrijf)
        {
            Assert.Throws<BezoekerException>(() => new Bezoeker(id, voornaam, achternaam, email, bedrijf));
        }
        #endregion
    }
}