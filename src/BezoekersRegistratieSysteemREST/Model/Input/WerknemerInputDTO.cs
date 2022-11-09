using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;

namespace BezoekersRegistratieSysteemREST.Model.Input {
	/// <summary>
	/// De DTO voor inkomende werknemer informatie
	/// </summary>
	public class WerknemerInputDTO {
		/// <summary>
		/// Zet de DTO om naar de business variant
		/// </summary>
		/// <returns>De business variant</returns>
		public Werknemer NaarBusiness(BedrijfManager bedrijfManager) {
			Werknemer werknemer = new(Voornaam, Achternaam);
			
			foreach(WerknemerInfoInputDTO info in WerknemerInfo) {
				Bedrijf bedrijf = bedrijfManager.GeefBedrijf(info.BedrijfId);

				foreach (string functie in info.Functies) {
					werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, info.Email, functie);
				}
			}

			return werknemer;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="werknemerInfo"></param>
		public WerknemerInputDTO(string voornaam, string achternaam, List<WerknemerInfoInputDTO> werknemerInfo) {
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
		/// De initiele werknemerinfo
		/// </summary>
		public List<WerknemerInfoInputDTO> WerknemerInfo { get; set; } = new();
	}
}