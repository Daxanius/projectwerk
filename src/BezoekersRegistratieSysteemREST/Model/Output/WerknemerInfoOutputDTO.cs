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
			return new(info.Bedrijf.Id, info.Email, info.GeefWerknemerFuncties().ToList());
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
				output.Add(WerknemerInfoOutputDTO.NaarDTO(info));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="email"></param>
		/// <param name="functies"></param>
		public WerknemerInfoOutputDTO(long bedrijfId, string email, List<string> functies)
		{
			BedrijfId = bedrijfId;
			Email = email;
			Functies = functies;
		}

		/// <summary>
		/// Het bedrijf van de info.
		/// </summary>
		public long BedrijfId { get; set; }

		/// <summary>
		/// De bedrijfsmail van de werknemer binnen dit bedrijf.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// De functies van de werknemer binnen dit bedrijf.
		/// </summary>
		public List<string> Functies { get; set; } = new();
	}
}
