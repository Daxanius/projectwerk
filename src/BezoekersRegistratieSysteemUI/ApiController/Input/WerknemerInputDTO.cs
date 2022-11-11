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
		public WerknemerInputDTO(string voornaam, string achternaam, IEnumerable<WerknemerInfoInputDTO> werknemerInfo) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
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
		public IEnumerable<WerknemerInfoInputDTO> WerknemerInfo { get; set; }
	}
}