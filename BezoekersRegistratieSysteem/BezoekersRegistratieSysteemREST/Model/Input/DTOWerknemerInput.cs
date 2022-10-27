using BezoekersRegistratieSysteemREST.Model.Input;

namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOWerknemerInput {
		public DTOWerknemerInput(string voornaam, string achternaam, List<DTOWerknemerInfoInput> info) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Info = info;
		}

		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public List<DTOWerknemerInfoInput> Info { get; set; } = new();
	}
}