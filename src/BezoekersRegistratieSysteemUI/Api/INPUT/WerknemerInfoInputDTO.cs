using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.Input {
	/// <summary>
	/// De DTO voor inkomende werknemer informatie
	/// </summary>
	public class WerknemerInfoInputDTO
	{
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="email"></param>
		/// <param name="functies"></param>
		public WerknemerInfoInputDTO(long bedrijfId, string email, List<string> functies)
		{
			BedrijfId = bedrijfId;
			Email = email;
			Functies = functies;
		}

		/// <summary>
		/// Het bedrijf van de werknemer.
		/// </summary>
		public long BedrijfId { get; set; }

		/// <summary>
		/// Het bedrijfsmail van de werknemer.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// De functies binnen het bedrijf van de werknemer.
		/// </summary>
		public List<string> Functies { get; set; } = new();
	}
}