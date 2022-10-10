using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		private readonly BedrijfsManager _bedrijfManager;

		public BedrijfController(BedrijfsManager afspraakManager) {
			_bedrijfManager = afspraakManager;
		}

		/// <summary>
		/// Verkrijgt een bedrijf op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpGet("{id}")]
		public Bedrijf GeefBedrijf(uint id) {
			// Implementatie NIET mogelijk, kan geen bedrijf opvragen op ID
			throw new NotImplementedException();
		}

		/// <summary>
		/// Verkrijgt een bedrijf op naam
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpGet("{naam}")]
		public ActionResult<Bedrijf> GeefBedrijf(string naam) {
			try {
				return Ok(_bedrijfManager.GeefBedrijf(naam));
			} catch (Exception) {
				return BadRequest();
			}
		}

		[HttpGet]
		public IEnumerable<Bedrijf> GeefAlleBedrijven() {
			// Kan dit fout gaan?
			return _bedrijfManager.Geefbedrijven();
		}

		/// <summary>
		/// Verwijdert een bedrijf, vraagt momenteel
		/// het ID als parameter
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		public IActionResult Delete([FromBody] uint id) {
			try {
				_bedrijfManager.VerwijderBedrijf(id);
				return Ok();
			} catch (Exception) {
				// Welke IActionResults zijn er??
				return BadRequest();
			}
		}

		/// <summary>
		/// Voegt een bedrijf toe
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPut]
		public ActionResult<Bedrijf> VoegBedrijfToe([FromBody] Bedrijf bedrijf) {
			try {
				_bedrijfManager.VoegBedrijfToe(bedrijf.Naam, bedrijf.BTW, bedrijf.Adres, bedrijf.Email, bedrijf.TelefoonNummer);
				return Ok(bedrijf);
			} catch (Exception) {
				return NotFound();
			}
		}
	}
}