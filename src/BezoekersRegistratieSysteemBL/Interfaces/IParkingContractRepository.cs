﻿using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	public interface IParkingContractRepository {
		bool BestaatParkingContract(ParkingContract parkingContract);
		bool BestaatParkingContractOpBedrijfId(ParkingContract parkingContract);
		void BewerkParkingContract(ParkingContract parkingContract);
		int GeefAantalParkeerplaatsenVoorBedrijf(long id);
		ParkingContract? GeefParkingContract(long bedrijfId);
		bool IsOverLappend(ParkingContract parkingContract);
		void VerwijderParkingContract(ParkingContract parkingContract);
		void VoegParkingContractToe(ParkingContract parkingContract);
	}
}
