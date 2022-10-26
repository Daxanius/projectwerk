using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
    public class BedrijfManager {

        private readonly IBedrijfRepository _bedrijfRepository;

        public BedrijfManager(IBedrijfRepository bedrijfRepository) {
            this._bedrijfRepository = bedrijfRepository;
        }

        public Bedrijf VoegBedrijfToe(Bedrijf bedrijf) {
            if (bedrijf == null) throw new BedrijfManagerException("BedrijfManager - VoegBedrijfToe - Bedrijf mag niet leeg zijn");
            if (_bedrijfRepository.BestaatBedrijf(bedrijf)) throw new BedrijfManagerException("BedrijfManager - VoegBedrijfToe - bedrijf bestaat al");
            try
            {
                return _bedrijfRepository.VoegBedrijfToe(bedrijf);
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public void VerwijderBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new BedrijfManagerException("BedrijfManager - VerwijderBedrijf - mag niet leeg zijn");
            if (!_bedrijfRepository.BestaatBedrijf(bedrijf)) throw new BedrijfManagerException("BedrijfManager - VerwijderBedrijf - bedrijf bestaat niet");
            try
            {
                _bedrijfRepository.VerwijderBedrijf(bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }
        
        public void BewerkBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new BedrijfManagerException("BedrijfManager - UpdateBedrijf - bedrijf mag niet leeg zijn");
            if (!_bedrijfRepository.BestaatBedrijf(bedrijf)) throw new BedrijfManagerException("BedrijfManager - UpdateBedrijf - bedrijf bestaat niet");
            if (_bedrijfRepository.GeefBedrijf(bedrijf.Id).BedrijfIsGelijk(bedrijf)) throw new BedrijfManagerException("BedrijfManager - UpdateBedrijf - bedrijf is niet gewijzigd");
            try
            {
                _bedrijfRepository.BewerkBedrijf(bedrijf);              
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public Bedrijf GeefBedrijf(uint id) {
            if (!_bedrijfRepository.BestaatBedrijf(id)) throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijf bestaat niet");
            try
            {
                return _bedrijfRepository.GeefBedrijf(id);
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public IReadOnlyList<Bedrijf> Geefbedrijven()
        {
            try
            {
                return _bedrijfRepository.Geefbedrijven();
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            if (string.IsNullOrWhiteSpace(bedrijfsnaam)) throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijfsnaam mag niet leeg zijn");
            if (_bedrijfRepository.BestaatBedrijf(bedrijfsnaam)) throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijf bestaat niet");
            try
            {
                return _bedrijfRepository.GeefBedrijf(bedrijfsnaam);
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }
    }
}