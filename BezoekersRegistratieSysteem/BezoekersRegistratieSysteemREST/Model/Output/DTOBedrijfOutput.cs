using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOBedrijfOutput {
		public DTOBedrijfOutput NaarDTO(Bedrijf bedrijf) {
			List<uint> werknemers = new();
			foreach(Werknemer w in bedrijf.GeefWerknemers()) {
				werknemers.Add(w.Id);
			}

			return new(bedrijf.Id, bedrijf.Naam, bedrijf.BTW, bedrijf.IsGecontroleert, bedrijf.TelefoonNummer, bedrijf.Email, bedrijf.Adres, werknemers);
		}

		public uint Id { get; set; }
		public string Naam { get; set; }
		public string BTW { get; set; }
		public bool IsGecontroleert { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }

		public List<uint> Werknemers { get; set; } = new();

		public DTOBedrijfOutput(uint id, string naam, string bTW, bool isGecontroleert, string telefoonNummer, string email, string adres, List<uint> werknemers) {
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
