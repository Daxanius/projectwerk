using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Input {
	/// <summary>
	/// De DTO voor inkomende werknemer informatie
	/// </summary>
	public class WerknemerInputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		public WerknemerInputDTO(string voornaam, string achternaam, List<WerknemerInfoInputDTO> WerknemerInfo) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = WerknemerInfo;
		}

		/// <summary>
		/// De voornaam van de werknemer.
		/// </summary>
		public string Voornaam { get; set; }

		/// <summary>
		/// De achternaam van de werknemer.
		/// </summary>
		public string Achternaam { get; set; }

		/// <summary>
		/// Werknemerinfo
		/// </summary>
		public List<WerknemerInfoInputDTO> WerknemerInfo { get; set; } = new();
	}
}