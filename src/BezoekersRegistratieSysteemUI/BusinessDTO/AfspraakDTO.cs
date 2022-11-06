﻿using System;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class AfspraakDTO {
		public long Id { get; set; }
		public BezoekerDTO Bezoeker { get; set; }
		public WerknemerDTO Werknemer { get; set; }
		public string WerknemerBedrijf { get; set; }
		public string StartTijd { get; set; }
		public string EindTijd { get; set; }
		public string Status { get; init; }

		public AfspraakDTO(long id, BezoekerDTO bezoeker, string werknemerBedrijf, WerknemerDTO werknemer, DateTime startTijd, DateTime? eindTijd) {
			Id = id;
			Bezoeker = bezoeker;
			WerknemerBedrijf = werknemerBedrijf;
			Werknemer = werknemer;
			StartTijd = startTijd.ToString("HH:mm - dd/MM/yyyy");
			EindTijd = eindTijd.HasValue ? eindTijd.Value.ToString("HH:mm - dd/MM/yyyy") : "";
			Status = eindTijd.HasValue ? "Afgerond" : "Lopend";
		}

		public AfspraakDTO(long id, BezoekerDTO bezoeker, string werknemerBedrijf, WerknemerDTO werknemer, DateTime startTijd) : this(id, bezoeker, werknemerBedrijf, werknemer, startTijd, null) {
		}
	}
}