using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	public class GrafiekDagDetailOutputDTO {
		public GrafiekDagDetailOutputDTO(Dictionary<string, int> checkInsPerUur, Dictionary<string, int> geparkeerdenTotaalPerUur) {
			CheckInsPerUur = checkInsPerUur;
			GeparkeerdenTotaalPerUur = geparkeerdenTotaalPerUur;
		}

		public Dictionary<string, int> CheckInsPerUur { get; set; }

		public Dictionary<string, int> GeparkeerdenTotaalPerUur { get; set; }
	}
}