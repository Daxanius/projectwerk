namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class GrafiekDag {
		private Dictionary<string, int> XwaardeGeparkeerdenTotaalPerWeek { get; set; } = new Dictionary<string, int>();
		public void VoegWaardeToe(string xwaarde, int GeparkeerdenTotaalPerWeek) {
			XwaardeGeparkeerdenTotaalPerWeek.Add(xwaarde, GeparkeerdenTotaalPerWeek);
		}
		public IReadOnlyDictionary<string, int> GeefXwaardeGeparkeerdenTotaalPerWeek() {
			return XwaardeGeparkeerdenTotaalPerWeek;
		}
	}
}
