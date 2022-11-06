using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Api.DTO {
	public class ApiWerknemerIn {
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("voornaam")]
		public string Voornaam { get; set; }

		[JsonProperty("achternaam")]
		public string Achternaam { get; set; }

		[JsonProperty("werknemerInfo")]
		public List<ApiWerknemerInfoIn> WerknemerInfo { get; set; }
	}
}
