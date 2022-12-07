namespace BezoekersRegistratieSysteemBL.Domeinen {
    public class GrafiekWeek {
        private Dictionary<string, int> XwaardeGeparkeerdenTotaalPerWeek { get; set; } = new Dictionary<string, int>();
        public void VoegWaardeToe(string xwaarde, int GeparkeerdenTotaalPerWeek) {
            if (XwaardeGeparkeerdenTotaalPerWeek.ContainsKey(xwaarde)) {
                throw new Exception("Bevat al deze waarde");
            }
            XwaardeGeparkeerdenTotaalPerWeek.Add(xwaarde, GeparkeerdenTotaalPerWeek);
        }
        public IReadOnlyDictionary<string, int> GeefXwaardeGeparkeerdenTotaalPerWeek() {
            return XwaardeGeparkeerdenTotaalPerWeek;
        }
    }
}
