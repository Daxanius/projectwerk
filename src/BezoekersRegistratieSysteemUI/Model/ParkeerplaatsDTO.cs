using System;

namespace BezoekersRegistratieSysteemUI.Model {
	/// <summary>
	/// De DTO voor uitgaande parkeerplaats informatie.
	/// </summary>
	public class ParkeerplaatsDTO {
		public BedrijfDTO Bedrijf { get; set; }
		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public string Nummerplaat { get; set; }

		public ParkeerplaatsDTO(BedrijfDTO bedrijf, DateTime starttijd, string nummerplaat) {
			Bedrijf = bedrijf;
			Starttijd = starttijd;
			Nummerplaat = nummerplaat;
		}

		public ParkeerplaatsDTO(BedrijfDTO bedrijf, DateTime starttijd, DateTime? eindtijd, string nummerplaat) {
			Bedrijf = bedrijf;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			Nummerplaat = nummerplaat;
		}
	}
}
