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
		[HttpGet("{id}")]
		public ActionResult<AfsrpaakOutputDTO> GeefAfspraak(long afspraakId)
		{
			try
			{
				return AfsrpaakOutputDTO.NaarDTO(_afspraakManager.GeefAfspraak(afspraakId));
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
		public ActionResult<IEnumerable<AfsrpaakOutputDTO>> GeefAfspraken([FromQuery] DateTime? dag, [FromQuery] long? werknemerId, [FromQuery] long? bedrijfId, [FromQuery] bool openstaand = false)
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
					return Ok(AfsrpaakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerWerknemerOpDag(werknemer, dag ?? DateTime.Now)));
				}

				// Als alleen de dag is meegegeven
				if (dag != null)
				{
					return Ok(AfsrpaakOutputDTO.NaarDTO(_afspraakManager.GeefAfsprakenPerDag(dag ?? DateTime.Now)));
				}

				// Als alleen de werknemer is meegegeven
				if (werknemer != null)
				{
					if (openstaand)
					{
						return Ok(AfsrpaakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerWerknemer(werknemer)));
					}

					// Zou dit niet ook beter een ID zijn?
					return Ok(AfsrpaakOutputDTO.NaarDTO(_afspraakManager.GeefAlleAfsprakenPerWerknemer(werknemer)));
				}

				// Geef alle openstaande afspraken per bedrijf
				if (bedrijf != null)
				{
					return Ok(AfsrpaakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfsprakenPerBedrijf(bedrijf)));
				}

				// Als niets is meegegeven
				return Ok(AfsrpaakOutputDTO.NaarDTO(_afspraakManager.GeefHuidigeAfspraken()));
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
		[HttpDelete("{id}")]
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
		public ActionResult<AfsrpaakOutputDTO> MaakAfspraak([FromBody] AfspraakInputDTO afrspraak)
		{
			try
			{
				// De bezoeker en de werknemer ophalen van de ids
				Bezoeker bezoeker = afrspraak.Bezoeker.NaarBusiness();
				Werknemer werknemer = _werknemerManager.GeefWerknemer(afrspraak.WerknemerId);
				return AfsrpaakOutputDTO.NaarDTO(
					_afspraakManager.VoegAfspraakToe(new(DateTime.Now, bezoeker, werknemer))
				);
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Beeindig een afspraak
		/// </summary>
		/// <param name="afspraakId"></param>
		/// <param name="bezoekerInput"></param>
		/// <returns></returns>
		[HttpPut("end/{id}")]
		public IActionResult End(long afspraakId, BezoekerInputDTO bezoekerInput)
		{
			try
			{
				Afspraak afspraak = _afspraakManager.GeefAfspraak(afspraakId);
				Bezoeker bezoeker = bezoekerInput.NaarBusiness();
				afspraak.ZetBezoeker(bezoeker);
				_afspraakManager.BeeindigAfspraakBezoeker(afspraak);
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
		[HttpPut("{afspraakId}")]
		public ActionResult<AfsrpaakOutputDTO> BewerkAfspraak(long afspraakId, [FromBody] AfspraakInputDTO afspraakInput)
		{
			try
			{
				Afspraak afspraak = afspraakInput.NaarBusiness(_werknemerManager);
				afspraak.ZetId(afspraakId);
				_afspraakManager.BewerkAfspraak(afspraak);
				return AfsrpaakOutputDTO.NaarDTO(afspraak);
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}