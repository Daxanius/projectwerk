using System;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	public class ParkingContractoutputDTO {
		public ParkingContractoutputDTO(IdInfoOutputDTO bedrijf, DateTime starttijd, DateTime? eindtijd, int aantalPlaatsen) {
			Bedrijf = bedrijf;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			AantalPlaatsen = aantalPlaatsen;
		}

		/// <summary>
		/// Het bedrijf van het contract.
		/// </summary>
		public IdInfoOutputDTO Bedrijf { get; set; }

		/// <summary>
		/// De starttijd van het contract.
		/// </summary>
		public DateTime Starttijd { get; set; }

		/// <summary>
		/// De eindtijd van het contract.
		/// </summary>
		public DateTime? Eindtijd { get; set; }

		/// <summary>
		/// Het aantal plaatsen van het contract.
		/// </summary>
		public int AantalPlaatsen { get; set; }
	}
}