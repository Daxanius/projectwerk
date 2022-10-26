using System.Collections.Generic;

namespace BezoekersRegistratieSysteem.UI.BeheerderWindow.DTO
{
	public class BedrijfDTO
	{


		public int Id { get; set; }
		public string Naam { get; set; }
		public string BTW { get; set; }
		public string TelefoonNummer { get; set; }
		public string Email { get; set; }
		public string Adres { get; set; }

		public List<WerknemerDTO> Werknemers = new List<WerknemerDTO>();

		public BedrijfDTO(int id, string naam, string bTW, string telefoonNummer, string email, string adres, List<WerknemerDTO> werknemers)
		{
			Id = id;
			Naam = naam;
			BTW = bTW;
			TelefoonNummer = telefoonNummer;
			Email = email;
			Adres = adres;
			Werknemers = werknemers;
		}
	}
}