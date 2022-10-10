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

		/// <summary>
		/// Geef een bezoeker op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Bezoeker> GeefBezoeker(uint id) {
			try {
				return _bezoekerManager.GeefBezoeker(id);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef een bezoeker op naam
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpGet("{naam}")]
		public ActionResult<Bezoeker> GeefOpNaam(string naam) {
			// De naam mag niet leeg zijn
			if (string.IsNullOrEmpty(naam)) return BadRequest();

			try {
				return _bezoekerManager.GeefBezoekerOpNaam(naam);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		[HttpGet]
		public IEnumerable<Bezoeker> GeefBEzoekers([FromQuery] DateTime? datum) {
			// Als een datum is meegegeven, geven we op datum
			if (datum != null) {
				return _bezoekerManager.GeefBezoekersOpDatum(datum ?? DateTime.Now);
			}

			return _bezoekerManager.GeefAanwezigeBezoekers();
		}

		/// <summary>
		/// Verwijder een bezoeker
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpDelete("{id}")]
		public IActionResult VerwijderBezoeker(uint id) {
			try {
				_bezoekerManager.VerwijderBezoeker(id);
				return Ok();
			} catch (Exception ex) {
				// Welke IActionResults zijn er??
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een bezoeker toe
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPost]
		public ActionResult<Bezoeker> VoegBezoekerTOe([FromBody] Bezoeker bezoeker) {
			if (bezoeker == null) return BadRequest();

			try {
				_bezoekerManager.VoegBezoekerToe(bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bezoeker.Bedrijf);
				return bezoeker;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}