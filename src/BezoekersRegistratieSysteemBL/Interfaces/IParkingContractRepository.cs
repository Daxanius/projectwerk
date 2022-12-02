using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	public interface IParkingContractRepository {
		bool BestaatParkingContract(ParkingContract parkingContract);
		void BewerkParkingContract(ParkingContract parkingContract);
		ParkingContract? GeefParkingContract(long bedrijfId);
		bool IsOverLappend(ParkingContract parkingContract);
		void VerwijderParkingContract(ParkingContract parkingContract);
		void VoegParkingContractToe(ParkingContract parkingContract);
	}
}
