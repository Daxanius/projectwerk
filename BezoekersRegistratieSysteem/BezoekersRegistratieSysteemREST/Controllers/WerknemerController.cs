using BezoekersRegistratieSysteemBL.Domeinen;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class WerknemerController : ControllerBase {
		[HttpGet("{id}")]
		public Werknemer Get(uint id) {
			throw new NotImplementedException();
		}

		[HttpGet("{naam}")]
		public Werknemer GetOpNaam(string id) {
			throw new NotImplementedException();
		}

		[HttpGet]
		public IEnumerable<Werknemer> GetAll([FromQuery] uint? bedrijf, [FromQuery] string? functie) {
			throw new NotImplementedException();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(uint id) {
			throw new NotImplementedException();
		}

		[HttpPut]
		public ActionResult<Werknemer> Post([FromBody] Werknemer afspraak) {
			throw new NotImplementedException();
		}
	}
}