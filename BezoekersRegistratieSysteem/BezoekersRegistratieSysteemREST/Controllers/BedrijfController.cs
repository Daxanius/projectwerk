﻿using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using BezoekersRegistratieSysteemREST.Model;
using BezoekersRegistratieSysteemREST.Model.Input;
using BezoekersRegistratieSysteemREST.Model.Output;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		private readonly BedrijfManager _bedrijfManager;
		private readonly WerknemerManager _werknemerManager;

		public BedrijfController(BedrijfManager afspraakManager, WerknemerManager werknemerManager) {
			_bedrijfManager = afspraakManager;
			_werknemerManager = werknemerManager;
		}

		/// <summary>
		/// Verkrijgt een bedrijf op ID
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<BedrijfOutputDTO> GeefBedrijf(uint bedrijfId) {
			try {
				return BedrijfOutputDTO.NaarDTO(_bedrijfManager.GeefBedrijf(bedrijfId));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verkrijg een bedrijf op naam
		/// </summary>
		/// <param name="bedrijfNaam"></param>
		/// <returns></returns>
		[HttpGet("{naam}")]
		public ActionResult<BedrijfOutputDTO> GeefBedrijf(string bedrijfNaam) {
			try {
				return BedrijfOutputDTO.NaarDTO(_bedrijfManager.GeefBedrijf(bedrijfNaam));
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geeft alle bedrijven
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult<IEnumerable<BedrijfOutputDTO>> GeefAlleBedrijven() {
			try {
				// Kan dit fout gaan?
				return Ok(BedrijfOutputDTO.NaarDTO(_bedrijfManager.Geefbedrijven()));
			} catch (Exception ex) {
				return BadRequest(ex);
			}
		}

		/// <summary>
		/// Verwijdert een bedrijf, vraagt momenteel
		/// het ID als parameter
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpDelete("{bedrijfId}")]
		public IActionResult VerwijderBedrijf(uint bedrijfId) {
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
		/// Voegt een bedrijf toe
		/// </summary>
		/// <param name="bedrijfData"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<BedrijfOutputDTO> VoegBedrijfToe([FromBody] BedrijfInputDTO bedrijfData) {
			try {
				return BedrijfOutputDTO.NaarDTO(_bedrijfManager.VoegBedrijfToe(bedrijfData.NaarBusiness()));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="bedrijfInput"></param>
		/// <returns></returns>
		[HttpPut("{bedrijfId}")]
		public ActionResult<BedrijfOutputDTO> BewerkBedrijf(uint bedrijfId, [FromBody] BedrijfInputDTO bedrijfInput) {
			try {
				Bedrijf bedrijf = bedrijfInput.NaarBusiness();
				bedrijf.ZetId(bedrijfId);
				
				_bedrijfManager.BewerkBedrijf(bedrijf);
				return BedrijfOutputDTO.NaarDTO(bedrijf);
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Geef de werknemers van dit bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <returns></returns>
		[HttpGet("werknemer/{bedrijfId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> GetWerknemers(uint bedrijfId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);

				return Ok(WerknemerOutputDTO.NaarDTO(bedrijf.GeefWerknemers()));
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Verwijder een werkneemr uit een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemerId"></param>
		/// <returns></returns>
		[HttpDelete("werknemer/{bedrijfId}/{werknemerId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> VerwijderWerknemerUitBedrijf(uint bedrijfId, uint werknemerId) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);
				bedrijf.VerwijderWerknemerUitBedrijf(werknemer);
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Voeg een werknemer toe aan een bedrijf
		/// </summary>
		/// <param name="bedrijfId"></param>
		/// <param name="werknemerId"></param>
		/// <param name="werknemerInfo"></param>
		/// <returns></returns>
		[HttpPost("werknemer/{bedrijfId}/{werknemerId}")]
		public ActionResult<IEnumerable<WerknemerOutputDTO>> VoegwegnemerToeAanBedrijf(uint bedrijfId, uint werknemerId, [FromBody] WerknemerInfoInputDTO werknemerInfo) {
			try {
				Bedrijf bedrijf = _bedrijfManager.GeefBedrijf(bedrijfId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				// Deze implementatie in de BL is questionable
				bedrijf.VoegWerknemerToeInBedrijf(werknemer, werknemerInfo.Email, werknemerInfo.Functies.First());
				return Ok();
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}