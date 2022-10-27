using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOWerknemerOutput {
		public DTOWerknemerOutput(uint id, string voornaam, string achternaam, Dictionary<uint, uint> werknemerInfo) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
		}

		public uint Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public Dictionary<uint, uint> WerknemerInfo { get; set; } = new();
	}
}