namespace BezoekersRegistratieSysteemBL.DTO {
	public class DTOBedrijf {
		public DTOBedrijf(string naam, string bTW, string telefoonNummer, string email, string adres) {
			Naam = naam;
			BTW = bTW;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
		}

		public string Naam { get; set; }
		public string BTW { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }
	}
}
