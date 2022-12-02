using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;

namespace BezoekersRegistratieSysteemREST.Model.Input {
	/// <summary>
	/// De DTO voor inkomende parkeerplaats informatie
	/// </summary>
	public class ParkeerplaatsInputDTO {
		/// <summary>
		/// Zet de DTO om naar de businessvariant
		/// </summary>
		/// <param name="bedrijfManager"></param>
		/// <returns></returns>
		public Parkeerplaats NaarBusiness(BedrijfManager bedrijfManager) {
			Bedrijf bedrijf = bedrijfManager.GeefBedrijf(BedrijfId);

			return new(bedrijf, Starttijd, Eindtijd, Nummerplaat);
		}

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
