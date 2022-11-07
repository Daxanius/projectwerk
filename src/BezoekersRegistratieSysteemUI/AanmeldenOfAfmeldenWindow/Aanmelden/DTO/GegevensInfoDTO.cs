using BezoekersRegistratieSysteemUI.AanmeldenOfAfmeldenWindow.Aanmelden.DTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using System;
using System.Net.Mail;

namespace BezoekersRegistratieSysteemUI.Aanmelden.DTO {

	internal class NieuweAfspraakGegevensDTO {

		/// <summary>
		/// Voornaam van Bezoeker
		/// </summary>
		internal string Voornaam { get; set; }

		/// <summary>
		/// Achternaam van bezoeker
		/// </summary>
		internal string Achternaam { get; set; }

		/// <summary>
		/// Email van bezoeker
		/// </summary>
		internal string Email { get; set; }

		/// <summary>
		/// Bedrijf van bezoeker
		/// </summary>
		internal string Bedrijf { get; set; }

		/// <summary>
		/// Werknemer waar de bezoeker een afspraak bij heeft
		/// </summary>
		internal WerknemerMetFunctieDTO Werknemer { get; set; }

		/// <summary>
		/// Er gaat een nieuw NieuweAfspraakGegevensDTO aangemaakt worden
		/// </summary>
		/// <param name="voornaam">Voornaam van Bezoeker</param>
		/// <param name="achternaam">Achternaam van bezoeker</param>
		/// <param name="email">Email van bezoeker</param>
		/// <param name="bedrijf">Bedrijf van bezoeker</param>
		/// <param name="werknemer">Werknemer waar de bezoeker een afspraak bij heeft</param>
		public NieuweAfspraakGegevensDTO(string voornaam, string achternaam, string email, string bedrijf, WerknemerMetFunctieDTO werknemer) {
			ZetVoornaam(voornaam);
			ZetAchternaam(achternaam);
			ZetEmail(email);
			ZetBedrijf(bedrijf);
			ZetWerknemer(werknemer);
		}

		/// <summary>
		/// Zet voornaam van bezoeker
		/// </summary>
		/// <param name="voornaam">Voornaam van bezoeker</param>
		internal void ZetVoornaam(string voornaam) {
			ControlleerInput(voornaam, "Voornaam is niet geldig");
			Voornaam = voornaam;
		}

		/// <summary>
		/// Zet achternaam van bezoeker
		/// </summary>
		/// <param name="achternaam">Achternaam van bezoeker</param>
		internal void ZetAchternaam(string achternaam) {
			ControlleerInput(achternaam, "Achternaam is niet geldig");
			Achternaam = achternaam;
		}

		/// <summary>
		/// Zet email van bezoeker
		/// </summary>
		/// <param name="email">Email van bezoeker</param>
		/// <exception cref="EmailException"></exception>
		internal void ZetEmail(string email) {
			ControlleerInput(email, "Email is niet geldig");
			try {
				new MailAddress(email);
			} catch (Exception) {
				throw new EmailException("Email is niet in het een correct formaat");
			}
			Email = email;
		}

		/// <summary>
		/// Zet bedrijf van bezoeker
		/// </summary>
		/// <param name="bedrijf">Bedrijf van bezoeker</param>
		internal void ZetBedrijf(string bedrijf) {
			ControlleerInput(bedrijf, "Bedrijf is niet geldig");
			Bedrijf = bedrijf;
		}

		/// <summary>
		/// Zet werknemer
		/// </summary>
		/// <param name="werknemer">Werknemer van het gekozen bedrijf</param>
		internal void ZetWerknemer(WerknemerMetFunctieDTO werknemer) {
			ControlleerInput(werknemer, "Werknemer is niet geldig");
			Werknemer = werknemer;
		}

		/// <summary>
		/// Controlleer Input op Null of Whitespace
		/// </summary>
		/// <param name="input">Input object</param>
		/// <param name="errorMessage">Error message als het null of whitespace is</param>
		private void ControlleerInput(object input, string errorMessage) {
			if (string.IsNullOrWhiteSpace(input?.ToString())) {
				throw new(errorMessage);
			}
		}
	}
}