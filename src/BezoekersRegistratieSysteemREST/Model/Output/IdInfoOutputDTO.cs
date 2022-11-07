using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// Een class die bedoelt is als vervanger voor IDs in de REST
	/// Dit zodat we niet elke keer een request moeten maken
	/// voor triviale informatie zoals de naam
	/// </summary>
	public class IdInfoOutputDTO {
		/// <summary>
		/// Zet een bedrijf om naar ID info.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <returns></returns>
		public static IdInfoOutputDTO NaarDTO(Bedrijf bedrijf) {
			return new(bedrijf.Id, bedrijf.Naam);
		}

		/// <summary>
		/// Zet een werknemer om naar ID info.
		/// </summary>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		public static IdInfoOutputDTO NaarDTO(Werknemer werknemer) {
			return new(werknemer.Id, $"{werknemer.Voornaam} {werknemer.Achternaam}");
		}

		/// <summary>
		/// Zet een bezoeker om naar ID info.
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <returns></returns>
		public static IdInfoOutputDTO NaarDTO(Bezoeker bezoeker) {
			return new(bezoeker.Id, $"{bezoeker.Voornaam} {bezoeker.Achternaam}");
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="naam"></param>
		public IdInfoOutputDTO(long id, string naam) {
			Id = id;
			Naam = naam;
		}

		/// <summary>
		/// De interne ID van de data.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// De naam van de data.
		/// </summary>
		public string Naam { get; set; }
	}
}