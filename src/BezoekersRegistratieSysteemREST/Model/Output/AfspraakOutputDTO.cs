using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output
{
	/// <summary>
	/// De DTO voor uitgaande afspraak informatie.
	/// </summary>
	public class AfspraakOutputDTO
	{
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns>De DTO variant.</returns>
		public static AfspraakOutputDTO NaarDTO(Afspraak afspraak)
		{
			return new(afspraak.Id, afspraak.Starttijd, afspraak.Eindtijd, afspraak.Bedrijf.Id, afspraak.Bedrijf.Naam, afspraak.Bezoeker.Id, afspraak.Bezoeker.Voornaam, afspraak.Bezoeker.Achternaam, afspraak.Werknemer.Id, afspraak.Werknemer.Voornaam, afspraak.Werknemer.Achternaam);
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="afspraken"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<AfspraakOutputDTO> NaarDTO(IEnumerable<Afspraak> afspraken)
		{
			List<AfspraakOutputDTO> output = new();
			foreach (Afspraak afspraak in afspraken)
			{
				output.Add(AfspraakOutputDTO.NaarDTO(afspraak));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="bedrijfId"></param>
		/// <param name="bedrijfNaam"></param>
		/// <param name="bezoekerId"></param>
		/// <param name="bezoekerVoornaam"></param>
		/// <param name="bezoekerAchternaam"></param>
		/// <param name="werknemerId"></param>
		/// <param name="werknemerVoornaam"></param>
		/// <param name="werknemerAchternaam"></param>
		public AfspraakOutputDTO(long id, DateTime starttijd, DateTime? eindtijd, long bedrijfId, string bedrijfNaam, long bezoekerId, string bezoekerVoornaam, string bezoekerAchternaam, long werknemerId, string werknemerVoornaam, string werknemerAchternaam)
		{
			Id = id;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			BedrijfId = bedrijfId;
			BedrijfNaam = bedrijfNaam;
			BezoekerId = bezoekerId;
			BezoekerVoornaam = bezoekerVoornaam;
			BezoekerAchternaam = bezoekerAchternaam;
			WerknemerId = werknemerId;
			WerknemerVoornaam = werknemerVoornaam;
			WerknemerAchternaam = werknemerAchternaam;
		}

		/// <summary>
		/// De afspraak ID.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// De starttijd van de afspraak.
		/// </summary>
		public DateTime Starttijd { get; set; }

		/// <summary>
		/// De eindtijd van de afspraak.
		/// </summary>
		public DateTime? Eindtijd { get; set; }

		/// <summary>
		/// Het bedrijf van de afspraak.
		/// </summary>
		public long BedrijfId { get; set; }

		/// <summary>
		/// De naam van het bedrijf. Dit word meegestuurd zodat
		/// we niet een volledig bedrijf moeten opvragen om
		/// de naam te verkrijgen.
		/// </summary>
		public string BedrijfNaam { get; set; }

		/// <summary>
		/// De bezoeker van de afspraak.
		/// </summary>
		public long BezoekerId { get; set; }

		/// <summary>
		/// De voornaam van de bezoeker.
		/// </summary>
		public string BezoekerVoornaam { get; set; }

		/// <summary>
		/// De achternaam van de bezoeker.
		/// </summary>
		public string BezoekerAchternaam { get; set; }

		/// <summary>
		/// De werknemer van de afpsraak.
		/// </summary>
		public long WerknemerId { get; set; }

		/// <summary>
		/// De voornaam van de werknemer.
		/// </summary>
		public string WerknemerVoornaam;

		/// <summary>
		/// De achternaam van de werknemer.
		/// </summary>
		public string WerknemerAchternaam { get; set; }
	}
}
