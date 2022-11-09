namespace BezoekersRegistratieSysteemUI.Api.Output {
	/// <summary>
	/// Een class die bedoelt is als vervanger voor IDs in de REST
	/// Dit zodat we niet elke keer een request moeten maken
	/// voor triviale informatie zoals de naam.
	/// </summary>
	public class IdInfoOutputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="naam"></param>
		/// <param name="email"></param>
		/// <param name="bezoekerBedrijf"></param>
		public IdInfoOutputDTO(long id, string naam, string? email, string? bezoekerBedrijf) {
			Id = id;
			Naam = naam;
			Email = email;
			BezoekerBedrijf = bezoekerBedrijf;
		}

		/// <summary>
		/// De interne ID van de data.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// De naam van de data.
		/// </summary>
		public string Naam { get; set; }

		/// <summary>
		/// Email van bedrijf of bezoeker
		/// </summary>
		public string? Email { get; set; }

		/// <summary>
		/// De naam van bedrijf van de bezoeker.
		/// </summary>
		public string? BezoekerBedrijf { get; set; }
	}
}