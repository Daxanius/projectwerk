namespace BezoekersRegistratieSysteemBL.DTO {
	public class DTOBezoeker : DTOPersoon {
		public DTOBezoeker(string voornaam, string achternaam, string email, uint bedrijfId) : base(voornaam, achternaam, email) {
			BedrijfId = bedrijfId;
		}

		public uint BedrijfId { get; set; }
	}
}