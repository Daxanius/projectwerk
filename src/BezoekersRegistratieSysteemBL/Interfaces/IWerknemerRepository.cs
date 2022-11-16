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

        StatusObject GeefWerknemer(long id);
        IReadOnlyList<StatusObject> GeefWerknemersOpNaamPerBedrijf(string voornaam, string achternaam, long bedrijfId);
		IReadOnlyList<StatusObject> GeefWerknemersPerBedrijf(long id);
        bool BestaatFunctie(string functie);
        void VoegFunctieToe(string functie);
		IReadOnlyList<StatusObject> GeefWerknemersOpFunctiePerBedrijf(string functie, long bedrijfId);
        IReadOnlyList<StatusObject> GeefVrijeWerknemersOpDitMomentVoorBedrijf(long id);
        IReadOnlyList<StatusObject> GeefBezetteWerknemersOpDitMomentVoorBedrijf(long id);
		void GeefWerknemerId(Werknemer werknemer);

	}
}