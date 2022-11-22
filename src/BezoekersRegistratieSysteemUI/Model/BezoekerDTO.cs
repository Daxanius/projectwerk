using System;

namespace BezoekersRegistratieSysteemUI.Model {

	public class BezoekerDTO {

		public BezoekerDTO(long id, string voornaam, string achternaam, string email, string bedrijf) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		public BezoekerDTO(string voornaam, string achternaam, string email, string bedrijf) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		public long Id { get; set; }
		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Email { get; set; }
		public string Bedrijf { get; set; }

		public override bool Equals(object? obj) {
			return obj is BezoekerDTO dTO &&
				   Id == dTO.Id &&
				   Voornaam == dTO.Voornaam &&
				   Achternaam == dTO.Achternaam &&
				   Email == dTO.Email &&
				   Bedrijf == dTO.Bedrijf;
		}

		public override int GetHashCode() {
			return HashCode.Combine(Id, Voornaam, Achternaam, Email, Bedrijf);
		}
	}
}