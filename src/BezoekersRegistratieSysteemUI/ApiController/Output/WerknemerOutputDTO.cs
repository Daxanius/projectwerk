using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	public class WerknemerOutputDTO {
		public WerknemerOutputDTO(long id, string voornaam, string achternaam, IEnumerable<WerknemerInfoOutputDTO> werknemerInfo, string statusNaam) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
			StatusNaam = statusNaam;
		}

		public long Id { get; set; }

		public string Voornaam { get; set; }

		public string Achternaam { get; set; }

		public IEnumerable<WerknemerInfoOutputDTO> WerknemerInfo { get; set; }

		public string? StatusNaam { get; set; }
	}
}