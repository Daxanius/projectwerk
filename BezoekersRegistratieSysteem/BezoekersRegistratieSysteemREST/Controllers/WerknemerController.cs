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
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef een werknemer op naam
		/// </summary>
		/// <param name="naam"></param>
		/// <returns></returns>
		[HttpGet("{naam}")]
		public ActionResult<Werknemer> GeefWerknemer(string naam) {
			if (string.IsNullOrEmpty(naam)) return BadRequest($"{nameof(naam)} is null");

			try {
				return _werknemerManager.GeefWerknemerOpNaam(naam);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef werknemers, op bedrijf OF op functie
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <param name="functie"></param>
		/// <returns></returns>
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
		[HttpDelete("{id}")]
		public IActionResult VerwijderWerknemer(uint id) {
			try {
				_werknemerManager.VerwijderWerknemer(id);
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
		public ActionResult<Werknemer> VoegWerknemerToe([FromBody] Werknemer werknemer) {
			if (werknemer == null) return BadRequest($"{nameof(werknemer)} is null");

			try {
				_werknemerManager.VoegWerknemerToe(werknemer.Voornaam, werknemer.Achternaam, werknemer.Email, werknemer.Bedrijf, werknemer.Functie);
				return werknemer;
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