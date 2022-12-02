using System;
using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Model {

	public class WerknemerInfoDTO {
		public BedrijfDTO Bedrijf { get; set; }
		public string Email { get; set; }
		public IEnumerable<string> Functies { get; set; }

		public WerknemerInfoDTO(BedrijfDTO bedrijf, string email, IEnumerable<string> functies) {
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}

		public override bool Equals(object? obj) {
			return obj is WerknemerInfoDTO dTO &&
				   EqualityComparer<BedrijfDTO>.Default.Equals(Bedrijf, dTO.Bedrijf) &&
				   Email == dTO.Email &&
				   EqualityComparer<IEnumerable<string>>.Default.Equals(Functies, dTO.Functies);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Bedrijf, Email, Functies);
		}
	}
}