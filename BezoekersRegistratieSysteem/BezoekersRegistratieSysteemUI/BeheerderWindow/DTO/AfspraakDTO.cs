using System;

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.DTO {

	public class AfspraakDTO {
		public int Id { get; set; }
		public BezoekerDTO Bezoeker { get; set; }
		public WerknemerDTO Werknemer { get; set; }
		public string WerknemerBedrijf { get; set; }
		public string StartTijd { get; set; }
		public string EindTijd { get; set; }
		public string Status { get; init; }

		public AfspraakDTO(int id, BezoekerDTO bezoeker, string werknemerBedrijf, WerknemerDTO werknemer, DateTime startTijd, DateTime? eindTijd) {
			Id = id;
			Bezoeker = bezoeker;
			WerknemerBedrijf = werknemerBedrijf;
			Werknemer = werknemer;
			StartTijd = startTijd.ToString("f");
			EindTijd = eindTijd.HasValue ? eindTijd.Value.ToString("f") : "";
			Status = eindTijd.HasValue ? "Afgerond" : "Lopend";
		}

		public AfspraakDTO(int id, BezoekerDTO bezoeker, string werknemerBedrijf, WerknemerDTO werknemer, DateTime startTijd) : this(id, bezoeker, werknemerBedrijf, werknemer, startTijd, null) {
		}
	}
}