using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using Moq;

namespace xUnitBezoekersRegistratieSysteem.Managers {
	public class UnitTestParkingContractManager {
		#region MOQ
		private readonly ParkingContractManager _parkingContractManager;
		private readonly Mock<IParkingContractRepository> _mockRepo;
		#endregion

		#region Valid Info
		private readonly Werknemer _w;
		private readonly Bedrijf _b1;
		private readonly ParkingContract _c;
		private readonly int _p;
		private readonly DateTime _st;
		private readonly DateTime _et;
		#endregion

		#region Initialiseren
		public UnitTestParkingContractManager() {
			_mockRepo = new();

			_w = new(10, "werknemer", "werknemersen");
			_b1 = new(10, "bedrijf", "BE0676747521", true, "012345678", "bedrijf@email.com", "bedrijfstraat 10");

			_p = 10;

			_st = DateTime.Now;
			_et = _st.AddHours(2);

			_b1.VoegWerknemerToeInBedrijf(_w, "werknemer.werknemerson@email.com", "werken");

			_c = new(_b1, _st, _et, _p);

			_parkingContractManager = new(_mockRepo.Object);
		}
		#endregion

		#region UnitTest VoegParkingContractToe
		[Fact]
		public void VoegParkingContractToe_Leeg() {
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.VoegParkingContractToe(null);
			});
		}

		[Fact]
		public void VoegParkingContractToe_Bestaat() {
			_mockRepo.Setup(x => x.BestaatParkingContract(_c)).Returns(true);
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.VoegParkingContractToe(_c);
			});
		}

		#endregion

		#region UnitTest VerwijderParkingContract
		[Fact]
		public void VerwijderParkingContract_Leeg() {
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.VerwijderParkingContract(null);
			});
		}

		[Fact]
		public void VerwijderParkingContract_BestaatNiet() {
			_mockRepo.Setup(x => x.BestaatParkingContract(_c)).Returns(false);
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.VerwijderParkingContract(_c);
			});
		}
		#endregion

		#region UnitTest BewerkParkingContract
		[Fact]
		public void BewerkParkingContract_Leeg() {
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.BewerkParkingContract(null);
			});
		}

		[Fact]
		public void BewerkParkingContract_BestaatNiet() {
			_mockRepo.Setup(x => x.BestaatParkingContract(_c)).Returns(false);
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.BewerkParkingContract(_c);
			});
		}

		#endregion

		#region GeefParkingContract
		[Fact]
		public void GeefParkingContract_Leeg() {
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.GeefParkingContract(null);
			});
		}
		#endregion
	}
}
