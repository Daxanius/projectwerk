﻿using BezoekersRegistratieSysteemBL;
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

namespace xUnitBezoekersRegistratiesysteem
{
    public class UnitTestNutvoorziening
    {
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
		public void ControleerBTWNummer_Invalid(string btw)
        {
            Assert.Throws<BtwControleException>(() => Nutsvoorziening.ControleerBTWNummer(btw));
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
		public void ControleerBTWNummer_Valid(string btw)
        {
            Nutsvoorziening.ControleerBTWNummer(btw);
        }
    }
}