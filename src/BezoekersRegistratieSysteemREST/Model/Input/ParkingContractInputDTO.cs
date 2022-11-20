using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;

namespace BezoekersRegistratieSysteemREST.Model.Input {
	/// <summary>
	/// De DTO voor inkomende contract informatie.
	/// </summary>
	public class ParkingContractInputDTO {
		/// <summary>
		/// Zet de DTO om naar de business variant.
		/// </summary>
		/// <param name="bedrijfManager"></param>
		/// <returns></returns>
		public ParkingContract NaarBusiness(BedrijfManager bedrijfManager) {
			Bedrijf bedrijf = bedrijfManager.GeefBedrijf(BedrijfId);

			return new(bedrijf, Starttijd, Eindtijd, AantalPlaatsen);
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="aantalPlaatsen"></param>
		public ParkingContractInputDTO(long bedrijfId, DateTime starttijd, DateTime? eindtijd, int aantalPlaatsen) {
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
		public DateTime? Eindtijd { get; set; }

		/// <summary>
		/// Het aantal gereserveerde plaatsen.
		/// </summary>
		public int AantalPlaatsen { get; set; }
	}
}
