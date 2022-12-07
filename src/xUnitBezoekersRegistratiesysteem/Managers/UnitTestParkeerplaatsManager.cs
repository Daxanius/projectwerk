using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.Managers {
	public class UnitTestParkeerplaatsManager {
		#region MOQ
		private readonly ParkeerplaatsManager _parkeerPlaatsManager;
		private readonly Mock<IParkeerplaatsRepository> _mockRepoParkeerplaats;
		private readonly Mock<IParkingContractRepository> _mockRepoParkingContract;
		#endregion

		#region Valid Info
		private readonly Werknemer _w;
		private readonly Bedrijf _b1;
		private readonly Bedrijf _b2;
		private readonly string _n;
		private readonly int _p;
		private readonly Parkeerplaats _pp;
		private readonly DateTime _st;
		private readonly DateTime _et;
		#endregion

		#region Initialiseren
		public UnitTestParkeerplaatsManager() {
			_mockRepoParkeerplaats = new();
			_mockRepoParkingContract = new();

			_w = new(10, "werknemer", "werknemersen");
			_b1 = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");
			_b2 = new(20, "IKEA", "BE0676747521", true, "012345678", "ikea@sweden.furniture", "Stockholm 32");

			_p = 10;

			_st = DateTime.Now;
			_et = _st.AddHours(2);

			_b1.VoegWerknemerToeInBedrijf(_w, "werknemer.werknemerson@email.com", "werken");

			_n = "0-KKK-000";

			_pp = new(_b1, _st, _n);

			_parkeerPlaatsManager = new(_mockRepoParkeerplaats.Object, _mockRepoParkingContract.Object);
		}
        #endregion

        #region UnitTest CheckNummerplaatIn
        [Fact]
		public void CheckNummerplaatIn_Leeg() {
			Assert.Throws<ParkeerplaatsManagerException>(() => {
				_parkeerPlaatsManager.CheckNummerplaatIn(null);
			});
		}

		[Fact]
		public void CheckNummerplaatIn_Bestaat() {
			_mockRepoParkeerplaats.Setup(x => x.BestaatNummerplaat(_pp.Nummerplaat)).Returns(true);
			Assert.Throws<ParkeerplaatsManagerException>(() => {
				_parkeerPlaatsManager.CheckNummerplaatIn(_pp);
			});
		}

        #endregion

        #region UnitTest CheckNummerplaatUit
        [Fact]
		public void CheckNummerplaatUit_Leeg() {
			Assert.Throws<ParkeerplaatsManagerException>(() => {
				_parkeerPlaatsManager.CheckNummerplaatUit(null);
			});
		}

		[Fact]
		public void CheckNummerplaatUit_BestaatNiet() {
			_mockRepoParkeerplaats.Setup(x => x.BestaatNummerplaat(_pp.Nummerplaat)).Returns(false);
			Assert.Throws<ParkeerplaatsManagerException>(() => {
				_parkeerPlaatsManager.CheckNummerplaatUit(_n);
			});
		}
		#endregion

		#region GeefParkingContract
		[Fact]
		public void GeefParkingContract_Leeg() {
			Assert.Throws<ParkeerplaatsManagerException>(() => {
				_parkeerPlaatsManager.GeefNummerplatenPerBedrijf(null);
			});
		}

		[Fact]
		public void GeefParkingContract_BestaatNiet() {
			Assert.Throws<ParkeerplaatsManagerException>(() => {
				_parkeerPlaatsManager.GeefNummerplatenPerBedrijf(_b2);
			});
		}

		#endregion
	}
}