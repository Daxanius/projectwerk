using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AfspraakController : ControllerBase {
		private readonly AfspraakManager _afspraakManager;

		public AfspraakController(AfspraakManager afspraakManager) {
			_afspraakManager = afspraakManager;
		}

		[HttpGet("{id}")]
		public Afspraak Get(uint id) {
			throw new NotImplementedException();
		}

		[HttpGet]
		public IEnumerable<Afspraak> GetAll([FromQuery] DateTime? dag, [FromQuery] uint werknemer) {
			throw new NotImplementedException();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(uint id) {
			throw new NotImplementedException();
		}

		[HttpPut]
		public ActionResult<Afspraak> Post([FromBody] Afspraak afspraak) {
			throw new NotImplementedException();
		}

		[HttpPost]
		[Route("end/{id}")]
		public Afspraak End(uint id) {
			throw new NotImplementedException();
		}
	}
}