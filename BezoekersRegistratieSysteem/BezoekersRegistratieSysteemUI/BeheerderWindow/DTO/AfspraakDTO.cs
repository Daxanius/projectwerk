using System;

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.DTO {

	public class AfspraakDTO {
		public int Id { get; set; }
		public BezoekerDTO Bezoeker { get; set; }
		public WerknemerDTO Werknemer { get; set; }
		public string StartTijd { get; set; }
		public string EindTijd { get; set; }

		public AfspraakDTO(int id, BezoekerDTO bezoeker, WerknemerDTO werknemer, DateTime startTijd, DateTime eindTijd) {
			Id = id;
			Bezoeker = bezoeker;
			Werknemer = werknemer;
			StartTijd = startTijd.ToString("T");
			EindTijd = eindTijd.ToString("T");
		}
	}
}