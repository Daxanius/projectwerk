using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
	/// <summary>
	/// Een klasse die de aanwezigheid van bezoekers
	/// bijhoudt
	/// </summary>
	public class Afspraak
	{
		public long Id { get; private set; }
		public DateTime Starttijd { get; private set; }
		public DateTime? Eindtijd { get; private set; }
		public Bedrijf Bedrijf { get; private set; }
		public Bezoeker Bezoeker { get; private set; }
		public Werknemer Werknemer { get; private set; }

		/// <summary>
		/// Constructor REST
		/// </summary>
		public Afspraak() { }

		/// <summary>
		/// Constructor voor het aanmaken van een afspraak in de BusinessLaag.
		/// </summary>
		/// <param name="starttijd"></param>
		/// <param name="bedrijf"></param>
		/// <param name="bezoeker"></param>
		/// <param name="werknemer"></param>
		public Afspraak(DateTime starttijd, Bedrijf bedrijf, Bezoeker bezoeker, Werknemer werknemer)
		{
			ZetStarttijd(starttijd);
			ZetBezoeker(bezoeker);
			ZetWerknemer(werknemer);
			ZetBedrijfEnWerknemer(bedrijf, werknemer);
		}

		/// <summary>
		/// Constructor voor het aanmaken van een afspraak in de DataLaag.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="bedrijf"></param>
		/// <param name="bezoeker"></param>
		/// <param name="werknemer"></param>
		public Afspraak(long id, DateTime starttijd, DateTime? eindtijd, Bedrijf bedrijf, Bezoeker bezoeker, Werknemer werknemer)
		{
			ZetId(id);
			ZetStarttijd(starttijd);
			ZetEindtijd(eindtijd);
			ZetBezoeker(bezoeker);
			ZetBedrijfEnWerknemer(bedrijf, werknemer);
		}

		/// <summary>
		/// Zet id.
		/// </summary>
		/// <param name="id"></param>
		public void ZetId(long id)
		{
			if (id <= 0)
				throw new AfspraakException("Afspraak - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn.");
			Id = id;
		}

		/// <summary>
		/// Zet starttijd.
		/// </summary>
		/// <param name="starttijd"></param>
		/// <exception cref="AfspraakException"></exception>
		public void ZetStarttijd(DateTime starttijd)
		{
			if (Eindtijd is not null)
				throw new AfspraakException("Afspraak - ZetStarttijd - afspraak is al afgelopen");
			if (starttijd.Date == new DateTime())
				throw new AfspraakException("Afspraak - ZetStarttijd - starttijd is niet ingevuld");
			Starttijd = starttijd;
		}

		/// <summary>
		/// Controleert & zet bedrijf
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <exception cref="AfspraakException"></exception>
		public void ZetBedrijfEnWerknemer(Bedrijf bedrijf, Werknemer werknemer) {
			if (bedrijf == null || werknemer == null)
				throw new AfspraakException("Afspraak - ZetBedrijfEnWerknemer - bedrijf en werknemer mogen niet leeg zijn");
			if (!bedrijf.GeefWerknemers().Contains(werknemer))
				throw new AfspraakException("Afspraak - ZetBedrijfEnWerknemer - bedrijf bevat werknemer niet");

			ZetBedrijf(bedrijf);
			ZetWerknemer(werknemer);
		}

		/// <summary>
		/// Controleert & zet eindtijd.
		/// </summary>
		/// <param name="eindtijd"></param>
		/// <exception cref="AfspraakException"></exception>
		public void ZetEindtijd(DateTime? eindtijd)
		{
			if (eindtijd.HasValue && eindtijd <= Starttijd)
				throw new AfspraakException("Afspraak - ZetEindtijd - eindtijd moet na starttijd liggen");
			Eindtijd = eindtijd;
		}

		/// <summary>
		/// Zet bezoeker.
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <exception cref="AfspraakException"></exception>
		public void ZetBezoeker(Bezoeker bezoeker)
		{
			Bezoeker = bezoeker ?? throw new AfspraakException("Afspraak - ZetBezoeker - bezoeker mag niet leeg zijn");
		}

		/// <summary>
		/// Zet bedrijf.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <exception cref="AfspraakException"></exception>
		public void ZetBedrijf(Bedrijf bedrijf) {
			Bedrijf = bedrijf ?? throw new AfspraakException("Afspraak - ZetBedrijf - bedrijf mag niet leeg zijn");
		}

		/// <summary>
		/// Zet werknemer.
		/// </summary>
		/// <param name="werknemer"></param>
		/// <exception cref="AfspraakException"></exception>
		public void ZetWerknemer(Werknemer werknemer)
		{
			Werknemer = werknemer ?? throw new AfspraakException("Afspraak - ZetWerknemer - werknemer mag niet leeg zijn");
		}

		/// <summary>
		/// Vergelijkt afspraken op inhoud.
		/// </summary>
		/// <exception cref="BedrijfException"></exception>
		public bool AfspraakIsGelijk(Afspraak afspraak)
		{
			if (afspraak is null)
				return false;
			if (afspraak.Id != Id)
				return false;
			if (afspraak.Starttijd != Starttijd)
				return false;
			if (afspraak.Eindtijd != Eindtijd)
				return false;
			if (afspraak.Bezoeker != Bezoeker)
				return false;
			if (afspraak.Werknemer != Werknemer)
				return false;
			return true;
		}
	}
}