using System.Collections.Generic;
using System.Linq;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerDTO {
		public long? Id { get; set; }
		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Email { get; set; }
		private readonly string? _functie = null;
		public string Functie {
			get {
				if (_functie is not null) {
					return _functie;
				}
				var lijstVanFuncties = WerknemerInfoLijst.Select(w => string.Join(", ", w.Functies));
				return string.Join(", ", string.Join(", ", lijstVanFuncties));
			}
		}
		public string Status { get; set; }

		public IEnumerable<WerknemerInfoDTO> WerknemerInfoLijst { get; set; }

		public WerknemerDTO(long id, string voornaam, string achternaam, IEnumerable<WerknemerInfoDTO> werknemerinfoLijst) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfoLijst = werknemerinfoLijst;
		}

		public WerknemerDTO(long id, string voornaam, string achternaam, string email, string isWerknemerVrij) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Status = isWerknemerVrij;
		}

		public WerknemerDTO(long id, string voornaam, string achternaam, string email, IEnumerable<string> functie, string isWerknemerVrij) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			_functie = string.Join(", ", functie);
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