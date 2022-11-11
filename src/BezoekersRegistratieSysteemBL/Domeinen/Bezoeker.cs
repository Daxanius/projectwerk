using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class Bezoeker {

		public long Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public string Email { get; private set; }
		public string Bedrijf { get; private set; }

		/// <summary>
		/// Constructor REST
		/// </summary>
		public Bezoeker() { }

		/// <summary>
		/// Constructor voor het aanmaken van een bezoeker.
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public Bezoeker(string voornaam, string achternaam, string email, string bedrijf) {
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
			ZetBedrijf(bedrijf);
		}

		/// <summary>
		/// Constructor voor het ophalen van een bezoeker.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public Bezoeker(long id, string voornaam, string achternaam, string email, string bedrijf) {
			ZetId(id);
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
			ZetBedrijf(bedrijf);
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt het id in.
		/// </summary>
		/// <param name="id">Unieke identificator | moet groter zijn dan 0.</param>
		/// <exception cref="BezoekerException">"Bezoeker - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn."</exception>
		/// <remarks>Id wordt automatisch gegenereerd door de databank.</remarks>
		public void ZetId(long id) {
			if (id <= 0)
				throw new BezoekerException("Bezoeker - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn.");
			Id = id;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt de voornaam in.
		/// </summary>
		/// <param name="voornaam">Mag geen Null/WhiteSpace waarde zijn.</param>
		/// <exception cref="BezoekerException">"Bezoeker - ZetVoornaam - voornaam mag niet leeg zijn"</exception>
		public void ZetVoornaam(string voornaam) {
			if (string.IsNullOrWhiteSpace(voornaam))
				throw new BezoekerException("Bezoeker - ZetVoornaam - voornaam mag niet leeg zijn");
			Voornaam = voornaam.Trim();
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt de achternaam in.
		/// </summary>
		/// <param name="achternaam">Mag geen Null/WhiteSpace waarde zijn.</param>
		/// <exception cref="BezoekerException">"Bezoeker - ZetAchternaam - achternaam mag niet leeg zijn"</exception>
		public void ZetAchternaam(string achternaam) {
			if (string.IsNullOrWhiteSpace(achternaam))
				throw new BezoekerException("Bezoeker - ZetAchternaam - achternaam mag niet leeg zijn");
			Achternaam = achternaam.Trim();
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt het mailadres in.
		/// </summary>
		/// <param name="email">Mag geen Null/WhiteSpace waarde zijn en moet aan de voorwaarden voldoen.</param>
		/// <exception cref="BezoekerException">"Bezoeker - ZetEmail - email mag niet leeg zijn"</exception>
		/// <exception cref="BezoekerException">"Bezoeker - ZetEmail - email is niet geldig"</exception>
		public void ZetEmail(string email) {
			if (string.IsNullOrWhiteSpace(email))
				throw new BezoekerException("Bezoeker - ZetEmail - email mag niet leeg zijn");
			//Checkt of email geldig is
			if (Nutsvoorziening.IsEmailGeldig(email.Trim()))
				Email = email.Trim();
			else
				throw new BezoekerException("Bezoeker - ZetEmail - email is niet geldig");
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt het bedrijf in.
		/// </summary>
		/// <param name="bedrijf">Mag geen Null/WhiteSpace waarde zijn.</param>
		public void ZetBedrijf(string bedrijf) {
			Bedrijf = bedrijf.Trim();
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en properties op gelijkheid.
		/// </summary>
		/// <param name="bezoeker">Te vergelijken bezoeker.</param>
		/// <returns>Boolean True als alle waarden gelijk zijn | False indien één of meerdere waarde(n) verschillend zijn.</returns>
		public bool BezoekerIsGelijk(Bezoeker bezoeker) {
			if (bezoeker == null)
				return false;
			if (bezoeker.Id != Id)
				return false;
			if (bezoeker.Voornaam != Voornaam)
				return false;
			if (bezoeker.Achternaam != Achternaam)
				return false;
			if (bezoeker.Email != Email)
				return false;
			if (bezoeker.Bedrijf != Bedrijf)
				return false;
			return true;
		}
	}
}