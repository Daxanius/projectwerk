﻿using BezoekersRegistratieSysteemUI.AanmeldenOfAfmeldenWindow.Aanmelden.DTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Aanmelden.DTO
{
	internal class GegevensInfoDTO
	{
		internal string Voornaam { get; set; }
		internal string Achternaam { get; set; }
		internal string Email { get; set; }
		internal string Bedrijf { get; set; }
		internal WerknemerMetFunctieDTO Werknemer { get; set; }

		public GegevensInfoDTO(string voornaam, string achternaam, string email, string bedrijf, WerknemerMetFunctieDTO werknemer)
		{
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
			ZetBedrijf(bedrijf);
			ZetWerknemer(werknemer);
		}

		internal void ZetVoornaam(string voornaam)
		{
			ControlleerInput(voornaam, "Voornaam is niet geldig");
			Voornaam = voornaam;
		}

		internal void ZetAchternaam(string achternaam)
		{
			ControlleerInput(achternaam, "Achternaam is niet geldig");
			Achternaam = achternaam;
		}

		internal void ZetEmail(string email)
		{
			ControlleerInput(email, "Email is niet geldig");
			try
			{
				new MailAddress(email);
			} catch (Exception)
			{
				throw new EmailException("Email is niet in het een correct formaat");
			}
			Email = email;
		}

		internal void ZetBedrijf(string bedrijf)
		{
			ControlleerInput(bedrijf, "Bedrijf is niet geldig");
			Bedrijf = bedrijf;
		}

		internal void ZetWerknemer(WerknemerMetFunctieDTO werknemer)
		{
			ControlleerInput(werknemer, "Werknemer is niet geldig");
			Werknemer = werknemer;
		}

		private void ControlleerInput(object input, string errorMessage)
		{
			if (string.IsNullOrWhiteSpace(input?.ToString()))
			{
				throw new(errorMessage);
			}
		}
	}
}