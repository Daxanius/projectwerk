using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class AfsrpaakOutputDTO {
		public static AfsrpaakOutputDTO NaarDTO(Afspraak afspraak) {
			return new(afspraak.Id, afspraak.Starttijd, afspraak.Eindtijd, afspraak.Bezoeker.Id, afspraak.Werknemer.Id);
		}

		public static IEnumerable<AfsrpaakOutputDTO> NaarDTO(IEnumerable<Afspraak> afspraken) {
			List<AfsrpaakOutputDTO> output = new();
			foreach (Afspraak afspraak in afspraken) {
				output.Add(AfsrpaakOutputDTO.NaarDTO(afspraak));
			}
			return output;
		}

		public AfsrpaakOutputDTO(uint id, DateTime starttijd, DateTime? eindtijd, uint bezoekerId, uint werknemerId) {
			Id = id;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			BezoekerId = bezoekerId;
			WerknemerId = werknemerId;
		}

		public uint Id { get; set; }
		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public uint BezoekerId { get; set; }
		public uint WerknemerId { get; set; }
	}
}
