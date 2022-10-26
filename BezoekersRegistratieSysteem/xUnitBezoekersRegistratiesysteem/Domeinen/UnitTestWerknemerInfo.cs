using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen
{
    public class UnitTestWerknemerInfo
    {
        #region Valid Info
        private Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
        private string _e = "werknemer.werknemersen@email.com";
        #endregion

        #region UnitTest Bedrijf
        [Fact]
        public void ZetBedrijf_Valid()
        {
            WerknemerInfo wi = new(_b, "werknemer.werknemersen@email.com");
            wi.ZetBedrijf(_b);
            Assert.Equal(_b, wi.Bedrijf);
        }
        
        [Fact]
        public void ZetBedrijf_Invalid()
        {
            WerknemerInfo wi = new(_b, "werknemer.werknemersen@email.com");
            Assert.Throws<WerknemerInfoException>(() => wi.ZetBedrijf(null));
        }
        #endregion

        #region UnitTest Email
        [Fact]
        public void ZetEmail_Valid()
        {
            WerknemerInfo wi = new(_b, "werknemer.werknemersen@email.com");
            wi.ZetEmail("werknemer.werknemersen@email.com");
            Assert.Equal("werknemer.werknemersen@email.com", wi.Email);
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
        [InlineData("werknemer.werknemersen@email.")]
        [InlineData("werknemer.werknemersen@.com")]
        [InlineData("werknemer.werknemersen@email")]
        [InlineData("werknemer.werknemersen@")]
        [InlineData("werknemer.werknemersen")]
        [InlineData("werknemer.werknemersen@.")]
        [InlineData("werknemer.werknemersen.com")]
        public void ZetEmail_Invalid(string email)
        {
            WerknemerInfo wi = new(_b, "werknemer.werknemersen@email.com");
            Assert.Throws<WerknemerInfoException>(() => wi.ZetEmail(email));
        }
        #endregion

        #region UnitTest WerknemerInfo ctor
        [Fact]
        public void ctor_Valid()
        {
            WerknemerInfo wi = new(_b, _e);
            Assert.Equal(_b, wi.Bedrijf);
            Assert.Equal(_e, wi.Email);
        }
        [Fact]
        public void ctor_Invalid()
        {
            Assert.Throws<WerknemerInfoException>(() => new WerknemerInfo(null, _e));
            Assert.Throws<WerknemerInfoException>(() => new WerknemerInfo(_b, null));
        }      
        #endregion
    }
}
