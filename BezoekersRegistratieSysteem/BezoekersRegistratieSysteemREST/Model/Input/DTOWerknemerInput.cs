namespace BezoekersRegistratieSysteemREST.Model {
	public class DTOWerknemerInput : DTOPersoonInput {
		public DTOWerknemerInput(string voornaam, string achternaam, string email, uint bedrijfId, string functie) : base(voornaam, achternaam, email) {
			BedrijfId = bedrijfId;
			Functie = functie;
		}

		public uint BedrijfId { get; set; }
		public string Functie { get; set; }
	}
}