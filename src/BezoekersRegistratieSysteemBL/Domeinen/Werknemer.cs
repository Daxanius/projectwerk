using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
	public class Werknemer
	{

        public long Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }

		private Dictionary<Bedrijf, WerknemerInfo> werknemerInfo = new();

		/// <summary>
		/// Constructor REST
		/// </summary>
		public Werknemer() { }

        /// <summary>
        /// Constructor voor het aanmaken van een werknemer.
        /// </summary>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        public Werknemer(string voornaam, string achternaam)
		{
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
		}

        /// <summary>
        /// Constructor voor het ophalen van een werknemer.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="voornaam"></param>
        /// <param name="achternaam"></param>
        public Werknemer(long id, string voornaam, string achternaam)
		{
			ZetId(id);
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt het id in.
        /// </summary>
        /// <param name="id">Unieke identificator | moet groter zijn dan 0.</param>
        /// <exception cref="WerknemerException">"Werknemer - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn."</exception>
        /// <remarks>Id wordt automatisch gegenereerd door de databank.</remarks>
        public void ZetId(long id)
		{
			if (id <= 0)
				throw new WerknemerException("Werknemer - ZetId - id mag niet kleiner dan of gelijk aan 0 zijn.");
			Id = id;
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt de voornaam in.
        /// </summary>
        /// <param name="voornaam">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <exception cref="WerknemerException">"Werknemer - ZetVoornaam - voornaam mag niet leeg zijn"</exception>
        public void ZetVoornaam(string voornaam)
		{
			if (string.IsNullOrWhiteSpace(voornaam))
				throw new WerknemerException("Werknemer - ZetVoornaam - voornaam mag niet leeg zijn");
			voornaam = voornaam.Trim();
			Voornaam = Nutsvoorziening.NaamOpmaak(voornaam);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en stelt de achternaam in.
        /// </summary>
        /// <param name="achternaam">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <exception cref="WerknemerException">"Werknemer - ZetAchternaam - achternaam mag niet leeg zijn"</exception>
        public void ZetAchternaam(string achternaam)
		{
			if (string.IsNullOrWhiteSpace(achternaam))
				throw new WerknemerException("Werknemer - ZetAchternaam - achternaam mag niet leeg zijn");
            Achternaam = Nutsvoorziening.NaamOpmaak(achternaam);
        }

        /// <summary>
        /// Controleert voorwaarden op geldigheid en voegt bedrijf toe aan werknemer.
        /// </summary>
        /// <param name="bedrijf">Mag geen Null waarde zijn.</param>
        /// <param name="email">Mag geen Null/WhiteSpace waarde zijn of duplicaat.</param>
        /// <param name="functie">Mag geen Null/WhiteSpace waarde zijn of duplicaat.</param>
        /// <exception cref="WerknemerException">"Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - bedrijf mag niet leeg zijn"</exception>
        /// <exception cref="WerknemerException">"Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - email foutief"</exception>
        /// <exception cref="WerknemerException">"Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerException">"Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - werknemer is in dit bedrijf al werkzaam onder deze functie"</exception>
        /// <remarks>bedrijf.VoegWerknemerToeInBedrijf(this, email, functie) => Kent DEZE werknemer toe aan bedrijf!</remarks>
		/// <remarks>werknemerInfo[bedrijf].VoegWerknemerFunctieToe(functie) => Kent functie toe aan DEZE werknemer in bedrijf!</remarks>
        public void VoegBedrijfEnFunctieToeAanWerknemer(Bedrijf bedrijf, string email, string functie)
		{
			if (bedrijf == null)
				throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - bedrijf mag niet leeg zijn");
            if (string.IsNullOrWhiteSpace(email) || Nutsvoorziening.IsEmailGeldig(email) == false)
                throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - email foutief");
			if (string.IsNullOrWhiteSpace(functie))
				throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - functie mag niet leeg zijn");
			if (werknemerInfo.ContainsKey(bedrijf))
			{
				if (!werknemerInfo[bedrijf].GeefWerknemerFuncties().Contains(Nutsvoorziening.NaamOpmaak(functie)))
				{
					werknemerInfo[bedrijf].VoegWerknemerFunctieToe(Nutsvoorziening.NaamOpmaak(functie));
				} else
					throw new WerknemerException("Werknemer - VoegBedrijfEnFunctieToeAanWerknemer - werknemer is in dit bedrijf al werkzaam onder deze functie");
			} else
			{
				if (!bedrijf.GeefWerknemers().Contains(this))
				{
					bedrijf.VoegWerknemerToeInBedrijf(this, email, Nutsvoorziening.NaamOpmaak(functie));
				} else
				{
					werknemerInfo.Add(bedrijf, new WerknemerInfo(bedrijf, email));
					werknemerInfo[bedrijf].VoegWerknemerFunctieToe(Nutsvoorziening.NaamOpmaak(functie));
				}
			}
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en voegt bedrijf toe aan het werknemer.
        /// </summary>
        /// <param name="bedrijf">Mag geen Null waarde zijn en bedrijf moet werknemer reeds bevatten.</param>
        /// <exception cref="WerknemerException">"Werknemer - VerwijderBedrijfVanWerknemer - bedrijf mag niet leeg zijn"</exception>
        /// <exception cref="WerknemerException">"Werknemer - VerwijderBedrijfVanWerknemer - bedrijf bevat deze werknemer niet"</exception>
        /// <remarks>bedrijf.VerwijderWerknemerUitBedrijf(this) => Verwijdert DEZE werknemer uit bedrijf!</remarks>
        public void VerwijderBedrijfVanWerknemer(Bedrijf bedrijf)
		{
			if (bedrijf == null)
				throw new WerknemerException("Werknemer - VerwijderBedrijfVanWerknemer - bedrijf mag niet leeg zijn");
			if (!werknemerInfo.Keys.Contains(bedrijf))
				throw new WerknemerException("Werknemer - VerwijderBedrijfVanWerknemer - bedrijf bevat deze werknemer niet");
			// Dit word gebruikt als we een werknemer uit bedrijf halen
			// hierdoor is Bedrijf nullable.
			if (bedrijf.GeefWerknemers().Contains(this))
				bedrijf.VerwijderWerknemerUitBedrijf(this);
			werknemerInfo.Remove(bedrijf);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en wijzigt functie van werknemer.
        /// </summary>
		/// <param name="bedrijf">Mag geen Null waarde zijn en bedrijf moet werknemer reeds bevatten.</param>
        /// <param name="oudeFunctie">Mag geen Null/WhiteSpace waarde zijn.</param>
        /// <param name="nieuweFunctie">Mag geen Null/WhiteSpace waarde zijn of duplicaat.</param>
        /// <exception cref="WerknemerException">"Werknemer - WijzigFunctie - bedrijf mag niet leeg zijn"</exception>
        /// <exception cref="WerknemerException">"Werknemer - WijzigFunctie - oude functie mag niet leeg zijn"</exception>
        /// <exception cref="WerknemerException">"Werknemer - WijzigFunctie - nieuw functie mag niet leeg zijn"</exception>
        /// <exception cref="WerknemerException">"Werknemer - WijzigFunctie - bedrijf bevat deze werknemer niet"</exception>
		/// <exception cref="WerknemerException">"Werknemer - WijzigFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie"</exception>
        /// <exception cref="WerknemerException">"Werknemer - WijzigFunctie - werknemer is in dit bedrijf al werkzaam onder deze functie"</exception>
		/// <remarks>werknemerInfo[bedrijf].WijzigWerknemerFunctie(oudeFunctie, nieuweFunctie) => wijzigt functie voor DEZE werknemer in bedrijf!</remarks>
        public void WijzigFunctie(Bedrijf bedrijf, string oudeFunctie, string nieuweFunctie)
		{
			if (bedrijf == null)
				throw new WerknemerException("Werknemer - WijzigFunctie - bedrijf mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(oudeFunctie))
				throw new WerknemerException("Werknemer - WijzigFunctie - oude functie mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(nieuweFunctie))
				throw new WerknemerException("Werknemer - WijzigFunctie - nieuw functie mag niet leeg zijn");
			if (!werknemerInfo.ContainsKey(bedrijf))
				throw new WerknemerException("Werknemer - WijzigFunctie - bedrijf bevat deze werknemer niet");
			oudeFunctie = Nutsvoorziening.NaamOpmaak(oudeFunctie);
            nieuweFunctie = Nutsvoorziening.NaamOpmaak(nieuweFunctie);
			if (!werknemerInfo[bedrijf].GeefWerknemerFuncties().Contains(oudeFunctie))
				throw new WerknemerException("Werknemer - WijzigFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie");
			if (werknemerInfo[bedrijf].GeefWerknemerFuncties().Contains(nieuweFunctie))
				throw new WerknemerException("Werknemer - WijzigFunctie - werknemer is in dit bedrijf al werkzaam onder deze functie");
			werknemerInfo[bedrijf].WijzigWerknemerFunctie(oudeFunctie, nieuweFunctie);
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en verwijdert functie van werknemer.
        /// </summary>
        /// <param name="functie">Mag geen Null/WhiteSpace waarde zijn of duplicaat.</param>
		/// <param name="bedrijf">Mag geen Null waarde zijn en bedrijf moet werknemer reeds bevatten.</param>
        /// <exception cref="WerknemerException">"Werknemer - VerwijderFunctie - bedrijf mag niet leeg zijn"</exception>
        /// <exception cref="WerknemerException">"Werknemer - VerwijderFunctie - functie mag niet leeg zijn"</exception>
        /// <exception cref="WerknemerException">"Werknemer - VerwijderFunctie - bedrijf bevat deze werknemer niet"</exception>
		/// <exception cref="WerknemerException">"Werknemer - VerwijderFunctie - werknemer is in dit bedrijf werkzaam onder deze functie en kan niet verwijderd worden"</exception>
        /// <exception cref="WerknemerException">"Werknemer - VerwijderFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie"</exception>
        public void VerwijderFunctie(Bedrijf bedrijf, string functie)
		{
			if (bedrijf == null)
				throw new WerknemerException("Werknemer - VerwijderFunctie - bedrijf mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(functie))
				throw new WerknemerException("Werknemer - VerwijderFunctie - functie mag niet leeg zijn");
			if (!werknemerInfo.ContainsKey(bedrijf))
				throw new WerknemerException("Werknemer - VerwijderFunctie - bedrijf bevat deze werknemer niet");
			if (werknemerInfo[bedrijf].GeefWerknemerFuncties().Count() == 1)
				throw new WerknemerException("Werknemer - VerwijderFunctie - werknemer is in dit bedrijf werkzaam onder deze functie en kan niet verwijderd worden");
			functie = Nutsvoorziening.NaamOpmaak(functie);
			if (werknemerInfo[bedrijf].GeefWerknemerFuncties().Contains(functie))
			{
				werknemerInfo[bedrijf].VerwijderWerknemerFunctie(functie);
			} else
				throw new WerknemerException("Werknemer - VerwijderFunctie - werknemer is in dit bedrijf niet werkzaam onder deze functie");
		}

        /// <summary>
        /// Haalt een lijst van functies en bedrijven op met enkel lees rechten voor een werknemer.
        /// </summary>
        /// <returns>IReadOnlyDictionary van bedrijven en werknemerInfo.</returns>
        public IReadOnlyDictionary<Bedrijf, WerknemerInfo> GeefBedrijvenEnFunctiesPerWerknemer()
		{
			return werknemerInfo;
		}

        /// <summary>
        /// Controleert voorwaarden op geldigheid en properties op gelijkheid.
        /// </summary>
        /// <param name="werknemer">Te vergelijken werknemer.</param>
        /// <returns>Boolean True als alle waarden gelijk zijn | False indien één of meerdere waarde(n) verschillend zijn.</returns>
        public bool WerknemerIsGelijk(Werknemer werknemer)
		{
			if (werknemer == null)
				return false;
			if (werknemer.Id != Id)
				return false;
			if (werknemer.Voornaam != Voornaam)
				return false;
			if (werknemer.Achternaam != Achternaam)
				return false;
			foreach (Bedrijf bedrijf in werknemerInfo.Keys)
			{
				if (!werknemer.werknemerInfo.ContainsKey(bedrijf))
					return false;
				if (werknemerInfo[bedrijf].GeefWerknemerFuncties().Count() != werknemer.werknemerInfo[bedrijf].GeefWerknemerFuncties().Count())
					return false;
				foreach (string functie in werknemerInfo[bedrijf].GeefWerknemerFuncties())
				{
					if (!werknemer.werknemerInfo[bedrijf].GeefWerknemerFuncties().Contains(functie))
						return false;
				}
			}
			return true;
		}

		public override bool Equals(object? obj)
		{
			return obj is Werknemer werknemer &&
				   Id == werknemer.Id;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id);
		}
	}
}