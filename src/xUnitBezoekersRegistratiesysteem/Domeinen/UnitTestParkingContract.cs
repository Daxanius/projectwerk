using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace xUnitBezoekersRegistratiesysteem.Domeinen {

	public class UnitTestParkingContract {
		#region Valid Info
		private Werknemer _w;
		private Bedrijf _bd;
		private DateTime _st;
		private DateTime _et;
		private int _p;
		#endregion

		#region Invalid Info
		private Werknemer _ivw;
		private Bedrijf _ivbd;
		private int _ip;
		#endregion

		#region Initialiseren
		public UnitTestParkingContract() {
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

			_p = 10;
			_ip = 0;
		}
		#endregion

		#region UnitTest Plaatsen
		[Fact]
		public void Plaatsen_Invalid() {
			ParkingContract pc = new(_bd, _st, _et, _p);
			Assert.Throws<ParkingContractException>(() => {
				pc.ZetAantalPlaatsen(_ip);
			});
		}

		[Fact]
		public void Plaatsen_Valid() {
			ParkingContract pc = new(_bd, _st, _et, _p);
			pc.ZetAantalPlaatsen(_p);
			Assert.Equal(_p, pc.AantalPlaatsen);
		}
		#endregion

		#region UnitTest Eindtijd
		[Fact]
		public void Eindtijd_Invalid() {
			ParkingContract pc = new(_bd, _st, _et, _p);
			Assert.Throws<ParkingContractException>(() => {
				pc.ZetEindtijd(_st.AddDays(-2));
			});
		}

		[Fact]
		public void Eindtijd_Valid() {
			ParkingContract pc = new(_bd, _st, _et, _p);
			pc.ZetEindtijd(_et);
			Assert.Equal(_et, pc.Eindtijd);
		}
		#endregion
	}
}