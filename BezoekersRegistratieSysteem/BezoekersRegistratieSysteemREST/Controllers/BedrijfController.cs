using BezoekersRegistratieSysteemBL.Domeinen;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		[HttpGet("{id}")]
		public Bedrijf Get(uint id) {
			throw new NotImplementedException();
		}

		[HttpGet]
		public IEnumerable<Bedrijf> GetAll() {
			throw new NotImplementedException();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(uint id) {
			throw new NotImplementedException();
		}

		[HttpPost]
		public ActionResult<Bedrijf> Post([FromBody] Bedrijf bedrijf) {
			throw new NotImplementedException();
		}
	}
}