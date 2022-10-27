using System.Collections.Generic;

namespace BezoekersRegistratieSysteem.UI.BeheerderWindow.DTO
{
	public class WerknemerInfoDTO
	{

		public string BedrijfsNaam { get; private set; }
		public string Email { get; private set; }

		private List<string> Functies = new();

		public WerknemerInfoDTO(string bedrijfsNaam, string email)
		{
			BedrijfsNaam = bedrijfsNaam;
			Email = email;
		}

		public void AddFunctie(string functie)
		{
			Functies.Add(functie);
		}
	}
}