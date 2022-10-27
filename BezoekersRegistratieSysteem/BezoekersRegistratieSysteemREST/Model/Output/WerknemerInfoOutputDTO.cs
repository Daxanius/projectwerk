using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class WerknemerInfoOutputDTO {
		public static WerknemerInfoOutputDTO NaarDTO(WerknemerInfo info) {
			return new(info.Bedrijf.Id, info.Email, info.GeefWerknemerFuncties().ToList());
		}

		public static IEnumerable<WerknemerInfoOutputDTO> NaarDTO(IEnumerable<WerknemerInfo> werknemers) {
			List<WerknemerInfoOutputDTO> output = new();
			foreach (WerknemerInfo info in werknemers) {
				output.Add(WerknemerInfoOutputDTO.NaarDTO(info));
			}
			return output;
		}

		public WerknemerInfoOutputDTO(uint bedrijfId, string email, List<string> functies) {
			BedrijfId = bedrijfId;
			Email = email;
			Functies = functies;
		}

		public uint BedrijfId { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();
	}
}
