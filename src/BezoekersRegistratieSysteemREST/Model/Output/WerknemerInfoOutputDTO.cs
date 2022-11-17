using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output
{
	/// <summary>
	/// De DTO voor uitgaande werknemerinfo informatie.
	/// </summary>
	public class WerknemerInfoOutputDTO
	{
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="info"></param>
		/// <returns>De DTO variant.</returns>
		public static WerknemerInfoOutputDTO NaarDTO(WerknemerInfo info)
		{
			return new(IdInfoOutputDTO.NaarDTO(info.Bedrijf), info.Email, info.GeefWerknemerFuncties().ToList());
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="werknemers"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<WerknemerInfoOutputDTO> NaarDTO(IEnumerable<WerknemerInfo> werknemers)
		{
			List<WerknemerInfoOutputDTO> output = new();
			foreach (WerknemerInfo info in werknemers)
			{
				output.Add(NaarDTO(info));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <param name="email"></param>
		/// <param name="functies"></param>
		public WerknemerInfoOutputDTO(IdInfoOutputDTO bedrijf, string email, IEnumerable<string> functies)
		{
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}

		/// <summary>
		/// Het bedrijf van de info.
		/// </summary>
		public IdInfoOutputDTO Bedrijf { get; set; }

		/// <summary>
		/// De bedrijfsmail van de werknemer binnen dit bedrijf.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// De functies van de werknemer binnen dit bedrijf.
		/// </summary>
		public IEnumerable<string> Functies { get; set; }
	}
}
