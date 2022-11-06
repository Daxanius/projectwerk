using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output
{
	/// <summary>
	/// De DTO voor uitgaande bezoeker informatie.
	/// </summary>
	public class BezoekerOutputDTO
	{
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <returns>De DTO variant.</returns>
		public static BezoekerOutputDTO NaarDTO(Bezoeker bezoeker)
		{
			return new(bezoeker.Id, bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bezoeker.Bedrijf);
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="bezoekers"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<BezoekerOutputDTO> NaarDTO(IEnumerable<Bezoeker> bezoekers)
		{
			List<BezoekerOutputDTO> output = new();
			foreach (Bezoeker bezoeker in bezoekers)
			{
				output.Add(BezoekerOutputDTO.NaarDTO(bezoeker));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public BezoekerOutputDTO(long id, string voornaam, string achternaam, string email, string bedrijf)
		{
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		public long Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public string Email { get; private set; }
		public string Bedrijf { get; private set; }
	}
}
