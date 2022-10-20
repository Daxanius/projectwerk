using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using System.Security;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class BezoekerTest
	{
		private readonly Bezoeker validBezoeker;
		public BezoekerTest()
		{
			validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");

			validBezoeker.ZetId(1);
		}

		#region InValid
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("	")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\f")]
		[InlineData("\v")]
		public void ZetBedrijf_Null_WhiteSpace_Exception(string bedrijf)
		{
			Assert.Throws<BezoekerException>(() => validBezoeker.ZetBedrijf(bedrijf));
		}
		#endregion

		#region Valid
		[Fact]
		public void ZetBedrijf()
		{
			string bedrijf = "Hogent";
			validBezoeker.ZetBedrijf(bedrijf);
			Assert.Equal(bedrijf, validBezoeker.Bedrijf);
		}
		#endregion
	}
}