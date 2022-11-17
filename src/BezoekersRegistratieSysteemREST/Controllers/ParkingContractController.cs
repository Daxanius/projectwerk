using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	/// <summary>
	/// De parking contract controller zorgt ervoor
	/// dat wij parking contracten kunnen beheren via de API.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ParkingContractController : ControllerBase {
		private readonly ParkingContractManager _parkingContractManager;
		private readonly BedrijfManager _bedrijfManager;

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="parkingContractManager"></param>
		/// <param name="bedrijfManager"></param>
		public ParkingContractController(ParkingContractManager parkingContractManager, BedrijfManager bedrijfManager) {
			_parkingContractManager = parkingContractManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geef een parking contract van een bedrijf
		/// </summary>
		/// <returns></returns>
		[HttpGet("bedrijf/{bedrijfId}")]
		public ActionResult<ParkingContractOutputDTO> GeefParkingContract(long bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(_parkingContractManager.GeefParkingContract(bedrijf));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}
	}
}