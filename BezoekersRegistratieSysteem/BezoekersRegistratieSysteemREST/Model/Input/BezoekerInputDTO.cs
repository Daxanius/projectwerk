using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model {

	public class BezoekerInputDTO {

		public Bezoeker NaarBusiness() {
			return new(Voornaam, Achternaam, Email, Bedrijf);
		}

		public BezoekerInputDTO(string voornaam, string achternaam, string email, string bedrijf) {
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