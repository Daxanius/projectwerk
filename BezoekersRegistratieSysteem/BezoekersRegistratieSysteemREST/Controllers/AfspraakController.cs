﻿using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Output;
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
		/// <param name="afspraakId"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<DTOAfspraakOutput> GeefAfspraak(uint afspraakId) {
			try {
				return DTOAfspraakOutput.NaarDTO(_afspraakManager.GeefAfspraak(afspraakId));
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
		public ActionResult<IEnumerable<DTOAfspraakOutput>> GeefAfspraken([FromQuery] DateTime? dag, [FromQuery] uint? werknemerId, [FromQuery] uint? bedrijfId, [FromQuery] bool openstaand = false) {
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
					return Ok(DTOAfspraakOutput.NaarDTO(_afspraakManager.GeefAfsprakenPerWerknemerOpDag(werknemer, dag ?? DateTime.Now)));
				}

				// Als alleen de dag is meegegeven
				if (dag != null) {
					return Ok(DTOAfspraakOutput.NaarDTO(_afspraakManager.GeefAfsprakenPerDag(dag ?? DateTime.Now)));
				}

				// Als alleen de werknemer is meegegeven
				if (werknemer != null) {
					if (openstaand) {
						return Ok(DTOAfspraakOutput.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerWerknemer(werknemer)));
					}

					// Zou dit niet ook beter een ID zijn?
					return Ok(DTOAfspraakOutput.NaarDTO(_afspraakManager.GeefAlleAfsprakenPerWerknemer(werknemer)));
				}

				// Geef alle openstaande afspraken per bedrijf
				if (bedrijf != null) {
					return Ok(DTOAfspraakOutput.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerBedrijf(bedrijf)));
				}

				// Als niets is meegegeven
				return Ok(DTOAfspraakOutput.NaarDTO(_afspraakManager.GeefHuidigeAfspraken()));
			} catch (Exception ex) {
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Verwijder een afspraak
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		//[HttpDelete("{id}")]
		//public IActionResult VerwijderAfspraak(uint id) {
		//	try {
		//		Afspraak afspraak = _afspraakManager.GeefAfspraak(id);
		//		_afspraakManager.VerwijderAfspraak(afspraak);
		//		return Ok();
		//	} catch (Exception ex) {
		//		return NotFound(ex.Message);
		//	}
		//}

		/// <summary>
		/// Maak een afspraak
		/// </summary>
		/// <param name="afrspraak"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<DTOAfspraakOutput> MaakAfspraak([FromBody] AfspraakInputDTO afrspraak) {
			try {
				// De bezoeker en de werknemer ophalen van de ids
				Bezoeker bezoeker = afrspraak.Bezoeker.NaarBusiness();
				Werknemer werknemer = _werknemerManager.GeefWerknemer(afrspraak.WerknemerId);
				return DTOAfspraakOutput.NaarDTO(
					_afspraakManager.VoegAfspraakToe(new(DateTime.Now, bezoeker, werknemer))
				);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Beeindig een afspraak op ID
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <returns></returns>
		[HttpPut("end/{id}")]
		public IActionResult End(uint afspraakId) {
			try {
				Afspraak afspraak = _afspraakManager.GeefAfspraak(afspraakId);
				_afspraakManager.BeeindigAfspraakBezoeker(afspraak);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <param name="afspraakInput"></param>
		/// <returns></returns>
		[HttpPut("{afspraakId}")]
		public ActionResult<DTOAfspraakOutput> BewerkAfspraak(uint afspraakId, [FromBody] AfspraakInputDTO afspraakInput) {
			try {
				Afspraak afspraak = afspraakInput.NaarBusiness(_werknemerManager);
				afspraak.ZetId(afspraakId);
				_afspraakManager.BewerkAfspraak(afspraak);
				return DTOAfspraakOutput.NaarDTO(afspraak);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}