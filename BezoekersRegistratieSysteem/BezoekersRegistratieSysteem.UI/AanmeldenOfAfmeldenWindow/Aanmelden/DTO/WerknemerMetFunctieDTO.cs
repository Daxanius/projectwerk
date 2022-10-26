namespace BezoekersRegistratieSysteem.UI.AanmeldenOfAfmeldenWindow.Aanmelden.DTO {
	public class WerknemerMetFunctieDTO {
		public WerknemerMetFunctieDTO(string naam, string functie) {
			Naam = naam;
			Functie = functie;
		}

		public string Naam { get; set; }
		public string Functie { get; set; }
	}
}