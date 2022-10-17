﻿using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace BezoekersRegistratieSysteemREST.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class AfspraakController : ControllerBase {
		private readonly AfspraakManager _afspraakManager;
		private readonly BezoekerManager _bezoekerManager;
		private readonly WerknemerManager _werknemerManager;

		public AfspraakController(AfspraakManager afspraakManager, BezoekerManager bezoekerManager, WerknemerManager werknemerManager) {
			_afspraakManager = afspraakManager;
			_bezoekerManager = bezoekerManager;
			_werknemerManager = werknemerManager;
		}

		/// <summary>
		/// Geeft een afspraak op ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Afspraak> GeefAfspraak(uint id) {
			try {
				return _afspraakManager.GeefAfspraak(id);
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Geef alle afspraken die overeenkomen met de
		/// gegeven query
		/// </summary>
		/// <param name="dag"></param>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		[HttpGet]
		public IEnumerable<Afspraak> GeefAfspraken([FromQuery] DateTime? dag, [FromQuery] Werknemer? werknemer) {
			// Als beide de dag EN de werknemer zijn meegegeven
			if (werknemer != null && dag != null) {
				return _afspraakManager.GeefAfsprakenPerWerknemerOpDag(werknemer, dag ?? DateTime.Now);
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
		[HttpDelete("{id}")]
		public IActionResult VerwijderAfspraak(uint id) {
			try {
				_afspraakManager.VerwijderAfspraak(id);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Maakt een afspraak op de ingestelde tijd voor bezoeker en werknemer
		/// </summary>
		/// <param name="bezoeker"></param>
		/// <param name="werknemer"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<Afspraak> MaakAfspraak([FromQuery] uint bezoekerId, [FromQuery] uint werknemerId) {
			try {
				/*
					Kunnen werknemer en bezoeker ids zijn? Zou dit niet ook beter de afspraak returnen?
					Ik gebruik hier nu DateTime.Now, om op dit moment een afspraak te maken, maar dit 
					laat mij even denken. Hoe regelen wij tijdzones? Als we DateTime.Now gebruiken
					dan is de starttijd ingesteld op de lokale tijd van de server, en niet op de tijd
					van de client.
				*/

				// De bezoeker en de werknemer ophalen
				Bezoeker bezoeker = _bezoekerManager.GeefBezoeker(bezoekerId);
				Werknemer werknemer = _werknemerManager.GeefWerknemer(werknemerId);

				Afspraak afspraak = new(0, DateTime.Now, null, bezoeker, werknemer);
				_afspraakManager.VoegAfspraakToe(afspraak);
				return afspraak;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}

		/// <summary>
		/// Beeindig een afspraak
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("end")]
		public IActionResult End([FromBody] Afspraak afspraak) {
			if (afspraak == null) return BadRequest($"{nameof(afspraak)} is null");

			try {
				_afspraakManager.BeeindigAfspraakBezoeker(afspraak);
				return Ok();
			} catch (Exception ex) {
				return NotFound(ex.Message);
			}
		}

		/// <summary>
		/// Bewerk een afspraak
		/// </summary>
		/// <param name="afspraak"></param>
		/// <returns></returns>
		[HttpPut]
		public ActionResult<Afspraak> BewerkAfspraak([FromBody] Afspraak afspraak) {
			if (afspraak == null) return BadRequest($"{nameof(afspraak)} is null");

			try {
				_afspraakManager.BewerkAfspraak(afspraak);
				return afspraak;
			} catch (Exception ex) {
				return BadRequest(ex.Message);
			}
		}
	}
}