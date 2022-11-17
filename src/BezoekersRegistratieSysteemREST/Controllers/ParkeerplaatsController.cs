using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	/// <summary>
	/// De parkeerplaats controller zorgt ervoor
	/// dat wij parkeerplaatsen kunnen beheren via de API.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class ParkeerplaatsController : ControllerBase {
		private readonly ParkeerplaatsManager _parkeerplaatsManager;
		private readonly BedrijfManager _bedrijfManager;

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="parkeerplaatsManager"></param>
		/// <param name="bedrijfManager"></param>
		public ParkeerplaatsController(ParkeerplaatsManager parkeerplaatsManager, BedrijfManager bedrijfManager) {
			_parkeerplaatsManager = parkeerplaatsManager;
			_bedrijfManager = bedrijfManager;	
		}
	}
}