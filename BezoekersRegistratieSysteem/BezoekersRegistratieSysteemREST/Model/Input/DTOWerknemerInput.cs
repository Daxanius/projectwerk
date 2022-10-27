using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;

namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOWerknemerInput {
		public static DTOWerknemerInput NaarDTO(Werknemer werknemer) {
			return new(werknemer.Voornaam, werknemer.Achternaam);
		}

		public Werknemer NaarBusiness() {
			return new(Voornaam, Achternaam);
		}

		public DTOWerknemerInput(string voornaam, string achternaam) {
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
	}
}