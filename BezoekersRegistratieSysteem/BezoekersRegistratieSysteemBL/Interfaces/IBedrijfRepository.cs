using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Bedrijf storage hook
	/// </summary>
	public interface IBedrijfRepository {
		Bedrijf VoegBedrijfToe(Bedrijf bedrijf);
		void VerwijderBedrijf(uint id);
		void BewerkBedrijf(Bedrijf bedrijf);
        
		bool BestaatBedrijf(Bedrijf bedrijf);
		bool BestaatBedrijf(uint bedrijf);
        bool BestaatBedrijf(string bedrijfsnaam);

        Bedrijf GeefBedrijf(uint id);
		IReadOnlyList<Bedrijf> Geefbedrijven();
		Bedrijf GeefBedrijf(string bedrijfsnaam);
    }
}