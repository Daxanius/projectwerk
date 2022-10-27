using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		private readonly BedrijfManager _bedrijfManager;

		public BedrijfController(BedrijfManager afspraakManager) {
			_bedrijfManager = afspraakManager;
		}

		/// <summary>
		/// Verkrijgt een bedrijf op ID
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Bedrijf> GeefBedrijf(uint bedrijfId) {
			try {
				return _bedrijfManager.GeefBedrijf(bedrijfId);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verkrijgt een bedrijf op naam
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{naam}")]
		public ActionResult<Bedrijf> GeefBedrijf(string naam) {
			try {
				return _bedrijfManager.GeefBedrijf(naam);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle bedrijven
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IEnumerable<Bedrijf> GeefAlleBedrijven() {
			// Kan dit fout gaan?
			return _bedrijfManager.Geefbedrijven();
		}

		/// <summary>
		/// Verwijdert een bedrijf, vraagt momenteel
		/// het ID als parameter
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public IActionResult VerwijderBedrijf(uint id) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(id);
				_bedrijfManager.VerwijderBedrijf(bedrijf);
				return Ok();
			} catch (Exception ex) {
				// Welke IActionResults zijn er??
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voegt een bedrijf toe
		/// </summary>
		/// <param name="bedrijfData"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<Bedrijf> VoegBedrijfToe([FromBody] DTOBedrijfInput bedrijfData) {
			try {
				return _bedrijfManager.VoegBedrijfToe(bedrijfData.NaarBusiness());
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een bedrijf
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <returns></returns>
		[HttpPut]
		public ActionResult<Bedrijf> BewerkBedrijf([FromBody] Bedrijf bedrijf) {
			try {
				_bedrijfManager.BewerkBedrijf(bedrijf);
				return bedrijf;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}