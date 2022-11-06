using BezoekersRegistratieSysteemUI.Api.DTO;
using System.Collections.Generic;
using System.Linq;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerDTO {
		public long? Id { get; set; }
		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Functie {
			get {
				var lijstVanFuncties = WerknemerInfoLijst.Select(w => string.Join(", ", w.Functies));
				return string.Join(", ", string.Join(", ", lijstVanFuncties));
			}
		}
		public bool Status { get; set; } = false;

		public List<ApiWerknemerInfoIn> WerknemerInfoLijst { get; set; } = new();

		public WerknemerDTO(long id, string voornaam, string achternaam, bool isWerknemerVrij, List<ApiWerknemerInfoIn> werknemerinfoLijst) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Status = isWerknemerVrij;
			WerknemerInfoLijst = werknemerinfoLijst;
		}

		public WerknemerDTO(long id, string voornaam, string achternaam, List<ApiWerknemerInfoIn> werknemerinfoLijst) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfoLijst = werknemerinfoLijst;
		}

		public WerknemerDTO(long id, string voornaam, string achternaam, bool isWerknemerVrij) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Status = isWerknemerVrij;
		}

		public WerknemerDTO(string voornaam, string achternaam, bool isWerknemerVrij, List<ApiWerknemerInfoIn> werknemerinfoLijst) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Status = isWerknemerVrij;
			WerknemerInfoLijst = werknemerinfoLijst;
		}

		public WerknemerDTO(string voornaam, string achternaam, bool isWerknemerVrij) {
			Voornaam = voornaam;
			Achternaam = achternaam;
			Status = isWerknemerVrij;
		}

		public override string ToString() {
			if (Functie.Length > 0) {
				return $"{Voornaam} {Achternaam} | {Functie}";
			} else {
				return $"{Voornaam} {Achternaam}";
			}
		}
	}
}