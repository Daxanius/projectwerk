using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	/// <summary>
	/// Een klasse die alle essentiele informatie van bedrijven bijhoudt,
	/// implementeerd IEquatable
	/// </summary>
	public class Bedrijf : IEquatable<Bedrijf?> {
		public uint Id { get; private set; }
		public string Naam { get; private set; }
		public string BTW { get; private set; }
		public string TelefoonNummer { get; private set; }
		public string Email { get; private set; }
		public string Adres { get; private set; }

		private readonly List<Werknemer> _werknemers = new List<Werknemer>();

        /// <summary>
        /// Constructor
        /// </summary>
        public Bedrijf() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="naam"></param>
        /// <param name="btw"></param>
        /// <param name="telefoonNummer"></param>
        /// <param name="email"></param>
        /// <param name="adres"></param>
        public Bedrijf(string naam, string btw, string telefoonNummer, string email, string adres) {
			ZetNaam(naam);
			ZetBTW(btw);
			ZetTelefoonNummer(telefoonNummer);
			ZetEmail(email);
			ZetAdres(adres);
		}

        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id"></param>
		/// <param name="naam"></param>
		/// <param name="btw"></param>
		/// <param name="telefoonNummer"></param>
		/// <param name="email"></param>
		/// <param name="adres"></param>
		public Bedrijf(uint id, string naam, string btw, string telefoonNummer, string email, string adres)
        {
			ZetId(id);
            ZetNaam(naam);
            ZetBTW(btw);
            ZetTelefoonNummer(telefoonNummer);
            ZetEmail(email);
            ZetAdres(adres);
        }

        /// <summary>
        /// Past de ID aan
        /// </summary>
        /// <param name="id"></param>
        public void ZetId(uint id) {
			Id = id;
		}

		/// <summary>
		/// Past de naam aan
		/// </summary>
		/// <param name="naam"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetNaam(string naam) {
            if (string.IsNullOrWhiteSpace(naam)) throw new BedrijfException("Bedrijf - Zetnaam - naam mag niet leeg zijn");
            Naam = naam;
		}

		/// <summary>
		/// Past het BTW nummer aan,
		/// controleert NIET of het BTW nummer geldig is
		/// </summary>
		/// <param name="btw"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetBTW(string btw) {
            if (string.IsNullOrWhiteSpace(btw)) throw new BedrijfException("Bedrijf - ZetBTW - BTW mag niet leeg zijn");
            BTW = btw;
		}

		/// <summary>
		/// Past het telefoonnummer aan,
		/// controleert NIET of het nummer geldig is
		/// </summary>
		/// <param name="telefoonNummer"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetTelefoonNummer(string telefoonNummer) {
            if (string.IsNullOrWhiteSpace(telefoonNummer)) throw new BedrijfException("Bedrijf - ZetTelefoonNummer - telefoonnummer mag niet leeg zijn");
            //Een telefoonnummer kan maximaal 15 cijfers bevatten. Het eerste deel van het telefoonnummer is de landcode (een tot drie cijfers),
			//Het tweede deel is de nationale bestemmingscode (NDC),
			//Het laatste deel is het abonneenummer (SN).
            if (telefoonNummer.Length > 15) throw new BedrijfException("Bedrijf - ZetTelefoonNummer - telefoonnummer mag niet langer zijn dan 15 karakters");
            TelefoonNummer = telefoonNummer;
		}

		/// <summary>
		/// Past het email-adres aan,
		/// controleert of het email geldig is
		/// </summary>
		/// <param name="email"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetEmail(string email) {
            if (string.IsNullOrWhiteSpace(email)) throw new BedrijfException("Bedrijf - ZetEmail - email mag niet leeg zijn");
            //Checkt of email geldig is
            if (Nutsvoorziening.IsEmailGeldig(email)) Email = email.Trim();
            else throw new BedrijfException("Bedrijf - ZetEmail - email is niet geldig");
        }

		/// <summary>
		/// Past het adres aan,
		/// controleert NIET op geldigheid
		/// </summary>
		/// <param name="adres"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetAdres(string adres) {
            if (string.IsNullOrWhiteSpace(adres)) throw new BedrijfException("Bedrijf - ZetAdres - adres mag niet leeg zijn");
            Adres = adres;
		}

        /// <summary>
        /// Voegt een werknemer toe
        /// en past de werknemer aan
        /// </summary>
        /// <param name="werknemer"></param>
        /// <param name="functie"></param>
        /// <exception cref="BedrijfException"></exception>
        public void VoegWerknemerToeInBedrijf(Werknemer werknemer, string functie) {
            if (werknemer == null) throw new BedrijfException("Bedrijf - VoegWerknemerToe - werknemer mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(functie)) throw new BedrijfException("Bedrijf - VoegWerknemerToe - functie mag niet leeg zijn");

			// VoegBedrijfEnFunctieToe voert al de nodige controles uit om het
			if (werknemer.GeefBedrijfEnFunctiesPerWerknemer().ToDictionary(x => x.Key, x => x.Value).ContainsKey(this))
			{
				_werknemers.Add(werknemer);
				werknemer.VoegBedrijfEnFunctieToeAanWerknemer(this, functie);
			}
		}

		/// <summary>
		/// Verwijdert een werknemer
		/// </summary>
		/// <param name="werknemer"></param>
		/// <exception cref="BedrijfException"></exception>
		public void VerwijderWerknemerUitBedrijf(Werknemer werknemer) {
            if (werknemer == null) throw new BedrijfException("Bedrijf - VerwijderWerknemer - werknemer mag niet leeg zijn");
            if (!_werknemers.Contains(werknemer)) throw new BedrijfException("Bedrijf - VerwijderWerknemer - werknemer bestaat niet");
			_werknemers.Remove(werknemer);
            werknemer.VerwijderBedrijfVanWerknemer(this);
        }
        
		public IReadOnlyList<Werknemer> GeefWerknemers() {
			return _werknemers.AsReadOnly();
		}

        public bool BedrijfIsGelijk(Bedrijf bedrijf)
		{
            if (bedrijf == null) return false;
            if (Id != bedrijf.Id) return false;
            if (Naam != bedrijf.Naam) return false;
            if (BTW != bedrijf.BTW) return false;
            if (TelefoonNummer != bedrijf.TelefoonNummer) return false;
            if (Email != bedrijf.Email) return false;
            if (Adres != bedrijf.Adres) return false;
			foreach (Werknemer werknemer in _werknemers)
			{
                if (!bedrijf._werknemers.Contains(werknemer)) return false;
            }
            return true;
        }

		public override bool Equals(object? obj) {
			return Equals(obj as Bedrijf);
		}

		public bool Equals(Bedrijf? other) {
			return other is not null &&
				   Id == other.Id;
		}

		public override int GetHashCode() {
			return HashCode.Combine(Id);
		}

		public static bool operator ==(Bedrijf? left, Bedrijf? right) {
			return EqualityComparer<Bedrijf>.Default.Equals(left, right);
		}

		public static bool operator !=(Bedrijf? left, Bedrijf? right) {
			return !(left == right);
		}
	}
}