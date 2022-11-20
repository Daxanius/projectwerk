using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// De DTO voor uitgaande contract informatie.
	/// </summary>
	public class ParkingContractOutputDTO {
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="contract"></param>
		/// <returns></returns>
		public static ParkingContractOutputDTO NaarDTO(ParkingContract contract) {
			return new(IdInfoOutputDTO.NaarDTO(contract.Bedrijf), contract.Starttijd, contract.Eindtijd, contract.AantalPlaatsen);
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="contracten"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<ParkingContractOutputDTO> NaarDTO(IEnumerable<ParkingContract> contracten) {
			List<ParkingContractOutputDTO> output = new();
			foreach (ParkingContract contract in contracten) {
				output.Add(NaarDTO(contract));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="aantalPlaatsen"></param>
		public ParkingContractOutputDTO(IdInfoOutputDTO bedrijf, DateTime starttijd, DateTime? eindtijd, int aantalPlaatsen) {
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
