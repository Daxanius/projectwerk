using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
	/// <summary>
	/// Bedrijf storage hook
	/// </summary>
	public interface IBedrijfRepository
	{
		Bedrijf VoegBedrijfToe(Bedrijf bedrijf);
		void VerwijderBedrijf(long id);
		void BewerkBedrijf(Bedrijf bedrijf);

		bool BestaatBedrijf(Bedrijf bedrijf);
		bool BestaatBedrijf(long bedrijf);
		bool BestaatBedrijf(string bedrijfsnaam);

		Bedrijf GeefBedrijf(long id);
		IReadOnlyList<Bedrijf> GeefBedrijven();
		Bedrijf GeefBedrijf(string bedrijfsnaam);
	}
}