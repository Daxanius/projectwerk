using System;

namespace BezoekersRegistratieSysteemUI.AanmeldenOfAfmeldenWindow.Aanmelden.DTO
{
	public class WerknemerMetFunctieDTO
	{
		public WerknemerMetFunctieDTO(int id, string voornaam, string achternaam, string functie)
		{
			Id = id;
			Voornaam = voornaam;
			Achternaam = achternaam;
			Functie = functie;
		}

		public WerknemerMetFunctieDTO(string voornaam, string achternaam, string functie)
		{
			Voornaam = voornaam;
			Achternaam = achternaam;
			Functie = functie;
		}

		public int? Id { get; set; }
		public string Voornaam { get; set; }
		public string Achternaam { get; set; }
		public string Functie { get; set; }
		public string NaamEnFunctie { get => $"{Voornaam} {Achternaam} - {Functie}"; }

		public override string ToString()
		{
			return NaamEnFunctie;
		}
	}
}