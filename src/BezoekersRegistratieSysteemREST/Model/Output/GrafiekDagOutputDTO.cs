using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	/// <summary>
	/// De DTO voor uitgaande grafiek informatie
	/// </summary>
	public class GrafiekDagOutputDTO {
		/// <summary>
		/// Zet de business variant om naar de DTO.
		/// </summary>
		/// <param name="grafiekDag"></param>
		/// <returns></returns>
		public static GrafiekDagOutputDTO NaarDTO(GrafiekDag grafiekDag) {
			return new(grafiekDag.GeefXwaardeGeparkeerdenTotaalPerWeek().ToList());
		}

		/// <summary>
		/// Zet een lijst van business variant instanties
		/// om naar een lijst van DTO instanties.
		/// </summary>
		/// <param name="grafieken"></param>
		/// <returns></returns>
		public static IEnumerable<GrafiekDagOutputDTO> NaarDTO(IEnumerable<GrafiekDag> grafieken) {
			List<GrafiekDagOutputDTO> output = new();
			foreach (var input in grafieken) {
				output.Add(NaarDTO(input));
			}
			return output;
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="geparkeerdenTotaalPerWeek"></param>
		public GrafiekDagOutputDTO(List<(string, int)> geparkeerdenTotaalPerWeek) {
			GeparkeerdenTotaalPerWeek = geparkeerdenTotaalPerWeek;
		}

		/// <summary>
		/// Totaal geparkeerden per week
		/// </summary>
		public IEnumerable<(string, int)> GeparkeerdenTotaalPerWeek { get; set; }
	}
}