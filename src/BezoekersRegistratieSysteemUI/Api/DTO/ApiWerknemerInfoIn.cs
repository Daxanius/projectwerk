using Newtonsoft.Json;
using System.Collections.Generic;

namespace BezoekersRegistratieSysteemUI.Api.DTO {
	public class ApiWerknemerInfoIn {
		[JsonProperty("bedrijfId")]
		public long BedrijfId { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("functies")]
		public List<string> Functies { get; set; }
	}
}