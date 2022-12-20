using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// De DTO voor uitgaande GrafiekDagDetail informatie
	/// </summary>
	public class GrafiekDagDetailOutputDTO {
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="grafiekDagDetail"></param>
		/// <returns></returns>
		public static GrafiekDagDetailOutputDTO NaarDTO(GrafiekDagDetail grafiekDagDetail) {
			return new(grafiekDagDetail.GeefXwaardeCheckInsPerUur(), grafiekDagDetail.GeefXwaardeGeparkeerdenTotaalPerUur());
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="grafieken"></param>
		/// <returns></returns>
		public static IEnumerable<GrafiekDagDetailOutputDTO> NaarDTO(IEnumerable<GrafiekDagDetail> grafieken) {
			List<GrafiekDagDetailOutputDTO> output = new();
			foreach (var input in grafieken) {
				output.Add(NaarDTO(input));
			}
			return output;
		}


		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="checkInsPerUur"></param>
		/// <param name="geparkeerdenTotaalPerUur"></param>
		public GrafiekDagDetailOutputDTO(IReadOnlyDictionary<string, int> checkInsPerUur, IReadOnlyDictionary<string, int> geparkeerdenTotaalPerUur) {
			CheckInsPerUur = checkInsPerUur;
			GeparkeerdenTotaalPerUur = geparkeerdenTotaalPerUur;
		}

		/// <summary>
		/// De check ins per uur voor een bedrijf.
		/// </summary>
		public IReadOnlyDictionary<string, int> CheckInsPerUur { get; set; }

		/// <summary>
		/// De geparkeerden per uur voor een bedrijf.
		/// </summary>
		public IReadOnlyDictionary<string, int> GeparkeerdenTotaalPerUur { get; set; }
	}
}