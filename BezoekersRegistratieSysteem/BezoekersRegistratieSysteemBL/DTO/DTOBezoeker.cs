namespace BezoekersRegistratieSysteemBL.DTO {
	public class DTOBezoeker : DTOPersoon {
		public DTOBezoeker(string voornaam, string achternaam, string email, string bedrijf) : base(voornaam, achternaam, email) {
			Bedrijf = bedrijf;
		}

		public string Bedrijf { get; set; }
	}
}