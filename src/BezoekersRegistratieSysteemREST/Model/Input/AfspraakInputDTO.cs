using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Input
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

			return new(Starttijd ?? DateTime.Now, Eindtijd, bedrijf, bezoeker, werknemer);
		}

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <param name="starttijd"></param>
		/// <param name="eindtijd"></param>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		public AfspraakInputDTO(BezoekerInputDTO bezoeker, DateTime? starttijd, DateTime? eindtijd, long werknemerId, long bedrijfId)
		{
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
			BedrijfId = bedrijfId;
			Starttijd = starttijd;
			Eindtijd = eindtijd;
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

		/// <summary>
		/// De starttijd van de Afspraak, is standaard DateTime.Now.
		/// </summary>
		public DateTime? Starttijd { get; set; }

		/// <summary>
		/// De eindtijd van de afpsraak.
		/// </summary>
		public DateTime? Eindtijd { get; set; }
	}
}