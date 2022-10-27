using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Input {
	public class DTOWerknemerInfoInput {
		public static DTOWerknemerInfoInput NaarDTO(WerknemerInfo info) {
			return new(info.Bedrijf.Id, info.Email, info.GeefWerknemerFuncties().ToList());
		}

		public DTOWerknemerInfoInput(uint bedrijf, string email, List<string> functies) {
			Bedrijf = bedrijf;
			Email = email;
			Functies = functies;
		}

		public uint Bedrijf { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();
	}
}