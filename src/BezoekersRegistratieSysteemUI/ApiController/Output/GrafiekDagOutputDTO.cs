using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	public class GrafiekDagOutputDTO {
		public GrafiekDagOutputDTO(IReadOnlyDictionary<string, int> geparkeerdenTotaalPerWeek) {
			GeparkeerdenTotaalPerWeek = geparkeerdenTotaalPerWeek;
		}

		public IReadOnlyDictionary<string, int> GeparkeerdenTotaalPerWeek { get; set; }
	}
}