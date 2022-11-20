using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	public interface IParkingContractRepository {
		bool BestaatParkingContract(ParkingContract parkingContract);
		void BewerkParkingContract(ParkingContract parkingContract);
		ParkingContract GeefParkingContract(long id);
		void VerwijderParkingContract(ParkingContract parkingContract);
		void VoegParkingContractToe(ParkingContract parkingContract);
	}
}
