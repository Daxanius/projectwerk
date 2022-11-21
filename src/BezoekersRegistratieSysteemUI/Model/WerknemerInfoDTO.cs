using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Model {

	public class WerknemerInfoDTO {
		public BedrijfDTO Bedrijf { get; set; }
		public string Email { get; set; }
		public IEnumerable<string> Functies { get; set; }

		public WerknemerInfoDTO(BedrijfDTO bedrijf, string email, IEnumerable<string> functies) {
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}
	}
}