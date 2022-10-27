﻿using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOAfspraakOutput {
		public static DTOAfspraakOutput NaarDTO(Afspraak afspraak) {
			return new(afspraak.Id, afspraak.Starttijd, afspraak.Eindtijd, afspraak.Bezoeker.Id, afspraak.Werknemer.Id);
		}

		public DTOAfspraakOutput(uint id, DateTime starttijd, DateTime? eindtijd, uint bezoekerId, uint werknemerId) {
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