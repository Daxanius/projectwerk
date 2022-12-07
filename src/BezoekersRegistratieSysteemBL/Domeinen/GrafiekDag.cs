namespace BezoekersRegistratieSysteemBL.Domeinen {
    public class GrafiekDag {
        private List<(string, int)> XwaardeGeparkeerdenTotaalPerWeek { get; set; } = new List<(string, int)>();
        public void VoegWaardeToe(string xwaarde, int GeparkeerdenTotaalPerWeek) {
            XwaardeGeparkeerdenTotaalPerWeek.Add((xwaarde, GeparkeerdenTotaalPerWeek));
        }
        public IReadOnlyList<(string, int)> GeefXwaardeGeparkeerdenTotaalPerWeek() {
            return XwaardeGeparkeerdenTotaalPerWeek;
        }
    }
}
