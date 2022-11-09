using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;

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
		/// <param name="werknemerManager"></param>
		/// <returns></returns>
		public static WerknemerOutputDTO NaarDTO(WerknemerManager werknemerManager, Werknemer werknemer)
		{
			var functies = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();
			List<WerknemerInfoOutputDTO> info = new();
			bool bezet = false;

			foreach (Bedrijf b in functies.Keys)
			{
				info.Add(WerknemerInfoOutputDTO.NaarDTO(functies[b]));

				// Om te kijken of de werknemer bezet is, deze oplossing is tijdelijk en zal nog
				// moeten geimplementeerd worde in de business
				// TODO: vervang dit met een werkende business method
				bezet = bezet || werknemerManager.GeefBezetteWerknemersOpDitMomentVoorBedrijf(b).Contains(werknemer);
			}

			return new(werknemer.Id, werknemer.Voornaam, werknemer.Achternaam, info, bezet);
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="werknemers"></param>
		/// <param name="werknemerManager"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<WerknemerOutputDTO> NaarDTO(WerknemerManager werknemerManager, IEnumerable<Werknemer> werknemers)
		{
			List<WerknemerOutputDTO> output = new();
			foreach (Werknemer werknemer in werknemers)
			{
				output.Add(NaarDTO(werknemerManager, werknemer));
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
		/// <param name="bezet"></param>
		public WerknemerOutputDTO(long id, string voornaam, string achternaam, IEnumerable<WerknemerInfoOutputDTO> werknemerInfo, bool bezet)
		{
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
			Bezet = bezet;
		}

		/// <summary>
		/// De ID van de werknemer
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// De voornaam van de werknemer.
		/// </summary>
		public string Voornaam { get; set; }

		/// <summary>
		/// De achternaam van de werknemer.
		/// </summary>
		public string Achternaam { get; set; }

		/// <summary>
		/// Alle bedrijven waarbij de werknemer werkt.
		/// </summary>
		public IEnumerable<WerknemerInfoOutputDTO> WerknemerInfo { get; set; }

		/// <summary>
		/// Of de werknemer bezet of vrij is.
		/// </summary>
		public bool Bezet { get; set; }
	}
}