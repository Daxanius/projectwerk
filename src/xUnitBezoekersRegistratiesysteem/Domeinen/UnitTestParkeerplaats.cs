using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratieSysteem.Domeinen {
	public class UnitTestParkeerplaats {
		#region Valid Info
		private string _n;
		private Werknemer _w;
		private Bedrijf _bd;
		private DateTime _st;
		private DateTime _et;
		#endregion

		#region Invalid Info
		private string _in;
		private Werknemer _ivw;
		private Bedrijf _ivbd;
		#endregion

		#region Initialiseren
		public UnitTestParkeerplaats() {
			_w = new(10, "werknemer", "werknemersen");
			_bd = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			_st = DateTime.Now;
			_et = _st.AddHours(2);

			_ivw = new(1, "anderewerknemer", "anderewerknemersen");
			_ivbd = new(1, "anderbedrijf", "BE0676747521", true, "876543210", "anderbedrijf@email.com", "anderbedrijfstraat 10");

			//For Valid Paths
			_bd.VoegWerknemerToeInBedrijf(_w, "werknemer.werknemersen@email.com", "functie");
			//

			//For Invalid Paths
			_ivbd.VoegWerknemerToeInBedrijf(_ivw, "anderewerknemer.werknemersen@email.com", "anderefunctie");
			//

			// Is dit een geldig nummerplaat?
			_n = "0-KKK-000";
			_in = "AZERTYUIOPQSDFGHJKLMWXCVBN";
		}
		#endregion

		#region UnitTest Nummerplaat
		[Theory]
		[InlineData("1-ABC-005")]
		[InlineData("6-YEE-040")]
		public void Nummerplaat_Valid(string nummerplaatIn) {
			Parkeerplaats pp = new(_bd, _st, nummerplaatIn);
			Assert.Equal(pp.Nummerplaat, nummerplaatIn);
		}

		[Theory]
		[InlineData("Dit is GEEN geldig nummerplaat...")]
		[InlineData(null)]
		[InlineData("⎅⟟⏁ ⟟⌇ ⟒⟒⋏ ⏚⎍⟟⏁⟒⋏⏃⏃⍀⎅⌇ ⋏⎍⋔⋔⟒⍀⌿⌰⏃⏃⏁")]
		public void Nummerplaat_Invalid(string nummerplaatIn) {
			Assert.Throws<ParkeerplaatsException>(() => {
				Parkeerplaats pp = new(_bd, _st, nummerplaatIn);
			});
		}
		#endregion

		#region UnitTest Eindtijd
		[Fact]
		public void Eindtijd_Invalid() {
			Parkeerplaats pp = new(_bd, _st, _n);
			Assert.Throws<ParkeerplaatsException>(() => {
				pp.ZetEindtijd(_st.AddDays(-2));
			});
		}

		[Fact]
		public void Eindtijd_Valid() {
			Parkeerplaats pp = new(_bd, _st, _n);
			pp.ZetEindtijd(_et);
			Assert.Equal(_et, pp.Eindtijd);
		}
		#endregion
	}
}