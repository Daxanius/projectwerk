using System;

namespace BezoekersRegistratieSysteemUI.Api.Input {
	public class ParkeerplaatsInputDTO {

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="nummerplaat"></param>
		public ParkeerplaatsInputDTO(long bedrijfId, DateTime starttijd, DateTime? eindtijd, string nummerplaat) {
			BedrijfId = bedrijfId;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			Nummerplaat = nummerplaat;
		}

		/// <summary>
		/// Het ID van het bedrijf.
		/// </summary>
		public long BedrijfId { get; set; }

		/// <summary>
		/// De starttijd van de plaats.
		/// </summary>
		public DateTime Starttijd { get; set; }

		/// <summary>
		/// De eindtijd van de plaats.
		/// </summary>
		public DateTime? Eindtijd { get; set; }

		/// <summary>
		/// De nummerplaat van de auto.
		/// </summary>
		public string Nummerplaat { get; set; }
	}
}
