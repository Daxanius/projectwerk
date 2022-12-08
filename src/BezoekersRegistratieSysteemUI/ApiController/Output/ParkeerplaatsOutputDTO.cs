using BezoekersRegistratieSysteemBL.Domeinen;
using System;

namespace BezoekersRegistratieSysteemUI.Api.Output
{
	/// <summary>
	/// De DTO voor uitgaande parkeerplaats informatie.
	/// </summary>
	public class ParkeerplaatsOutputDTO {

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="nummerplaat"></param>
		public ParkeerplaatsOutputDTO(IdInfoOutputDTO bedrijf, DateTime starttijd, DateTime? eindtijd, string nummerplaat) {
			Bedrijf = bedrijf;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			Nummerplaat = nummerplaat;
		}

		/// <summary>
		/// De info van het bedrijf.
		/// </summary>
		public IdInfoOutputDTO Bedrijf { get; set; }

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
