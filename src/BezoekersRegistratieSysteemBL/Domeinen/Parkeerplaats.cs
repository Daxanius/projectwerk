using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class Parkeerplaats {
		public Bedrijf Bedrijf { get; private set; }
		public DateTime Starttijd { get; private set; }
		public DateTime? Eindtijd { get; private set; }
		public string Nummerplaat { get; private set; }

		/// <summary>
		/// Constructor REST
		/// </summary>
		public Parkeerplaats() {
		}

		/// <summary>
		/// Constructor voor het aanmaken van een parkeerplaats.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <param name="starttijd"></param>
		/// <param name="nummerplaat"></param>
		public Parkeerplaats(Bedrijf bedrijf, DateTime starttijd, string nummerplaat) {
			Bedrijf = bedrijf;
			Starttijd = starttijd;
			Nummerplaat = nummerplaat;
		}

		/// <summary>
		/// Constructor voor het aanmaken van een parkeerplaats.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="nummerplaat"></param>
		public Parkeerplaats(Bedrijf bedrijf, DateTime starttijd, DateTime? eindtijd, string nummerplaat) {
			Bedrijf = bedrijf;
			Starttijd = starttijd;
			Nummerplaat = nummerplaat;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt bedrijf in.
		/// </summary>
		/// <param name="bedrijf">Mag geen Null waarde zijn.</param>
		/// <exception cref="ParkeerplaatsException">"Parkeerplaats - ZetBedrijf - Bedrijf mag niet leeg zijn"</exception>
		public void ZetBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new ParkeerplaatsException("Parkeerplaats - ZetBedrijf - Bedrijf mag niet leeg zijn");
			Bedrijf = bedrijf;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt de startijd in.
		/// </summary>
		/// <param name="starttijd">Mag geen Null/Default waarde zijn en eindtijd mag nog geen waarde hebben.</param>
		/// <exception cref="ParkeerplaatsException">"Parkeerplaats - ZetStarttijd - Parking is al afgelopen"</exception>
		public void ZetStarttijd(DateTime starttijd) {
			if (Eindtijd is not null)
				throw new ParkeerplaatsException("Parkeerplaats - ZetStarttijd - Parking is al afgelopen");
			Starttijd = starttijd;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt de eindtijd in.
		/// </summary>
		/// <param name="eindtijd">Mag niet na starttijd liggen.</param>
		/// <exception cref="ParkeerplaatsException">"Parkeerplaats - ZetEindtijd - Parking is al afgelopen"</exception>
		public void ZetEindtijd(DateTime eindtijd) {
			if (eindtijd < Starttijd)
				throw new ParkeerplaatsException("Parkeerplaats - ZetEindtijd - Parking is al afgelopen");
			Eindtijd = eindtijd;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt de nummerplaat in.
		/// </summary>
		/// <param name="nummerplaat"></param>
		/// <exception cref="ParkeerplaatsException">"Parkeerplaats - ZetNummerplaat - Nummerplaat mag niet leeg zijn"</exception>
		/// <exception cref="ParkeerplaatsException">"Parkeerplaats - ZetNummerplaat - Nummerplaat mag niet langer zijn dan 9 karakters"</exception>
		public void ZetNummerplaat(string nummerplaat) {
			if (string.IsNullOrWhiteSpace(nummerplaat))
				throw new ParkeerplaatsException("Parkeerplaats - ZetNummerplaat - Nummerplaat mag niet leeg zijn");
			//if (Nutsvoorziening.IsNummerplaatGeldig(nummerplaat))
			//throw new ParkeerplaatsException("Parkeerplaats - ZetNummerplaat - Nummerplaat is niet geldig");
			if (nummerplaat.Length > 9)
				throw new ParkeerplaatsException("Parkeerplaats - ZetNummerplaat - Nummerplaat mag niet langer zijn dan 9 karakters");
			Nummerplaat = nummerplaat.Trim();
		}
	}
}
