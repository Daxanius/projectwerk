namespace BezoekersRegistratieSysteemBL.DTO {
	public class DTOParkingContract {
		public DTOParkingContract(DateTime starttijd, DateTime? eindtijd, int aantalPlaatsen) {
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			AantalPlaatsen = aantalPlaatsen;
		}

		public DateTime Starttijd { get; set; }
		public DateTime? Eindtijd { get; set; }
		public int AantalPlaatsen { get; set; }
	}
}