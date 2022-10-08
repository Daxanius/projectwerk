using BezoekersRegistratieSysteemBL.Domeinen;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		[HttpGet("{id}")]
		public Bedrijf Get(int id) {
			throw new NotImplementedException();
		}
	}
}