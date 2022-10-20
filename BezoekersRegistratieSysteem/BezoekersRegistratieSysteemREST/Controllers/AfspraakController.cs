using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AfspraakController : ControllerBase {
		private readonly AfspraakManager _afspraakManager;
		private readonly BezoekerManager _bezoekerManager;
		private readonly WerknemerManager _werknemerManager;
		private readonly BedrijfManager _bedrijfManager;

		public AfspraakController(AfspraakManager afspraakManager, BezoekerManager bezoekerManager, WerknemerManager werknemerManager, BedrijfManager bedrijfManager) {
			_afspraakManager = afspraakManager;
			_bezoekerManager = bezoekerManager;
			_werknemerManager = werknemerManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geeft een afspraak op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Afspraak> GeefAfspraak(uint id) {
			try {
				return _afspraakManager.GeefAfspraak(id);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef alle afspraken die overeenkomen met de query
		/// </summary>
		/// <param name="dag">Geef afspraken van dag</param>
		/// <param name="werknemerId">Geef afspraken van werknemer</param>
		/// <param name="bedrijfId">Geef afspraken van bedrijf</param>
		/// <param name="openstaand">Als openstaand true is, geef huidige afspraken, werkt alleen voor werknemer</param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult<IEnumerable<Afspraak>> GeefAfspraken([FromQuery] DateTime? dag, [FromQuery] uint? werknemerId, [FromQuery] uint? bedrijfId, [FromQuery] bool openstaand = false) {
			try {
				// Ophalen via ID
				Werknemer? werknemer = null;
				Bedrijf? bedrijf = null;

				if (werknemerId != null) {
					werknemer = _werknemerManager.GeefWerknemer(werknemerId ?? 0);
				}

				if (bedrijfId != null) {
					bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId ?? 0);
				}

				// Als beide de dag EN de werknemer zijn meegegeven
				if (werknemer != null && dag != null) {
					return Ok(_afspraakManager.GeefAfsprakenPerWerknemerOpDag(werknemer, dag ?? DateTime.Now));
				}

				// Als alleen de dag is meegegeven
				if (dag != null) {
					return Ok(_afspraakManager.GeefAfsprakenPerDag(dag ?? DateTime.Now));
				}

				// Als alleen de werknemer is meegegeven
				if (werknemer != null) {
					if (openstaand) {
						return Ok(_afspraakManager.GeefHuidigeAfsprakenPerWerknemer(werknemer));
					}

					// Zou dit niet ook beter een ID zijn?
					return Ok(_afspraakManager.GeefAlleAfsprakenPerWerknemer(werknemer));
				}

				// Geef alle openstaande afspraken per bedrijf
				if (bedrijf != null) {
					return Ok(_afspraakManager.GeefHuidigeAfsprakenPerBedrijf(bedrijf));
				}

				// Als niets is meegegeven
				return Ok(_afspraakManager.GeefHuidigeAfspraken());
			} catch (Exception ex) {
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Verwijder een afspraak
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public IActionResult VerwijderAfspraak(uint id) {
			try {
				Afspraak afspraak = _afspraakManager.GeefAfspraak(id);
				_afspraakManager.VerwijderAfspraak(afspraak);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Maakt een afspraak tussen een bezoeker en een werknemer op dit moment van de dag
		/// </summary>
		/// <param name="bezoekerId"></param>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<Afspraak> MaakAfspraak([FromQuery] uint bezoekerId, [FromQuery] uint werknemerId) {
			try {
				// De bezoeker en de werknemer ophalen van de ids
				Bezoeker bezoeker = _bezoekerManager.GeefBezoeker(bezoekerId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				Afspraak afspraak = new(0, DateTime.Now, null, bezoeker, werknemer);
				return _afspraakManager.VoegAfspraakToe(afspraak);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Beeindig een afspraak op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPut("end/{id}")]
		public IActionResult End(uint id) {
			try {
				Afspraak afspraak = _afspraakManager.GeefAfspraak(id);
				_afspraakManager.BeeindigAfspraakBezoeker(afspraak);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een afspraak
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns></returns>
		[HttpPut]
		public ActionResult<Afspraak> BewerkAfspraak([FromBody] Afspraak afspraak) {
			if (afspraak == null) return BadRequest($"{nameof(afspraak)} is null");

			try {
				_afspraakManager.BewerkAfspraak(afspraak);
				return afspraak;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}