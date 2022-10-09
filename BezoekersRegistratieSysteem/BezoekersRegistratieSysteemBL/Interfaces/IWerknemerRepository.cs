using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Werknemer storage hook
	/// </summary>
	public interface IWerknemerRepository {
		void VoegWerknemerToe(Werknemer werknemer);
		void VerwijderWerknemer(uint id);
		void WijzigWerknemer(uint id, Werknemer werknemer);
		Werknemer GeefWerknemerOpNaam(string naam);
		//IReadOnlyList<Werknemer> GeefAanwezigeWerknemers();
		IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id);
		IReadOnlyList<Werknemer> GeefWerknemersPerFunctie(string functie);
	}
}