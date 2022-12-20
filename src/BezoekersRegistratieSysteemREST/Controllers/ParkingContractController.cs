using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model.Input;
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
		/// Geef een parking contract van een bedrijf.
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
		/// Bewerkt een contract.
		/// </summary>
		/// <param name="contractData"></param>
		/// <returns></returns>
		[HttpPut]
		public IActionResult BewerkParkingContract([FromBody] ParkingContractInputDTO contractData) {
			try {
				_parkingContractManager.BewerkParkingContract(contractData.NaarBusiness(_bedrijfManager));
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
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

        [HttpGet("bedrijf/{bedrijfId}/overzicht/plaatsen")]
        public ActionResult<int> GeefAantalParkeerplaatsenVoorBedrijf(long bedrijfId)
        {
            try
            {
                Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
                return Ok(_parkingContractManager.GeefAantalParkeerplaatsenVoorBedrijf(bedrijf));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}