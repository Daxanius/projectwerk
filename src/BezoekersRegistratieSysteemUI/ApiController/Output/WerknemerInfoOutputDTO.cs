using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Output {
	/// <summary>
	/// De DTO voor uitgaande werknemerinfo informatie.
	/// </summary>
	public class WerknemerInfoOutputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <param name="email"></param>
		/// <param name="functies"></param>
		public WerknemerInfoOutputDTO(IdInfoOutputDTO bedrijf, string email, IEnumerable<string> functies) {
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}

		/// <summary>
		/// Het bedrijf van de info.
		/// </summary>
		public IdInfoOutputDTO Bedrijf { get; set; }

		/// <summary>
		/// De bedrijfsmail van de werknemer binnen dit bedrijf.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// De functies van de werknemer binnen dit bedrijf.
		/// </summary>
		public IEnumerable<string> Functies { get; set; }
	}
}
