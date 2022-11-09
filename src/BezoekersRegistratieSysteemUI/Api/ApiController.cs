﻿using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
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
		public static async Task<(bool, T?)> GetMetBody<T>(string url, string body, string defaultFoutMelding = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{_baseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				var request = new HttpRequestMessage {
					Method = HttpMethod.Get,
					RequestUri = new Uri(apiUrl),
					Content = new StringContent(body, Encoding.UTF8, "application/json"),
				};

				HttpResponseMessage response = await client.SendAsync(request);

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

		public static async Task<bool> Post(string url, string json = "", string defaultFoutMelding = "") {
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

				return true;
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

		public static AfspraakDTO PostAfspraak(AfspraakInputDTO afspraak) {
			return Task.Run(async () => {
				string body = JsonConvert.SerializeObject(afspraak);

				(bool isvalid, AfspraakOutputDTO apiAfspraken) = await Post<AfspraakOutputDTO>($"afspraak", body);
				if (isvalid) {
					WerknemerDTO werknemer = new WerknemerDTO(apiAfspraken.Werknemer.Id, apiAfspraken.Werknemer.Naam.Split(";")[0], apiAfspraken.Werknemer.Naam.Split(";")[1], null);
					BezoekerDTO bezoeker = new BezoekerDTO(apiAfspraken.Bezoeker.Id, apiAfspraken.Bezoeker.Naam.Split(";")[0], apiAfspraken.Bezoeker.Naam.Split(";")[1], apiAfspraken.Bezoeker.Email, apiAfspraken.Bezoeker.BezoekerBedrijf);
					return new AfspraakDTO(apiAfspraken.Id, bezoeker, apiAfspraken.Bezoeker.BezoekerBedrijf, werknemer, apiAfspraken.Starttijd, apiAfspraken.Eindtijd);
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het toevoegen van het bedrijf");
				}
			}).Result;
		}

		public static IEnumerable<WerknemerDTO> FetchWerknemersVanBedrijf(BedrijfDTO bedrijf) {
			return Task.Run(async () => {
				List<WerknemerDTO> ItemSource = new();
				(bool isvalid, List<WerknemerOutputDTO> apiWerknemers) = await Get<List<WerknemerOutputDTO>>($"bedrijf/{bedrijf.Id}/werknemer");
				if (isvalid) {
					apiWerknemers.ForEach((api) => {
						List<WerknemerInfoDTO> lijstWerknemerInfo = new(api.WerknemerInfo.Select(w => new WerknemerInfoDTO(bedrijf, w.Email, w.Functies)).ToList());
						WerknemerInfoOutputDTO werknemerInfo = api.WerknemerInfo.First(w => w.Bedrijf.Id == bedrijf.Id);
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
				(bool isvalid, List<BezoekerOutputDTO> apiBezoekers) = await Get<List<BezoekerOutputDTO>>($"afspraak/bedrijf/{bedrijfsId}?datum={datum.ToString("M/dd/yyyy")}");
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

		public static IEnumerable<BedrijfDTO> FetchBedrijven() {
			return Task.Run(async () => {
				List<BedrijfDTO> _bedrijven = new();
				(bool isvalid, List<BedrijfOutputDTO> apiBedrijven) = await Get<List<BedrijfOutputDTO>>($"bedrijf");
				if (isvalid) {
					apiBedrijven.ForEach(api => {
						_bedrijven.Add(new BedrijfDTO(api.Id, api.Naam, api.BTW, api.TelefoonNummer, api.Email, api.Adres));
					});
					return _bedrijven;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de bedrijven");
				}
			}).Result;
		}

		public static IEnumerable<AfspraakDTO> FetchBezoekerAfsprakenVanBedrijf(long bedrijfsId, BezoekerDTO bezoeker, DateTime? dag = null) {
			return Task.Run(async () => {

				List<AfspraakDTO> ItemSource = new();
				string payload = JsonConvert.SerializeObject(bezoeker);
				string dagString = (dag is not null ? dag.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy")).ToString();
				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await GetMetBody<List<AfspraakOutputDTO>>($"afspraak/bedrijf/{bedrijfsId}/bezoeker/{bezoeker.Id}/all?dag={dagString}", payload);

				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd));
					});

					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}

		public static IEnumerable<AfspraakDTO> FetchWerknemerAfsprakenVanBedrijf(long bedrijfsId, WerknemerDTO werknemer, DateTime? datum = null, bool alleenLopendeAfspraken = false) {
			return Task.Run(async () => {
				List<AfspraakDTO> ItemSource = new();

				if (!datum.HasValue)
					datum = DateTime.Now;

				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>($"afspraak?dag={datum.Value.ToString("MM/dd/yyyy")}&werknemerId={werknemer.Id}&bedrijfId={bedrijfsId}&openstaand={alleenLopendeAfspraken}");

				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}

		public static IEnumerable<AfspraakDTO> FetchAfsprakenOpDatumVanBedrijf(long bedrijfsId, string datum) {
			return Task.Run(async () => {
				List<AfspraakDTO> ItemSource = new();

				if (string.IsNullOrWhiteSpace(datum))
					datum = DateTime.Now.ToString("MM/dd/yyyy");
				else
					datum = DateTime.Parse(datum).ToString("MM/dd/yyyy");

				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>($"afspraak?dag={datum}&bedrijfId={bedrijfsId}");

				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}

		public static IEnumerable<AfspraakDTO> FetchAfspraken() {
			return Task.Run(async () => {
				List<AfspraakDTO> alleAfspraken = new();

				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>("afspraak?dag=" + DateTime.Now.ToString("MM/dd/yyy"));
				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						alleAfspraken.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd));
					});
					return alleAfspraken;
				} else
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van alle afspraaken");
			}).Result;
		}

		#endregion

		#region Bedrijf
		public static BedrijfOutputDTO? FetchBedrijf(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, BedrijfOutputDTO bedrijf) = await Get<BedrijfOutputDTO>($"bedrijf/{bedrijfId}");
				if (isvalid) {
					return bedrijf;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van het bedrijf");
				}
			}).Result;
		}

		public static BedrijfDTO PostBedrijf(BedrijfInputDTO bedrijf) {
			return Task.Run(async () => {
				string body = JsonConvert.SerializeObject(bedrijf);
				(bool isvalid, BedrijfOutputDTO apiBedrijf) = await Post<BedrijfOutputDTO>($"bedrijf/", body);
				if (isvalid) {
					return new BedrijfDTO(apiBedrijf.Id, apiBedrijf.Naam, apiBedrijf.BTW, apiBedrijf.TelefoonNummer, apiBedrijf.Email, apiBedrijf.Adres);
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het toevoegen van het bedrijf");
				}
			}).Result;
		}
		#endregion

		#region Werknemer
		public static WerknemerOutputDTO? FetchWerknemer(long werknemerId) {
			return Task.Run(async () => {
				(bool isvalid, WerknemerOutputDTO werknemer) = await Get<WerknemerOutputDTO>($"werknemer/{werknemerId}");
				if (isvalid) {
					return werknemer;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemer");
				}
			}).Result;
		}

		public static IEnumerable<WerknemerOutputDTO>? FetchWerknemersVanBedrijf(long bedrijfId, string naam, string achternaam) {
			return Task.Run(async () => {
				(bool isvalid, IEnumerable<WerknemerOutputDTO> werknemers) = await Get<IEnumerable<WerknemerOutputDTO>>($"werknemer/{naam}/{achternaam}/bedrijf/{bedrijfId}");
				if (isvalid) {
					return werknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}

		public static WerknemerOutputDTO? PostWerknemer(WerknemerInputDTO werknemer) {
			return Task.Run(async () => {
				string body = JsonConvert.SerializeObject(werknemer);
				(bool isvalid, WerknemerOutputDTO apiWerknemers) = await Post<WerknemerOutputDTO>($"werknemer/", body);
				if (isvalid) {
					return apiWerknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}

		public static IEnumerable<WerknemerOutputDTO>? FetchWerknemersOpFunctie(long bedrijfId, string functie) {
			return Task.Run(async () => {
				(bool isvalid, IEnumerable<WerknemerOutputDTO> werknemers) = await Get<IEnumerable<WerknemerOutputDTO>>($"werknemer/bedrijf/{bedrijfId}/functie/{functie}");
				if (isvalid) {
					return werknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}

		public static IEnumerable<WerknemerOutputDTO>? FetchVrijeWerknemersVanBedrijf(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, IEnumerable<WerknemerOutputDTO> werknemers) = await Get<IEnumerable<WerknemerOutputDTO>>($"werknemer/bedrijf/vb/{bedrijfId}");
				if (isvalid) {
					return werknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}

		public static WerknemerOutputDTO? PostWerknemerInfo(long werknemerId, WerknemerInfoInputDTO werknemerInfo) {
			return Task.Run(async () => {
				string body = JsonConvert.SerializeObject(werknemerInfo);
				(bool isvalid, WerknemerOutputDTO werknemerOutput) = await Post<WerknemerOutputDTO>($"werknemer/{werknemerId}/info", body);
				if (isvalid) {
					return werknemerOutput;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het toevoegen van de werknemer");
				}
			}).Result;
		}
		#endregion
	}
}