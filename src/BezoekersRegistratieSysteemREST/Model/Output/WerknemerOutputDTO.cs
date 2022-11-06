using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output
{
	/// <summary>
	/// De DTO voor uitgaande werknemer informatie.
	/// </summary>
	public class WerknemerOutputDTO
	{
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="werknemer"></param>
		/// <returns>De DTO variant.</returns>
		public static WerknemerOutputDTO NaarDTO(Werknemer werknemer)
		{
			var functies = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();
			Dictionary<long, WerknemerInfoOutputDTO> info = new();
			foreach (Bedrijf b in functies.Keys)
			{
				info.Add(b.Id, WerknemerInfoOutputDTO.NaarDTO(functies[b]));
			}

			return new(werknemer.Id, werknemer.Voornaam, werknemer.Achternaam, info.Values.ToList());
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="werknemers"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<WerknemerOutputDTO> NaarDTO(IEnumerable<Werknemer> werknemers)
		{
			List<WerknemerOutputDTO> output = new();
			foreach (Werknemer werknemer in werknemers)
			{
				output.Add(WerknemerOutputDTO.NaarDTO(werknemer));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="werknemerInfo"></param>
		public WerknemerOutputDTO(long id, string voornaam, string achternaam, List<WerknemerInfoOutputDTO> werknemerInfo)
		{
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
		}

		public long Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public List<WerknemerInfoOutputDTO> WerknemerInfo { get; set; } = new();
	}
}