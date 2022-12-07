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
        GrafiekDagDetail GeefUuroverzichtParkingVoorBedrijf(long id);
        GrafiekDag GeefWeekoverzichtParkingVoorBedrijf(long id);
    }
}