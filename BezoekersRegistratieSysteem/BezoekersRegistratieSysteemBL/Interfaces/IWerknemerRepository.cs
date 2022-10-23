﻿using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Werknemer storage hook
	/// </summary>
	public interface IWerknemerRepository {
		Werknemer VoegWerknemerToe(Werknemer werknemer);
		void VerwijderWerknemer(Werknemer werknemer);
        void VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie);
        void WijzigWerknemer(Werknemer werknemer);

		bool BestaatWerknemer(Werknemer werknemer);
        bool BestaatWerknemer(uint id);

        Werknemer GeefWerknemer(uint id);
        IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string voornaam, string achternaam);
		IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id);
	}
}