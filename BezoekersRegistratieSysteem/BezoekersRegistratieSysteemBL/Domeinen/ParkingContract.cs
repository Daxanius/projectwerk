using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class ParkingContract {
		public DateTime Starttijd { get; private set; }
		public DateTime? Eindtijd { get; private set; }
		public int AantalPlaatsen { get; private set; }

		/// <summary>
		/// Constructor REST
		/// </summary>
		public ParkingContract() { }

		/// <summary>
		/// Constructor voor het aanmaken van een contract in de BusinessLaag.
		/// </summary>
		/// <param name="starttijd"></param>
		/// <param name="aantalPlaatsen"></param>
		public ParkingContract(DateTime starttijd, int aantalPlaatsen) {
			ZetStarttijd(starttijd);
			ZetAantalPlaatsen(aantalPlaatsen);
		}

		/// <summary>
		/// Constructor voor het aanmaken van een contract in de DataLaag.
		/// </summary>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="aantalPlaatsen"></param>
		public ParkingContract(DateTime starttijd, DateTime? eindtijd, int aantalPlaatsen) {
			ZetStarttijd(starttijd);
			ZetEindtijd(eindtijd);
			ZetAantalPlaatsen(aantalPlaatsen);
		}

		/// <summary>
		/// Zet starttijd.
		/// </summary>
		/// <param name="starttijd"></param>
		/// <exception cref="ParkingContractException"></exception>
		public void ZetStarttijd(DateTime starttijd) {
			if (Eindtijd is not null) throw new ParkingContractException("ParkingContract - ZetStarttijd - Parking is al afgelopen");
			Starttijd = starttijd;
		}

		/// <summary>
		/// Controleert & zet eindtijd.
		/// </summary>
		/// <param name="eindtijd"></param>
		/// <exception cref="ParkingContractException"></exception>
		public void ZetEindtijd(DateTime? eindtijd) {
			if (eindtijd < Starttijd) throw new ParkingContractException("ParkingContract - ZetEindtijd - Eindtijd moet na starttijd liggen");
			Eindtijd = eindtijd;
		}

		/// <summary>
		/// Zet aantal plaatsen.
		/// </summary>
		/// <param name="aantalPlaatsen"></param>
		/// <exception cref="ParkingContractException"></exception>
		public void ZetAantalPlaatsen(int aantalPlaatsen) {
			if (aantalPlaatsen < 0) throw new ParkingContractException("ParkingContract - ZetAantalPlaatsen - Aantal plaatsen moet groter dan 0 zijn");
			AantalPlaatsen = aantalPlaatsen;
		}
	}
}