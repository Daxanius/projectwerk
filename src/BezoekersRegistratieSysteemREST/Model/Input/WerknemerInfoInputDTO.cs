using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;

namespace BezoekersRegistratieSysteemREST.Model.Input {
	/// <summary>
	/// De DTO voor inkomende werknemer informatie
	/// </summary>
	public class WerknemerInfoInputDTO {
		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="email"></param>
		/// <param name="functies"></param>
		public WerknemerInfoInputDTO(long bedrijfId, string email, IEnumerable<string> functies) {
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
		public IEnumerable<string> Functies { get; set; }

		/// <summary>
		/// Zet de DTO om naar de business variant.
		/// </summary>
		/// <param name="bedrijfManager"></param>
		/// <returns></returns>
		public WerknemerInfo NaarBusiness(BedrijfManager bedrijfManager) {
			WerknemerInfo werknemerinfo = new(bedrijfManager.GeefBedrijf(BedrijfId), Email);
			foreach (string functie in Functies) {
				werknemerinfo.VoegWerknemerFunctieToe(functie);
			}
			return werknemerinfo;
		}
	}
}