using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Input;
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
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Werknemer> GeefWerknemer(uint id) {
			try {
				return _werknemerManager.GeefWerknemer(id);
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
		public ActionResult<IEnumerable<Werknemer>> GeefWerknemersOpNaam(string naam, string achternaam) {
			if (string.IsNullOrEmpty(naam)) return BadRequest($"{nameof(naam)} is null");

			try {
				return Ok(_werknemerManager.GeefWerknemersOpNaam(naam, achternaam).AsEnumerable());
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
		public ActionResult<IEnumerable<Werknemer>> GeefWerknemersPerBedrijf(uint bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(_werknemerManager.GeefWerknemersPerBedrijf(bedrijf));
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
		public ActionResult<Werknemer> VoegWerknemerToe([FromBody] DTOWerknemerInput werknemerData) {
			if (werknemerData == null) return BadRequest($"{nameof(werknemerData)} is null");

			try {
				Werknemer werknemer = new(werknemerData.Voornaam, werknemerData.Achternaam);
				return _werknemerManager.VoegWerknemerToe(werknemer);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Wijzig werknemerinfo van een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		[HttpPut("{bedrijfId}")]
		public ActionResult<Werknemer> BewerkWerknemer(uint bedrijfId, [FromBody] Werknemer werknemer) {
			if (werknemer == null) return BadRequest($"{nameof(werknemer)} is null");

			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				_werknemerManager.WijzigWerknemer(werknemer, bedrijf);
				return werknemer;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}