using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Input {
	/// <summary>
	/// De DTO voor inkomende bezoeker informatie
	/// </summary>
	public class BezoekerInputDTO {
		/// <summary>
		/// Zet de DTO om naar de business variant
		/// </summary>
		/// <returns></returns>
		public Bezoeker NaarBusiness() {
			return new(Voornaam, Achternaam, Email, Bedrijf);
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public BezoekerInputDTO(string voornaam, string achternaam, string email, string bedrijf) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		/// <summary>
		/// De voornaam van de bezoeker.
		/// </summary>
		public string Voornaam { get; set; }

		/// <summary>
		/// De achternaam van de bezoeker.
		/// </summary>
		public string Achternaam { get; set; }

		/// <summary>
		/// Het email van de bezoeker.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// De bedrijfsnaam van de bezoeker.
		/// </summary>
		public string Bedrijf { get; set; }
	}
}