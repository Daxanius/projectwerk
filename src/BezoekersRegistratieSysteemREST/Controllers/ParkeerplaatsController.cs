using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	/// <summary>
	/// De parkeerplaats controller zorgt ervoor
	/// dat wij parkeerplaatsen kunnen beheren via de API.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ParkeerplaatsController : ControllerBase {
		private readonly ParkingContractManager _parkingContractManager;
		private readonly ParkeerplaatsManager _parkeerplaatsManager;
		private readonly BedrijfManager _bedrijfManager;

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="parkingContractManager"></param>
		/// <param name="parkeerplaatsManager"></param>
		/// <param name="bedrijfManager"></param>
		public ParkeerplaatsController(ParkingContractManager parkingContractManager, ParkeerplaatsManager parkeerplaatsManager, BedrijfManager bedrijfManager) {
			_parkingContractManager = parkingContractManager;
			_parkeerplaatsManager = parkeerplaatsManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geef de nummerplaten van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("bedrijf/{bedrijfId}")]
		public ActionResult<IEnumerable<string>> GeefNummerplaten(long bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(_parkeerplaatsManager.GeefNummerplatenPerBedrijf(bedrijf));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef de nummerplaten van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("bedrijf/{bedrijfId}/overzicht/week")]
		public ActionResult<GrafiekDagOutputDTO> GeefWeekoverzichtParkingVoorBedrijf(long bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(GrafiekDagOutputDTO.NaarDTO(_parkeerplaatsManager.GeefWeekoverzichtParkingVoorBedrijf(bedrijf)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef de nummerplaten van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("bedrijf/{bedrijfId}/overzicht/uur")]
		public ActionResult<GrafiekDagDetailOutputDTO> GeefUuroverzichtParkingVoorBedrijf(long bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(GrafiekDagDetailOutputDTO.NaarDTO(_parkeerplaatsManager.GeefUuroverzichtParkingVoorBedrijf(bedrijf)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voegt een contract toe.
		/// </summary>
		/// <param name="contractData"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult VoegParkingContractToe([FromBody] ParkingContractInputDTO contractData) {
			try {
                _parkingContractManager.VoegParkingContractToe(contractData.NaarBusiness(_bedrijfManager));
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Check een parking in.
		/// </summary>
		/// <param name="parkeerplaats"></param>
		/// <returns></returns>
		[HttpPost("checkin")]
		public IActionResult CheckNummerplaatIn([FromBody] ParkeerplaatsInputDTO parkeerplaats) {
			try {
				_parkeerplaatsManager.CheckNummerplaatIn(parkeerplaats.NaarBusiness(_bedrijfManager));
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Check een parking uit.
		/// </summary>
		/// <param name="nummerplaat"></param>
		/// <returns></returns>
		[HttpPost("checkout")]
		public IActionResult CheckNummerplaatIn([FromQuery] string nummerplaat) {
			try {
				_parkeerplaatsManager.CheckNummerplaatUit(nummerplaat);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verwijdert een contract van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpDelete("bedrijf/{bedrijfId}")]
		public IActionResult VerwijderParkingContract(long bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				ParkingContract contract = _parkingContractManager.GeefParkingContract(bedrijf);
				_parkingContractManager.VerwijderParkingContract(contract);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}
	}
}