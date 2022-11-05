using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WerknemerController : ControllerBase
	{
		private readonly WerknemerManager _werknemerManager;
		private readonly BedrijfManager _bedrijfManager;

		public WerknemerController(WerknemerManager werknemerManager, BedrijfManager bedrijfManager)
		{
			_werknemerManager = werknemerManager;
			_bedrijfManager = bedrijfManager;
		}

		/// <summary>
		/// Geef een werknemer op ID
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpGet("id/{werknemerId}")]
		public ActionResult<WerknemerOutputDTO> GeefWerknemerOpId(long werknemerId)
		{
			try
			{
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemer(werknemerId)));
			} catch (Exception ex)
			{
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
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersOpNaam(string naam, string achternaam)
		{
			try
			{
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersOpNaam(naam, achternaam).AsEnumerable()));
			} catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef een lijst van werknemers op functie
		/// </summary>
		/// <param name="functie"></param>
		/// <returns></returns>
		[HttpGet("functie/{functie}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersOpFunctie(string functie) {
			try {
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersOpFunctie(functie).AsEnumerable()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef alle werknemers van een bedrijf
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("bedrijf/id/{id}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GeefWerknemersPerBedrijf(long id) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(id);

				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.GeefWerknemersPerBedrijf(bedrijf).AsEnumerable()));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verwijder een werknemer van een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpDelete("{beddrijfId}/{werknemerId}")]
		public IActionResult VerwijderWerknemer(long bedrijfId, long werknemerId)
		{
			try
			{
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				_werknemerManager.VerwijderWerknemer(werknemer, bedrijf);
				return Ok();
			} catch (Exception ex)
			{
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
		public ActionResult<WerknemerOutputDTO> VoegWerknemerToe([FromBody] WerknemerInputDTO werknemerData)
		{
			try
			{
				return Ok(WerknemerOutputDTO.NaarDTO(_werknemerManager.VoegWerknemerToe(werknemerData.NaarBusiness())));
			} catch (Exception ex)
			{
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
		public ActionResult<WerknemerOutputDTO> BewerkWerknemer(long werknemerId, long bedrijfId, [FromBody] WerknemerInputDTO werknemerInput)
		{
			try
			{
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = werknemerInput.NaarBusiness();
				werknemer.ZetId(werknemerId);

				// Waarom heeft dit een bedrijf nodig om werknemer te weizigen?
				_werknemerManager.BewerkWerknemer(werknemer, bedrijf);
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geef een lijst met bedrijven en informatie over een werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpGet("info/id/{werknemerId}")]
		public ActionResult<Dictionary<long, WerknemerInfoOutputDTO>> GeefBedrijvenEnFunctiesPerWerknemer(long werknemerId)
		{
			try
			{
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				var bedrijven = werknemer.GeefBedrijvenEnFunctiesPerWerknemer();

				// Een conversie naar de DTO
				Dictionary<long, WerknemerInfoOutputDTO> output = new();
				foreach (Bedrijf b in bedrijven.Keys)
				{
					output.Add(b.Id, WerknemerInfoOutputDTO.NaarDTO(bedrijven[b]));
				}

				return Ok(output);
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een functie toe
		/// </summary>
		/// <param name="naam"></param>
		/// <returns></returns>
		[HttpPost("functie/{naam}")]
		public IActionResult VoegFunctieToe(string naam) {
			try {
				_werknemerManager.VoegFunctieToe(naam);
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een werknemer functie toe
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfID"></param>
		/// <param name="naam"></param>
		/// <returns></returns>
		[HttpPost("functie/{werknemerId}/{bedrijfId}/{naam}")]
		public IActionResult VoegWerknemerFunctieToe(long werknemerId, long bedrijfID, string naam) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfID);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				_werknemerManager.VoegWerknemerFunctieToe(werknemer, bedrijf, naam);
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijder een werknemer functie
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfID"></param>
		/// <param name="naam"></param>
		/// <returns></returns>
		[HttpDelete("functie/{werknemerId}/{bedrijfId}/{naam}")]
		public IActionResult VerwijderWerknemerFunctie(long werknemerId, long bedrijfID, string naam) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfID);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				_werknemerManager.VerwijderWerknemerFunctie(werknemer, bedrijf, naam);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voeg info toe aan werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		[HttpPost("info/id/{werknemerId}")]
		public ActionResult<WerknemerOutputDTO> VoegInfoToe(long werknemerId, [FromBody] WerknemerInfoInputDTO info)
		{
			try
			{
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(info.BedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				// Dit is nogal een vreemde manier om functies toe te voegen, wat heeft Email hiermee te maken?
				werknemer.VoegBedrijfEnFunctieToeAanWerknemer(bedrijf, info.Email, info.Functies.First());
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een functie van een werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		/// <param name="oudeFunctie"></param>
		/// <returns></returns>
		[HttpPut("info/{werknemerId}/{bedrijfId}/{oudeFunctie}/")]
		public ActionResult<WerknemerOutputDTO> BewerkFunctie(long werknemerId, long bedrijfId, string oudeFunctie, [FromQuery] string nieuweFunctie)
		{
			try
			{
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				werknemer.WijzigFunctie(bedrijf, oudeFunctie, nieuweFunctie);
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijder een functie van werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		/// <param name="functie"></param>
		/// <returns></returns>
		[HttpDelete("info/{werknemerId}/{bedrijfId}/{functie}/")]
		public ActionResult<WerknemerOutputDTO> VerwijderFunctie(long werknemerId, long bedrijfId, string functie)
		{
			try
			{
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				werknemer.VerwijderFunctie(bedrijf, functie);
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijder een bedrijf van werknemer
		/// </summary>
		/// <param name="werknemerId"></param>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpDelete("info/{werknemerId}/{bedrijfId}")]
		public ActionResult<WerknemerOutputDTO> VerwijderBedrijf(long werknemerId, long bedrijfId)
		{
			try
			{
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				werknemer.VerwijderBedrijfVanWerknemer(bedrijf);
				return Ok(WerknemerOutputDTO.NaarDTO(werknemer));
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}