using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model
{
	public class AfspraakInputDTO
	{
		public Afspraak NaarBusiness(WerknemerManager werknemerManager, BedrijfManager bedrijfManager)
		{
			Werknemer werknemer = werknemerManager.GeefWerknemer(WerknemerId);
			Bezoeker bezoeker = Bezoeker.NaarBusiness();
			Bedrijf bedrijf = bedrijfManager.GeefBedrijf(BedrijfId);
			return new(DateTime.Now, bedrijf, bezoeker, werknemer);
		}

		public AfspraakInputDTO(BezoekerInputDTO bezoeker, long werknemerId, long bedrijfId)
		{
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
			BedrijfId = bedrijfId;
		}

		public BezoekerInputDTO Bezoeker { get; set; }
		public long WerknemerId { get; set; }
		public long BedrijfId { get; set; }
	}
}