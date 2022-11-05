using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
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
		/// Constructor voor het aanmaken van een afspraak.
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
		/// Constructor voor het ophalen van een afspraak.
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
        /// Controleert voorwaarden op geldigheid en stelt het id in.
        /// </summary>
        /// <param name="id">Unieke identificator | moet groter zijn dan 0.</param>
        /// <exception cref="AfspraakException">"Afspraak - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn."</exception>
		/// <remarks>Id wordt automatisch gegenereerd door de databank.</remarks>
        public void ZetId(long id)
		{
			if (id <= 0)
				throw new AfspraakException("Afspraak - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn.");
			Id = id;
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt de startijd in.
        /// </summary>
        /// <param name="starttijd">Mag geen Null/Default waarde zijn en eindtijd mag nog geen waarde hebben.</param>
        /// <exception cref="AfspraakException">"Afspraak - ZetStarttijd - afspraak is al afgelopen"</exception>
		/// <exception cref="AfspraakException">"Afspraak - ZetStarttijd - starttijd is niet ingevuld"</exception>
        public void ZetStarttijd(DateTime starttijd)
		{
			if (Eindtijd is not null)
				throw new AfspraakException("Afspraak - ZetStarttijd - afspraak is al afgelopen");
			if (starttijd.Date == new DateTime())
				throw new AfspraakException("Afspraak - ZetStarttijd - starttijd is niet ingevuld");
			Starttijd = starttijd;
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt overkoepelend bedrijf en werknemer in.
        /// </summary>
        /// <param name="bedrijf">Roept ZetBedrijf(bedrijf) aan.</param>
        /// <param name="werknemer">Roept ZetWerknemer(werknemer) aan.</param>
        /// <exception cref="AfspraakException">"Afspraak - ZetBedrijfEnWerknemer - bedrijf en werknemer mogen niet leeg zijn"</exception>
		/// <exception cref="AfspraakException">"Afspraak - ZetBedrijfEnWerknemer - bedrijf bevat werknemer niet"</exception>
		/// <remarks>Voert extra controle uit om te verifiëren of de werknemer wel in desbetreffend bedrijf werkzaam is.</remarks>
        public void ZetBedrijfEnWerknemer(Bedrijf bedrijf, Werknemer werknemer) {
			if (bedrijf == null || werknemer == null)
				throw new AfspraakException("Afspraak - ZetBedrijfEnWerknemer - bedrijf en werknemer mogen niet leeg zijn");
			if (!bedrijf.GeefWerknemers().Contains(werknemer))
				throw new AfspraakException("Afspraak - ZetBedrijfEnWerknemer - bedrijf bevat werknemer niet");

			ZetBedrijf(bedrijf);
			ZetWerknemer(werknemer);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt de eindtijd in.
        /// </summary>
        /// <param name="eindtijd">Mag niet na starttijd liggen.</param>
        /// <exception cref="AfspraakException">"Afspraak - ZetEindtijd - eindtijd moet na starttijd liggen"</exception>
        public void ZetEindtijd(DateTime? eindtijd)
		{
			if (eindtijd.HasValue && eindtijd <= Starttijd)
				throw new AfspraakException("Afspraak - ZetEindtijd - eindtijd moet na starttijd liggen");
			Eindtijd = eindtijd;
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt bezoeker in.
        /// </summary>
        /// <param name="bezoeker">Mag geen Null waarde zijn.</param>
        /// <exception cref="AfspraakException">"Afspraak - ZetBezoeker - bezoeker mag niet leeg zijn"</exception>
        public void ZetBezoeker(Bezoeker bezoeker)
		{
			Bezoeker = bezoeker ?? throw new AfspraakException("Afspraak - ZetBezoeker - bezoeker mag niet leeg zijn");
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt bedrijf in.
        /// </summary>
        /// <param name="bedrijf">Mag geen Null waarde zijn.</param>
        /// <exception cref="AfspraakException">"Afspraak - ZetBedrijf - bedrijf mag niet leeg zijn"</exception>
        public void ZetBedrijf(Bedrijf bedrijf) {
			Bedrijf = bedrijf ?? throw new AfspraakException("Afspraak - ZetBedrijf - bedrijf mag niet leeg zijn");
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt werknemer in.
        /// </summary>
        /// <param name="werknemer">Mag geen Null waarde zijn.</param>
        /// <exception cref="AfspraakException">"Afspraak - ZetWerknemer - werknemer mag niet leeg zijn"</exception>
        public void ZetWerknemer(Werknemer werknemer)
		{
			Werknemer = werknemer ?? throw new AfspraakException("Afspraak - ZetWerknemer - werknemer mag niet leeg zijn");
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en properties op gelijkheid.
        /// </summary>
		/// <param name="afspraak">Te vergelijken afspraak.</param>
        /// <returns>Boolean True als alle waarden gelijk zijn | False indien één of meerdere waarde(n) verschillend zijn.</returns>
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
			if (afspraak.Bedrijf != Bedrijf)
				return false;
			return true;
		}
	}
}