﻿using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class BedrijfController : ControllerBase {
		private readonly BedrijfsManager _bedrijfManager;

		public BedrijfController(BedrijfsManager afspraakManager) {
			_bedrijfManager = afspraakManager;
		}

		/// <summary>
		/// Verkrijgt een bedrijf op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Bedrijf> GeefBedrijf(uint id) {
			try {
				return _bedrijfManager.GeefBedrijf(id);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Verkrijgt een bedrijf op naam
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{naam}")]
		public ActionResult<Bedrijf> GeefBedrijf(string naam) {
			// Je kunt nooit genoeg controles hebben
			if (string.IsNullOrEmpty(naam)) return BadRequest();

			try {
				return _bedrijfManager.GeefBedrijf(naam);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		[HttpGet]
		public IEnumerable<Bedrijf> GeefAlleBedrijven() {
			// Kan dit fout gaan?
			return _bedrijfManager.Geefbedrijven();
		}

		/// <summary>
		/// Verwijdert een bedrijf, vraagt momenteel
		/// het ID als parameter
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		public IActionResult VerwijderBedrijf([FromBody] uint id) {
			try {
				_bedrijfManager.VerwijderBedrijf(id);
				return Ok();
			} catch (Exception ex) {
				// Welke IActionResults zijn er??
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Voegt een bedrijf toe
		/// </summary>
		/// <param name="bedrijf"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<Bedrijf> VoegBedrijfToe([FromBody] Bedrijf bedrijf) {
			if (bedrijf == null) return BadRequest();

			try {
				_bedrijfManager.VoegBedrijfToe(bedrijf.Naam, bedrijf.BTW, bedrijf.Adres, bedrijf.Email, bedrijf.TelefoonNummer);
				return bedrijf;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}