using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOWerknemerOutput {
		public static DTOWerknemerOutput NaarDTO(Werknemer werknemer) {
			var functies = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();
			Dictionary<uint, string> info = new();
			foreach(Bedrijf b in functies.Keys) {
				info.Add(b.Id, functies[b].Email);
			}

			return new(werknemer.Id, werknemer.Voornaam, werknemer.Achternaam, info);
		}

		public DTOWerknemerOutput(uint id, string voornaam, string achternaam, Dictionary<uint, string> werknemerInfo) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfo = werknemerInfo;
		}

		public uint Id { get; private set; }
		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }
		public Dictionary<uint, string> WerknemerInfo { get; set; } = new();
	}
}