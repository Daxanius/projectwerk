using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class BedrijfManager
	{
        /// <summary>
        /// Private lokale Interface variabele.
        /// </summary>
        private readonly IBedrijfRepository _bedrijfRepository;
		private readonly IAfspraakRepository _afspraakRepository;

        /// <summary>
        /// BedrijfManager constructor krijgt een instantie van de IBedrijfRepository interface als parameter.
        /// </summary>
        /// <param name="bedrijfRepository">Interface</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_bedrijfRepository] gelijk aan een instantie van de IBedrijfRepository.</remarks>
        public BedrijfManager(IBedrijfRepository bedrijfRepository, IAfspraakRepository afspraakRepository)
		{
			this._bedrijfRepository = bedrijfRepository;
            this._afspraakRepository = afspraakRepository;
        }

		/// <summary>
		/// Voegt bedrijf toe in de databank adhv een bedrijf object.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object dat toegevoegd wenst te worden.</param>
		/// <returns>Gewenste bedrijf object MET id</returns>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - VoegBedrijfToe - Bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - VoegBedrijfToe - bedrijf bestaat al"</exception>
		/// <exception cref="BedrijfManagerException">ex.Message</exception>
		public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new BedrijfManagerException("BedrijfManager - VoegBedrijfToe - Bedrijf mag niet leeg zijn");
			if (_bedrijfRepository.BestaatBedrijf(bedrijf))
				throw new BedrijfManagerException("BedrijfManager - VoegBedrijfToe - bedrijf bestaat al");
			try {
				return _bedrijfRepository.VoegBedrijfToe(bedrijf);
			} catch (Exception ex) {
				throw new BedrijfManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Verwijdert gewenste bedrijf adhv een bedrijf object.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object dat verwijderd wenst te worden.</param>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - VerwijderBedrijf - Bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - VerwijderBedrijf - bedrijf bestaat niet"</exception>
		/// <exception cref="BedrijfManagerException">ex.Message</exception>
		public void VerwijderBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new BedrijfManagerException("BedrijfManager - VerwijderBedrijf - mag niet leeg zijn");
			if (!_bedrijfRepository.BestaatBedrijf(bedrijf))
				throw new BedrijfManagerException("BedrijfManager - VerwijderBedrijf - bedrijf bestaat niet");
            if (_afspraakRepository.GeefHuidigeAfsprakenPerBedrijf(bedrijf.Id).Count > 0)
                throw new BedrijfManagerException("BedrijfManager - VerwijderBedrijf - bedrijf heeft lopende afspraken");
            try
			{
				_bedrijfRepository.VerwijderBedrijf(bedrijf.Id);
			} catch (Exception ex) {
				throw new BedrijfManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Bewerkt gegevens van een bedrijf adhv bedrijf object.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object dat gewijzigd wenst te worden.</param>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - BewerkBedrijf - bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - BewerkBedrijf - bedrijf bestaat niet"</exception>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - BewerkBedrijf - bedrijf is niet gewijzigd"</exception>
		/// <exception cref="BedrijfManagerException">ex.Message</exception>
		public void BewerkBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new BedrijfManagerException("BedrijfManager - BewerkBedrijf - bedrijf mag niet leeg zijn");
			if (!_bedrijfRepository.BestaatBedrijf(bedrijf))
				throw new BedrijfManagerException("BedrijfManager - BewerkBedrijf - bedrijf bestaat niet");
			if (_bedrijfRepository.GeefBedrijf(bedrijf.Id).BedrijfIsGelijk(bedrijf))
				throw new BedrijfManagerException("BedrijfManager - BewerkBedrijf - bedrijf is niet gewijzigd");
			try {
				_bedrijfRepository.BewerkBedrijf(bedrijf);
			} catch (Exception ex) {
				throw new BedrijfManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Haalt bedrijf op adhv parameter bedrijf id.
		/// </summary>
		/// <param name="id">Id van het gewenste bedrijf.</param>
		/// <returns>Gewenst bedrijf object</returns>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - GeefBedrijf - bedrijf bestaat niet"</exception>
		/// <exception cref="BedrijfManagerException">ex.Message</exception>
		public Bedrijf GeefBedrijf(long id) {
			if (!_bedrijfRepository.BestaatBedrijf(id))
				throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijf bestaat niet");
			try {
				return _bedrijfRepository.GeefBedrijf(id);
			} catch (Exception ex) {
				throw new BedrijfManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Haalt bedrijven op uit de databank.
		/// </summary>
		/// <returns>IReadOnlyList van bedrijf objecten.</returns>
		/// <exception cref="BedrijfManagerException">ex.Message</exception>
		public IReadOnlyList<Bedrijf> GeefBedrijven() {
			try {
				return _bedrijfRepository.GeefBedrijven();
			} catch (Exception ex) {
				throw new BedrijfManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Haalt alle bedrijven op adhv een bedrijfnaam.
		/// </summary>
		/// <param name="bedrijfsnaam">Naam van het gewenste bedrijf.</param>
		/// <returns>Gewenst bedrijf object</returns>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - GeefBedrijf - bedrijfsnaam mag niet leeg zijn"</exception>
		/// <exception cref="BedrijfManagerException">"BedrijfManager - GeefBedrijf - bedrijf bestaat niet"</exception>
		/// <exception cref="BedrijfManagerException">ex.Message</exception>
		public Bedrijf GeefBedrijf(string bedrijfsnaam) {
			if (string.IsNullOrWhiteSpace(bedrijfsnaam))
				throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijfsnaam mag niet leeg zijn");
			if (!_bedrijfRepository.BestaatBedrijf(bedrijfsnaam))
				throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijf bestaat niet");
			try {
				return _bedrijfRepository.GeefBedrijf(bedrijfsnaam);
			} catch (Exception ex) {
				throw new BedrijfManagerException(ex.Message);
			}
		}
	}
}