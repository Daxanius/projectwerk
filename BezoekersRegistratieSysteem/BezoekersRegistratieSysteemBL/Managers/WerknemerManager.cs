using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers
{
	public class WerknemerManager
	{
		private readonly IWerknemerRepository _werknemerRepository;

		public WerknemerManager(IWerknemerRepository werknemerRepository)
		{
			this._werknemerRepository = werknemerRepository;
		}

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
				_werknemerRepository.VoegWerknemerFunctieToe(werknemer, bedrijf, functie);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

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
				_werknemerRepository.VerwijderWerknemerFunctie(werknemer, bedrijf, functie);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

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

		public IReadOnlyList<Werknemer> GeefWerknemersOpNaam(string voornaam, string achternaam)
		{
			if (string.IsNullOrWhiteSpace(voornaam) || string.IsNullOrWhiteSpace(achternaam))
				throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpNaam - naam mag niet leeg zijn");
			try
			{
				return _werknemerRepository.GeefWerknemersOpNaam(voornaam, achternaam);
			} catch (Exception ex)
			{
				throw new WerknemerManagerException(ex.Message);
			}
		}

        public IReadOnlyList<Werknemer> GeefWerknemersOpFunctie(string functie)
        {
            if (string.IsNullOrWhiteSpace(functie))
                throw new WerknemerManagerException("WerknemerManager - GeefWerknemerOpFunctie - functie mag niet leeg zijn");
            try
            {
                return _werknemerRepository.GeefWerknemersOpFunctie(functie);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

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

        public void VoegFunctieToe(string functie)
		{
            if (string.IsNullOrWhiteSpace(functie))
                throw new WerknemerManagerException("WerknemerManager - VoegFunctieToe - functie mag niet leeg zijn");
            if (_werknemerRepository.BestaatFunctie(functie))
                throw new WerknemerManagerException("WerknemerManager - VoegFunctieToe - functie bestaat al");
            try
            {
                _werknemerRepository.VoegFunctieToe(functie);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Werknemer> GeefVrijeWerknemersOpDitMomentVoorBedrijf(Bedrijf bedrijf)
        {
            try
            {
                return _werknemerRepository.GeefVrijeWerknemersOpDitMomentVoorBedrijf(bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new WerknemerManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Werknemer> GeefBezetteWerknemersOpDitMomentVoorBedrijf(Bedrijf bedrijf)
        {
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