using System.Collections.Generic;
using System.Linq;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class BedrijfDTO {
		public long Id { get; set; }
		public string Naam { get; set; }
		public string BTW { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }

		public List<WerknemerDTO> Werknemers = new List<WerknemerDTO>();

		public BedrijfDTO(long id, string naam, string btw, string telefoonNummer, string email, string adres, List<WerknemerDTO> werknemers) {
			Id = id;
			if (naam.Length > 1) {
				List<string> woorden = naam.Split(" ").ToList();
				Naam = string.Join(" ", woorden.Select(w => w[0].ToString().ToUpper() + w[1..]));
			} else
				Naam = naam;
			BTW = btw;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
			Werknemers = werknemers;
		}

		public BedrijfDTO(long id, string naam, string btw, string telefoonNummer, string email, string adres) {
			Id = id;
			if (naam.Length > 1) {
				List<string> woorden = naam.Split(" ").ToList();
				Naam = string.Join(" ", woorden.Select(w => w[0].ToString().ToUpper() + w[1..]));
			} else
				Naam = naam;
			BTW = btw;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
		}

		public BedrijfDTO(string naam, string btw, string telefoonNummer, string email, string adres) {
			if (naam.Length > 1) {
				List<string> woorden = naam.Split(" ").ToList();
				Naam = string.Join(" ", woorden.Select(w => w[0].ToString().ToUpper() + w[1..]));
			} else
				Naam = naam;
			BTW = btw;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
		}
	}
}