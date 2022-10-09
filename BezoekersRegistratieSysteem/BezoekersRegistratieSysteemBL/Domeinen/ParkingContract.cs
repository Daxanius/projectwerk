using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;

namespace BezoekersRegistratieSysteemBL.Domeinen {
	public class ParkingContract {
		public DateTime Starttijd { get; private set; }
		public DateTime? Eindtijd { get; private set; }
		public int AantalPlaatsen { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="starttijd"></param>
		/// <param name="aantalPlaatsen"></param>
		public ParkingContract(DateTime starttijd, int aantalPlaatsen) {
			ZetStarttijd(starttijd);
			ZetAantalPlaatsen(aantalPlaatsen);
		}

		/// <summary>
		/// Past starttijd aan
		/// </summary>
		/// <param name="starttijd"></param>
		/// <exception cref="ParkingContractException"></exception>
		public void ZetStarttijd(DateTime starttijd) {
			if (Eindtijd is not null) throw new ParkingContractException("Starttijd mag niet leeg zijn");
			Starttijd = starttijd;
		}

		/// <summary>
		/// Past eindtijd aan
		/// </summary>
		/// <param name="eindtijd"></param>
		/// <exception cref="ParkingContractException"></exception>
		public void ZetEindtijd(DateTime eindtijd) {
			if (eindtijd < Starttijd) throw new ParkingContractException("Eindtijd mag niet leeg zijn");
			Eindtijd = eindtijd;
		}

		/// <summary>
		/// Past de plaatsen aan
		/// </summary>
		/// <param name="aantalPlaatsen"></param>
		/// <exception cref="ParkingContractException"></exception>
		public void ZetAantalPlaatsen(int aantalPlaatsen) {
			if (aantalPlaatsen < 0) throw new ParkingContractException("Aantal plaatsen mag niet leeg zijn");
			AantalPlaatsen = aantalPlaatsen;
		}
	}
}