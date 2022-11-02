using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerInfoDTO {
		public BedrijfDTO Bedrijf { get; private set; }
		public string Email { get; private set; }

		private List<string> Functies = new();

		public WerknemerInfoDTO(BedrijfDTO bedrijf, string email, List<string> functies) {
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}
	}
}