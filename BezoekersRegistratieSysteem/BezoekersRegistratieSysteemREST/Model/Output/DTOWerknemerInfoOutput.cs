using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Output {
	public class DTOWerknemerInfoOutput {
		public DTOWerknemerInfoOutput(string bedrijf, string email, List<string> functies) {
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}

		public string Bedrijf { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();
	}
}
