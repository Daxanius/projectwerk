using System;

namespace BezoekersRegistratieSysteemUI.AanmeldenOfAfmeldenWindow.Aanmelden.DTO
{
	public class WerknemerMetFunctieDTO
	{
		/// <summary>
		/// Nieuw WerknemerMetFunctieDTO wordt gemaakt
		/// </summary>
		/// <param name="id">Id van Werknemer</param>
		/// <param name="voornaam">Voornaam van Werknemer</param>
		/// <param name="achternaam">Achternaam van Werknemer</param>
		/// <param name="functie">Functie van Werknemer</param>
		public WerknemerMetFunctieDTO(int id, string voornaam, string achternaam, string functie)
		{
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Functie = functie;
		}

		/// <summary>
		/// Nieuw WerknemerMetFunctieDTO wordt gemaakt
		/// </summary>
		/// <param name="voornaam">Voornaam van Werknemer</param>
		/// <param name="achternaam">Achternaam van Werknemer</param>
		/// <param name="functie">Functie van Werknemer</param>
		public WerknemerMetFunctieDTO(string voornaam, string achternaam, string functie)
		{
			Voornaam = voornaam;
			Achternaam = achternaam;
			Functie = functie;
		}

		/// <summary>
		/// Id van werknemer
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// Voornaam van werknemer
		/// </summary>
		public string Voornaam { get; set; }
		/// <summary>
		/// Achternaam van werknemer
		/// </summary>
		public string Achternaam { get; set; }
		/// <summary>
		/// Functie van werknemer
		/// </summary>
		public string Functie { get; set; }
		/// <summary>
		/// Funtie en naam om te tonen in list
		/// </summary>
		public string NaamEnFunctie { get => $"{Voornaam} {Achternaam} - {Functie}"; }

		/// <summary>
		/// Ovveride van base om NaamenFunctie te returen
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return NaamEnFunctie;
		}
	}
}