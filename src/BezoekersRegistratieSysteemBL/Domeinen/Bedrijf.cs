using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
	public class Bedrijf : IEquatable<Bedrijf?>
	{
		public long Id { get; private set; }
		public string Naam { get; private set; }
		public string BTW { get; private set; }
		public bool BtwGeverifieerd { get; private set; }
		public string TelefoonNummer { get; private set; }
		public string Email { get; private set; }
		public string Adres { get; private set; }

		private readonly List<Werknemer> _werknemers = new List<Werknemer>();

		/// <summary>
		/// Constructor REST
		/// </summary>
		public Bedrijf() { }

        /// <summary>
        /// Constructor voor het aanmaken van een bedrijf.
        /// </summary>
        /// <param name="naam"></param>
        /// <param name="btw"></param>
        /// <param name="telefoonNummer"></param>
        /// <param name="email"></param>
        /// <param name="adres"></param>
        public Bedrijf(string naam, string btw, string telefoonNummer, string email, string adres)
		{
			ZetNaam(naam);
			ZetBTWControle(btw);
			ZetTelefoonNummer(telefoonNummer);
			ZetEmail(email);
			ZetAdres(adres);
		}

        /// <summary>
        /// Constructor voor het ophalen van een bedrijf.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="naam"></param>
        /// <param name="btw"></param>
        /// <param name="telefoonNummer"></param>
        /// <param name="email"></param>
        /// <param name="adres"></param>
        public Bedrijf(long id, string naam, string btw, bool btwGeverifieerd, string telefoonNummer, string email, string adres)
		{
			ZetId(id);
			ZetNaam(naam);
			ZetBTW(btw, btwGeverifieerd);
			ZetTelefoonNummer(telefoonNummer);
			ZetEmail(email);
			ZetAdres(adres);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt het id in.
        /// </summary>
        /// <param name="id">Unieke identificator | moet groter zijn dan 0.</param>
        /// <exception cref="BedrijfException">"Bedrijf - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn."</exception>
        /// <remarks>Id wordt automatisch gegenereerd door de databank.</remarks>
        public void ZetId(long id)
		{
			if (id <= 0)
				throw new BedrijfException("Bedrijf - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn.");
			Id = id;
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt de bedrijfsnaam in.
        /// </summary>
        /// <param name="naam">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <exception cref="BedrijfException">"Bedrijf - Zetnaam - naam mag niet leeg zijn"</exception>
        public void ZetNaam(string naam)
		{
			if (string.IsNullOrWhiteSpace(naam))
				throw new BedrijfException("Bedrijf - Zetnaam - naam mag niet leeg zijn");
			naam = naam.Trim();
			Naam = Nutsvoorziening.NaamOpmaak(naam);
        }

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt het btwnummer in.
        /// </summary>
        /// <param name="btw">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <param name="geverifieerd">Staat wordt bijgewerkt adhv Nutsvoorziening.GeefBTWInfo(btw).</param>
        /// <exception cref="BedrijfException">"Bedrijf - ZetBTW - BTW mag niet leeg zijn"</exception>
		/// <remarks>True indien geverifieerd door API | False indien API offline.</remarks>
        public void ZetBTW(string btw, bool geverifieerd)
		{
			if (string.IsNullOrWhiteSpace(btw))
				throw new BedrijfException("Bedrijf - ZetBTW - BTW mag niet leeg zijn");
			BTW = Nutsvoorziening.VerwijderWhitespace(btw);
			BtwGeverifieerd = geverifieerd;
		}

        /// <summary>
        /// Verifieërt de geldigheid van het btwnummer.
        /// </summary>
        /// <param name="btw">Mag geen Null/WhiteSpace waarde zijn en moet een officieel erkend nummer zijn.</param>
        /// <exception cref="BedrijfException">"Bedrijf - ZetBTWControle - Btw mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfException">"Bedrijf - ZetBTWControle - Btw is niet geldig"</exception>
        /// <remarks>True indien geverifieerd door API | False indien API offline.</remarks>
        public void ZetBTWControle(string btw)
		{
			btw = Nutsvoorziening.VerwijderWhitespace(btw);
			if (string.IsNullOrWhiteSpace(btw))
				throw new BedrijfException("Bedrijf - ZetBTWControle - Btw mag niet leeg zijn");
			(bool validNummer, DTOBtwInfo? info) = Nutsvoorziening.GeefBTWInfo(btw);
			if (!validNummer)
				throw new BedrijfException("Bedrijf - ZetBTWControle - Btw is niet geldig");
			if (info is null)
				// Als de BTW controle service plat ligt dan is BTW niet gecontoleerd 
				// Maar wel geldig
				ZetBTW(btw, false);
			ZetBTW(btw, true);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt de bedrijfsnaam in.
        /// </summary>
        /// <param name="telefoonNummer">Mag geen Null/WhiteSpace waarde zijn en moet aan de voorwaarden voldoen.</param>
        /// <exception cref="BedrijfException">"Bedrijf - ZetTelefoonNummer - telefoonnummer mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfException">"Bedrijf - ZetTelefoonNummer - telefoonnummer mag niet langer zijn dan 15 karakters"</exception>
        public void ZetTelefoonNummer(string telefoonNummer)
		{
			if (string.IsNullOrWhiteSpace(telefoonNummer))
				throw new BedrijfException("Bedrijf - ZetTelefoonNummer - telefoonnummer mag niet leeg zijn");
			//Een telefoonnummer kan maximaal 15 cijfers bevatten. Het eerste deel van het telefoonnummer is de landcode (een tot drie cijfers),
			//Het tweede deel is de nationale bestemmingscode (NDC),
			//Het laatste deel is het abonneenummer (SN).
			if (!Nutsvoorziening.IsTelefoonnummerGeldig(telefoonNummer))
				throw new BedrijfException("Bedrijf - ZetTelefoonNummer - telefoonnummer mag niet langer zijn dan 15 karakters");
			TelefoonNummer = telefoonNummer.Trim();
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt het mailadres in.
        /// </summary>
        /// <param name="email">Mag geen Null/WhiteSpace waarde zijn en moet aan de voorwaarden voldoen.</param>
        /// <exception cref="BedrijfException">"Bedrijf - ZetEmail - email mag niet leeg zijn"</exception>
        /// <exception cref="BedrijfException">"Bedrijf - ZetEmail - email is niet geldig"</exception>
        public void ZetEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new BedrijfException("Bedrijf - ZetEmail - email mag niet leeg zijn");
			//Checkt of email geldig is
			if (Nutsvoorziening.IsEmailGeldig(email.Trim()))
				Email = email.Trim();
			else
				throw new BedrijfException("Bedrijf - ZetEmail - email is niet geldig");
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt het adres in.
        /// </summary>
        /// <param name="adres">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <exception cref="BedrijfException">"Bedrijf - ZetAdres - adres mag niet leeg zijn"</exception>
        public void ZetAdres(string adres)
		{
			if (string.IsNullOrWhiteSpace(adres))
				throw new BedrijfException("Bedrijf - ZetAdres - adres mag niet leeg zijn");
			Adres = adres.Trim();
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en voegt werknemer toe aan het bedrijf.
        /// </summary>
		/// <param name="werknemer">Mag geen Null waarde zijn.</param>
		/// <param name="email">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <param name="functie">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <exception cref="BedrijfException">"Bedrijf - VoegWerknemerToeInBedrijf - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfException">"Bedrijf - VoegWerknemerToeInBedrijf - email mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfException">"Bedrijf - VoegWerknemerToeInBedrijf - functie mag niet leeg zijn"</exception>
		/// <remarks>werknemer.VoegBedrijfEnFunctieToeAanWerknemer(this, email, functie) => Kent DIT bedrijf toe aan werknemer!</remarks>
        public void VoegWerknemerToeInBedrijf(Werknemer werknemer, string email, string functie)
		{
			if (werknemer == null)
				throw new BedrijfException("Bedrijf - VoegWerknemerToeInBedrijf - werknemer mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(email))
                throw new BedrijfException("Bedrijf - VoegWerknemerToeInBedrijf - email mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(functie))
				throw new BedrijfException("Bedrijf - VoegWerknemerToeInBedrijf - functie mag niet leeg zijn");

			// VoegBedrijfEnFunctieToe voert al de nodige controles uit om het
			if (!werknemer.GeefBedrijvenEnFunctiesPerWerknemer().ContainsKey(this))
			{
				_werknemers.Add(werknemer);
			}
			werknemer.VoegBedrijfEnFunctieToeAanWerknemer(this, email, functie);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en voegt werknemer toe aan het bedrijf.
        /// </summary>
        /// <param name="werknemer">Mag geen Null waarde zijn en bedrijf moet werknemer reeds bevatten.</param>
        /// <exception cref="BedrijfException">"Bedrijf - VerwijderWerknemerUitBedrijf - werknemer mag niet leeg zijn"</exception>
        /// <exception cref="BedrijfException">"Bedrijf - VerwijderWerknemerUitBedrijf - werknemer bestaat niet"</exception>
        /// <remarks>werknemer.VerwijderBedrijfVanWerknemer(this) => Verwijdert DIT bedrijf van werknemer!</remarks>
        public void VerwijderWerknemerUitBedrijf(Werknemer werknemer)
		{
			if (werknemer == null)
				throw new BedrijfException("Bedrijf - VerwijderWerknemerUitBedrijf - werknemer mag niet leeg zijn");
			if (!_werknemers.Contains(werknemer))
				throw new BedrijfException("Bedrijf - VerwijderWerknemerUitBedrijf - werknemer bestaat niet");
			_werknemers.Remove(werknemer);
			werknemer.VerwijderBedrijfVanWerknemer(this);
		}


        /// <summary>
        /// Haalt een lijst van werknemers op met enkel lees rechten voor dit bedrijf.
        /// </summary>
        /// <returns>IReadOnlyList van werknemer objecten.</returns>
        public IReadOnlyList<Werknemer> GeefWerknemers()
		{
			return _werknemers.AsReadOnly();
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en properties op gelijkheid.
        /// </summary>
        /// <param name="bedrijf">Te vergelijken bedrijf.</param>
        /// <returns>Boolean True als alle waarden gelijk zijn | False indien één of meerdere waarde(n) verschillend zijn.</returns>
        public bool BedrijfIsGelijk(Bedrijf bedrijf)
		{
			if (bedrijf == null)
				return false;
			if (Id != bedrijf.Id)
				return false;
			if (Naam != bedrijf.Naam)
				return false;
			if (BTW != bedrijf.BTW)
				return false;
			if (TelefoonNummer != bedrijf.TelefoonNummer)
				return false;
			if (Email != bedrijf.Email)
				return false;
			if (Adres != bedrijf.Adres)
				return false;
			foreach (Werknemer werknemer in _werknemers)
			{
				if (!bedrijf._werknemers.Contains(werknemer))
					return false;
			}
			return true;
		}

		public override bool Equals(object? obj)
		{
			return Equals(obj as Bedrijf);
		}

		public bool Equals(Bedrijf? other)
		{
			return other is not null &&
				   Id == other.Id;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}

		public static bool operator ==(Bedrijf? left, Bedrijf? right)
		{
			return EqualityComparer<Bedrijf>.Default.Equals(left, right);
		}

		public static bool operator !=(Bedrijf? left, Bedrijf? right)
		{
			return !(left == right);
		}
	}
}