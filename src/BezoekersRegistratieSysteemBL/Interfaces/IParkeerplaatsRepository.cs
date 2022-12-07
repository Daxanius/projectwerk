using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	public interface IParkeerplaatsRepository {
		bool BestaatNummerplaat(string nummerplaat);
		//bool BestaatParkeerplaats(Parkeerplaats parkeerplaats);
		//void BewerkParkeerplaats(Parkeerplaats parkeerplaats);
		void CheckNummerplaatIn(Parkeerplaats parkeerplaats);
		void CheckNummerplaatUit(string nummerplaat);
		int GeefHuidigBezetteParkeerplaatsenVoorBedrijf(long id);
		IReadOnlyList<Parkeerplaats> GeefNummerplatenPerBedrijf(Bedrijf bedrijf);
        GrafiekDag GeefUuroverzichtParkingVoorBedrijf(long id);
        GrafiekWeek GeefWeekoverzichtParkingVoorBedrijf(long id);
    }
}