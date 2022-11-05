using BezoekersRegistratieSysteemUI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Api {
	public static class ApiController {
		public const string _baseAddres = "http://localhost:5049/api/";
		public static async Task<(bool, T?)> Fetch<T>(string url, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = TimeSpan.FromSeconds(10d);

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
		public static async Task<(bool, T?)> Post<T>(string url, string json, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = TimeSpan.FromSeconds(10d);

				HttpResponseMessage response = await client.PostAsync(apiUrl, new StringContent(json));

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
	}
}
