using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.DTO;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		private readonly BedrijfManager _bedrijfManager;

		public BedrijfController(BedrijfManager afspraakManager) {
			_bedrijfManager = afspraakManager;
		}

		/// <summary>
		/// Verkrijgt een bedrijf op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Bedrijf> GeefBedrijf(uint id) {
			try {
				return _bedrijfManager.GeefBedrijf(id);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verkrijgt een bedrijf op naam
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{naam}")]
		public ActionResult<Bedrijf> GeefBedrijf(string naam) {
			// Je kunt nooit genoeg controles hebben
			if (string.IsNullOrEmpty(naam)) return BadRequest($"{nameof(naam)} is niet geldig");

			try {
				return _bedrijfManager.GeefBedrijf(naam);
			} catch (Exception ex) {
				return NotFound(ex.Message);
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
		public IActionResult VerwijderBedrijf([FromBody] Bedrijf bedrijf) {
			try {
				_bedrijfManager.VerwijderBedrijf(bedrijf);
				return Ok();
			} catch (Exception ex) {
				// Welke IActionResults zijn er??
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voegt een bedrijf toe
		/// </summary>
		/// <param name="bedrijfData"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<Bedrijf> VoegBedrijfToe([FromBody] DTOBedrijf bedrijfData) {
			if (bedrijfData == null) return BadRequest($"{nameof(bedrijfData)} is null");

			try {
				Bedrijf bedrijf = new(bedrijfData.Naam, bedrijfData.BTW, bedrijfData.TelefoonNummer, bedrijfData.Email, bedrijfData.Adres);
				_bedrijfManager.VoegBedrijfToe(bedrijf);
				return bedrijf;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een bedrijf
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <returns></returns>
		[HttpPut]
		public ActionResult<Bedrijf> BewerkBedrijf([FromBody] Bedrijf bedrijf) {
			if (bedrijf == null) return BadRequest($"{nameof(bedrijf)} is null");

			try {
				// Waarom vraagt bewerk nu opeens een instantie van Bedrijf ipv primitives gelijk
				// bij VoegBedrijfToe?
				_bedrijfManager.BewerkBedrijf(bedrijf);
				return bedrijf;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}