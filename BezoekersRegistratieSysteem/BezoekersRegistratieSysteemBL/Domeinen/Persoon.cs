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
        /// Constructor REST
        /// </summary>
        public Persoon() { }

        /// <summary>
        /// Constructor voor het aanmaken van een persoon in de BusinessLaag.
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
        /// Constructor voor het aanmaken van een persoon in de DataLaag.
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
        /// Zet id.
        /// </summary>
        /// <param name="id"></param>
        public void ZetId(uint id) {
			Id = id;
		}

        /// <summary>
        /// Zet voornaam.
        /// </summary>
        /// <param name="voornaam"></param>
        /// <exception cref="PersoonException"></exception>
        public void ZetVoornaam(string voornaam) {
            if (string.IsNullOrWhiteSpace(voornaam)) throw new PersoonException("Persoon - ZetVoornaam - voornaam mag niet leeg zijn");
            Voornaam = voornaam.Trim();
		}

        /// <summary>
        /// Zet achternaam.
        /// </summary>
        /// <param name="achternaam"></param>
        /// <exception cref="PersoonException"></exception>
        public void ZetAchternaam(string achternaam) {
            if (string.IsNullOrWhiteSpace(achternaam)) throw new PersoonException("Persoon - ZetAchternaam - achternaam mag niet leeg zijn");
            Achternaam = achternaam.Trim();
		}

        /// <summary>
        /// Roept email controle op uit Nutsvoorziening & zet email.
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