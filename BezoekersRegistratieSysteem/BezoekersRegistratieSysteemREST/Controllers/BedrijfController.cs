using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		private readonly BedrijfsManager _bedrijfManager;

		public BedrijfController(BedrijfsManager afspraakManager) {
			_bedrijfManager = afspraakManager;
		}

		[HttpGet("{id}")]
		public Bedrijf Get(uint id) {
			throw new NotImplementedException();
		}

		[HttpGet]
		public IEnumerable<Bedrijf> GetAll() {
			// Kan dit fout gaan?
			return _bedrijfManager.Geefbedrijven();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(uint id) {
			// Id opvragen? Leeg levert een crash op
			Bedrijf bedrijf = new("", "", "", "", "");
			bedrijf.ZetId(id);

			try {
				_bedrijfManager.VerwijderBedrijf(bedrijf);
				return Ok();
			} catch (Exception) {
				// Welke IActionResults zijn er??
				return BadRequest();
			}
		}

		[HttpPut]
		public ActionResult<Bedrijf> Post([FromBody] Bedrijf bedrijf) {
			throw new NotImplementedException();
		}
	}
}