
namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// De DTO voor uitgaande bezoeker informatie.
	/// </summary>
	public class BezoekerOutputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public BezoekerOutputDTO(long id, string voornaam, string achternaam, string email, string bedrijf) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		/// <summary>
		/// De ID van de bezoeker.
		/// </summary>
		public long Id { get; private set; }

		/// <summary>
		/// De voornaam van de bezoeker.
		/// </summary>
		public string Voornaam { get; private set; }

		/// <summary>
		/// De achternaam van de bezoeker.
		/// </summary>
		public string Achternaam { get; private set; }

		/// <summary>
		/// Het email van de bezoeker.
		/// </summary>
		public string Email { get; private set; }

		/// <summary>
		/// Het bedrijf van de bezoeker.
		/// </summary>
		public string Bedrijf { get; private set; }
	}
}