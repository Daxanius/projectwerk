using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	/// <summary>
	/// De DTO voor uitgaande werknemer informatie.
	/// </summary>
	public class WerknemerOutputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="werknemerInfo"></param>
		public WerknemerOutputDTO(long id, string voornaam, string achternaam, List<WerknemerInfoOutputDTO> werknemerInfo, bool bezet) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
			Bezet = bezet;
		}

		/// <summary>
		/// De ID van de werknemer
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// De voornaam van de werknemer.
		/// </summary>
		public string Voornaam { get; set; }

		/// <summary>
		/// De achternaam van de werknemer.
		/// </summary>
		public string Achternaam { get; set; }

		/// <summary>
		/// Alle bedrijven waarbij de werknemer werkt.
		/// </summary>
		public List<WerknemerInfoOutputDTO> WerknemerInfo { get; set; } = new();

		/// <summary>
		/// Of de werknemer bezet of vrij is.
		/// </summary>
		public bool Bezet { get; set; }
	}
}