using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOAfpsraakInput {
		public Afspraak NaarBusiness(WerknemerManager werknemerManager) {
			Werknemer werknemer = werknemerManager.GeefWerknemer(WerknemerId);
			Bezoeker bezoeker = Bezoeker.NaarBusiness();
			return new(DateTime.Now, bezoeker, werknemer);
		}

		public DTOAfpsraakInput(DTOBezoekerInput bezoeker, uint werknemerId) {
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
		}

		public DTOBezoekerInput Bezoeker { get; set; }
		public uint WerknemerId { get; set; }
	}
}