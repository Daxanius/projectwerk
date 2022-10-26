using BezoekersRegistratieSysteemBL;
using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.DTO;

namespace xUnitBezoekersRegistratiesysteem
{
    public class UnitTestNutvoorziening
    {
		#region VerwijderWhitespace

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\t\t\t\t")]
		[InlineData("\v")]
		[InlineData("\v\t\r\n")]
		public void VerwijderWhitespace_Leeg(string whitespace) {
			Assert.Equal(string.Empty, Nutsvoorziening.VerwijderWhitespace(whitespace));
		}

		[Theory]
		[InlineData("ABC")]
		[InlineData("   ABC")]
		[InlineData("ABC   ")]
		[InlineData("   ABC   ")]
		[InlineData("\tABC")]
		[InlineData("A   BC")]
		[InlineData("AB    C   ")]
		public void VerwijderWhitespace_Tekst(string whitespace) {
			Assert.Equal("ABC", Nutsvoorziening.VerwijderWhitespace(whitespace));
		}

		#endregion

		#region ControleerBTWNummer
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		[InlineData("B E")]
		[InlineData("B  E")]
		[InlineData("BE06767475219282727191928292920292")]
		public void ControleerBTWNummer_Invalid(string btw) {
			Assert.False(Nutsvoorziening.ControleerBTWNummer(btw));
		}

		[Theory]
		[InlineData("BE0676747521")]
		[InlineData("BE0676747521    ")]
		[InlineData("   BE0676747521")]
		[InlineData("\tBE0676747521")]
		[InlineData("BE0676747521\t")]
		[InlineData("\tBE0676747521\t")]
		[InlineData("   BE0676747521    ")]
		[InlineData("BE    0676747521")]
		public void ControleerBTWNummer_Valid(string btw) {
			(bool valid, BtwInfoDTO? info) = Nutsvoorziening.GeefBTWInfo(btw);
			Assert.True(Nutsvoorziening.ControleerBTWNummer(btw));
		}
		#endregion

		#region BTWInfo
		[Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData("\t")]
        [InlineData("\v")]
		[InlineData("B E")]
		[InlineData("B  E")]
		[InlineData("BE06767475219282727191928292920292")]
		public void GeefBTWInfo_Invalid(string btw)
        {
            Assert.Throws<BtwControleException>(() => Nutsvoorziening.GeefBTWInfo(btw));
        }

		[Theory]
        [InlineData("BE0676747521")]
        [InlineData("BE0676747521    ")]
        [InlineData("   BE0676747521")]
        [InlineData("\tBE0676747521")]
        [InlineData("BE0676747521\t")]
        [InlineData("\tBE0676747521\t")]
        [InlineData("   BE0676747521    ")]
		[InlineData("BE    0676747521")]
		public void GeefBTWInfo_Valid(string btw)
        {
            (bool valid, BtwInfoDTO? info) = Nutsvoorziening.GeefBTWInfo(btw);

            Assert.True(valid);

            // ALs dit faalt, dan gaan we ervan uit dat de BTW service plat ligt
            Assert.NotNull(info);
        }

		[Fact]
		public void GeefBTWInfo_NotExist() {
			// Tijdens het opmaken van deze test, bestaat dit bedrijf nog niet
			(bool valid, BtwInfoDTO? info) = Nutsvoorziening.GeefBTWInfo("BE1234567891");

			Assert.False(valid);
			Assert.Null(info);
		}
		#endregion

		#region IsEmailGeldig
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		[InlineData("B E")]
		[InlineData("mens......")]
		[InlineData("kind@")]
		[InlineData(".com")]
		public void IsEmailGeldig_Invalid(string email) {
			Assert.False(Nutsvoorziening.IsEmailGeldig(email));
		}

		[Theory]
		[InlineData("mens.guysen@student.hogent.be")]
		[InlineData("joost.boost@gmail.com    ")]
		[InlineData("joss.veeee@hotmail.be")]
		public void IsEmailGeldig_Valid(string email) {
			Assert.True(Nutsvoorziening.IsEmailGeldig(email));
		}
		#endregion
	}
}
