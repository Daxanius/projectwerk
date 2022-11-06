using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model {
	/// <summary>
	/// De DTO voor inkomende werknemer informatie
	/// </summary>
	public class WerknemerInputDTO {
		/// <summary>
		/// Zet de DTO om naar de business variant
		/// </summary>
		/// <returns>De business variant</returns>
		public Werknemer NaarBusiness() {
			return new(Voornaam, Achternaam);
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		public WerknemerInputDTO(string voornaam, string achternaam) {
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		/// <summary>
		/// De voornaam van de werknemer.
		/// </summary>
		public string Voornaam { get; set; }

		/// <summary>
		/// De achternaam van de werknemer.
		/// </summary>
		public string Achternaam { get; set; }
	}
}