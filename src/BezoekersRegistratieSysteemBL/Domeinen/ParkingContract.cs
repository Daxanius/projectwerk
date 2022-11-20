using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {

	public class ParkingContract {

		public Bedrijf Bedrijf { get; private set; }
		public DateTime Starttijd { get; private set; }
		public DateTime? Eindtijd { get; private set; }
		public int AantalPlaatsen { get; private set; }

		/// <summary>
		/// Constructor REST
		/// </summary>
		public ParkingContract() { }

		/// <summary>
		/// Constructor voor het aanmaken van een parkingcontract.
		/// </summary>
		/// <param name="starttijd"></param>
		/// <param name="aantalPlaatsen"></param>
		public ParkingContract(Bedrijf bedrijf, DateTime starttijd, int aantalPlaatsen) {
			ZetBedrijf(bedrijf);
			ZetStarttijd(starttijd);
			ZetAantalPlaatsen(aantalPlaatsen);
		}

		/// <summary>
		/// Constructor voor het aanmaken van een parkingcontract.
		/// </summary>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="aantalPlaatsen"></param>
		public ParkingContract(Bedrijf bedrijf, DateTime starttijd, DateTime? eindtijd, int aantalPlaatsen) {
			ZetBedrijf(bedrijf);
			ZetStarttijd(starttijd);
			ZetEindtijd(eindtijd);
			ZetAantalPlaatsen(aantalPlaatsen);
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt bedrijf in.
		/// </summary>
		/// <param name="bedrijf">Mag geen Null waarde zijn.</param>
		/// <exception cref="ParkingContractException">"ParkingContract - ZetBedrijf - Bedrijf mag niet leeg zijn"</exception>
		public void ZetBedrijf(Bedrijf bedrijf) {
			if (Bedrijf == null) throw new ParkingContractException("ParkingContract - ZetBedrijf - Bedrijf mag niet leeg zijn");
			Bedrijf = bedrijf;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt de startijd in.
		/// </summary>
		/// <param name="starttijd">Mag geen Null/Default waarde zijn en eindtijd mag nog geen waarde hebben.</param>
		/// <exception cref="ParkingContractException">"ParkingContract - ZetStarttijd - Parking is al afgelopen"</exception>
		public void ZetStarttijd(DateTime starttijd) {
			if (Eindtijd is not null)
				throw new ParkingContractException("ParkingContract - ZetStarttijd - Parking is al afgelopen");
			Starttijd = starttijd;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt de eindtijd in.
		/// </summary>
		/// <param name="eindtijd">Mag niet na starttijd liggen.</param>
		/// <exception cref="ParkingContractException">"ParkingContract - ZetEindtijd - Eindtijd moet na starttijd liggen"</exception>
		public void ZetEindtijd(DateTime? eindtijd) {
			if (eindtijd < Starttijd)
				throw new ParkingContractException("ParkingContract - ZetEindtijd - Eindtijd moet na starttijd liggen");
			Eindtijd = eindtijd;
		}

		/// <summary>
		/// Controleert voorwaarden op geldigheid en stelt het aantal plaatsen in.
		/// </summary>
		/// <param name="aantalPlaatsen"></param>
		/// <exception cref="ParkingContractException">"ParkingContract - ZetAantalPlaatsen - Aantal plaatsen moet groter dan 0 zijn"</exception>
		public void ZetAantalPlaatsen(int aantalPlaatsen) {
			if (aantalPlaatsen < 0)
				throw new ParkingContractException("ParkingContract - ZetAantalPlaatsen - Aantal plaatsen moet groter dan 0 zijn");
			AantalPlaatsen = aantalPlaatsen;
		}
	}
}