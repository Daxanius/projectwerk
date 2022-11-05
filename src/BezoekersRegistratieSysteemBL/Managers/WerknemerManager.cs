using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class WerknemerManager
	{
        /// <summary>
        /// Private lokale Interface variabele.
        /// </summary>
        private readonly IWerknemerRepository _werknemerRepository;

        /// <summary>
        /// WerknemerManager constructor krijgt een instantie van de IWerknemerRepository interface als parameter.
        /// </summary>
        /// <param name="werknemerRepository">Interface</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_werknemerRepository] gelijk aan een instantie van de IWerknemerRepository.</remarks>
        public WerknemerManager(IWerknemerRepository werknemerRepository)
		{
			this._werknemerRepository = werknemerRepository;
		}

        /// <summary>
        /// Voegt werknemer toe.
        /// </summary>
        /// <param name="werknemer">Werknemer object dat toegevoegd wenst te worden.</param>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerToe - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerToe - werknemer bestaat al"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
		public Werknemer VoegWerknemerToe(Werknemer werknemer)
		{
			if (werknemer == null)
				throw new WerknemerManagerException("WerknemerManager - VoegWerknemerToe - werknemer mag niet leeg zijn");
			if (_werknemerRepository.BestaatWerknemer(werknemer))
				throw new WerknemerManagerException("WerknemerManager - VoegWerknemerToe - werknemer bestaat al");
			try
			{
				return _werknemerRepository.VoegWerknemerToe(werknemer);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Verwijdert gewenste werknemer uit specifiek bedrijf.
        /// </summary>
        /// <param name="werknemer">Werknemer object dat verwijderd wenst te worden.</param>
        /// <param name="bedrijf">Bedrijf object waaruit werknemer verwijderd wenst te worden.</param>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemer - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemer - werknemer bestaat niet"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
        public void VerwijderWerknemer(Werknemer werknemer, Bedrijf bedrijf)
		{
			if (werknemer == null)
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemer - werknemer mag niet leeg zijn");
			if (!_werknemerRepository.BestaatWerknemer(werknemer))
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemer - werknemer bestaat niet");
			try
			{
				_werknemerRepository.VerwijderWerknemer(werknemer, bedrijf);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Voegt gewenste functie toe aan werknemer uit specifiek bedrijf.
        /// </summary>
        /// <param name="werknemer">Werknemer object die functie toegewezen wenst te worden.</param>
        /// <param name="bedrijf">Bedrijf object waar werknemer werkzaam.</param>
        /// <param name="functie">Functie die toegevoegd wenst te worden.</param>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerFunctieToe - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerFunctieToe - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerFunctieToe - functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerFunctieToe - werknemer bestaat niet"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerFunctieToe - werknemer niet werkzaam bij dit bedrijf"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VoegWerknemerFunctieToe - werknemer heeft deze functie al bij dit bedrijf"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
        public void VoegWerknemerFunctieToe(Werknemer werknemer, Bedrijf bedrijf, string functie)
		{
			if (werknemer == null)
				throw new WerknemerManagerException("WerknemerManager - VoegWerknemerFunctieToe - werknemer mag niet leeg zijn");
			if (bedrijf == null)
				throw new WerknemerManagerException("WerknemerManager - VoegWerknemerFunctieToe - bedrijf mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(functie))
				throw new WerknemerManagerException("WerknemerManager - VoegWerknemerFunctieToe - functie mag niet leeg zijn");
			if (!_werknemerRepository.BestaatWerknemer(werknemer))
				throw new WerknemerManagerException("WerknemerManager - VoegWerknemerFunctieToe - werknemer bestaat niet");
            if (!_werknemerRepository.GeefWerknemer(werknemer.Id).GeefBedrijvenEnFunctiesPerWerknemer().ContainsKey(bedrijf))
                throw new WerknemerManagerException("WerknemerManager - VoegWerknemerFunctieToe - werknemer niet werkzaam bij dit bedrijf");
            if (_werknemerRepository.GeefWerknemer(werknemer.Id).GeefBedrijvenEnFunctiesPerWerknemer()[bedrijf].GeefWerknemerFuncties().Contains(functie))
				throw new WerknemerManagerException("WerknemerManager - VoegWerknemerFunctieToe - werknemer heeft deze functie al bij dit bedrijf");
			try
			{
				functie = Nutsvoorziening.VerwijderWhitespace(functie).ToLower();
				_werknemerRepository.VoegWerknemerFunctieToe(werknemer, bedrijf, functie);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Verwijdert gewenste functie van werknemer uit specifiek bedrijf.
        /// </summary>
        /// <param name="werknemer">Werknemer object waar men de functie van wenst te verwijderen.</param>
        /// <param name="bedrijf">Bedrijf object waar werknemer werkzaam is onder functie.</param>
        /// <param name="functie">Functie die verwijderd wenst te worden.</param>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemerFunctie - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemerFunctie - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemerFunctie - functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemerFunctie - werknemer bestaat niet"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemerFunctie - werknemer niet werkzaam bij dit bedrijf"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemerFunctie - werknemer heeft geen functie bij dit bedrijf"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VerwijderWerknemerFunctie - werknemer moet minstens 1 functie hebben"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
		public void VerwijderWerknemerFunctie(Werknemer werknemer, Bedrijf bedrijf, string functie)
		{
			if (werknemer == null)
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - werknemer mag niet leeg zijn");
			if (bedrijf == null)
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - bedrijf mag niet leeg zijn");
			if (string.IsNullOrWhiteSpace(functie))
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - functie mag niet leeg zijn");
			if (!_werknemerRepository.BestaatWerknemer(werknemer))
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - werknemer bestaat niet");
            if (!_werknemerRepository.GeefWerknemer(werknemer.Id).GeefBedrijvenEnFunctiesPerWerknemer().ContainsKey(bedrijf))
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - werknemer niet werkzaam bij dit bedrijf");
			if (!_werknemerRepository.GeefWerknemer(werknemer.Id).GeefBedrijvenEnFunctiesPerWerknemer()[bedrijf].GeefWerknemerFuncties().Contains(functie))
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - werknemer heeft geen functie bij dit bedrijf");
			if (_werknemerRepository.GeefWerknemer(werknemer.Id).GeefBedrijvenEnFunctiesPerWerknemer()[bedrijf].GeefWerknemerFuncties().Count() == 1)
				throw new WerknemerManagerException("WerknemerManager - VerwijderWerknemerFunctie - werknemer moet minstens 1 functie hebben");
			try
			{
				functie = Nutsvoorziening.VerwijderWhitespace(functie).ToLower();
				_werknemerRepository.VerwijderWerknemerFunctie(werknemer, bedrijf, functie);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Bewerkt gegevens van een werknemer adhv werknemer object en bedrijf object.
        /// </summary>
        /// <param name="werknemer">Werknemer object dat gewijzigd wenst te worden.</param>
        /// <param name="bedrijf">bedrijf object waaruit werknemer gewijzigd wenst te worden.</param>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - BewerkWerknemer - werknemer mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - BewerkWerknemer - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - BewerkWerknemer - werknemer bestaat niet"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - BewerkWerknemer - werknemer is niet gewijzigd"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
		public void BewerkWerknemer(Werknemer werknemer, Bedrijf bedrijf)
		{
			if (werknemer == null)
				throw new WerknemerManagerException("WerknemerManager - BewerkWerknemer - werknemer mag niet leeg zijn");
            if (bedrijf == null)
                throw new WerknemerManagerException("WerknemerManager - BewerkWerknemer - bedrijf mag niet leeg zijn");
            if (!_werknemerRepository.BestaatWerknemer(werknemer))
				throw new WerknemerManagerException("WerknemerManager - BewerkWerknemer - werknemer bestaat niet");
			if (_werknemerRepository.GeefWerknemer(werknemer.Id).WerknemerIsGelijk(werknemer))
                throw new WerknemerManagerException("WerknemerManager - BewerkWerknemer - werknemer is niet gewijzigd");
            try
			{
				_werknemerRepository.BewerkWerknemer(werknemer, bedrijf);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Haalt werknemer op adhv parameter werknemer id.
        /// </summary>
        /// <param name="id">Id van de gewenste werknemer.</param>
        /// <returns>Gewenst werknemer object</returns>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - GeefWerknemer - werknemer bestaat niet"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
		public Werknemer GeefWerknemer(long id)
		{
			if (!_werknemerRepository.BestaatWerknemer(id))
				throw new WerknemerManagerException("WerknemerManager - GeefWerknemer - werknemer bestaat niet");
			try
			{
				return _werknemerRepository.GeefWerknemer(id);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Stelt lijst van werknemers samen met enkel lees rechten adhv bedrij object en parameters werknemer voornaam/achternaam.
        /// </summary>
        /// <param name="voornaam">Voornaam van de gewenste werknemer.</param>
        /// <param name="achternaam">Achternaam van de gewenste werknemer.</param>
		/// <param name="bedrijf">Bedrijf object van het gewenste bedrijf.</param>
        /// <returns>IReadOnlyList van werknemer objecten op werknemernaam PER bedrijf.</returns>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - GeefWerknemerOpNaam - naam mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - GeefWerknemerOpNaam - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
		public IReadOnlyList<Werknemer> GeefWerknemersOpNaamPerBedrijf(string voornaam, string achternaam, Bedrijf bedrijf)
		{
			if (string.IsNullOrWhiteSpace(voornaam) || string.IsNullOrWhiteSpace(achternaam))
				throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpNaam - naam mag niet leeg zijn");
            if (bedrijf == null)
                throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpNaam - bedrijf mag niet leeg zijn");
            try
			{
				return _werknemerRepository.GeefWerknemersOpNaamPerBedrijf(voornaam, achternaam, bedrijf.Id);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Stelt lijst van werknemers samen met enkel lees rechten adhv bedrijf object en parameter werknemer functie.
        /// </summary>
        /// <param name="functie">Functie van de gewenste werknemer</param>
		/// <param name="bedrijf">Bedrijf object van het gewenste bedrijf.</param>
        /// <returns>IReadOnlyList van werknemer objecten op werknemerfunctie PER bedrijf.</returns>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - GeefWerknemerOpFunctie - functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - GeefWerknemerOpFunctie - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
        public IReadOnlyList<Werknemer> GeefWerknemersOpFunctiePerBedrijf(string functie, Bedrijf bedrijf)
        {
            if (string.IsNullOrWhiteSpace(functie))
                throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpFunctie - functie mag niet leeg zijn");
            if (bedrijf == null)
                throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpFunctie - bedrijf mag niet leeg zijn");
            try
            {
                return _werknemerRepository.GeefWerknemersOpFunctiePerBedrijf(functie, bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

        /// <summary>
        /// Stelt lijst van werknemers samen met enkel lees rechten adhv bedrijf object.
        /// </summary>
        /// <param name="bedrijf">Bedrijf object van het gewenste bedrijf.</param>
        /// <returns>IReadOnlyList van werknemer objecten.</returns>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - GeefWerknemersPerBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
        public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(Bedrijf bedrijf)
		{
			if (bedrijf == null)
				throw new WerknemerManagerException("WerknemerManager - GeefWerknemersPerBedrijf - bedrijf mag niet leeg zijn");
			try
			{
				return _werknemerRepository.GeefWerknemersPerBedrijf(bedrijf.Id);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        /// <summary>
        /// Voegt functie toe adhv parameter functienaam.
        /// </summary>
        /// <param name="functie">Functie die toegevoegd wenst te worden.</param>
        /// <returns>Boolean - True = Bestaat | False = Bestaat niet</returns>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - VoegFunctieToe - functie mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">"WerknemerManager - VoegFunctieToe - functie bestaat al"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
        public void VoegFunctieToe(string functienaam)
		{
            if (string.IsNullOrWhiteSpace(functienaam))
                throw new WerknemerManagerException("WerknemerManager - VoegFunctieToe - functie mag niet leeg zijn");
            if (_werknemerRepository.BestaatFunctie(functienaam))
                throw new WerknemerManagerException("WerknemerManager - VoegFunctieToe - functie bestaat al");
            try
            {
                functienaam = Nutsvoorziening.VerwijderWhitespace(functienaam).ToLower();
				_werknemerRepository.VoegFunctieToe(functienaam);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

        /// <summary>
        /// Stelt lijst van afspraakloze werknemers samen met enkel lees rechten adhv bedrijf object.
        /// </summary>
        /// <param name="bedrijf">Bedrijf object waar men de werknemer van wenst op te vragen die niet in afspraak zijn.</param>
        /// <returns>IReadOnlyList van werknemer objecten.</returns>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - GeefVrijeWerknemersOpDitMomentVoorBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
        public IReadOnlyList<Werknemer> GeefVrijeWerknemersOpDitMomentVoorBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new WerknemerManagerException("WerknemerManager - GeefVrijeWerknemersOpDitMomentVoorBedrijf - bedrijf mag niet leeg zijn");
            try
            {
                return _werknemerRepository.GeefVrijeWerknemersOpDitMomentVoorBedrijf(bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

        /// <summary>
        /// Stelt lijst van bezette werknemers samen met enkel lees rechten adhv bedrijf object.
        /// </summary>
        /// <param name="bedrijf">Bedrijf object waar men de werknemer van wenst op te vragen die momenteel in afspraak zijn.</param>
        /// <returns>IReadOnlyList van werknemer objecten.</returns>
        /// <exception cref="WerknemerManagerException">"WerknemerManager - GeefBezetteWerknemersOpDitMomentVoorBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="WerknemerManagerException">ex.Message</exception>
        public IReadOnlyList<Werknemer> GeefBezetteWerknemersOpDitMomentVoorBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new WerknemerManagerException("WerknemerManager - GeefBezetteWerknemersOpDitMomentVoorBedrijf - bedrijf mag niet leeg zijn");
            try
            {
                return _werknemerRepository.GeefBezetteWerknemersOpDitMomentVoorBedrijf(bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }
    }
}