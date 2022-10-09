using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Bedrijf storage hook
	/// </summary>
	public interface IBedrijfRepository {
		void VerwijderBedrijf(uint id);
		void VoegBedrijfToe(Bedrijf bedrijf);
		void WijzigBedrijf(uint id, Bedrijf bedrijf);
	}
}