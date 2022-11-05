using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AfspraakController : ControllerBase
	{
		private readonly AfspraakManager _afspraakManager;
		private readonly WerknemerManager _werknemerManager;
		private readonly BedrijfManager _bedrijfManager;

		public AfspraakController(AfspraakManager afspraakManager, WerknemerManager werknemerManager, BedrijfManager bedrijfManager)
		{
			_afspraakManager = afspraakManager;
			_werknemerManager = werknemerManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geeft een afspraak op ID
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <returns></returns>
		[HttpGet("id/{afspraakId}")]
		public ActionResult<AfspraakOutputDTO> GeefAfspraak(long afspraakId)
		{
			try
			{
				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfspraak(afspraakId)));
			} catch (Exception ex)
			{
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
		public ActionResult<IEnumerable<AfspraakOutputDTO>> GeefAfspraken([FromQuery] DateTime? dag, [FromQuery] long? werknemerId, [FromQuery] long? bedrijfId, [FromQuery] bool openstaand = false)
		{
			try
			{
				// Ophalen via ID
				Werknemer? werknemer = null;
				Bedrijf? bedrijf = null;

				if (werknemerId != null)
				{
					werknemer = _werknemerManager.GeefWerknemer(werknemerId ?? 0);
				}

				if (bedrijfId != null)
				{
					bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId ?? 0);
				}
				
				// Als alleen de werknemer is meegegeven
				if (werknemer != null && bedrijf != null)
				{
					if (openstaand)
					{
						return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerWerknemerPerBedrijf(werknemer, bedrijf)));
					}

					// Als je ook een dag meegeeft
					if (dag != null) {
						return Ok(_afspraakManager.GeefAfsprakenPerWerknemerOpDagPerBedrijf(werknemer, dag.Value, bedrijf));
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
				if (bedrijf != null)
				{
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerBedrijf(bedrijf)));
				}

				// Als niets is meegegeven
				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfspraken()));
			} catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Verwijder een afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <returns></returns>
		[HttpDelete("id/{afspraakId}")]
		public IActionResult VerwijderAfspraak(long afspraakId)
		{
			try
			{
				Afspraak afspraak = _afspraakManager.GeefAfspraak(afspraakId);
				_afspraakManager.VerwijderAfspraak(afspraak);
				return Ok();
			} catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Maak een afspraak
		/// </summary>
		/// <param name="afspraakInput"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<AfspraakOutputDTO> MaakAfspraak([FromBody] AfspraakInputDTO afspraakInput)
		{
			try
			{
				Afspraak afspraak = afspraakInput.NaarBusiness(_werknemerManager, _bedrijfManager);
				return Ok(AfspraakOutputDTO.NaarDTO(
					_afspraakManager.VoegAfspraakToe(afspraak)
				));
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Beeindig een afspraak op Email
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		[HttpPut("end")]
		public IActionResult End([FromQuery] string email)
		{
			try
			{
				_afspraakManager.BeeindigAfspraakOpEmail(email);
				return Ok();
			} catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <param name="afspraakInput"></param>
		/// <returns></returns>
		[HttpPut("id/{afspraakId}")]
		public ActionResult<AfspraakOutputDTO> BewerkAfspraak(long afspraakId, [FromBody] AfspraakInputDTO afspraakInput)
		{
			try
			{
				Afspraak afspraak = afspraakInput.NaarBusiness(_werknemerManager, _bedrijfManager);
				afspraak.ZetId(afspraakId);
				_afspraakManager.BewerkAfspraak(afspraak);
				return Ok(AfspraakOutputDTO.NaarDTO(afspraak));
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geef huidige afspraak per bezoeker
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="bezoekerInput"></param>
		/// <returns></returns>
		[HttpGet("bezoeker/id/{bedrijfId}")]
		public ActionResult<AfspraakOutputDTO> GeefAfspraakOpBezoeker(long bedrijfId, [FromBody] BezoekerInputDTO bezoekerInput) {
			try {
				Bezoeker bezoeker = bezoekerInput.NaarBusiness();
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfspraakBezoekerPerBedrijf(bezoeker, bedrijf)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef afspraken per bedrijfsbezoeker
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="bezoekerInput"></param>
		/// <param name="dag"></param>
		/// <returns></returns>
		[HttpGet("bezoeker/afspraken/id/{bedrijfId}")]
		public ActionResult<IEnumerable<AfspraakOutputDTO>> GeefAfsprakenOpBezoeker(long bedrijfId, [FromBody] BezoekerInputDTO bezoekerInput, [FromQuery] DateTime? dag) {
			try {
				Bezoeker bezoeker = bezoekerInput.NaarBusiness();
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);

				if (dag != null) {
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerBezoekerOpDagPerBedrijf(bezoeker, dag ?? DateTime.Now, bedrijf)));
				}

				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email, bedrijf)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef alle aanwezige bezoekers
		/// </summary>
		/// <returns></returns>
		[HttpGet("bezoeker/aanwezig")]
		public ActionResult<IEnumerable<BezoekerOutputDTO>> GeefAanwezigeBezoekers() {
			try {
				return Ok(BezoekerOutputDTO.NaarDTO(_afspraakManager.GeefAanwezigeBezoekers()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}
	}
}