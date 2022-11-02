using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen {
	public class UnitTestWerknemerInfo {
		// AF

		#region Valid Info
		private Bedrijf _b;
		private string _e;
		#endregion

		#region Initialiseren
		public UnitTestWerknemerInfo()
		{
			_b = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			_e = "werknemer.werknemersen@email.com";
		}
        #endregion

        #region UnitTest Bedrijf
        [Fact]
		public void ZetBedrijf_Valid() {
			WerknemerInfo wi = new(_b, "werknemer.werknemersen@email.com");
			wi.ZetBedrijf(_b);
			Assert.Equal(_b, wi.Bedrijf);
		}

		[Fact]
		public void ZetBedrijf_Invalid() {
			WerknemerInfo wi = new(_b, "werknemer.werknemersen@email.com");
			Assert.Throws<WerknemerInfoException>(() => wi.ZetBedrijf(null));
		}
		#endregion

		#region UnitTest Email
		[Fact]
		public void ZetEmail_Valid() {
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
		public void ZetEmail_Invalid(string email) {
			WerknemerInfo wi = new(_b, "werknemer.werknemersen@email.com");
			Assert.Throws<WerknemerInfoException>(() => wi.ZetEmail(email));
		}
		#endregion

		#region UnitTest Geef Functies
		[Fact]
		public void GeefWerknemerFuncties_Valid() {
			Werknemer w = new(10, "werknemer", "werknemersen");
			w.VoegBedrijfEnFunctieToeAanWerknemer(_b, _e, "functie1");

			Assert.Collection(w.GeefBedrijvenEnFunctiesPerWerknemer(),
				item => Assert.Equal("functie1", item.Value.GeefWerknemerFuncties()[0]));

			w.VoegBedrijfEnFunctieToeAanWerknemer(_b, _e, "functie2");
			Assert.Collection(w.GeefBedrijvenEnFunctiesPerWerknemer(), item => {
				Assert.Equal("functie1", item.Value.GeefWerknemerFuncties()[0]);
				Assert.Equal("functie2", item.Value.GeefWerknemerFuncties()[1]);
			});
		}
		#endregion

		#region UnitTest Voeg Functies Toe
		[Fact]
		public void VoegWerknemerFunctieToe_Valid() {
			WerknemerInfo wi = new(_b, _e);
			wi.VoegWerknemerFunctieToe("functie1");
			Assert.Collection(wi.GeefWerknemerFuncties(), item => Assert.Equal("functie1", item));

			//CHECK: Meerdere functies
			wi.VoegWerknemerFunctieToe("functie2");
			Assert.Equal("functie1", wi.GeefWerknemerFuncties()[0]);
			Assert.Equal("functie2", wi.GeefWerknemerFuncties()[1]);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void VoegWerknemerFunctieToe_Invalid(string functie) {
			WerknemerInfo wi = new(_b, _e);
			Assert.Throws<WerknemerInfoException>(() => wi.VoegWerknemerFunctieToe(functie));
		}
		#endregion

		#region UnitTest Wijzig Functies
		[Fact]
		public void WijzigWerknemerFunctie_Valid() {
			WerknemerInfo wi = new(_b, _e);
			wi.VoegWerknemerFunctieToe("functie1");
			wi.WijzigWerknemerFunctie("functie1", "functie2");
			Assert.Collection(wi.GeefWerknemerFuncties(), item => Assert.Equal("functie2", item));
		}

		[Theory]
		[InlineData(null, "functie2")]
		[InlineData("", "functie2")]
		[InlineData(" ", "functie2")]
		[InlineData("\n", "functie2")]
		[InlineData("\r", "functie2")]
		[InlineData("\t", "functie2")]
		[InlineData("\v", "functie2")]
		[InlineData("functie2", "functie3")]
		[InlineData("functie5", "functie1")]
		[InlineData("functie1", null)]
		[InlineData("functie1", "")]
		[InlineData("functie1", " ")]
		[InlineData("functie1", "\n")]
		[InlineData("functie1", "\r")]
		[InlineData("functie1", "\t")]
		[InlineData("functie1", "\v")]
		public void WijzigWerknemerFunctie_Invalid(string oudefunctie, string nieuwefunctie) {
			WerknemerInfo wi = new(_b, _e);
			wi.VoegWerknemerFunctieToe("functie1");
			wi.VoegWerknemerFunctieToe("functie5");
			Assert.Throws<WerknemerInfoException>(() => wi.WijzigWerknemerFunctie(oudefunctie, nieuwefunctie));
		}
		#endregion

		#region UnitTest Verwijder Functies
		[Fact]
		public void VerwijderWerknemerFunctie_Valid() {
			WerknemerInfo wi = new(_b, _e);
			wi.VoegWerknemerFunctieToe("functie1");
			wi.VoegWerknemerFunctieToe("functie2");
			wi.VerwijderWerknemerFunctie("functie1");
			Assert.Collection(wi.GeefWerknemerFuncties(), item => Assert.Equal("functie2", item));
		}

		[Theory]
		[InlineData("functie2")]
		[InlineData(null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("\n")]
		[InlineData("\r")]
		[InlineData("\t")]
		[InlineData("\v")]
		public void VerwijderWerknemerFunctie_Invalid(string functie) {
			WerknemerInfo wi = new(_b, _e);
			wi.VoegWerknemerFunctieToe("functie1");
			wi.VoegWerknemerFunctieToe("functie5");
			Assert.Throws<WerknemerInfoException>(() => wi.VerwijderWerknemerFunctie(functie));

			wi.VerwijderWerknemerFunctie("functie5");
			Assert.Throws<WerknemerInfoException>(() => wi.VerwijderWerknemerFunctie("functie1"));
		}
		#endregion

		#region UnitTest WerknemerInfo is gelijk
		[Fact]
		public void WerknemerInfoIsGelijk_Valid() {
			WerknemerInfo wi = new(_b, _e);
			Assert.True(wi.WerknemerInfoIsGelijk(wi));
		}

		[Fact]
		public void WerknemerInfoIsGelijk_Invalid() {
			Bedrijf bb = _b;
			bb.ZetId(1);

			WerknemerInfo wi1 = new(_b, _e);
			WerknemerInfo wi2 = new(bb, "bedrijf@email.com");
			Assert.False(wi1.WerknemerInfoIsGelijk(wi2));
			wi2 = new(_b, "anderbedrijf@email.com");
			Assert.False(wi1.WerknemerInfoIsGelijk(wi2));
		}
		#endregion

		#region UnitTest WerknemerInfo ctor
		[Fact]
		public void ctor_Valid() {
			WerknemerInfo wi = new(_b, _e);
			Assert.Equal(_b, wi.Bedrijf);
			Assert.Equal(_e, wi.Email);
		}
		[Fact]
		public void ctor_Invalid() {
			Assert.Throws<WerknemerInfoException>(() => new WerknemerInfo(null, _e));
			Assert.Throws<WerknemerInfoException>(() => new WerknemerInfo(_b, null));
		}
		#endregion
	}
}
