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
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, _werknemerManager.GeefWerknemer(werknemerId)));
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
		[HttpGet("{bedrijfId}/{naam}/{achternaam}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersOpNaam(long bedrijfId, string naam, string achternaam) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, _werknemerManager.GeefWerknemersOpNaamPerBedrijf(naam, achternaam, bedrijf).AsEnumerable()));
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
		[HttpGet("functie/{bedrijfId}/{functie}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersOpFunctie(long bedrijfId, string functie) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, _werknemerManager.GeefWerknemersOpFunctiePerBedrijf(functie, bedrijf).AsEnumerable()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle werknemers van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpGet("bedrijf/{bedrijfId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersPerBedrijf(long bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);

				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, _werknemerManager.GeefWerknemersPerBedrijf(bedrijf).AsEnumerable()));
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
					return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, _werknemerManager.GeefBezetteWerknemersOpDitMomentVoorBedrijf(bedrijf).AsEnumerable()));
				}

				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, _werknemerManager.GeefVrijeWerknemersOpDitMomentVoorBedrijf(bedrijf).AsEnumerable()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verwijdert een werknemer van een bedrijf.
		/// </summary>
		/// <param name="bedrijfId">De ID van het bedrijf waaruit we de werknemer willen halen</param>
		/// <param name="werknemerId">De ID van de werknemer dat we uit het bedrijf willen halen</param>
		/// <returns>NotFound bij mislukking</returns>
		[HttpDelete("{bedrijfId}/{werknemerId}")]
		public IActionResult VerwijderWerknemer(long bedrijfId, long werknemerId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				_werknemerManager.VerwijderWerknemer(werknemer, bedrijf);
				return Ok();
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
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, _werknemerManager.VoegWerknemerToe(werknemerData.NaarBusiness())));
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
		[HttpPut("{werknemerId}/{bedrijfId}")]
		public ActionResult<WerknemerOutputDTO> BewerkWerknemer(long werknemerId, long bedrijfId, [FromBody] WerknemerInputDTO werknemerInput) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = werknemerInput.NaarBusiness();
				werknemer.ZetId(werknemerId);

				// Waarom heeft dit een bedrijf nodig om werknemer te weizigen?
				_werknemerManager.BewerkWerknemer(werknemer, bedrijf);
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, werknemer));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geeft een lijst met bedrijven en informatie van een werknemer.
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpGet("info/{werknemerId}")]
		public ActionResult<Dictionary<long, WerknemerInfoOutputDTO>> GeefBedrijvenEnFunctiesPerWerknemer(long werknemerId) {
			try {
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				var bedrijven = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();

				// Een conversie naar de DTO
				Dictionary<long, WerknemerInfoOutputDTO> output = new();
				foreach (Bedrijf b in bedrijven.Keys) {
					output.Add(b.Id, WerknemerInfoOutputDTO.NaarDTO(bedrijven[b]));
				}

				return Ok(output);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Voegt een functie toe aan een werknemer binnen een bedrijf.
		/// </summary>
		/// <param name="werknemerId">De ID van de werknemer</param>
		/// <param name="bedrijfId">De ID van het bedrijf</param>
		/// <param name="naam">De naam van de functie</param>
		/// <returns>BadRequest bij mislukking</returns>
		[HttpPost("functie/{werknemerId}/{bedrijfId}/{naam}")]
		public IActionResult VoegWerknemerFunctieToe(long werknemerId, long bedrijfId, string naam) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				_werknemerManager.VoegWerknemerFunctieToe(werknemer, bedrijf, naam);
				return Ok();
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
		[HttpDelete("functie/{werknemerId}/{bedrijfId}/{naam}")]
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
		[HttpPost("info/{werknemerId}")]
		public ActionResult<WerknemerOutputDTO> VoegInfoToe(long werknemerId, [FromBody] WerknemerInfoInputDTO info) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(info.BedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				// Dit is nogal een vreemde manier om functies toe te voegen, wat heeft Email hiermee te maken?
				werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, info.Email, info.Functies.First());
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, werknemer));
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
		[HttpPut("info/{werknemerId}/{bedrijfId}/{oudeFunctie}/")]
		public ActionResult<WerknemerOutputDTO> BewerkFunctie(long werknemerId, long bedrijfId, string oudeFunctie, [FromQuery] string nieuweFunctie) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				werknemer.WijzigFunctie(bedrijf, oudeFunctie, nieuweFunctie);
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager, werknemer));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}