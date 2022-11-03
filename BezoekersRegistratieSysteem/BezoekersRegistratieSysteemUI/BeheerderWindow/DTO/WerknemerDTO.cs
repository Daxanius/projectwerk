using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerDTO {
		public int? Id { get; set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public string Email { get; private set; }

		public WerknemerDTO(int id, string voornaam, string achternaam, string email) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
		}

		public WerknemerDTO(string voornaam, string achternaam, string email) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
		}
	}
}