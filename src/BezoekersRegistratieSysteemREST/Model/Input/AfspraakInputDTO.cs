using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model
{
	/// <summary>
	/// De DTO voor inkomende afspraak informatie
	/// </summary>
	public class AfspraakInputDTO
	{
		/// <summary>
		/// Zet de DTO om naar de business variant
		/// </summary>
		/// <param name="werknemerManager">De werknemer manager</param>
		/// <param name="bedrijfManager">De bedrijf manager</param>
		/// <returns>De Business variant</returns>
		public Afspraak NaarBusiness(WerknemerManager werknemerManager, BedrijfManager bedrijfManager)
		{
			Werknemer werknemer = werknemerManager.GeefWerknemer(WerknemerId);
			Bezoeker bezoeker = Bezoeker.NaarBusiness();
			Bedrijf bedrijf = bedrijfManager.GeefBedrijf(BedrijfId);
			return new(DateTime.Now, bedrijf, bezoeker, werknemer);
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		public AfspraakInputDTO(BezoekerInputDTO bezoeker, long werknemerId, long bedrijfId)
		{
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
			BedrijfId = bedrijfId;
		}

		/// <summary>
		/// De bezoeker van de afspraak.
		/// </summary>
		public BezoekerInputDTO Bezoeker { get; set; }

		/// <summary>
		/// De werknemer waarmee de afspraak is gemaakt.
		/// </summary>
		public long WerknemerId { get; set; }

		/// <summary>
		/// Het bedrijf waarbinnen de afspraak is gemaakt.
		/// </summary>
		public long BedrijfId { get; set; }
	}
}