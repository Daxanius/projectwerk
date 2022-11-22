using BezoekersRegistratieSysteemBL.Domeinen;

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
			_n = "028AB";
			_in = "AZERTYUIOPQSDFGHJKLMWXCVBN";
		}
		#endregion
	}
}