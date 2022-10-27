namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOBezoekerInput : DTOPersoonInput {
		public DTOBezoekerInput(string voornaam, string achternaam, string email, string bedrijf) : base(voornaam, achternaam, email) {
			Bedrijf = bedrijf;
		}

		public string Bedrijf { get; set; }
	}
}