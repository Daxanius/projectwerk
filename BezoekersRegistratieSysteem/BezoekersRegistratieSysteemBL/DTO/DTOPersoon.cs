namespace BezoekersRegistratieSysteemBL.DTO {
	public class DTOPersoon {
		public DTOPersoon(string voornaam, string achternaam, string email) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
		}

		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Email { get; set; }
	}
}