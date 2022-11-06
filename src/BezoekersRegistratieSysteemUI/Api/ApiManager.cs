using BezoekersRegistratieSysteemUI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Api {
	public static class ApiController {
		private static TimeSpan _timeout = TimeSpan.FromSeconds(500d);

		public const string _baseAddres = "http://localhost:5049/api/";
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
	}
}
