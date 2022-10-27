using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Output;
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
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<DTOBedrijfOutput> GeefBedrijf(uint bedrijfId) {
			try {
				return DTOBedrijfOutput.NaarDTO(_bedrijfManager.GeefBedrijf(bedrijfId));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verkrijg een bedrijf op naam
		/// </summary>
		/// <param name="bedrijfNaam"></param>
		/// <returns></returns>
		[HttpGet("{naam}")]
		public ActionResult<DTOBedrijfOutput> GeefBedrijf(string bedrijfNaam) {
			try {
				return DTOBedrijfOutput.NaarDTO(_bedrijfManager.GeefBedrijf(bedrijfNaam));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle bedrijven
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult<IEnumerable<DTOBedrijfOutput>> GeefAlleBedrijven() {
			try {
				// Kan dit fout gaan?
				return Ok(DTOBedrijfOutput.NaarDTO(_bedrijfManager.Geefbedrijven()));
			} catch (Exception ex) {
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Verwijdert een bedrijf, vraagt momenteel
		/// het ID als parameter
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpDelete("{bedrijfId}")]
		public IActionResult VerwijderBedrijf(uint bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
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
		public ActionResult<DTOBedrijfOutput> VoegBedrijfToe([FromBody] BedrijfInputDTO bedrijfData) {
			try {
				return DTOBedrijfOutput.NaarDTO(_bedrijfManager.VoegBedrijfToe(bedrijfData.NaarBusiness()));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="bedrijfInput"></param>
		/// <returns></returns>
		[HttpPut("{bedrijfId}")]
		public ActionResult<DTOBedrijfOutput> BewerkBedrijf(uint bedrijfId, [FromBody] BedrijfInputDTO bedrijfInput) {
			try {
				Bedrijf bedrijf = bedrijfInput.NaarBusiness();
				bedrijf.ZetId(bedrijfId);

				_bedrijfManager.BewerkBedrijf(bedrijf);
				return DTOBedrijfOutput.NaarDTO(bedrijf);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}