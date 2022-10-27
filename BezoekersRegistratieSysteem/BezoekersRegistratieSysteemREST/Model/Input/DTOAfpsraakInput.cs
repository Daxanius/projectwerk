namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOAfpsraakInput {
		public DTOAfpsraakInput(DateTime starttijd, DateTime? eindtijd, DTOBezoekerInput bezoeker, uint werknemerId) {
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
		}

		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public DTOBezoekerInput Bezoeker { get; set; }
		public uint WerknemerId { get; set; }
	}
}