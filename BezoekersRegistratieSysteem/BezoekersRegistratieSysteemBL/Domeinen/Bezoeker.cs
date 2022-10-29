using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
	/// <summary>
	/// Informatie over bezoekers
	/// </summary>
	public class Bezoeker
	{

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
		/// Constructor voor het aanmaken van een bezoeker in de BusinessLaag.
		/// </summary>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public Bezoeker(string voornaam, string achternaam, string email, string bedrijf)
		{
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
			ZetBedrijf(bedrijf);
		}

		/// <summary>
		/// Constructor voor het aanmaken van een bezoeker in de DataLaag.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="voornaam"></param>
		/// <param name="achternaam"></param>
		/// <param name="email"></param>
		/// <param name="bedrijf"></param>
		public Bezoeker(long id, string voornaam, string achternaam, string email, string bedrijf)
		{
			ZetId(id);
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
			ZetBedrijf(bedrijf);
		}

		/// <summary>
		/// Zet id bezoeker.
		/// </summary>
		/// <param name="id"></param>
		/// <exception cref="BezoekerException"></exception>
		public void ZetId(uint id) {
			if (id == 0)
				throw new BezoekerException("Bezoeker - ZetId - Id mag niet 0 zijn");
			Id = id;
		}

		/// <summary>
		/// Zet voornaam bezoeker.
		/// </summary>
		/// <param name="voornaam"></param>
		/// <exception cref="BezoekerException"></exception>
		public void ZetVoornaam(string voornaam) {
			if (string.IsNullOrWhiteSpace(voornaam))
				throw new BezoekerException("Bezoeker - ZetVoornaam - voornaam mag niet leeg zijn");
			Voornaam = voornaam.Trim();
		}

		/// <summary>
		/// Zet achternaam bezoeker.
		/// </summary>
		/// <param name="achternaam"></param>
		/// <exception cref="BezoekerException"></exception>
		public void ZetAchternaam(string achternaam) {
			if (string.IsNullOrWhiteSpace(achternaam))
				throw new BezoekerException("Bezoeker - ZetAchternaam - achternaam mag niet leeg zijn");
			Achternaam = achternaam.Trim();
		}

		/// <summary>
		/// Roept email controle op uit Nutsvoorziening & zet email.
		/// </summary>
		/// <param name="email"></param>
		/// <exception cref="BezoekerException"></exception>
		public void ZetEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new BezoekerException("Bezoeker - ZetEmail - email mag niet leeg zijn");
			//Checkt of email geldig is
			if (Nutsvoorziening.IsEmailGeldig(email.Trim()))
				Email = email.Trim();
			else
				throw new BezoekerException("Bezoeker - ZetEmail - email is niet geldig");
		}

		/// <summary>
		/// Zet bedrijf.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <exception cref="BezoekerException"></exception>
		public void ZetBedrijf(string bedrijf) {
			if (string.IsNullOrWhiteSpace(bedrijf))
				throw new BezoekerException("Bezoeker - ZetBedrijf - bedrijf mag niet leeg zijn");
			Bedrijf = bedrijf.Trim();
		}

		/// <summary>
		/// Vergelijkt bezoekers op inhoud.
		/// </summary>
		/// <exception cref="BedrijfException"></exception>
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