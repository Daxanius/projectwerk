using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// Een class die bedoelt is als vervanger voor IDs in de REST
	/// Dit zodat we niet elke keer een request moeten maken
	/// voor triviale informatie zoals de naam.
	/// </summary>
	public class IdInfoOutputDTO {
		/// <summary>
		/// Zet een bedrijf om naar ID info.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <returns></returns>
		public static IdInfoOutputDTO NaarDTO(Bedrijf bedrijf) {
			return new(bedrijf.Id, bedrijf.Naam, bedrijf.Email, null);
		}

		/// <summary>
		/// Zet een werknemer om naar ID info.
		/// </summary>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		public static IdInfoOutputDTO NaarDTO(Werknemer werknemer) {
			return new(werknemer.Id, $"{werknemer.Voornaam};{werknemer.Achternaam}", null, null);
		}

		/// <summary>
		/// Zet een bezoeker om naar ID info.
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <returns></returns>
		public static IdInfoOutputDTO NaarDTO(Bezoeker bezoeker) {
			return new(bezoeker.Id, $"{bezoeker.Voornaam};{bezoeker.Achternaam}", bezoeker.Email, bezoeker.Bedrijf);
		}

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