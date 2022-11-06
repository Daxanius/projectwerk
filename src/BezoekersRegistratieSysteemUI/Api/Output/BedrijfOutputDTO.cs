using System.Collections.Generic;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class BedrijfOutputDTO {
		public long Id { get; set; }
		public string Naam { get; set; }
		public string BTW { get; set; }
		public bool IsGecontroleerd { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }

		public List<long> Werknemers { get; set; } = new();

		public BedrijfOutputDTO(long id, string naam, string bTW, bool isGecontroleert, string telefoonNummer, string email, string adres, List<long> werknemers) {
			Id = id;
			Naam = naam;
			BTW = bTW;
			IsGecontroleerd = isGecontroleert;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
			this.Werknemers = werknemers;
		}
	}
}
