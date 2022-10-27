﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteem.UI.BeheerderWindow.DTO
{
	public class WerknemerDTO
	{

		public string Voornaam { get; private set; }
		public string Achternaam { get; private set; }

		public Dictionary<string, WerknemerInfoDTO> WerknemerInfo = new();

		public WerknemerDTO(string voornaam, string achternaam)
		{
			Voornaam = voornaam;
			Achternaam = achternaam;
		}

		public void VoegBedrijfEnFunctieToe(string bedrijfsNaam, WerknemerInfoDTO werknemerInfo) => WerknemerInfo.Add(bedrijfsNaam, werknemerInfo);
	}
}
