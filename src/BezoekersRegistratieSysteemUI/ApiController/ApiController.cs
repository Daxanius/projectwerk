﻿using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Exceptions;
using BezoekersRegistratieSysteemUI.Model;
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

		public static string BaseAddres = "http://localhost:5049/api/";

		#region Request Methods

		public static async Task<(bool, T?)> Get<T>(string url) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}
		public static async Task<(bool, T?)> GetMetBody<T>(string url, string body) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				var request = new HttpRequestMessage {
					Method = HttpMethod.Get,
					RequestUri = new Uri(apiUrl),
					Content = new StringContent(body, Encoding.UTF8, "application/json"),
				};

				HttpResponseMessage response = await client.SendAsync(request);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}
		public static async void Get(string url) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}

		public static async Task<(bool, T?)> Put<T>(string url) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.PutAsync(apiUrl, null);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}
		public static async Task<(bool, T?)> Put<T>(string url, string json) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.PutAsJsonAsync(apiUrl, json);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}
		public static async Task Put(string url) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.PutAsync(apiUrl, null);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}
		public static async Task Put(string url, string json) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PutAsync(apiUrl, httpContent);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}

		public async static Task Delete(string url) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				HttpResponseMessage response = await client.DeleteAsync(apiUrl);

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}

		public static async Task<(bool, T?)> Post<T>(string url, string json) {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				var response = await client.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json"));

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}

				string responseBody = await response.Content.ReadAsStringAsync();

				T? parsed = JsonConvert.DeserializeObject<T?>(responseBody);

				if (parsed is T) {
					return (true, parsed);
				} else {
					return (false, parsed);
				}
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}
		public static async Task<bool> Post(string url, string json = "") {
			try {
				if (url.Length > 1 && url[0] == '/') {
					url = url[1..];
				}

				string apiUrl = $"{BaseAddres}{url}";

				using HttpClient client = new();
				client.Timeout = _timeout;

				var response = await client.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json"));

				if (!response.IsSuccessStatusCode) {
					throw new FetchApiException(response.Content.ReadAsStringAsync().Result.Split("-", StringSplitOptions.RemoveEmptyEntries).Last().Trim());
				}

				return true;
			} catch (Exception ex) {
				throw new FetchApiException(ex.Message, ex.InnerException);
			}
		}

		#endregion

		#region Bedrijven
		public static BedrijfDTO MaakBedrijf(BedrijfInputDTO bedrijf) {
			return Task.Run(async () => {
				string body = JsonConvert.SerializeObject(bedrijf);
				(bool isvalid, BedrijfOutputDTO apiBedrijf) = await Post<BedrijfOutputDTO>($"bedrijf", body);
				if (isvalid) {
					return new BedrijfDTO(apiBedrijf.Id, apiBedrijf.Naam, apiBedrijf.BTW, apiBedrijf.TelefoonNummer, apiBedrijf.Email, apiBedrijf.Adres);
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het toevoegen van het bedrijf");
				}
			}).Result;
		}
		public static BedrijfOutputDTO? GeefBedrijf(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, BedrijfOutputDTO bedrijf) = await Get<BedrijfOutputDTO>($"bedrijf/{bedrijfId}");
				if (isvalid) {
					return bedrijf;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van het bedrijf");
				}
			}).Result;
		}
		public static IEnumerable<BezoekerDTO> GeefBezoekersVanBedrijf(long bedrijfsId, DateTime datum) {
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
		public static IEnumerable<BedrijfDTO> GeefBedrijven() {
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
		public static IEnumerable<AfspraakDTO> GeefBezoekerAfsprakenVanBedrijf(long bedrijfsId, BezoekerDTO bezoeker, DateTime? dag = null) {
			return Task.Run(async () => {

				List<AfspraakDTO> ItemSource = new();
				string payload = JsonConvert.SerializeObject(bezoeker);
				string dagString = (dag is not null ? dag.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy")).ToString();
				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await GetMetBody<List<AfspraakOutputDTO>>($"afspraak/bedrijf/{bedrijfsId}/bezoeker/{bezoeker.Id}/all?dag={dagString}", payload);

				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd, api.StatusNaam));
					});

					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}
		#endregion

		#region Werknemers
		public static WerknemerDTO MaakWerknemer(WerknemerInputDTO werknemer) {
			return Task.Run(async () => {
				string body = JsonConvert.SerializeObject(werknemer);
				(bool isvalid, WerknemerOutputDTO apiWerknemer) = await Post<WerknemerOutputDTO>($"werknemer", body);
				if (isvalid) {
					List<WerknemerInfoDTO> lijstWerknemerInfo = apiWerknemer.WerknemerInfo.Select(w => new WerknemerInfoDTO(BeheerderWindow.GeselecteerdBedrijf, w.Email, w.Functies)).ToList();
					string emailVanGeselecteerdBedrijf = lijstWerknemerInfo.First(w => w.Bedrijf.Id == BeheerderWindow.GeselecteerdBedrijf.Id).Email;
					WerknemerDTO werknemer = new WerknemerDTO(apiWerknemer.Id, apiWerknemer.Voornaam, apiWerknemer.Achternaam, lijstWerknemerInfo);
					werknemer.Email = emailVanGeselecteerdBedrijf;
					return werknemer;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}
		public static WerknemerOutputDTO? MaakWerknemerInfo(long werknemerId, WerknemerInfoInputDTO werknemerInfo) {
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
		public static async Task<WerknemerDTO?> GeefWerknemer(long werknemerId) {
			(bool isvalid, WerknemerOutputDTO apiWerknemer) = await Get<WerknemerOutputDTO>($"werknemer/{werknemerId}");
			if (isvalid) {
				List<WerknemerInfoDTO> lijstWerknemerInfo = apiWerknemer.WerknemerInfo.Select(w => new WerknemerInfoDTO(BeheerderWindow.GeselecteerdBedrijf, w.Email, w.Functies)).ToList();
				string emailVanGeselecteerdBedrijf = lijstWerknemerInfo.First(w => w.Bedrijf.Id == BeheerderWindow.GeselecteerdBedrijf.Id).Email;
				WerknemerDTO werknemer = new WerknemerDTO(apiWerknemer.Id, apiWerknemer.Voornaam, apiWerknemer.Achternaam, lijstWerknemerInfo);
				werknemer.Email = emailVanGeselecteerdBedrijf;
				return werknemer;
			} else {
				throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemer");
			}
		}
		public static IEnumerable<WerknemerOutputDTO>? GeefWerknemersVanBedrijf(long bedrijfId, string naam, string achternaam) {
			return Task.Run(async () => {
				(bool isvalid, IEnumerable<WerknemerOutputDTO> werknemers) = await Get<IEnumerable<WerknemerOutputDTO>>($"werknemer/{naam}/{achternaam}/bedrijf/{bedrijfId}");
				if (isvalid) {
					return werknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}
		public static IEnumerable<WerknemerOutputDTO>? GeefWerknemersOpFunctie(long bedrijfId, string functie) {
			return Task.Run(async () => {
				(bool isvalid, IEnumerable<WerknemerOutputDTO> werknemers) = await Get<IEnumerable<WerknemerOutputDTO>>($"werknemer/bedrijf/{bedrijfId}/functie/{functie}");
				if (isvalid) {
					return werknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}
		public static IEnumerable<WerknemerOutputDTO>? GeefVrijeWerknemersVanBedrijf(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, IEnumerable<WerknemerOutputDTO> werknemers) = await Get<IEnumerable<WerknemerOutputDTO>>($"werknemer/bedrijf/vb/{bedrijfId}");
				if (isvalid) {
					return werknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}
		public static IEnumerable<BezoekerDTO> GeefAanwezigeBezoekers() {
			return Task.Run(async () => {
				List<BezoekerDTO> ItemSource = new();
				(bool isvalid, List<BezoekerOutputDTO> apiBezoekers) = await Get<List<BezoekerOutputDTO>>("afspraak/aanwezig");
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
		public static IEnumerable<WerknemerDTO> GeefWerknemersVanBedrijf(BedrijfDTO bedrijf) {
			return Task.Run(async () => {
				List<WerknemerDTO> werknemers = new();
				(bool isvalid, List<WerknemerOutputDTO> apiWerknemers) = await Get<List<WerknemerOutputDTO>>($"bedrijf/{bedrijf.Id}/werknemer");
				if (isvalid) {
					apiWerknemers.ForEach((api) => {
						List<WerknemerInfoDTO> lijstWerknemerInfo = new(api.WerknemerInfo.Select(w => new WerknemerInfoDTO(bedrijf, w.Email, w.Functies)).ToList());
						WerknemerInfoOutputDTO werknemerInfo = api.WerknemerInfo.First(w => w.Bedrijf.Id == bedrijf.Id);
						var werknemer = new WerknemerDTO(api.Id, api.Voornaam, api.Achternaam, werknemerInfo.Email, werknemerInfo.Functies, werknemerInfo.StatusNaam ?? "");
						werknemer.WerknemerInfoLijst = lijstWerknemerInfo;
						werknemers.Add(werknemer);
					});
					return werknemers;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de werknemers");
				}
			}).Result;
		}
		public static IEnumerable<AfspraakDTO> GeefWerknemerAfsprakenVanBedrijf(long bedrijfsId, WerknemerDTO werknemer, string? datum = null, bool alleenLopendeAfspraken = false) {
			return Task.Run(async () => {
				List<AfspraakDTO> ItemSource = new();

				if (datum is null)
					datum = DateTime.Now.ToString("MM/dd/yyyy");
				else
					datum = DateTime.Parse(datum).ToString("MM/dd/yyyy");

				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>($"afspraak?dag={datum}&werknemerId={werknemer.Id}&bedrijfId={bedrijfsId}&openstaand={alleenLopendeAfspraken}");

				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd, api.StatusNaam));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}
		public static async Task VerwijderBedrijf(long id) {
			await Delete($"bedrijf/{id}");
		}
		public static List<WerknemerDTO> BestaatWerknemerInPark(string voornaam, string achternaam) {
			return Task.Run(async () => {
				List<WerknemerDTO> ItemSource = new();
				(bool isvalid, List<WerknemerOutputDTO> apiWerknemers) = await Get<List<WerknemerOutputDTO>>($"werknemer/bestaat/{voornaam}/{achternaam}");
				if (isvalid) {
					apiWerknemers.ForEach((api) => {
						List<WerknemerInfoDTO> lijstWerknemerInfo = new(api.WerknemerInfo.Select(w => {
							BedrijfOutputDTO bedrijfOutputDTO = GeefBedrijf(w.Bedrijf.Id);
							BedrijfDTO bedrijf = new BedrijfDTO(bedrijfOutputDTO.Naam, bedrijfOutputDTO.BTW, bedrijfOutputDTO.TelefoonNummer, bedrijfOutputDTO.Email, bedrijfOutputDTO.Adres);
							return new WerknemerInfoDTO(bedrijf, w.Email, w.Functies);
						}).ToList());

						IEnumerable<WerknemerInfoDTO> werknemerInfo = api.WerknemerInfo.Select(w => {
							BedrijfOutputDTO bedrijfOutputDTO = GeefBedrijf(w.Bedrijf.Id);
							BedrijfDTO bedrijf = new BedrijfDTO(bedrijfOutputDTO.Naam, bedrijfOutputDTO.BTW, bedrijfOutputDTO.TelefoonNummer, bedrijfOutputDTO.Email, bedrijfOutputDTO.Adres);
							return new WerknemerInfoDTO(bedrijf, w.Email, w.Functies);
						});

						var werknemer = new WerknemerDTO(api.Id, api.Voornaam, api.Achternaam, werknemerInfo);
						werknemer.WerknemerInfoLijst = lijstWerknemerInfo;
						ItemSource.Add(werknemer);
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het controlleren of de werknemer al bestaat in het park");
				}
			}).Result;
		}
		public static WerknemerDTO VoegWerknemerFunctieToe(long werknemerId, WerknemerInfoInputDTO info) {
			return Task.Run(async () => {
				string payload = JsonConvert.SerializeObject(info);
				(bool isvalid, WerknemerOutputDTO apiWerknemer) = await Post<WerknemerOutputDTO>($"werknemer/{werknemerId}/info", payload);

				if (isvalid) {
					List<WerknemerInfoDTO> lijstWerknemerInfo = apiWerknemer.WerknemerInfo.Select(w => new WerknemerInfoDTO(BeheerderWindow.GeselecteerdBedrijf, w.Email, w.Functies)).ToList();
					string emailVanGeselecteerdBedrijf = lijstWerknemerInfo.First(w => w.Bedrijf.Id == BeheerderWindow.GeselecteerdBedrijf.Id).Email;
					WerknemerDTO werknemer = new WerknemerDTO(apiWerknemer.Id, apiWerknemer.Voornaam, apiWerknemer.Achternaam, lijstWerknemerInfo);
					werknemer.Email = emailVanGeselecteerdBedrijf;
					return werknemer;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}
		#endregion

		#region Afspraken
		public static AfspraakDTO MaakAfspraak(AfspraakInputDTO afspraak) {
			return Task.Run(async () => {
				string body = JsonConvert.SerializeObject(afspraak);

				(bool isvalid, AfspraakOutputDTO apiAfspraken) = await Post<AfspraakOutputDTO>($"afspraak", body);
				if (isvalid) {
					WerknemerDTO werknemer = new WerknemerDTO(apiAfspraken.Werknemer.Id, apiAfspraken.Werknemer.Naam.Split(";")[0], apiAfspraken.Werknemer.Naam.Split(";")[1], null);
					BezoekerDTO bezoeker = new BezoekerDTO(apiAfspraken.Bezoeker.Id, apiAfspraken.Bezoeker.Naam.Split(";")[0], apiAfspraken.Bezoeker.Naam.Split(";")[1], apiAfspraken.Bezoeker.Email, apiAfspraken.Bezoeker.BezoekerBedrijf);
					return new AfspraakDTO(apiAfspraken.Id, bezoeker, BeheerderWindow.GeselecteerdBedrijf.Naam, werknemer, apiAfspraken.Starttijd, apiAfspraken.Eindtijd, apiAfspraken.StatusNaam);
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het maken van een afspraak");
				}
			}).Result;
		}
		public static IEnumerable<AfspraakDTO> GeefAfsprakenVanBedrijf(long bedrijfsId) {
			return Task.Run(async () => {
				List<AfspraakDTO> ItemSource = new();
				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>($"afspraak?bedrijfId={bedrijfsId}&openstaand=true");
				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd, api.StatusNaam));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}
		public static IEnumerable<AfspraakDTO> GeefAfsprakenOpDatumVanBedrijf(long bedrijfsId, string? datum = null) {
			return Task.Run(async () => {
				List<AfspraakDTO> ItemSource = new();

				if (datum is null) datum = DateTime.Now.ToString("MM/dd/yyyy");
				else
					datum = DateTime.Parse(datum).ToString("MM/dd/yyyy");

				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>($"afspraak?dag={datum}&bedrijfId={bedrijfsId}");

				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						ItemSource.Add(new AfspraakDTO(api.Id, bezoeker, api.Bezoeker.BezoekerBedrijf, werknemer, api.Starttijd, api.Eindtijd, api.StatusNaam));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de afspraaken");
				}
			}).Result;
		}
		public static IEnumerable<AfspraakDTO> GeefAfspraken() {
			return Task.Run(async () => {
				List<AfspraakDTO> alleAfspraken = new();

				(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await Get<List<AfspraakOutputDTO>>("afspraak?dag=" + DateTime.Now.ToString("MM/dd/yyy"));
				if (isvalid) {
					apiAfspraken.ForEach((api) => {
						WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
						BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
						alleAfspraken.Add(new AfspraakDTO(api.Id, bezoeker, api.Bedrijf.Naam, werknemer, api.Starttijd, api.Eindtijd, api.StatusNaam));
					});
					return alleAfspraken;
				} else
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van alle afspraaken");
			}).Result;
		}

		public static void BeeindigAlleOnAfgeslotenAfspraken() {
			Task.Run(async () => await Put("afspraak/end/lopend"));
		}

		public async static Task VerwijderWerknemerVanBedrijf(long id) {
			await Task.Run(() => Delete($"bedrijf/{BeheerderWindow.GeselecteerdBedrijf.Id}/werknemer/{id}"));
		}

		public async static Task VerwijderAfspraak(AfspraakDTO afspraak) {
			if (afspraak.EindTijd is null) return;
			await Task.Run(() => Delete($"afspraak/{afspraak.Id}"));
		}

		public static BedrijfDTO UpdateBedrijf(long bedrijfId, BedrijfInputDTO nieuwBedrijf) {
			string payload = JsonConvert.SerializeObject(nieuwBedrijf);
			Task.Run(() => Put($"bedrijf/{bedrijfId}", payload));
			return new(bedrijfId, nieuwBedrijf.Naam, nieuwBedrijf.BTW, nieuwBedrijf.TelefoonNummer, nieuwBedrijf.Email, nieuwBedrijf.Adres);
		}

		public static async Task UpdateAfspraak(AfspraakInputDTO afspraakInput, long afspraakId, long bezoekerId) {
			string payload = JsonConvert.SerializeObject(afspraakInput);
			await Put($"afspraak/{afspraakId}/{bezoekerId}", payload);
		}

		public static async Task<AfspraakDTO> GeefAfspraak(long afspraakId) {
			(bool isvalid, AfspraakOutputDTO apiAfspraak) = await Get<AfspraakOutputDTO>($"afspraak/{afspraakId}");
			if (isvalid) {
				WerknemerDTO werknemer = new WerknemerDTO(apiAfspraak.Werknemer.Id, apiAfspraak.Werknemer.Naam.Split(";")[0], apiAfspraak.Werknemer.Naam.Split(";")[1], null);
				BezoekerDTO bezoeker = new(apiAfspraak.Bezoeker.Id, apiAfspraak.Bezoeker.Naam.Split(";")[0], apiAfspraak.Bezoeker.Naam.Split(";")[1], apiAfspraak.Bezoeker.Email, apiAfspraak.Bezoeker.BezoekerBedrijf);
				return new AfspraakDTO(apiAfspraak.Id, bezoeker, BeheerderWindow.GeselecteerdBedrijf.Naam, werknemer, apiAfspraak.Starttijd, apiAfspraak.Eindtijd, apiAfspraak.StatusNaam);
			} else {
				throw new FetchApiException("Er is iets fout gegaan bij het ophalen van een afspraak");
			}
		}

		public static async Task UpdateWerknemer(WerknemerInputDTO werknemerInput, long werknemerId) {
			string payload = JsonConvert.SerializeObject(werknemerInput);
			await Put($"werknemer/{werknemerId}/bedrijf/{BeheerderWindow.GeselecteerdBedrijf.Id}", payload);
		}
		#endregion

		#region Parking
		public static GrafiekDagOutputDTO? GeefParkeerplaatsWeekoverzichtVanBedrijf(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, GrafiekDagOutputDTO grafiek) = await Get<GrafiekDagOutputDTO>($"parkeerplaats/bedrijf/{bedrijfId}/overzicht/week");
				if (isvalid) {
					return grafiek;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de grafiek");
				}
			}).Result;
		}
		public static GrafiekDagDetailOutputDTO? GeefParkeerplaatsDagoverzichtVanBedrijf(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, GrafiekDagDetailOutputDTO grafiek) = await Get<GrafiekDagDetailOutputDTO>($"parkeerplaats/bedrijf/{bedrijfId}/overzicht/dag");
				if (isvalid) {
					return grafiek;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de grafiek");
				}
			}).Result;
		}
		public static ParkingContractoutputDTO? GeefParkingContract(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, ParkingContractoutputDTO parkingContract) = await Get<ParkingContractoutputDTO?>($"parkingcontract/bedrijf/{bedrijfId}");
				return parkingContract;
			}).Result;
		}
		public static int GeefHuidigBezetteParkeerplaatsenVoorBedrijf(long bedrijfId) {
			return Task.Run(async () => {
				(bool isvalid, int aantal) = await Get<int>($"parkeerplaats/bedrijf/{bedrijfId}/overzicht/bezet");
				if (isvalid) {
					return aantal;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van het aantal bezette parkeerplaatsen");
				}
			}).Result;
		}
		public static IEnumerable<ParkeerplaatsDTO> GeefNummerplaten(long bedrijfId) {
			return Task.Run(async () => {
				List<ParkeerplaatsDTO> ItemSource = new();
				(bool isvalid, List<ParkeerplaatsOutputDTO> nummerplaten) = await Get<List<ParkeerplaatsOutputDTO>>($"parkeerplaats/bedrijf/{bedrijfId}");
				if (isvalid) {
					nummerplaten.ForEach((nummerplaat) => {
						ItemSource.Add(new ParkeerplaatsDTO(nummerplaat.Starttijd, nummerplaat.Nummerplaat));
					});
					return ItemSource;
				} else {
					throw new FetchApiException("Er is iets fout gegaan bij het ophalen van de nummerplaten");
				}
			}).Result;
		}
		public static void CheckNummerplaatOut(ParkeerplaatsDTO parkeerplaatsDto) {
			Task.Run(async () => {
				await Post($"parkeerplaats/checkout/{parkeerplaatsDto.Nummerplaat}");
			}).Wait();
		}
		public static void CheckNummerplaatIn(long bedrijfId, DateTime starttijd, DateTime? eindtijd, string nummerplaat) {
			Task.Run(async () => {
				ParkeerplaatsInputDTO parkeerplaats = new ParkeerplaatsInputDTO(bedrijfId, starttijd, eindtijd, nummerplaat);
				string payload = JsonConvert.SerializeObject(parkeerplaats);
				await Post($"parkeerplaats/checkin/", payload);
			}).Wait();
		}
		public static void VoegParkingContractToe(long bedrijfId, DateTime starttijd, DateTime eindtijd, int aantalPlaatsen) {
			Task.Run(async () => {
				ParkingContractInputDTO parkeerplaats = new ParkingContractInputDTO(bedrijfId, starttijd, eindtijd, aantalPlaatsen);
				string payload = JsonConvert.SerializeObject(parkeerplaats);
				await Post($"parkingcontract", payload);
			}).Wait();
		}
		public static void UpdateParkingContract(long bedrijfId, DateTime starttijd, DateTime eindtijd, int aantalPlaatsen) {
			Task.Run(async () => {
				ParkingContractInputDTO parkeerplaats = new ParkingContractInputDTO(bedrijfId, starttijd, eindtijd, aantalPlaatsen);
				string payload = JsonConvert.SerializeObject(parkeerplaats);
				await Put($"parkingcontract", payload);
			}).Wait();
		}
		public static void VerwijderParkingContract(long bedrijfId) {
			Task.Run(async () => {
				await Delete($"parkingcontract/bedrijf/{bedrijfId}");
			}).Wait();
		}
		#endregion
	}
}
