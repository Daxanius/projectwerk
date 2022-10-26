﻿namespace BezoekersRegistratieSysteemBL.DTO {
	public class DTOAfpsraak {
		public DTOAfpsraak(DateTime starttijd, DateTime? eindtijd, uint werknemerId, uint bezoekerId) {
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			WerknemerId = werknemerId;
			BezoekerId = bezoekerId;
		}

		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public uint WerknemerId { get; set; }
		public uint BezoekerId { get; set; }
	}
}