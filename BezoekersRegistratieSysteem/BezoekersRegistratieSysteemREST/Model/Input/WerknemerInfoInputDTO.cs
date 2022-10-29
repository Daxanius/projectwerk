namespace BezoekersRegistratieSysteemREST.Model.Input {

	public class WerknemerInfoInputDTO {

		public WerknemerInfoInputDTO(uint bedrijfId, string email, List<string> functies) {
			BedrijfId = bedrijfId;
			Email = email;
			Functies = functies;
		}

		public uint BedrijfId { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();
	}
}