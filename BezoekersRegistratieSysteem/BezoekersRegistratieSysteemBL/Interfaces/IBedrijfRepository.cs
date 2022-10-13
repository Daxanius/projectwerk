using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Bedrijf storage hook
	/// </summary>
	public interface IBedrijfRepository {
		void VoegBedrijfToe(Bedrijf bedrijf);
		void VerwijderBedrijf(uint id);
		void BewerkBedrijf(uint id, Bedrijf bedrijf);
        Bedrijf GetBedrijf(uint id);
		IReadOnlyList<Bedrijf> Geefbedrijven();
		Bedrijf GeefBedrijf(string bedrijfsnaam);
    }
}