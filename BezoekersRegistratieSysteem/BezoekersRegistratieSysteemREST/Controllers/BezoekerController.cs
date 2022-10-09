using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BezoekerController : ControllerBase {
		private readonly BezoekerManager _bezoekerManager;

		public BezoekerController(BezoekerManager bezoekerManager) {
			_bezoekerManager = bezoekerManager;
		}

		[HttpGet("{id}")]
		public Bezoeker Get(uint id) {
			throw new NotImplementedException();
		}

		[HttpGet("{naam}")]
		public Bezoeker GetOpNaam(string id) {
			throw new NotImplementedException();
		}

		[HttpGet]
		public IEnumerable<Bezoeker> GetAll([FromQuery] DateTime? datum) {
			throw new NotImplementedException();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(uint id) {
			throw new NotImplementedException();
		}

		[HttpPut]
		public ActionResult<Bezoeker> Post([FromBody] Bezoeker afspraak) {
			throw new NotImplementedException();
		}
	}
}