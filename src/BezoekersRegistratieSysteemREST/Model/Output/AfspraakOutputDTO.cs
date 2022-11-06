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

		public AfspraakOutputDTO(long id, DateTime starttijd, DateTime? eindtijd, long bedrijfId,long bezoekerId, long werknemerId)
		{
			Id = id;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			BedrijfId = bedrijfId;
			BezoekerId = bezoekerId;
			WerknemerId = werknemerId;
		}

		public long Id { get; set; }
		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public long BedrijfId { get; set; }
		public long BezoekerId { get; set; }
		public long WerknemerId { get; set; }
	}
}
