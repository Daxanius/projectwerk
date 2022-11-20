using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	/// <summary>
	/// De bedrijf controller zorgt ervoor dat 
	/// wij bedrijven kunnen beheren via de API.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		private readonly BedrijfManager _bedrijfManager;
		private readonly WerknemerManager _werknemerManager;

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="bedrijfManager">De bedrijf manager</param>
		/// <param name="werknemerManager">De werknemer manager</param>
		public BedrijfController(BedrijfManager bedrijfManager, WerknemerManager werknemerManager) {
			_bedrijfManager = bedrijfManager;
			_werknemerManager = werknemerManager;
		}

		/// <summary>
		/// Verkrijgt een bedrijf op ID.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("{bedrijfId}")]
		public ActionResult<BedrijfOutputDTO> GeefBedrijfOpId(long bedrijfId) {
			try {
				return Ok(BedrijfOutputDTO.NaarDTO(_bedrijfManager.GeefBedrijf(bedrijfId)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verkrijgt een bedrijf op naam.
		/// </summary>
		/// <param name="bedrijfNaam">De naam van het bedrijf</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("naam/{bedrijfNaam}")]
		public ActionResult<BedrijfOutputDTO> GeefBedrijfOpNaam(string bedrijfNaam) {
			try {
				return Ok(BedrijfOutputDTO.NaarDTO(_bedrijfManager.GeefBedrijf(bedrijfNaam)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle bedrijven.
		/// </summary>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpGet]
		public ActionResult<IEnumerable<BedrijfOutputDTO>> GeefAlleBedrijven() {
			try {
				// Kan dit fout gaan?
				return Ok(BedrijfOutputDTO.NaarDTO(_bedrijfManager.GeefBedrijven()));
			} catch (Exception ex) {
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Verwijdert een bedrijf op ID.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpDelete("{bedrijfId}")]
		public IActionResult VerwijderBedrijf(long bedrijfId) {
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
		/// Voegt een bedrijf toe.
		/// </summary>
		/// <param name="bedrijfData">De informatie van het bedrijf</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPost]
		public ActionResult<BedrijfOutputDTO> VoegBedrijfToe([FromBody] BedrijfInputDTO bedrijfData) {
			try {
				return Ok(BedrijfOutputDTO.NaarDTO(_bedrijfManager.VoegBedrijfToe(bedrijfData.NaarBusiness())));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerkt een bedrijf.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="bedrijfInput">De nieuwe informatie van het bedrijf</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPut("{bedrijfId}")]
		public ActionResult<BedrijfOutputDTO> BewerkBedrijf(long bedrijfId, [FromBody] BedrijfInputDTO bedrijfInput) {
			try {
				Bedrijf bedrijf = bedrijfInput.NaarBusiness();
				bedrijf.ZetId(bedrijfId);

				_bedrijfManager.BewerkBedrijf(bedrijf);
				return Ok(BedrijfOutputDTO.NaarDTO(bedrijf));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geeft de werknemers van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpGet("{bedrijfId}/werknemer")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemers(long bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);


				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersPerBedrijf(bedrijf)));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijdert een werknemer uit een bedrijf.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="werknemerId">De ID van de werknemer om te verwijderen</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpDelete("{bedrijfId}/werknemer/{werknemerId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> VerwijderWerknemerUitBedrijf(long bedrijfId, long werknemerId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				bedrijf.VerwijderWerknemerUitBedrijf(werknemer);
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}