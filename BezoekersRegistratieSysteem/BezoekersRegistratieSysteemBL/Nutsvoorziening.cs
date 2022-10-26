using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Exceptions;
using BezoekersRegistratieSysteemBL.Exceptions.DomeinException;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BezoekersRegistratieSysteemBL
{
    public static class Nutsvoorziening
    {
        /// <summary>
        /// Controleert de geldigheid van het btwnummer.
        /// </summary>
        /// <param name="btwNummer"></param>
        public static (bool, BtwInfoDTO?) ControleerBTWNummer(string btwNummer)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(btwNummer))
                {
                    throw new BtwControleException("Btwnummer is leeg");
                }

                if (btwNummer.Length <= 2 || btwNummer.Length > 20)
                {
                    throw new BtwControleException("Europees btwnummer moet minstens 3 karakters lang en niet groter dan 20 characters lang zijn");
                }

                string apiUrl = $"https://controleerbtwnummer.eu/api/validate/{btwNummer}.json";

                using HttpClient client = new();
                // Om ervoor te zorgen dat we niet voor elk bedrijf lang moeten wachten
                client.Timeout = TimeSpan.FromSeconds(10d);
                HttpResponseMessage response = client.GetAsync(apiUrl).Result;

                // Als we geen geldige response hebben gekregen, AKA, de API ligt plat
                if (!response.IsSuccessStatusCode)
                {
                    // TODO: aanpassen deze handel
                    throw new BtwControleException("ControleerBTWNummer - Response: " + response.StatusCode);
                }

                string responseBody = response.Content.ReadAsStringAsync().Result;

                BtwInfoDTO? btwInfo = JsonConvert.DeserializeObject<BtwInfoDTO>(responseBody);

                if (btwInfo is null)
                {
                    throw new BtwControleException("ControleerBTWNummer - btw is null");
                }

                if (!btwInfo.Valid)
                    return (false, null);
                return (true, btwInfo);
            }
            catch (Exception ex)
            {
                if (ex is BtwControleException)
                    throw (BtwControleException)ex;
                return (false, null);
            }
        }

        /// <summary>
        /// Controleert het formaat van het emailadres.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmailGeldig(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new EmailControleException("email is leeg");
            //Email Authenticator
            string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            //

            if (regex.IsMatch(email))
            {
                //Email is geldig
                return true;
            }
            else
            {
                //Email is ongeldig
                return false;
            }
        }
    }
}
