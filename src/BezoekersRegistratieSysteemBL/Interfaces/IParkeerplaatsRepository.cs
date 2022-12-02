using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	public interface IParkeerplaatsRepository {
		bool BestaatNummerplaat(string nummerplaat);
		//bool BestaatParkeerplaats(Parkeerplaats parkeerplaats);
		//void BewerkParkeerplaats(Parkeerplaats parkeerplaats);
		void CheckNummerplaatIn(Parkeerplaats parkeerplaats);
		void CheckNummerplaatUit(string nummerplaat);
		int GeefHuidigBezetteParkeerplaatsenPerBedrijf(long id);
		IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf);
	}
}