using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOAfpsraakInput {
		public static DTOAfpsraakInput NaarDTO(Afspraak afpsraak) {
			return new(DTOBezoekerInput.NaarDTO(afpsraak.Bezoeker), afpsraak.Werknemer.Id);
		}

		public DTOAfpsraakInput(DTOBezoekerInput bezoeker, uint werknemerId) {
			WerknemerId = werknemerId;
			Bezoeker = bezoeker;
		}

		public DTOBezoekerInput Bezoeker { get; set; }
		public uint WerknemerId { get; set; }
	}
}