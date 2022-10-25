using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
    public class UnitTestBedrijf
    {
        #region Valid Info
        private Werknemer _w = new(10, "werknemer1", "werknemersen1", "werknemer.werknemersen@email.com");
        private string _f1 = "functie1";
        private string _f2 = "functie2";
        #endregion

        #region UnitTest Naam
        [Theory]
        [InlineData("bedrijf", "bedrijf")]
        [InlineData("     bedrijf", "bedrijf")]
        [InlineData("bedrijf     ", "bedrijf")]
        public void ZetNaam_Valid(string naamIn, string naamUit)
        {
            Bedrijf b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
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
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Assert.Throws<BedrijfException>(() => _b.ZetNaam(naam));
        }
        #endregion

        #region UnitTest BTW
        [Theory]
        [InlineData("BE0676747521", "BE0676747521")]
        [InlineData("     BE0676747521", "BE0676747521")]
        [InlineData("BE0676747521     ", "BE0676747521")]
        public void ZetBTW_Valid(string btwIn, string btwUit)
        {
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            _b.ZetBTW(btwIn);
            Assert.Equal(btwUit, _b.BTW);
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
            Bedrijf _b = new(10, "bedrijf", "BE123456789", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Assert.Throws<BedrijfException>(() => _b.ZetBTW(btw));
        }
        #endregion

        #region UnitTest BTWControle
        [Theory]
        [InlineData("BE0676747521", "BE0676747521")]
        [InlineData("     BE0676747521", "BE0676747521")]
        [InlineData("BE0676747521     ", "BE0676747521")]
        public async void ZetBTWControle_Valid(string btwIn, string btwUit)
        {
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            await _b.ZetBTWControle(btwIn);
            Assert.Equal(btwUit, _b.BTW);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
        [InlineData("BE123456789")] //nep BTWnummer
        public void ZetBTWControle_Invalid(string btw)
        {
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Assert.ThrowsAsync<BedrijfException>(() => _b.ZetBTWControle(btw));
        }
        #endregion

        #region UnitTest Telefoonnummer
        [Theory]
        [InlineData("012345678", "012345678")]
        [InlineData("     012345678", "012345678")]
        [InlineData("012345678     ", "012345678")]
        public void ZetTelefoonnummer_Valid(string telefoonnummerIn, string telefoonnummerUit)
        {
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            _b.ZetTelefoonNummer(telefoonnummerIn);
            Assert.Equal(telefoonnummerUit, _b.TelefoonNummer);
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
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Assert.Throws<BedrijfException>(() => _b.ZetTelefoonNummer(telefoonnummer));
        }
        #endregion

        #region UnitTest Email
        [Theory]
        [InlineData("bedrijf@email.com", "bedrijf@email.com")]
        [InlineData("     bedrijf@email.com", "bedrijf@email.com")]
        [InlineData("bedrijf@email.com     ", "bedrijf@email.com")]
        public void ZetEmail_Valid(string emailIn, string emailUit)
        {
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            _b.ZetEmail(emailIn);
            Assert.Equal(emailUit, _b.Email);
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
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Assert.Throws<BedrijfException>(() => _b.ZetEmail(email));
        }
        #endregion

        #region UnitTest Adres
        [Theory]
        [InlineData("bedrijfstraat 10", "bedrijfstraat 10")]
        [InlineData("     bedrijfstraat 10", "bedrijfstraat 10")]
        [InlineData("bedrijfstraat 10     ", "bedrijfstraat 10")]
        public void ZetAdres_Valid(string adresIn, string adresUit)
        {
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            _b.ZetAdres(adresIn);
            Assert.Equal(adresUit, _b.Adres);
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
            Bedrijf _b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Assert.Throws<BedrijfException>(() => _b.ZetAdres(adres));

        }
        #endregion

        #region UnitTest Voeg Werknemer Toe
        [Fact]
        public void VoegWerknemerToeInBedrijf_Valid()
        {
            Bedrijf b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            b.VoegWerknemerToeInBedrijf(_w, _f1);
            Assert.Equal(1, b.GeefWerknemers().Count);
            //Check: Meerdere functies per werknemer bij 1 bedrijf
            b.VoegWerknemerToeInBedrijf(_w, _f2);
            Assert.Equal(1, b.GeefWerknemers().Count);
        }

        [Fact]
        public void VoegWerknemerToeInBedrijf_Invalid()
        {
            Bedrijf b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");

            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(null, _f1));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(null, null));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, null));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, ""));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, " "));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, "\n"));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, "\r"));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, "\t"));
            Assert.Throws<BedrijfException>(() => b.VoegWerknemerToeInBedrijf(_w, "\v"));
            //"Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - werknemer is in dit bedrijf al werkzaam onder deze functie"
            b.VoegWerknemerToeInBedrijf(_w, _f1);
            Assert.Throws<WerknemerException>(() => b.VoegWerknemerToeInBedrijf(_w, _f1));
        }
        #endregion

        #region UnitTest Verwijder Werknemer
        [Fact]
        public void VerwijderWerknemerUitBedrijf_Valid()
        {
            Bedrijf b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            b.VoegWerknemerToeInBedrijf(_w, _f1);
            b.VerwijderWerknemerUitBedrijf(_w);
            Assert.Equal(0, b.GeefWerknemers().Count);
        }

        [Fact]
        public void VerwijderWerknemerUitBedrijf_Invalid()
        {
            Bedrijf b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");

            Assert.Throws<BedrijfException>(() => b.VerwijderWerknemerUitBedrijf(null));
            Assert.Throws<BedrijfException>(() => b.VerwijderWerknemerUitBedrijf(_w));
        }
        #endregion

        #region UnitTest Geef Werknemers
        [Fact]
        public void GeefWerknemers_Valid()
        {
            Bedrijf b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            b.VoegWerknemerToeInBedrijf(_w, _f1);
            Assert.Contains(_w, b.GeefWerknemers());
        }
        #endregion

        #region UnitTest Bedrijf is gelijk
        [Fact]
        public void BedrijfIsGelijk_Valid()
        {
            Bedrijf b = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Assert.True(b.BedrijfIsGelijk(b));
        }

        [Theory]
        [InlineData(0, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "fjirdeb", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "bedrijf", "BE0724540609", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "bedrijf", "BE0676747521", "876543210", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "bedrijf", "BE0676747521", "012345678", "anderbedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "anderebedrijfstraat 10")]
        public async void BedrijfIsGelijk_Invalid(uint id, string naam, string btwNummer, string telefoonNummer, string email, string adres)
        {
            Bedrijf b1 = new(10, "bedrijf", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10");
            Bedrijf b2 = new(id, naam, "BE0676747521", telefoonNummer, email, adres);
            await b2.ZetBTWControle(btwNummer);
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
        public async void ctor_Valid(uint idIn, string naamIn, string btwNummerIn, string telefoonNummerIn, string emailIn, string adresIn, uint idUit, string naamUit, string btwNummerUit, string telefoonNummerUit, string emailUit, string adresUit)
        {
            Bedrijf b = new(idIn, naamIn, "BE0676747521", telefoonNummerIn, emailIn, adresIn);
            await b.ZetBTWControle(btwNummerIn);
            Assert.Equal(idUit, b.Id);
            Assert.Equal(naamUit, b.Naam);
            Assert.Equal(btwNummerUit, b.BTW);
            Assert.Equal(emailUit, b.Email);
            Assert.Equal(telefoonNummerUit, b.TelefoonNummer);
            Assert.Equal(adresUit, b.Adres);
        }

        [Theory]
        [InlineData(10, null, "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, " ", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "\n", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "\r", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "\t", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        [InlineData(10, "\v", "BE0676747521", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

        //[InlineData(10, "bedrijf", null, "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        //[InlineData(10, "bedrijf", "", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        //[InlineData(10, "bedrijf", " ", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        //[InlineData(10, "bedrijf", "\n", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        //[InlineData(10, "bedrijf", "\r", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        //[InlineData(10, "bedrijf", "\t", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        //[InlineData(10, "bedrijf", "\v", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]
        //[InlineData(10, "bedrijf", "BE123456789", "012345678", "bedrijf@email.com", "bedrijfstraat 10")]

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
        public async void ctor_Invalid(uint id, string naam, string btwNummer, string telefoonNummer, string email, string adres)
        {
            //Bedrijf b = new(id, naam, "BE0676747521", telefoonNummer, email, adres);
            //await b.ZetBTWControle(btwNummer);
            Assert.Throws<BedrijfException>(() => new Bedrijf(id, naam, btwNummer, telefoonNummer, email, adres));
        }
        #endregion
    }
}
