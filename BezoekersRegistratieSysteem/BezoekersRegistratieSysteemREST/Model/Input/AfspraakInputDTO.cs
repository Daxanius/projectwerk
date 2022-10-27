using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model {
	public class AfspraakInputDTO {
		public Afspraak NaarBusiness(WerknemerManager werknemerManager) {
			Werknemer werknemer = werknemerManager.GeefWerknemer(WerknemerId);
			Bezoeker bezoeker = Bezoeker.NaarBusiness();
			return new(DateTime.Now, bezoeker, werknemer);
		}

		public AfspraakInputDTO(BezoekerInputDTO bezoeker, uint werknemerId) {
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
		}

		public BezoekerInputDTO Bezoeker { get; set; }
		public uint WerknemerId { get; set; }
	}
}