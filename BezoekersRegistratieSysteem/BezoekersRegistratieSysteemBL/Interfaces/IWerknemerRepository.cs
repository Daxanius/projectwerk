using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Werknemer storage hook
	/// </summary>
	public interface IWerknemerRepository {
		void VoegWerknemerToe(Werknemer werknemer);
		void VerwijderWerknemer(uint id);
		void WijzigWerknemer(uint id, Werknemer werknemer);
        Werknemer GeefWerknemer(uint id);
        IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string naam);
		IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id);
	}
}