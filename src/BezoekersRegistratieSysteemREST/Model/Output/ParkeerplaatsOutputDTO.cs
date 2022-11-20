using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// De DTO voor uitgaande parkeerplaats informatie.
	/// </summary>
	public class ParkeerplaatsOutputDTO {
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="parkeerplaats"></param>
		/// <returns></returns>
		public static ParkeerplaatsOutputDTO NaarDTO(Parkeerplaats parkeerplaats) {
			return new(IdInfoOutputDTO.NaarDTO(parkeerplaats.Bedrijf), parkeerplaats.Starttijd, parkeerplaats.Eindtijd, parkeerplaats.Nummerplaat);
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="parkeerplaatsen"></param>
		/// <returns>Een lijst van de DTO variant.</returns>
		public static IEnumerable<ParkeerplaatsOutputDTO> NaarDTO(IEnumerable<Parkeerplaats> parkeerplaatsen) {
			List<ParkeerplaatsOutputDTO> output = new();
			foreach (Parkeerplaats parkeerplaats in parkeerplaatsen) {
				output.Add(NaarDTO(parkeerplaats));
			}
			return output;
		}

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
