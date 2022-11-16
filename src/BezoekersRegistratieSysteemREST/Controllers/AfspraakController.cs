using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	/// <summary>
	/// De afspraak controller zorgt ervoor dat 
	/// wij afspraken kunnen beheren via de API.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class AfspraakController : ControllerBase {
		private readonly AfspraakManager _afspraakManager;
		private readonly WerknemerManager _werknemerManager;
		private readonly BedrijfManager _bedrijfManager;

		/// <summary>
		/// De constructor, heeft practisch alle managers nodig.
		/// </summary>
		/// <param name="afspraakManager">De afspraaak manager</param>
		/// <param name="werknemerManager">De werknemer manager</param>
		/// <param name="bedrijfManager">De bedrijf manager</param>
		public AfspraakController(AfspraakManager afspraakManager, WerknemerManager werknemerManager, BedrijfManager bedrijfManager) {
			_afspraakManager = afspraakManager;
			_werknemerManager = werknemerManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geeft een afspraak op ID.
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("{afspraakId}")]
		public ActionResult<AfspraakOutputDTO> GeefAfspraak(long afspraakId) {
			try {
				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfspraak(afspraakId)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef alle afspraken die overeenkomen met de query.
		/// </summary>
		/// <param name="dag">Geef afspraken van dag</param>
		/// <param name="werknemerId">Geef afspraken van werknemer</param>
		/// <param name="bedrijfId">Geef afspraken van bedrijf</param>
		/// <param name="openstaand">Als openstaand true is, geef huidige afspraken, werkt alleen voor werknemer</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpGet]
		public ActionResult<IEnumerable<AfspraakOutputDTO>> GeefAfspraken([FromQuery] DateTime? dag, [FromQuery] long? werknemerId, [FromQuery] long? bedrijfId, [FromQuery] bool openstaand = false) {
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

				// Als alleen de werknemer is meegegeven
				if (werknemer != null && bedrijf != null) {
					if (openstaand) {
						return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(werknemer, bedrijf)));
					}

					// Als je ook een dag meegeeft
					if (dag != null) {
						return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerWerknemerOpDagPerBedrijf(werknemer, dag.Value, bedrijf)));
					}

					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAlleAfsprakenPerWerknemerPerBedrijf(werknemer, bedrijf)));
				}

				// Als een bedrijf en een dag zijn gegeven
				if (bedrijf != null && dag != null) {
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerBedrijfOpDag(bedrijf, dag.Value)));
				}

				// Als alleen de dag is meegegeven
				if (dag != null) {
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerDag(dag.Value)));
				}

				// Geef alle openstaande afspraken per bedrijf
				if (bedrijf != null) {
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerBedrijf(bedrijf)));
				}

				// Als niets is meegegeven
				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfspraken()));
			} catch (Exception ex) {
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Verwijdert een afspraak op ID.
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpDelete("{afspraakId}")]
		public IActionResult VerwijderAfspraak(long afspraakId) {
			try {
				Afspraak afspraak = _afspraakManager.GeefAfspraak(afspraakId);
				_afspraakManager.VerwijderAfspraak(afspraak);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Maakt een afspraak.
		/// </summary>
		/// <param name="afspraakInput">De afspraak informatie om mee te geven</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPost]
		public ActionResult<AfspraakOutputDTO> MaakAfspraak([FromBody] AfspraakInputDTO afspraakInput) {
			try {
				Afspraak afspraak = afspraakInput.NaarBusiness(_werknemerManager, _bedrijfManager);
				return Ok(AfspraakOutputDTO.NaarDTO(
					_afspraakManager.VoegAfspraakToe(afspraak)
				));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Beëindigt een afspraak op Email.
		/// </summary>
		/// <param name="email"></param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpPut("end")]
		public IActionResult End([FromQuery] string email) {
			try {
				_afspraakManager.BeeindigAfspraakOpEmail(email);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Beëindigt alle lopende afspraken.
		/// </summary>
		/// <returns></returns>
		[HttpPut("end/lopend")]
		public IActionResult End() {
			try {
				// Haal alle lopende afspraken op
				List<Afspraak> afspraken = _afspraakManager.GeefHuidigeAfspraken().ToList();

				// Beeindig elke afspraak
				foreach (var afspraak in afspraken) {
					_afspraakManager.BeeindigAfspraakSysteem(afspraak);
				}
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Bewerkt een afspraak op ID.
		/// </summary>
		/// <param name="afspraakId">De ID van de afspraak</param>
		/// <param name="afspraakInput">De nieuwe informatie van de afspraak</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPut("{afspraakId}")]
		public ActionResult<AfspraakOutputDTO> BewerkAfspraak(long afspraakId, [FromBody] AfspraakInputDTO afspraakInput) {
			try {
				Afspraak afspraak = afspraakInput.NaarBusiness(_werknemerManager, _bedrijfManager);
				afspraak.ZetId(afspraakId);
				_afspraakManager.BewerkAfspraak(afspraak);
				return Ok(AfspraakOutputDTO.NaarDTO(afspraak));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geef alle afspraken binnen een bedrijf.
		/// </summary>
		/// <param name="bedrijfId">Het bedrijf waarbij de bezoeker zit</param>
		/// <param name="datum">Datum van van welke dag je de bezoekers wil van het bedrijf met bedrijfsId</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("bedrijf/{bedrijfId}")]
		public ActionResult<BezoekerOutputDTO> GeefAfspraakOpBezoeker(long bedrijfId, [FromQuery] DateTime? datum) {
			try {
				if (!datum.HasValue)
					datum = DateTime.Now;
				return Ok(BezoekerOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerDag(datum.Value).Where(a => a.Bedrijf.Id == bedrijfId).Select(a => a.Bezoeker)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft de huidige afspraak per bezoeker.
		/// </summary>
		/// <param name="bedrijfId">Het bedrijf waarbij de bezoeker zit</param>
		/// <param name="bezoekerId">De bezoeker informatie</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("bedrijf/{bedrijfId}/bezoeker/{bezoekerId}")]
		public ActionResult<AfspraakOutputDTO> GeefAfspraakOpBezoeker(long bedrijfId, long bezoekerId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfspraakBezoekerPerBedrijf(bezoekerId, bedrijf)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle afspraken per bezoeker op een bedrijf.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="bezoekerId">De bezoeker informatie</param>
		/// <param name="dag">De dag waarop de afspraak plaatsvond</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("bedrijf/{bedrijfId}/bezoeker/{bezoekerId}/all")]
		public ActionResult<IEnumerable<AfspraakOutputDTO>> GeefAfsprakenOpBezoeker(long bedrijfId, long bezoekerId, [FromQuery] DateTime dag) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);

				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerBezoekerOpDagPerBedrijf(bezoekerId, dag, bedrijf)));
				//return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bedrijf)));

			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle aanwezige bezoekers.
		/// </summary>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("aanwezig")]
		public ActionResult<IEnumerable<BezoekerOutputDTO>> GeefAanwezigeBezoekers() {
			try {
				return Ok(BezoekerOutputDTO.NaarDTO(_afspraakManager.GeefAanwezigeBezoekers()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}
	}
}