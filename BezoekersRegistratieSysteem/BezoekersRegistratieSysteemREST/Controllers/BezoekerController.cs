using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BezoekerController : ControllerBase {
		private readonly BezoekerManager _bezoekerManager;

		public BezoekerController(BezoekerManager bezoekerManager, BedrijfManager bedrijfManager) {
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
		[HttpGet("{naam}/{achternaam}")]
		public ActionResult<IEnumerable<Bezoeker>> GeefOpNaam(string naam, string achternaam) {
			// De naam mag niet leeg zijn
			if (string.IsNullOrEmpty(naam)) return BadRequest($"{nameof(naam)} is null");
			if (string.IsNullOrEmpty(achternaam)) return BadRequest($"{nameof(achternaam)} is null");

			try {
				return Ok(_bezoekerManager.GeefBezoekerOpNaam(naam, achternaam));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef bezoekers, optioneel op datum
		/// </summary>
		/// <param name="datum"></param>
		/// <returns></returns>
		[HttpGet]
		public IEnumerable<Bezoeker> GeefBezoekers([FromQuery] DateTime? datum) {
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
		[HttpDelete("{id}")]
		public IActionResult VerwijderBezoeker(uint id) {
			try {
				Bezoeker bezoeker = _bezoekerManager.GeefBezoeker(id);
				_bezoekerManager.VerwijderBezoeker(bezoeker);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een bezoeker toe
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<Bezoeker> VoegBezoekerTOe([FromBody] DTOBezoeker bezoekerData) {
			if (bezoekerData == null) return BadRequest($"{nameof(bezoekerData)} is null");

			try {
				Bezoeker bezoeker = new(bezoekerData.Voornaam, bezoekerData.Achternaam, bezoekerData.Email, bezoekerData.Bedrijf);
				_bezoekerManager.VoegBezoekerToe(bezoeker);
				return bezoeker;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een bezoeker
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <returns></returns>
		[HttpPut]
		public ActionResult<Bezoeker> BewerkBezoeker([FromBody] Bezoeker bezoeker) {
			if (bezoeker == null) return BadRequest($"{nameof(bezoeker)} is null");

			try {
				_bezoekerManager.WijzigBezoeker(bezoeker);
				return bezoeker;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}