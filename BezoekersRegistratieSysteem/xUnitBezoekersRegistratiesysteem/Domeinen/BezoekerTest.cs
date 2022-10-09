using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domein
{
	public class BezoekerTest
	{
		private readonly Bezoeker validBezoeker;
		public BezoekerTest()
		{
			validBezoeker = new(voornaam: "stan", achternaam: "persoons", email: "stan@gmail.com", bedrijf: "Artevelde");
		}

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
	}
}