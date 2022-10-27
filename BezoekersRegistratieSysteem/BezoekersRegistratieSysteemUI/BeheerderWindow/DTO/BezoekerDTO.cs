using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.DTO
{
	public class BezoekerDTO
	{
		public BezoekerDTO(string voornaam, string achternaam, string email, string bedrijf)
		{
			Voornaam = voornaam;
			Achternaam = achternaam;
			Email = email;
			Bedrijf = bedrijf;
		}

		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Email { get; set; }
		public string Bedrijf { get; set; }
	}
}
