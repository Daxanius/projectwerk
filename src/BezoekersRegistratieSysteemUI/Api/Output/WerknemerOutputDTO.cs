using System.Collections.Generic;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class WerknemerOutputDTO {
		public WerknemerOutputDTO(long id, string voornaam, string achternaam, List<WerknemerInfoOutputDTO> werknemerInfo) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
		}

		public long Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public List<WerknemerInfoOutputDTO> WerknemerInfo { get; set; } = new();
	}
}