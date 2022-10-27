using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOBezoekerInput {
		public static DTOBezoekerInput NaarDTO(Bezoeker bezoeker) {
			return new(bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bezoeker.Bedrijf);
		}

		public static Bezoeker NaarBusiness(DTOBezoekerInput bezoeker) {
			return new(bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bezoeker.Bedrijf);
		}

		public DTOBezoekerInput(string voornaam, string achternaam, string email, string bedrijf) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Email { get; set; }
		public string Bedrijf { get; set; }
	}
}