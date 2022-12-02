using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// De DTO voor uitgaande werknemer informatie.
	/// </summary>
	public class WerknemerOutputDTO {

		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		public static WerknemerOutputDTO NaarDTO(Werknemer werknemer) {
			var functies = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();
			List<WerknemerInfoOutputDTO> info = new();

			foreach (Bedrijf b in functies.Keys) {
				info.Add(WerknemerInfoOutputDTO.NaarDTO(functies[b]));
			}

			return new(werknemer.Id, werknemer.Voornaam, werknemer.Achternaam, info);
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="werknemers"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<WerknemerOutputDTO> NaarDTO(IEnumerable<Werknemer> werknemers) {
			List<WerknemerOutputDTO> output = new();
			foreach (Werknemer werknemer in werknemers) {
				output.Add(NaarDTO(werknemer));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="werknemerInfo"></param>
		/// <param name="statusNaam"></param>
		public WerknemerOutputDTO(long id, string voornaam, string achternaam, IEnumerable<WerknemerInfoOutputDTO> werknemerInfo, string statusNaam) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="werknemerInfo"></param>
		public WerknemerOutputDTO(long id, string voornaam, string achternaam, IEnumerable<WerknemerInfoOutputDTO> werknemerInfo) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
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
		public IEnumerable<WerknemerInfoOutputDTO> WerknemerInfo { get; set; }
	}
}