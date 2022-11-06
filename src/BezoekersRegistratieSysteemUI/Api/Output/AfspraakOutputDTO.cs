using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System;
using System.Collections.Generic;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class AfspraakOutputDTO {
		public AfspraakOutputDTO(long id, DateTime starttijd, DateTime? eindtijd, long bedrijfId, long bezoekerId, long werknemerId) {
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
