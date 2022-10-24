using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class UnitTestWerknemer
	{
		private Bedrijf _b1 = new(10, "bedrijf", "BE123456789", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
        private Bedrijf _b2 = new(1, "anderbedrijf", "BE987654321", "876543210", "anderbedrijf@email.com", "anderebedrijfstraat 10");
        private string _of = "oudefunctie";
        private string _nf = "nieuwefunctie";

        #region UnitTest Voeg Bedrijf & Functie Toe Aan Werknemer
        [Fact]
        public void VoegBedrijvenEnFunctieToeAanWerknemer_Valid()
        {
            Werknemer w = new(10, "werknemer", "werknemersen", "werknemer.werknemersen@email.com");
            w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _nf);
            IReadOnlyDictionary<Bedrijf, List<string>> actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
            Assert.Collection(actual,
                expected =>
                {
                    Assert.Equal(_b1, expected.Key);
                    Assert.Collection(expected.Value,
                        functie => Assert.Equal(_nf, functie));
                });

            //meerdere functies check
            w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _of);
            actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
            Assert.Collection(actual,
                expected =>
                {
                    Assert.Equal(_b1, expected.Key);
                    Assert.Collection(expected.Value,
                        functie => Assert.Equal(_nf, functie),
                        functie => Assert.Equal(_of, functie));
                });
        }
        
        [Fact]
        public void VoegBedrijvenEnFunctieToeAanWerknemer_Invalid()
        {
            Werknemer w = new(10, "werknemer", "werknemersen", "werknemer.werknemersen@email.com");
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(null, _nf));
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, null));
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, ""));
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, " "));
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, "\n"));
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, "\r"));
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, "\t"));
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, "\v"));
            //CHECK duplicates
            w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _nf);
            Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _nf));
        }
        #endregion

        #region UnitTest Verwijder Bedrijf
        [Fact]
        public void VerwijderBedrijfVanWerknemer_Valid()
        {
            Werknemer w = new(10, "werknemer", "werknemersen", "werknemer.werknemersen@email.com");
            w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _nf);
            w.VerwijderBedrijfVanWerknemer(_b1);
            IReadOnlyDictionary<Bedrijf, List<string>> actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
            Assert.Empty(actual);

            //Meerdere bedrijven check
            w.VoegBedrijfEnFunctieToeAanWerknemer(_b1, _nf);
            w.VoegBedrijfEnFunctieToeAanWerknemer(_b2, _nf);
            w.VerwijderBedrijfVanWerknemer(_b1);
            actual = w.GeefBedrijvenEnFunctiesPerWerknemer();
            Assert.Collection(actual,
                expected =>
                {
                    Assert.Equal(_b2, expected.Key);
                    Assert.Collection(expected.Value,
                        functie => Assert.Equal(_nf, functie));
                });
        }

        [Fact]
        public void VerwijderBedrijfVanWerknemer_Invalid()
        {
            Werknemer w = new(10, "werknemer", "werknemersen", "werknemer.werknemersen@email.com");
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(null, _nf));
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, null));
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, ""));
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, " "));
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, "\n"));
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, "\r"));
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, "\t"));
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, "\v"));
            ////CHECK duplicates
            //w.VoegBedrijfEnFunctieToeAanWerknemer(_b, _nf);
            //Assert.Throws<WerknemerException>(() => w.VoegBedrijfEnFunctieToeAanWerknemer(_b, _nf));
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
        public void ctor_Invalid_PersoonException(uint id, string voornaam, string achternaam, string email, string bedrijf)
        {
            Assert.Throws<PersoonException>(() => new Bezoeker(id, voornaam, achternaam, email, bedrijf));
        }

        [Theory]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", null)]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", " ")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\n")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\r")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\t")]
        [InlineData(10, "bezoeker", "bezoekersen", "bezoeker.bezoekersen@email.com", "\v")]

        public void ctor_Invalid_BezoekerException(uint id, string voornaam, string achternaam, string email, string bedrijf)
        {
            Assert.Throws<BezoekerException>(() => new Bezoeker(id, voornaam, achternaam, email, bedrijf));
        }
        #endregion
    }
}