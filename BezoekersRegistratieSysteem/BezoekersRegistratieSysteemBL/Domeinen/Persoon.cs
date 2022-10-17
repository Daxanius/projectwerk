using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	/// <summary>
	/// Algemene informatie over personen
	/// </summary>
	public abstract class Persoon {
		public uint Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public string Email { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Persoon() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="email"></param>
        public Persoon(string voornaam, string achternaam, string email) {
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
		}

        /// <summary>
        /// Constructor
        /// </summary>
		/// <param name="id"></param>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        /// <param name="email"></param>
        public Persoon(uint id, string voornaam, string achternaam, string email)
        {
            ZetId(id);
            ZetVoornaam(voornaam);
            ZetAchternaam(achternaam);
            ZetEmail(email);
        }

        /// <summary>
        /// Past de ID aan
        /// </summary>
        /// <param name="id"></param>
        public void ZetId(uint id) {
			Id = id;
		}

		/// <summary>
		/// Past de voornaam aan
		/// </summary>
		/// <param name="voornaam"></param>
		/// <exception cref="PersoonException"></exception>
		public void ZetVoornaam(string voornaam) {
            if (string.IsNullOrWhiteSpace(voornaam)) throw new PersoonException("Persoon - ZetVoornaam - voornaam mag niet leeg zijn");
            Voornaam = voornaam;
		}

		/// <summary>
		/// Past de achternaam aan
		/// </summary>
		/// <param name="achternaam"></param>
		/// <exception cref="PersoonException"></exception>
		public void ZetAchternaam(string achternaam) {
            if (string.IsNullOrWhiteSpace(achternaam)) throw new PersoonException("Persoon - ZetAchternaam - achternaam mag niet leeg zijn");
            Achternaam = achternaam;
		}

		/// <summary>
		/// Past het email-adres aan, controleert op
		/// notatie
		/// </summary>
		/// <param name="email"></param>
		/// <exception cref="PersoonException"></exception>
		public void ZetEmail(string email) {
            if (string.IsNullOrWhiteSpace(email)) throw new PersoonException("Persoon - ZetEmail - email mag niet leeg zijn");
            //Checkt of email geldig is
            if (Nutsvoorziening.IsEmailGeldig(email)) Email = email.Trim();
            else throw new PersoonException("Persoon - ZetEmail - email is niet geldig");
        }
	}
}