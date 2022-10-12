using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
	/// <summary>
	/// Een klasse die alle essentiele informatie van bedrijven bijhoudt,
	/// implementeerd IEquatable
	/// </summary>
	public class Bedrijf : IEquatable<Bedrijf?>
	{
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
		/// <param name="naam"></param>
		/// <param name="btw"></param>
		/// <param name="telefoonNummer"></param>
		/// <param name="email"></param>
		/// <param name="adres"></param>
		public Bedrijf(string naam, string btw, string telefoonNummer, string email, string adres)
		{
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
		public void ZetId(uint id)
		{
			Id = id;
		}

		/// <summary>
		/// Past de naam aan
		/// </summary>
		/// <param name="naam"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetNaam(string naam)
		{
			if (string.IsNullOrWhiteSpace(naam))
				throw new BedrijfException("Naam mag niet leeg zijn");
			Naam = naam;
		}

		/// <summary>
		/// Past het BTW nummer aan,
		/// controleert NIET of het BTW nummer geldig is
		/// </summary>
		/// <param name="btw"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetBTW(string btw)
		{
			if (string.IsNullOrWhiteSpace(btw))
				throw new BedrijfException("BTW mag niet leeg zijn");
			BTW = btw;
		}

		/// <summary>
		/// Past het telefoonnummer aan,
		/// controleert NIET of het nummer geldig is
		/// </summary>
		/// <param name="telefoonNummer"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetTelefoonNummer(string telefoonNummer)
		{
			if (string.IsNullOrWhiteSpace(telefoonNummer))
				throw new BedrijfException("Telefoonnummer mag niet leeg zijn");
			TelefoonNummer = telefoonNummer; //TODO: Controlleren of het nummer een max aantal cijfers heeft
		}

		/// <summary>
		/// Past het email-adres aan,
		/// controleert of het email geldig is
		/// </summary>
		/// <param name="email"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new BedrijfException("Email mag niet leeg zijn");
			//Checkt of email geldig is
			if (Nutsvoorziening.IsEmailGeldig(email))
				Email = email.Trim();
			else
				throw new BedrijfException("Email is niet geldig");
		}

		/// <summary>
		/// Past het adres aan,
		/// controleert NIET op geldigheid
		/// </summary>
		/// <param name="adres"></param>
		/// <exception cref="BedrijfException"></exception>
		public void ZetAdres(string adres)
		{
			if (string.IsNullOrWhiteSpace(adres))
				throw new BedrijfException("Adres mag niet leeg zijn");
			Adres = adres;
		}

		/// <summary>
		/// Voegt een werknemer toe
		/// en past de werknemer aan
		/// </summary>
		/// <param name="werknemer"></param>
		/// <exception cref="BedrijfException"></exception>
		public void VoegWerknemerToe(Werknemer werknemer)
		{
			if (werknemer == null)
				throw new BedrijfException("Werknemer mag niet leeg zijn");
			if (_werknemers.Contains(werknemer))
				throw new BedrijfException("Werknemer bestaat al");

			// ZetBedrijf voert al de nodige controles uit om het
			// vorige bedrijf te vervangen door dit bedrijf
			_werknemers.Add(werknemer);
			werknemer.ZetBedrijf(this);
		}

		/// <summary>
		/// Verwijdert een werknemer
		/// </summary>
		/// <param name="werknemer"></param>
		/// <exception cref="BedrijfException"></exception>
		public void VerwijderWerknemer(Werknemer werknemer)
		{
			if (werknemer == null)
				throw new BedrijfException("Werknemer mag niet leeg zijn");
			if (!_werknemers.Contains(werknemer))
				throw new BedrijfException("Werknemer bestaat niet");
			werknemer.VerwijderBedrijf();

			// Is dit werkelijk nodig? Kan een werknemer zonder een bedrijf zitten?
			_werknemers.Remove(werknemer);
		}

		public IReadOnlyList<Werknemer> GeefWerknemers()
		{
			return _werknemers.AsReadOnly();
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