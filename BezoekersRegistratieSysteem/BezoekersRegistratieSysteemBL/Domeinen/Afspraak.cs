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
		/// <param name="bezoeker"></param>
		/// <param name="werknemer"></param>
		public Afspraak(DateTime starttijd, Bezoeker bezoeker, Werknemer werknemer)
		{
			ZetStarttijd(starttijd);
			ZetBezoeker(bezoeker);
			ZetWerknemer(werknemer);
		}

		/// <summary>
		/// Constructor voor het aanmaken van een afspraak in de DataLaag.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="bezoeker"></param>
		/// <param name="werknemer"></param>
		public Afspraak(long id, DateTime starttijd, DateTime? eindtijd, Bezoeker bezoeker, Werknemer werknemer)
		{
			ZetId(id);
			ZetStarttijd(starttijd);
			ZetEindtijd(eindtijd);
			ZetBezoeker(bezoeker);
			ZetWerknemer(werknemer);
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
		/// Zet bezoeker.
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