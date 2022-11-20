using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	/// <summary>
	/// De werknemer controller zorgt ervoor dat 
	/// wij werknemers kunnen beheren via de API.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class WerknemerController : ControllerBase {
		private readonly WerknemerManager _werknemerManager;
		private readonly BedrijfManager _bedrijfManager;

		/// <summary>
		/// De constructor.
		/// </summary>
		/// <param name="werknemerManager">De werknemer manager</param>
		/// <param name="bedrijfManager">De bedrijf manager</param>
		public WerknemerController(WerknemerManager werknemerManager, BedrijfManager bedrijfManager) {
			_werknemerManager = werknemerManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geeft een werknemer op ID.
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("{werknemerId}")]
		public ActionResult<WerknemerOutputDTO> GeefWerknemerOpId(long werknemerId) {
			try {
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemer(werknemerId)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft een werknemer van een bedrijf op naam.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="naam">De voornaam van de werknemer</param>
		/// <param name="achternaam">De achternaam van de werknemer</param>
		/// <returns>NotFoudn bij mislukking</returns>
		[HttpGet("{naam}/{achternaam}/bedrijf/{bedrijfId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersOpNaam(long bedrijfId, string naam, string achternaam) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersOpNaamPerBedrijf(naam, achternaam, bedrijf)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft een lijst van werknemers uit een bedrijf op functie.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="functie">De functie van de werknemers</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("bedrijf/{bedrijfId}/functie/{functie}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersOpFunctie(long bedrijfId, string functie) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersOpFunctiePerBedrijf(functie, bedrijf)));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle vrije of bezette werknemers van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="vrij">Of de werknemer vrij is</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("bedrijf/vb/{bedrijfId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersPerBedrijfVrijOfBezet(long bedrijfId, [FromQuery] bool vrij) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);

				if (!vrij) {
					return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefBezetteWerknemersOpDitMomentVoorBedrijf(bedrijf).AsEnumerable()));
				}

				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefVrijeWerknemersOpDitMomentVoorBedrijf(bedrijf).AsEnumerable()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voegt een werknemer toe.
		/// </summary>
		/// <param name="werknemerData">De informatie van de werknemer</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPost]
		public ActionResult<WerknemerOutputDTO> VoegWerknemerToe([FromBody] WerknemerInputDTO werknemerData) {
			try {
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.VoegWerknemerToe(werknemerData.NaarBusiness(_bedrijfManager))));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Wijzigt werknemer info van een bedrijf.
		/// </summary>
		/// <param name="werknemerId">De ID van de werknemer</param>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="werknemerInput">De nieuwe info van de werknemer</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPut("{werknemerId}/bedrijf/{bedrijfId}")]
		public ActionResult<WerknemerOutputDTO> BewerkWerknemer(long werknemerId, long bedrijfId, [FromBody] WerknemerInputDTO werknemerInput) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = werknemerInput.NaarBusiness(_bedrijfManager);
				werknemer.ZetId(werknemerId);

				// Waarom heeft dit een bedrijf nodig om werknemer te weizigen?
				_werknemerManager.BewerkWerknemer(werknemer, bedrijf);
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijdert een functie van een werknemer binnen een bedrijf.
		/// </summary>
		/// <param name="werknemerId">De ID van de werknemer</param>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="naam">De naam van de functie</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpDelete("{werknemerId}/bedrijf/{bedrijfId}/functie/{naam}")]
		public IActionResult VerwijderWerknemerFunctie(long werknemerId, long bedrijfId, string naam) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				_werknemerManager.VerwijderWerknemerFunctie(werknemer, bedrijf, naam);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voegt info toe aan een werknemer.
		/// </summary>
		/// <param name="werknemerId">De ID van de werknemer</param>
		/// <param name="info">De info om toe te voegen aan de werknemer</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPost("{werknemerId}/info")]
		public ActionResult<WerknemerOutputDTO> VoegWerknemerFunctieToe(long werknemerId, [FromBody] WerknemerInfoInputDTO info) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(info.BedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
                WerknemerInfo werknemerInfo = info.NaarBusiness(_bedrijfManager);

				// Dit is nogal een vreemde manier om functies toe te voegen, wat heeft Email hiermee te maken?
				_werknemerManager.VoegWerknemerFunctieToe(werknemer, werknemerInfo);
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerkt de functie van een werknemer binnen een bedrijf.
		/// </summary>
		/// <param name="werknemerId">De ID van de werknemer</param>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="oudeFunctie">De oude functie de moet vervangen worden</param>
		/// <param name="nieuweFunctie">De nieuwe functie waarmee de oude vervangen wordt</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPut("{werknemerId}/bedrijf/{bedrijfId}/functie/{oudeFunctie}/")]
		public ActionResult<WerknemerOutputDTO> BewerkFunctie(long werknemerId, long bedrijfId, string oudeFunctie, [FromQuery] string nieuweFunctie) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				werknemer.WijzigFunctie(bedrijf, oudeFunctie, nieuweFunctie);
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}