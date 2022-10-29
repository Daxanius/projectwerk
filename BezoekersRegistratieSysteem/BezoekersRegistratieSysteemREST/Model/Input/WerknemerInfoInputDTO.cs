using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemREST.Model.Input
{
	public class WerknemerInfoInputDTO
	{
		public WerknemerInfoInputDTO(long bedrijfId, string email, List<string> functies)
		{
			BedrijfId = bedrijfId;
			Email = email;
			Functies = functies;
		}

		public long BedrijfId { get; set; }
		public string Email { get; set; }
		public List<string> Functies { get; set; } = new();
	}
}