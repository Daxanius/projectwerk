using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output
{
	public class AfsrpaakOutputDTO
	{
		public static AfsrpaakOutputDTO NaarDTO(Afspraak afspraak)
		{
			return new(afspraak.Id, afspraak.Starttijd, afspraak.Eindtijd, afspraak.Bezoeker.Id, afspraak.Werknemer.Id);
		}

		public static IEnumerable<AfsrpaakOutputDTO> NaarDTO(IEnumerable<Afspraak> afspraken)
		{
			List<AfsrpaakOutputDTO> output = new();
			foreach (Afspraak afspraak in afspraken)
			{
				output.Add(AfsrpaakOutputDTO.NaarDTO(afspraak));
			}
			return output;
		}

		public AfsrpaakOutputDTO(long id, DateTime starttijd, DateTime? eindtijd, long bezoekerId, long werknemerId)
		{
			Id = id;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			BezoekerId = bezoekerId;
			WerknemerId = werknemerId;
		}

		public long Id { get; set; }
		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public long BezoekerId { get; set; }
		public long WerknemerId { get; set; }
	}
}
