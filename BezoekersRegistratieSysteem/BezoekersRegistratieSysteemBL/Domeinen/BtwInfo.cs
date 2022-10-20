using Newtonsoft.Json;
using System.Net;

namespace BezoekersRegistratieSysteemBL.Domeinen
{
	public class BtwInfo
	{
		[JsonProperty("valid")]
		public bool Valid { get; set; }

		[JsonProperty("countryCode")]
		public string LandCode { get; set; }

		[JsonProperty("vatNumber")]
		public string BtwNumber { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("strAddress")]
		public string Address { get; set; }
	}
}