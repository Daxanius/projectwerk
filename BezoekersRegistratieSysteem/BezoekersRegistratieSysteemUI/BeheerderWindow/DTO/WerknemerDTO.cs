using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerDTO {
		public int? Id { get; set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }

		public Dictionary<BedrijfDTO, WerknemerInfoDTO> WerknemerInfo = new();

		public WerknemerDTO(int id, string voornaam, string achternaam) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		public WerknemerDTO(string voornaam, string achternaam) {
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		public void VoegBedrijfEnFunctieToe(BedrijfDTO bedrijf, WerknemerInfoDTO werknemerInfo) => WerknemerInfo.Add(bedrijf, werknemerInfo);
	}
}