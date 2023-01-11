using System;

namespace BezoekersRegistratieSysteemUI.Api.Input {
	public class ParkingContractInputDTO {
		public ParkingContractInputDTO(long bedrijfId, DateTime starttijd, DateTime eindtijd, int aantalPlaatsen) {
			BedrijfId = bedrijfId;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			AantalPlaatsen = aantalPlaatsen;
		}

		/// <summary>
		/// Het ID van het bedrijf.
		/// </summary>
		public long BedrijfId { get; set; }

		/// <summary>
		/// De starttijd van het contract.
		/// </summary>
		public DateTime Starttijd { get; set; }

		/// <summary>
		/// De eindtijd van het contract.
		/// </summary>
		public DateTime Eindtijd { get; set; }

		/// <summary>
		/// Het aantal gereserveerde plaatsen.
		/// </summary>
		public int AantalPlaatsen { get; set; }
	}
}
