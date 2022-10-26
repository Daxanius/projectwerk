using BezoekersRegistratieSysteemBL;
using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace xUnitBezoekersRegistratiesysteem
{
    public class UnitTestNutvoorziening
    {
		#region ControleerBTWNummer
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
		public void ControleerBTWNummer_Invalid(string btw) {
			Assert.False(Nutsvoorziening.ControleerBTWNummer(btw));
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
	}
}
