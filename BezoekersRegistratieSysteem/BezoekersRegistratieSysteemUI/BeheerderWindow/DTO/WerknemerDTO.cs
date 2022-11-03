using System.Collections.Generic;
using System.Linq;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerDTO {
		public int? Id { get; set; }
		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Email { get; set; }
		public string Functie { get => string.Join(", ", Functies); }
		public bool Status { get; set; }
		public BedrijfDTO Bedrijf;

		public List<string> Functies { get; set; } = new();

		public WerknemerDTO(int id, string voornaam, string achternaam, string email, BedrijfDTO bedrijf, bool isWerknemerVrij) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Status = isWerknemerVrij;
			Bedrijf = bedrijf;
		}

		public WerknemerDTO(string voornaam, string achternaam, string email, BedrijfDTO bedrijf, bool isWerknemerVrij) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Status = isWerknemerVrij;
			Bedrijf = bedrijf;
		}
	}
}