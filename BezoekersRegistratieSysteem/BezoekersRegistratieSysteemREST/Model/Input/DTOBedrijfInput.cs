using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOBedrijfInput {
		public static DTOBedrijfInput NaarDTO(Bedrijf bedrijf) {
			return new(bedrijf.Naam, bedrijf.BTW, bedrijf.TelefoonNummer, bedrijf.Email, bedrijf.Adres);
		}

		public Bedrijf NaarBusiness() {
			return new(Naam, BTW, TelefoonNummer, Email, Adres);
		}

		public DTOBedrijfInput(string naam, string bTW, string telefoonNummer, string email, string adres) {
			Naam = naam;
			BTW = bTW;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
		}

		public string Naam { get; set; }
		public string BTW { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }
	}
}
