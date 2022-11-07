﻿using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowDTO {

	public class WerknemerDTO {
		public long? Id { get; set; }
		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Email { get; set; }
		private string? _functie = null;
		public string Functie {
			get {
				if (_functie is not null) {
					return _functie;
				}
				var lijstVanFuncties = WerknemerInfoLijst.Select(w => string.Join(", ", w.Functies));
				return string.Join(", ", string.Join(", ", lijstVanFuncties));
			}
		}
		public bool Status { get; set; } = false;

		public List<WerknemerInfoDTO> WerknemerInfoLijst { get; set; } = new();

		public WerknemerDTO(long id, string voornaam, string achternaam, List<WerknemerInfoDTO> werknemerinfoLijst) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			WerknemerInfoLijst = werknemerinfoLijst;
		}

		public WerknemerDTO(long id, string voornaam, string achternaam, string email, bool isWerknemerVrij) {
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Status = isWerknemerVrij;
		}

		public WerknemerDTO(long id, string voornaam, string achternaam, string email, List<string> functie, bool isWerknemerVrij) {
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