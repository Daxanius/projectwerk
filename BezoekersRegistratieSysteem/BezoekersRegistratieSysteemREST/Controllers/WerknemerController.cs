using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class WerknemerController : ControllerBase {
		private readonly WerknemerManager _werknemerManager;

		public WerknemerController(WerknemerManager werknemerManager) {
			_werknemerManager = werknemerManager;
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
			} catch (Exception) {
				return NotFound();
			}
		}

		/// <summary>
		/// Geef een werknemer op naam
		/// </summary>
		/// <param name="naam"></param>
		/// <returns></returns>
		[HttpGet("{naam}")]
		public ActionResult<Werknemer> GeefWerknemer(string naam) {
			try {
				return _werknemerManager.GeefWerknemerOpNaam(naam);
			} catch (Exception) {
				return NotFound();
			}
		}

		[HttpGet]
		public ActionResult<IEnumerable<Werknemer>> GeefWerknemers([FromQuery] Bedrijf? bedrijf, [FromQuery] string? functie) {
			// Zou bedrijf niet beter een ID zijn?
			if (bedrijf != null) {
				return Ok(_werknemerManager.GeefWerknemersPerBedrijf(bedrijf));
			}

			if (!string.IsNullOrEmpty(functie)) {
				return Ok(_werknemerManager.GeefWerknemersPerFunctie(functie));
			}

			// Kan niet alle werknemers opvragen
			return BadRequest();
		}

		/// <summary>
		/// Verwijder een werknemer op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpDelete("{id}")]
		public IActionResult VerwijderWerknemer(uint id) {
			try {
				_werknemerManager.VerwijderWerknemer(id);
				return Ok();
			} catch (Exception) {
				// Welke IActionResults zijn er??
				return NotFound();
			}
		}

		/// <summary>
		/// Voeg een werknemer toe
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPut]
		public ActionResult<Werknemer> VoegWerknemerToe([FromBody] Werknemer werknemer) {
			try {
				_werknemerManager.VoegWerknemerToe(werknemer.Voornaam, werknemer.Achternaam, werknemer.Email, werknemer.Bedrijf, werknemer.Functie);
				return werknemer;
			} catch (Exception) {
				return BadRequest();
			}
		}
	}
}