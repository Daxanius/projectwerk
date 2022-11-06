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
			return new(afspraak.Id, afspraak.Starttijd, afspraak.Eindtijd, afspraak.Bedrijf.Id, afspraak.Bezoeker.Id, afspraak.Werknemer.Id);
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
		/// <param name="bezoekerId"></param>
		/// <param name="werknemerId"></param>
		public AfspraakOutputDTO(long id, DateTime starttijd, DateTime? eindtijd, long bedrijfId,long bezoekerId, long werknemerId)
		{
			Id = id;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			BedrijfId = bedrijfId;
			BezoekerId = bezoekerId;
			WerknemerId = werknemerId;
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
		/// De bezoeker van de afspraak.
		/// </summary>
		public long BezoekerId { get; set; }

		/// <summary>
		/// De werknemer van de afpsraak.
		/// </summary>
		public long WerknemerId { get; set; }
	}
}
