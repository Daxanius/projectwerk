using System.Collections.Generic;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class WerknemerInfoOutputDTO {
		public WerknemerInfoOutputDTO(long bedrijfId, string email, List<string> functies) {
			BedrijfId = bedrijfId;
			Email = email;
			Functies = functies;
		}

		public long BedrijfId { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();
	}
}
