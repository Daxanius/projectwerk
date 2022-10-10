﻿using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AfspraakController : ControllerBase {
		private readonly AfspraakManager _afspraakManager;

		public AfspraakController(AfspraakManager afspraakManager) {
			_afspraakManager = afspraakManager;
		}

		/// <summary>
		/// Geeft een afspraak op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpGet("{id}")]
		public ActionResult<Afspraak> GeefAfspraak(uint id) {
			try {
				return _afspraakManager.GeefAfspraak(id);
			} catch (Exception) {
				return NotFound();
			}
		}

		/// <summary>
		/// Geef alle afspraken die overeenkomen met de
		/// gegeven query
		/// </summary>
		/// <param name="dag"></param>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpGet]
		public IEnumerable<Afspraak> GeefAfspraken([FromQuery] DateTime? dag, [FromQuery] Werknemer? werknemer) {
			// Als beide de dag EN de werknemer zijn meegegeven
			if (werknemer != null && dag != null) {
				return _afspraakManager.GeefAfsprakenPerWerknemerOpDatum(werknemer, dag ?? DateTime.Now);
			}

			// Als alleen de dag is meegegeven
			if (dag != null) {
				return _afspraakManager.GeefAfsprakenPerDag(dag ?? DateTime.Now);
			}

			// Als alleen de werknemer is meegegeven
			if (werknemer != null) {
				// Zou dit niet ook beter een ID zijn?
				return _afspraakManager.GeefAlleAfsprakenPerWerknemer(werknemer);
			}

			// Als niets is meegegeven
			return _afspraakManager.GeefHuidigeAfspraken();
		}

		/// <summary>
		/// Verwijder een afspraak
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpDelete("{id}")]
		public IActionResult VerwijderAfspraak(uint id) {
			try {
				_afspraakManager.VerwijderAfspraak(id);
				return Ok();
			} catch (Exception) {
				return NotFound();
			}
		}

		/// <summary>
		/// Maakt een afspraak op de ingestelde tijd voor bezoeker en werknemer
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		[HttpPut]
		public IActionResult MaakAfspraak([FromQuery] Bezoeker bezoeker, [FromQuery] Werknemer werknemer) {
			if (bezoeker == null) {
				return BadRequest();
			}

			try {
				/*
					Kunnen werknemer en bezoeker ids zijn? Zou dit niet ook beter de afspraak returnen?
					Ik gebruik hier nu DateTime.Now, om op dit moment een afspraak te maken, maar dit 
					laat mij even denken. Hoe regelen wij tijdzones? Als we DateTime.Now gebruiken
					dan is de starttijd ingesteld op de lokale tijd van de server, en niet op de tijd
					van de client.
				*/
				_afspraakManager.MaakAfspraak(DateTime.Now, bezoeker, werknemer);
				return Ok();
			} catch (Exception) {
				return BadRequest();
			}
		}

		/// <summary>
		/// Beeindig een afspraak
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		[HttpPost]
		[Route("end")]
		public IActionResult End([FromBody] Afspraak afspraak) {
			if (afspraak == null) {
				return BadRequest();
			}

			try {
				_afspraakManager.BeeindigAfspraak(afspraak);
				return Ok();
			} catch (Exception) {
				return NotFound();
			}
		}
	}
}