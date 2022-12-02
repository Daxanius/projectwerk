using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BezoekersRegistratieSysteemBL {

	public static class Nutsvoorziening {

        /// <summary>
        /// Private lokale variabele van het datatype Regex met enkel lees rechten.
        /// </summary>
        private static readonly Regex RegexWhitespace = new(@"\s+");

        /// <summary>
        /// Private lokale variabele van het datatype Regex met enkel lees rechten.
        /// </summary>
        private static readonly Regex RegexBtw = new("^[A-Za-z]{2,4}(?=.{2,12}$)[-_\\s0-9]*(?:[a-zA-Z][-_\\s0-9]*){0,2}$");

        /// <summary>
        /// Private lokale variabele van het datatype Regex met enkel lees rechten.
        /// </summary>
        private static readonly Regex RegexEmail = new(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Private lokale variabele van het datatype Regex met enkel lees rechten.
        /// </summary>
        private static readonly Regex RegexTelefoonnummer = new(@"^\+?[0-9]{1,15}$");

        /// <summary>
        /// Private lokale variabele van het datatype Regex met enkel lees rechten.
        /// </summary>
        private static readonly List<Regex> RegexNummerplaat = new()
		{
            new(@"^CD[ABFJNST]\d{1,3}$"),			// 0	 DIPLOMATEN
            new(@"^([A-Z]{2})(\d{2})(\d{2})$"),		// 1     XX-99-99    (since 1951)
            new(@"^(\d{2})(\d{2})([A-Z]{2})$"),		// 2     99-99-XX    (since 1965)
            new(@"^(\d{2})([A-Z]{2})(\d{2})$"),		// 3     99-XX-99    (since 1973)
            new(@"^([A-Z]{2})(\d{2})([A-Z]{2})$"),	// 4     XX-99-XX    (since 1978)
            new(@"^([A-Z]{2})([A-Z]{2})(\d{2})$"),	// 5     XX-XX-99    (since 1991)
            new(@"^(\d{2})([A-Z]{2})([A-Z]{2})$"),	// 6     99-XX-XX    (since 1999)
            new(@"^(\d{2})([A-Z]{3})(\d{1})$"),		// 7     99-XXX-9    (since 2005)
            new(@"^(\d{1})([A - Z]{3})(\d{2})$"),	// 8     9-XXX-99    (since 2009)
            new(@"^([A-Z]{2})(\d{3})([A-Z]{1})$"),	// 9     XX-999-X    (since 2006)
            new(@"^([A-Z]{1})(\d{3})([A-Z]{2})$"),	// 10    X-999-XX    (since 2008)
            new(@"^([A-Z]{3})(\d{2})([A-Z]{1})$"),	// 11    XXX-99-X    (since 2015)
            new(@"^([A-Z]{1})(\d{2})([A-Z]{3})$"),	// 12    X-99-XXX
            new(@"^(\d{1})([A-Z]{2})(\d{3})$"),		// 13    9-XX-999
            new(@"^(\d{3})([A-Z]{2})(\d{1})$"),		// 14    999-XX-9
            new(@"^(\d{3})(\d{2})([A-Z]{1})$"),		//       999-99-X
            new(@"^([A-Z]{3})(\d{2})(\d{1})$"),		//       XXX-99-9
            new(@"^([A-Z]{3})([A-Z]{2})(\d{1})$")	//       XXX-XX-9
        };

        /// <summary>
        /// Verwijdert alle whitespace van een string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Opgemaakte tekst</returns>
        public static string VerwijderWhitespace(string input) {
			return RegexWhitespace.Replace(input, string.Empty);
		}

        /// <summary>
        /// Verwijdert buitenstaande whitespaces en spaties die teveel zijn tussen woorden.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Opgemaakte tekst</returns>
        public static string NaamOpmaak(string input) {
			string aangepasteInput = RegexWhitespace.Replace(input, " ").Trim();
            aangepasteInput = $"{char.ToUpper(aangepasteInput[0])}{aangepasteInput[1..].ToLower()}";
			return Regex.Replace(aangepasteInput, @"\b\w", (Match match) => match.ToString().ToUpper());
		}

        /// <summary>
        /// Controleert een BTW nummer op notatie.
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
		/// Controleert of een BTW nummer bestaat, als de service plat ligt returnt
		/// false voor IsGecontroleert, maar de notatie is wel gecontroleert.
		/// </summary>
		/// <param name="btwNummer"></param>
		/// <returns>is gecontroleert bool, btwinfo resultaat</returns>
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

		/// <summary>
		/// Controleert of het telefoonnumer in het correcte formaat is.
		/// </summary>
		/// <param name="nummer"></param>
		/// <returns></returns>
		public static bool IsTelefoonnummerGeldig(string nummer) {
			if (string.IsNullOrWhiteSpace(nummer))
				return false;

			return RegexTelefoonnummer.IsMatch(VerwijderWhitespace(nummer));
		}

        /// <summary>
		/// Controleert of de nummerplaat in het correcte formaat is.
		/// </summary>
		/// <param name="nummerplaat"></param>
		/// <returns></returns>
        public static bool IsNummerplaatGeldig(string nummerplaat)
        {
            if (string.IsNullOrWhiteSpace(nummerplaat))
                return false;

            nummerplaat = VerwijderWhitespace(nummerplaat).ToUpper();

            foreach (Regex regex in RegexNummerplaat)
            {
                if (regex.IsMatch(nummerplaat))
                    return true;
            }

            return false;
        }
    }
}