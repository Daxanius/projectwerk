namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOAfpsraakInput {
		public DTOAfpsraakInput(DTOBezoekerInput bezoeker, uint werknemerId) {
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
		}

		public DTOBezoekerInput Bezoeker { get; set; }
		public uint WerknemerId { get; set; }
	}
}