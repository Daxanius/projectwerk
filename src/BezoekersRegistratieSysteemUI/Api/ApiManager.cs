using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI.Api {
	public static class ApiController {
		private static TimeSpan _timeout = TimeSpan.FromSeconds(500d);

		public const string _baseAddres = "http://localhost:5049/api/";
		#region Request Methods

		public static async Task<(bool, T?)> Get<T>(string url, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException("De request is niet gelukt: statuscode: " + response.StatusCode);
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				if (defaultFoutMelding != "")
					throw new FetchApiException(defaultFoutMelding, ex);
				throw new FetchApiException(ex.Message);
			}
		}
		public static async void Get(string url, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException("De request is niet gelukt: statuscode: " + response.StatusCode);
				}
			} catch (Exception ex) {
				if (defaultFoutMelding != "")
					throw new FetchApiException(defaultFoutMelding, ex);
				throw new FetchApiException(ex.Message);
			}
		}

		public static async Task<(bool, T?)> Put<T>(string url, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.PutAsync(apiUrl, null);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException("De request is niet gelukt: statuscode: " + response.StatusCode);
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				if (defaultFoutMelding != "")
					throw new FetchApiException(defaultFoutMelding, ex);
				throw new FetchApiException(ex.Message);
			}
		}
		public static async Task<(bool, T?)> Put<T>(string url, string json, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, json);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException("De request is niet gelukt: statuscode: " + response.StatusCode);
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				if (defaultFoutMelding != "")
					throw new FetchApiException(defaultFoutMelding, ex);
				throw new FetchApiException(ex.Message);
			}
		}
		public static async void Put(string url, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.PutAsync(apiUrl, null);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException("De request is niet gelukt: statuscode: " + response.StatusCode);
				}
			} catch (Exception ex) {
				if (defaultFoutMelding != "")
					throw new FetchApiException(defaultFoutMelding, ex);
				throw new FetchApiException(ex.Message);
			}
		}
		public static async void Put(string url, string json, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, json);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException("De request is niet gelukt: statuscode: " + response.StatusCode);
				}
			} catch (Exception ex) {
				if (defaultFoutMelding != "")
					throw new FetchApiException(defaultFoutMelding, ex);
				throw new FetchApiException(ex.Message);
			}
		}

		public static async Task<(bool, T?)> Post<T>(string url, string json, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				var response = await client.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json"));

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException($"De request is niet gelukt!\n\nError: {response.Content.ReadAsStringAsync().Result}\n\nStatuscode: " + response.StatusCode);
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				if (defaultFoutMelding != "")
					throw new FetchApiException(defaultFoutMelding, ex);
				throw new FetchApiException(ex.Message);
			}
		}

		#endregion

		#region Fetch Methods

		public static IEnumerable<AfspraakDTO> FetchAfsprakenVanBedrijf(long bedrijfsId) {
			return Task.Run(async () => {
				List<AfspraakDTO> ItemSource = new();
				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>($"afspraak?bedrijfId={bedrijfsId}&openstaand=true");
				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}

		public static IEnumerable<WerknemerDTO> FetchWerknemersVanBedrijf(long bedrijfsId) {
			return Task.Run(async () => {
				List<WerknemerDTO> ItemSource = new();
				(bool isvalid, List<WerknemerOutputDTO> apiWerknemers) = await Get<List<WerknemerOutputDTO>>($"werknemer/bedrijf/id/{bedrijfsId}");
				if (isvalid) {
					apiWerknemers.ForEach((api) => {
						List<WerknemerInfoDTO> lijstWerknemerInfo = new(api.WerknemerInfo.Select(w => new WerknemerInfoDTO(BeheerderWindow.GeselecteerdBedrijf, w.Email, w.Functies)).ToList());
						WerknemerInfoOutputDTO werknemerInfo = api.WerknemerInfo.First(w => w.Bedrijf.Id == BeheerderWindow.GeselecteerdBedrijf.Id);
						ItemSource.Add(new WerknemerDTO(api.Id, api.Voornaam, api.Achternaam, werknemerInfo.Email, werknemerInfo.Functies, true));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}

		public static IEnumerable<BezoekerDTO> FetchBezoekersVanBedrijf(long bedrijfsId, DateTime datum) {
			return Task.Run(async () => {
				List<BezoekerDTO> ItemSource = new();
				(bool isvalid, List<BezoekerOutputDTO> apiBezoekers) = await Get<List<BezoekerOutputDTO>>($"afspraak/bezoekers/{bedrijfsId}?datum={datum.ToString("MM/dd/yyyy")}");
				if (isvalid) {
					apiBezoekers.ForEach((api) => {
						ItemSource.Add(new BezoekerDTO(api.Id, api.Voornaam, api.Achternaam, api.Email, api.Bedrijf));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}

		#endregion
	}
}
