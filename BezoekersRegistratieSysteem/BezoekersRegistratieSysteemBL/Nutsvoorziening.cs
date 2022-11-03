using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemBL.Managers;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BezoekersRegistratieSysteemBL {

	public static class Nutsvoorziening {

        public static readonly Regex RegexWhitespace = new(@"\s+");
		public static readonly Regex RegexBtw = new("^[A-Za-z]{2,4}(?=.{2,12}$)[-_\\s0-9]*(?:[a-zA-Z][-_\\s0-9]*){0,2}$");
		public static readonly Regex RegexEmail = new(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);
		public static readonly Regex RegexTelefoonNummer = new(@"^\+?[0-9]{1,15}$");

		/// <summary>
		/// Verwijdert alle whitespace van een string
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string VerwijderWhitespace(string input) {
			return RegexWhitespace.Replace(input, string.Empty);
		}

		/// <summary>
		/// Controleert een BTW nummer op notatie
		/// </summary>
		/// <param name="btwNummer"></param>
		/// <returns></returns>
		public static bool ControleerBTWNummer(string btwNummer) {
			if (string.IsNullOrWhiteSpace(btwNummer)) {
				return false;
			}

			return RegexBtw.Match(VerwijderWhitespace(btwNummer)).Success;
		}

		/// <summary>
		/// Controleert of een BTW nummer bestaat, als de service plat ligt
		/// controleert of het nummer een geldige notatie heeft, geeft informatie
		/// over het nummer terug
		/// </summary>
		/// <param name="btwNummer"></param>
		/// <returns>notatie valid bool, btwinfo resultaat</returns>
		/// <exception cref="BtwControleException"></exception>
		public static (bool, DTOBtwInfo?) GeefBTWInfo(string btwNummer) {
			if (string.IsNullOrWhiteSpace(btwNummer)) {
				throw new BtwControleException("ControleerBTWNummer - btw is leeg");
			}

			btwNummer = VerwijderWhitespace(btwNummer);
			if (!ControleerBTWNummer(btwNummer)) {
				throw new BtwControleException("ControleerBTWNummer - notatie is niet geldig");
			}

			try {
				string apiUrl = $"https://controleerbtwnummer.eu/api/validate/{btwNummer}.json";

				using HttpClient client = new();
				// Om ervoor te zorgen dat we niet voor elk bedrijf lang moeten wachten
				client.Timeout = TimeSpan.FromSeconds(10d);
				HttpResponseMessage response = client.GetAsync(apiUrl).Result;

				// Als we geen geldige response hebben gekregen, AKA, de API ligt plat
				if (!response.IsSuccessStatusCode) {
					return (true, null);
				}

				string responseBody = response.Content.ReadAsStringAsync().Result;

				DTOBtwInfo? btwInfo = JsonConvert.DeserializeObject<DTOBtwInfo>(responseBody);

				if (btwInfo is null) {
					throw new BtwControleException("ControleerBTWNummer - kon btwInfo niet deserialiseren");
				}

				if (!btwInfo.Valid)
					return (false, null);
				return (true, btwInfo);
			} catch (Exception ex) {
				throw new BtwControleException(ex.Message);
			}
		}

		/// <summary>
		/// Controleert het formaat van het emailadres.
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public static bool IsEmailGeldig(string email) {
			if (string.IsNullOrWhiteSpace(email))
				return false;

			//Email Authenticator
			return RegexEmail.IsMatch(email.Trim());
		}

		public static bool IsTelefoonNummerGeldig(string nummer) {
			if (string.IsNullOrWhiteSpace(nummer))
				return false;

			return RegexTelefoonNummer.IsMatch(VerwijderWhitespace(nummer));
		}
    }
}