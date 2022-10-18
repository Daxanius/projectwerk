using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Managers;
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
		/// Verwijder een werknemer op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		public IActionResult VerwijderWerknemer([FromBody] Werknemer werknemer) {
			try {
				_werknemerManager.VerwijderWerknemer(werknemer);
				return Ok();
			} catch (Exception ex) {
				// Welke IActionResults zijn er??
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een werknemer toe
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<Werknemer> VoegWerknemerToe([FromBody] DTOWerknemer werknemerData) {
			if (werknemerData == null) return BadRequest($"{nameof(werknemerData)} is null");

			try {
				Werknemer werknemer = new(werknemerData.Voornaam, werknemerData.Achternaam, werknemerData.Email);
				return _werknemerManager.VoegWerknemerToe(werknemer);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een werknemer
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns></returns>
		[HttpPut]
		public ActionResult<Werknemer> BewerkWerknemer([FromBody] Werknemer werknemer) {
			if (werknemer == null) return BadRequest($"{nameof(werknemer)} is null");

			try {
				_werknemerManager.WijzigWerknemer(werknemer);
				return werknemer;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}