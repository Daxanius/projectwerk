using System;

namespace BezoekersRegistratieSysteemUI.Model {

	public class AfspraakDTO {
		public long Id { get; set; }
		public BezoekerDTO Bezoeker { get; set; }
		public WerknemerDTO Werknemer { get; set; }
		public string WerknemerBedrijf { get; set; }
		public string StartTijd { get; set; }
		public DateTime StartTijdDate { get; set; }
		public string EindTijd { get; set; }
		public DateTime? EindTijdDate { get; set; }
		public string Status { get; set; }

		public AfspraakDTO(long id, BezoekerDTO bezoeker, string werknemerBedrijf, WerknemerDTO werknemer, DateTime startTijd, DateTime? eindTijd, string statusNaam) {
			Id = id;
			Bezoeker = bezoeker;
			WerknemerBedrijf = werknemerBedrijf;
			Werknemer = werknemer;
			StartTijd = startTijd.ToString("HH:mm - dd/MM/yyyy");
			StartTijdDate = startTijd;
			EindTijd = eindTijd.HasValue ? eindTijd.Value.ToString("HH:mm - dd/MM/yyyy") : "";
			EindTijdDate = eindTijd;
			Status = statusNaam;
		}

		public override bool Equals(object? obj) {
			return obj is AfspraakDTO dTO &&
				   Id == dTO.Id;
		}

		public override int GetHashCode() {
			HashCode hash = new HashCode();
			hash.Add(Id);
			return hash.ToHashCode();
		}
	}
}