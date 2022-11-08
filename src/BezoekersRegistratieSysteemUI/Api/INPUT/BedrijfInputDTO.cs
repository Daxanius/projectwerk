namespace BezoekersRegistratieSysteemUI.Api.Input {
	/// <summary>
	/// De DTO voor inkomende bedrijf informatie
	/// </summary>
	public class BedrijfInputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="naam"></param>
		/// <param name="BTW"></param>
		/// <param name="telefoonNummer"></param>
		/// <param name="email"></param>
		/// <param name="adres"></param>
		public BedrijfInputDTO(string naam, string BTW, string telefoonNummer, string email, string adres) {
			Naam = naam;
			this.BTW = BTW;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
		}

		/// <summary>
		/// De naam van het bedrijf.
		/// </summary>
		public string Naam { get; set; }

		/// <summary>
		/// Het BTW nummer van het bedrijf.
		/// </summary>
		public string BTW { get; set; }

		/// <summary>
		/// Het telefoon nummer van het bedrijf.
		/// </summary>
		public string TelefoonNummer { get; set; }

		/// <summary>
		/// Het email van het bedrijf.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Het adres van het bedrijf.
		/// </summary>
		public string Adres { get; set; }
	}
}