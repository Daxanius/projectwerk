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
				return AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfspraak(afspraakId));
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

				// Als beide de dag EN de werknemer zijn meegegeven
				if (werknemer != null && dag != null)
				{
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerWerknemerOpDag(werknemer, dag ?? DateTime.Now)));
				}

				// Als alleen de dag is meegegeven
				if (dag != null)
				{
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerDag(dag ?? DateTime.Now)));
				}
				
				// Als alleen de werknemer is meegegeven
				if (werknemer != null)
				{
					if (openstaand)
					{
						return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfspraakPerWerknemer(werknemer)));
					}

					// Zou dit niet ook beter een ID zijn?
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAlleAfsprakenPerWerknemer(werknemer)));
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
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("id/{id}")]
		public IActionResult VerwijderAfspraak(long id)
		{
			try
			{
				Afspraak afspraak = _afspraakManager.GeefAfspraak(id);
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
		/// <param name="afrspraak"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<AfspraakOutputDTO> MaakAfspraak([FromBody] AfspraakInputDTO afrspraak)
		{
			try
			{
				// De bezoeker en de werknemer ophalen van de ids
				Bezoeker bezoeker = afrspraak.Bezoeker.NaarBusiness();
				Werknemer werknemer = _werknemerManager.GeefWerknemer(afrspraak.WerknemerId);
				return AfspraakOutputDTO.NaarDTO(
					_afspraakManager.VoegAfspraakToe(new(DateTime.Now, bezoeker, werknemer))
				);
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
				Afspraak afspraak = afspraakInput.NaarBusiness(_werknemerManager);
				afspraak.ZetId(afspraakId);
				_afspraakManager.BewerkAfspraak(afspraak);
				return AfspraakOutputDTO.NaarDTO(afspraak);
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geef huidige afspraak op bezoeker
		/// </summary>
		/// <param name="bezoekerInput"></param>
		/// <returns></returns>
		[HttpGet("bezoeker")]
		public ActionResult<AfspraakOutputDTO> GeefAfspraakOpBezoeker([FromBody] BezoekerInputDTO bezoekerInput) {
			try {
				Bezoeker bezoeker = bezoekerInput.NaarBusiness();
				return AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfspraakBezoeker(bezoeker));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef afspraken per bezoeker
		/// </summary>
		/// <param name="bezoekerInput"></param>
		/// <param name="dag"></param>
		/// <returns></returns>
		[HttpGet("bezoeker/afspraken")]
		public ActionResult<IEnumerable<AfspraakOutputDTO>> GeefAfsprakenOpBezoeker([FromBody] BezoekerInputDTO bezoekerInput, [FromQuery] DateTime? dag) {
			try {
				Bezoeker bezoeker = bezoekerInput.NaarBusiness();

				if (dag != null) {
					return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerBezoekerOpDag(bezoeker, dag ?? DateTime.Now)));
				}

				return Ok(AfspraakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerBezoekerOpNaamOfEmail(bezoeker.Voornaam, bezoeker.Achternaam, bezoeker.Email)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}
	}
}