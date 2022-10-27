using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.DTO
{
	public class WerknemerDTO
	{

		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }

		public Dictionary<BedrijfDTO, WerknemerInfoDTO> WerknemerInfo = new();

		public WerknemerDTO(string voornaam, string achternaam)
		{
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		public void VoegBedrijfEnFunctieToe(BedrijfDTO bedrijf, WerknemerInfoDTO werknemerInfo) => WerknemerInfo.Add(bedrijf, werknemerInfo);
	}
}
