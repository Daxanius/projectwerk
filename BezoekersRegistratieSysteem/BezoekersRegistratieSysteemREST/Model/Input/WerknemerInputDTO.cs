using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;

namespace BezoekersRegistratieSysteemREST.Model {
	public class WerknemerInputDTO {
		public Werknemer NaarBusiness() {
			return new(Voornaam, Achternaam);
		}

		public WerknemerInputDTO(string voornaam, string achternaam) {
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
	}
}