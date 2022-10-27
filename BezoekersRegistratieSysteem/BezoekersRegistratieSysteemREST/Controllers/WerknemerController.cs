using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class WerknemerController : ControllerBase {
		private readonly WerknemerManager _werknemerManager;
		private readonly BedrijfManager _bedrijfManager;

		public WerknemerController(WerknemerManager werknemerManager, BedrijfManager bedrijfManager) {
			_werknemerManager = werknemerManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geef een werknemer op ID
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpGet("{werknemerId}")]
		public ActionResult<DTOWerknemerOutput> GeefWerknemer(uint werknemerId) {
			try {
				return DTOWerknemerOutput.NaarDTO(_werknemerManager.GeefWerknemer(werknemerId));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef een werknemer op naam
		/// </summary>
		/// <param name="naam"></param>
		/// <param name="achternaam"></param>
		/// <returns></returns>
		[HttpGet("{naam}/{achternaam}")]
		public ActionResult<IEnumerable<DTOWerknemerOutput>> GeefWerknemersOpNaam(string naam, string achternaam) {
			try {
				return Ok(DTOWerknemerOutput.NaarDTO(_werknemerManager.GeefWerknemersOpNaam(naam, achternaam).AsEnumerable()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef werknemers per bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("{bedrijfId}")]
		public ActionResult<IEnumerable<DTOWerknemerOutput>> GeefWerknemersPerBedrijf(uint bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(DTOWerknemerOutput.NaarDTO(_werknemerManager.GeefWerknemersPerBedrijf(bedrijf)));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijder een werknemer van een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpDelete("{beddrijfId}/{werknemerId}")]
		public IActionResult VerwijderWerknemer(uint bedrijfId, uint werknemerId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				_werknemerManager.VerwijderWerknemer(werknemer, bedrijf);
				return Ok();
			} catch (Exception ex) {
				// Welke IActionResults zijn er??
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een werknemer toe
		/// </summary>
		/// <param name="werknemerData"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<DTOWerknemerOutput> VoegWerknemerToe([FromBody] WerknemerInputDTO werknemerData) {
			try {
				return DTOWerknemerOutput.NaarDTO(_werknemerManager.VoegWerknemerToe(werknemerData.NaarBusiness()));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Wijzig werknemerinfo van een bedrijf
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemerInput"></param>
		/// <returns></returns>
		[HttpPut("{werknemerId}/{bedrijfId}")]
		public ActionResult<DTOWerknemerOutput> BewerkWerknemer(uint werknemerId, uint bedrijfId, [FromBody] WerknemerInputDTO werknemerInput) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = werknemerInput.NaarBusiness();
				werknemer.ZetId(werknemerId);

				_werknemerManager.WijzigWerknemer(werknemer, bedrijf);
				return DTOWerknemerOutput.NaarDTO(werknemer);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}