using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class WerknemerOutputDTO {
		public static WerknemerOutputDTO NaarDTO(Werknemer werknemer) {
			var functies = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();
			Dictionary<uint, WerknemerInfoOutputDTO> info = new();
			foreach(Bedrijf b in functies.Keys) {
				info.Add(b.Id, WerknemerInfoOutputDTO.NaarDTO(functies[b]));
			}

			return new(werknemer.Id, werknemer.Voornaam, werknemer.Achternaam, info);
		}

		public static IEnumerable<WerknemerOutputDTO> NaarDTO(IEnumerable<Werknemer> werknemers) {
			List<WerknemerOutputDTO> output = new();
			foreach (Werknemer werknemer in werknemers) {
				output.Add(WerknemerOutputDTO.NaarDTO(werknemer));
			}
			return output;
		}

		public WerknemerOutputDTO(uint id, string voornaam, string achternaam, Dictionary<uint, WerknemerInfoOutputDTO> werknemerInfo) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
		}

		public uint Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public Dictionary<uint, WerknemerInfoOutputDTO> WerknemerInfo { get; set; } = new();
	}
}