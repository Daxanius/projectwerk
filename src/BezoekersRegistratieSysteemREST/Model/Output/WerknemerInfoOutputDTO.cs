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

		public static IEnumerable<WerknemerInfoOutputDTO> NaarDTO(IEnumerable<WerknemerInfo> werknemers)
		{
			List<WerknemerInfoOutputDTO> output = new();
			foreach (WerknemerInfo info in werknemers)
			{
				output.Add(WerknemerInfoOutputDTO.NaarDTO(info));
			}
			return output;
		}

		public WerknemerInfoOutputDTO(long bedrijfId, string email, List<string> functies)
		{
			BedrijfId = bedrijfId;
			Email = email;
			Functies = functies;
		}

		public long BedrijfId { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();
	}
}
