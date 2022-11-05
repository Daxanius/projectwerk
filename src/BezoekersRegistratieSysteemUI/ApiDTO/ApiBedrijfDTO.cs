using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.ApiDTO {
	public class ApiBedrijfDTO {
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("naam")]
		public string Naam { get; set; }

		[JsonProperty("btw")]
		public string Btw { get; set; }

		[JsonProperty("isGecontroleerd")]
		public bool IsGecontroleerd { get; set; }

		[JsonProperty("telefoonNummer")]
		public string TelefoonNummer { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("adres")]
		public string Adres { get; set; }

		[JsonProperty("werknemers")]
		public List<int> Werknemers { get; set; }
	}
}
