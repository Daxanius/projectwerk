namespace BezoekersRegistratieSysteemBL.DTO {
	public class DTOParkingContract {
		public DTOParkingContract(DateTime starttijd, DateTime? eindtijd, int aantalPlaatsen) {
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			AantalPlaatsen = aantalPlaatsen;
		}

		public DateTime Starttijd { get; private set; }
		public DateTime? Eindtijd { get; private set; }
		public int AantalPlaatsen { get; private set; }
	}
}