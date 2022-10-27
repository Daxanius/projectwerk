using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOBedrijfOutput {
		public uint Id { get; set; }
		public string Naam { get; set; }
		public string BTW { get; set; }
		public bool IsGecontroleert { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }

		public List<string> Werknemers { get; set; } = new();

		public DTOBedrijfOutput(uint id, string naam, string bTW, bool isGecontroleert, string telefoonNummer, string email, string adres, List<string> werknemers) {
			Id = id;
			Naam = naam;
			BTW = bTW;
			IsGecontroleert = isGecontroleert;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
			this.Werknemers = werknemers;
		}
	}
}
