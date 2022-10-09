using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public abstract class Persoon {
		public uint Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public string Email { get; private set; }

		public Persoon(string voornaam, string achternaam, string email) {
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
		}

		public void ZetId(uint id) {
			Id = id;
		}
		public void ZetVoornaam(string voornaam) {
			if (string.IsNullOrWhiteSpace(voornaam)) throw new PersoonException("Voornaam mag niet leeg zijn");
			Voornaam = voornaam;
		}
		public void ZetAchternaam(string achternaam) {
			if (string.IsNullOrWhiteSpace(achternaam)) throw new PersoonException("Achternaam mag niet leeg zijn");
			Achternaam = achternaam;
		}
		public void ZetEmail(string email) {
			if (string.IsNullOrWhiteSpace(email)) throw new PersoonException("Email mag niet leeg zijn");
			//Checkt of email geldig is
			if (Nutsvoorziening.IsEmailGeldig(email)) Email = email.Trim();
			else throw new PersoonException("Email is niet geldig");
		}
	}
}