using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class WerknemerController : ControllerBase {
		private readonly WerknemerManager _werknemerManager;
		private readonly BedrijfManager _bedrijfManager;

		public WerknemerController(WerknemerManager werknemerManager, BedrijfManager bedrijfManager) {
			_werknemerManager = werknemerManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geef een werknemer op ID
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpGet("{werknemerId}")]
		public ActionResult<WerknemerOutputDTO> GeefWerknemer(uint werknemerId) {
			try {
				return WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemer(werknemerId));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef een werknemer op naam
		/// </summary>
		/// <param name="naam"></param>
		/// <param name="achternaam"></param>
		/// <returns></returns>
		[HttpGet("{naam}/{achternaam}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersOpNaam(string naam, string achternaam) {
			try {
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersOpNaam(naam, achternaam).AsEnumerable()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef werknemers per bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("{bedrijfId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersPerBedrijf(uint bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersPerBedrijf(bedrijf)));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijder een werknemer van een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpDelete("{beddrijfId}/{werknemerId}")]
		public IActionResult VerwijderWerknemer(uint bedrijfId, uint werknemerId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				_werknemerManager.VerwijderWerknemer(werknemer, bedrijf);
				return Ok();
			} catch (Exception ex) {
				// Welke IActionResults zijn er??
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een werknemer toe
		/// </summary>
		/// <param name="werknemerData"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<WerknemerOutputDTO> VoegWerknemerToe([FromBody] WerknemerInputDTO werknemerData) {
			try {
				return WerknemerOutputDTO.NaarDTO(_werknemerManager.VoegWerknemerToe(werknemerData.NaarBusiness()));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geef een lijst met bedrijven en informatie over een werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpGet("info/{werknemerId}")]
		public ActionResult<Dictionary<uint, WerknemerInfoOutputDTO>> GeefBedrijvenEnFunctiesPerWerknemer(uint werknemerId) {
			try {
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				var bedrijven = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();

				// Een conversie naar de DTO
				Dictionary<uint, WerknemerInfoOutputDTO> output = new();
				foreach (Bedrijf b in bedrijven.Keys) {
					output.Add(b.Id, WerknemerInfoOutputDTO.NaarDTO(bedrijven[b]));
				}

				return Ok(output);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Voeg info toe aan werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		[HttpPost("info/{werknemerId}")]
		public ActionResult<WerknemerOutputDTO> VoegInfoToe(uint werknemerId, [FromBody] WerknemerInfoInputDTO info) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(info.BedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				
				// Dit is nogal een vreemde manier om functies toe te voegen, wat heeft Email hiermee te maken?
				werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, info.Email, info.Functies.First());
				return WerknemerOutputDTO.NaarDTO(werknemer);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Wijzig werknemerinfo van een bedrijf
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemerInput"></param>
		/// <returns></returns>
		[HttpPut("{werknemerId}/{bedrijfId}")]
		public ActionResult<WerknemerOutputDTO> BewerkWerknemer(uint werknemerId, uint bedrijfId, [FromBody] WerknemerInputDTO werknemerInput) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = werknemerInput.NaarBusiness();
				werknemer.ZetId(werknemerId);

				// Waarom heeft dit een bedrijf nodig om werknemer te weizigen?
				_werknemerManager.WijzigWerknemer(werknemer, bedrijf);
				return WerknemerOutputDTO.NaarDTO(werknemer);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}