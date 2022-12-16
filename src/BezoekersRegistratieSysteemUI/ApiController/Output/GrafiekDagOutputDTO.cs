using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	public class GrafiekDagOutputDTO {
		public GrafiekDagOutputDTO(List<(string, int)> geparkeerdenTotaalPerWeek) {
			GeparkeerdenTotaalPerWeek = geparkeerdenTotaalPerWeek;
		}

		public IEnumerable<(string, int)> GeparkeerdenTotaalPerWeek { get; set; }
	}
}