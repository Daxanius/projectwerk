namespace BezoekersRegistratieSysteemREST.Model.Output {
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