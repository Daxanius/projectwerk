using System;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	/// <summary>
	/// De DTO voor uitgaande afspraak informatie.
	/// </summary>
	public class AfspraakOutputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="bedrijf"></param>
		/// <param name="bezoeker"></param>
		/// <param name="werknemer"></param>
		public AfspraakOutputDTO(long id, DateTime starttijd, DateTime? eindtijd, IdInfoOutputDTO bedrijf, IdInfoOutputDTO bezoeker, IdInfoOutputDTO werknemer, string statusNaam) {
			Id = id;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
			Bedrijf = bedrijf;
			Bezoeker = bezoeker;
			Werknemer = werknemer;
			StatusNaam = statusNaam;
		}

		/// <summary>
		/// De afspraak ID.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// De starttijd van de afspraak.
		/// </summary>
		public DateTime Starttijd { get; set; }

		/// <summary>
		/// De eindtijd van de afspraak.
		/// </summary>
		public DateTime? Eindtijd { get; set; }

		/// <summary>
		/// Het bedrijf van de afspraak.
		/// </summary>
		public IdInfoOutputDTO Bedrijf { get; set; }

		/// <summary>
		/// De bezoeker van de afspraak.
		/// </summary>
		public IdInfoOutputDTO Bezoeker { get; set; }

		/// <summary>
		/// De werknemer van de afpsraak.
		/// </summary>
		public IdInfoOutputDTO Werknemer { get; set; }
        /// <summary>
        /// De status van de afpsraak.
        /// </summary>
        public string StatusNaam { get; set; }
	}
}
