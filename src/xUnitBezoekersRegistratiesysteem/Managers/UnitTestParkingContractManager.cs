﻿using BezoekersRegistratieSysteemBL.Domeinen;
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
		private readonly Bedrijf _b2;
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
			_b2 = new(20, "IKEA", "BE0676747521", true, "012345678", "ikea@sweden.furniture", "Stockholm 32");

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

		[Fact]
		public void VoegParkingContractToe_Overlappend() {
			_mockRepo.Setup(x => x.BestaatParkingContract(_c)).Returns(true);
			_mockRepo.Setup(x => x.GeefParkingContract(0)).Returns(_c);
			ParkingContract overlappend = new(_b1, _st, _et, 12);
			Assert.Throws<ParkingContractManagerException>(() => {
				_parkingContractManager.VoegParkingContractToe(overlappend);
			});
		}

		#endregion
	}
}
