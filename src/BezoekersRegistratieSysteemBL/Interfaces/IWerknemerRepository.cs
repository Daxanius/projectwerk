using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {

	/// <summary>
	/// Werknemer storage hook
	/// </summary>
	public interface IWerknemerRepository {

		Werknemer VoegWerknemerToe(Werknemer werknemer);

		void VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf);

		//void VoegWerknemerFunctieToe(Werknemer werknemer, Bedrijf bedrijf, string functie);
		void VoegWerknemerFunctieToe(Werknemer werknemer, WerknemerInfo value, string functie);
		void VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie);
		void BewerkWerknemer(Werknemer werknemer, Bedrijf bedrijf);

		bool BestaatWerknemer(Werknemer werknemer);
		bool BestaatWerknemer(long id);

		Werknemer GeefWerknemer(long id);
		IReadOnlyList<Werknemer> GeefWerknemersOpNaamPerBedrijf(string voornaam, string achternaam, long bedrijfId);
		IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(long id);
		bool BestaatFunctie(string functie);
		void VoegFunctieToe(string functie);
		IReadOnlyList<Werknemer> GeefWerknemersOpFunctiePerBedrijf(string functie, long bedrijfId);
		IReadOnlyList<Werknemer> GeefVrijeWerknemersOpDitMomentVoorBedrijf(long id);
		IReadOnlyList<Werknemer> GeefBezetteWerknemersOpDitMomentVoorBedrijf(long id);
		void GeefWerknemerId(Werknemer werknemer);
        Werknemer? WerknemerPotentieelReedsWerkzaamInBedrijvenpark(Werknemer werknemer);
    }
}