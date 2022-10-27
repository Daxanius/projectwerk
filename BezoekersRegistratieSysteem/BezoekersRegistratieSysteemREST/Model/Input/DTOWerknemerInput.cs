namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOWerknemerInput {
		public DTOWerknemerInput(string voornaam, string achternaam) {
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
	}
}