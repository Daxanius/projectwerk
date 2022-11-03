using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerInfoDTO {
		public BedrijfDTO Bedrijf { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();

		public WerknemerInfoDTO(BedrijfDTO bedrijf, string email, List<string> functies) {
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}
	}
}