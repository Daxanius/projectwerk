using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOAfspraakOutput {
		public DTOAfspraakOutput(uint id, DateTime starttijd, DateTime? eindtijd, string bezoeker, string werknemer) {
			Id = id;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			Bezoeker = bezoeker;
			Werknemer = werknemer;
		}

		public uint Id { get; set; }
		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public string Bezoeker { get; set; }
		public string Werknemer { get; set; }
	}
}
