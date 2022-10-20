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
	public class NutvoorzieningTest
	{
		public NutvoorzieningTest()
		{
			
		}

		#region InValid
		[Theory]
		[InlineData("BE0475730464")]
		[InlineData("BE0475730241")]
		[InlineData("B0475730	241")]
		[InlineData("0475730241")]
		[InlineData("BE04757302 41")]
		[InlineData("BE047_5730241")]
		public async Task ControlleerBtwNummer_FoutNummer_Exception(string btwNummer)
		{
			Assert.False((await Nutsvoorziening.ControleerBTWNummer(btwNummer)).Item1);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public async Task ControlleerBtwNummer_NullOrWhiteSpace_Exception(string btwNummer)
		{
			await Assert.ThrowsAsync<BtwControleException>(() => Nutsvoorziening.ControleerBTWNummer(btwNummer));
		}

		[Theory]
		[InlineData("BE")]
		[InlineData("BE0475730241879124534")]
		public async Task ControlleerBtwNummer_Foute_Lente_Exception(string btwNummer)
		{
			await Assert.ThrowsAsync<BtwControleException>(() => Nutsvoorziening.ControleerBTWNummer(btwNummer));
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void IsEmailGeldig_NullAndWhiteSpace_Exception(string emailAdres)
		{
			Assert.Throws<EmailControleException>(() => Nutsvoorziening.IsEmailGeldig(emailAdres));
		}

		[Theory]
		[InlineData("a")]
		[InlineData("a@")]
		[InlineData("a@aa")]
		[InlineData("a@a.")]
		[InlineData("a@a.co-")]
		[InlineData("dffqsdqsdsd.qsddqdsd.dqssdd.com")]
		public void IsEmailGeldig_FoutEmailAdres_Exception(string emailAdres)
		{
			Assert.False(Nutsvoorziening.IsEmailGeldig(emailAdres));
		}
		#endregion

		#region Valid
		[Theory]
		[InlineData("BE0475730461")]
		[InlineData("BE0894943477")]
		public async Task ControlleerBtwNummer_CorrectNummer(string btwNummer)
		{
			Assert.True((await Nutsvoorziening.ControleerBTWNummer(btwNummer)).Item1);
		}

		[Theory]
		[InlineData("a@gmail.com")]
		[InlineData("a.a@hotmail.com")]
		[InlineData("a.a.a.a@aa.com")]
		[InlineData("a@a.be")]
		public void IsEmailGeldig_CorrectEmailAdres(string emailAdres)
		{
			Assert.True(Nutsvoorziening.IsEmailGeldig(emailAdres));
		}
		#endregion
	}
}
