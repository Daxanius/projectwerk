using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
    public class BedrijfManager {
        private readonly Dictionary<uint, Bedrijf> _bedrijven = new();

        private readonly IBedrijfRepository _bedrijfRepository;

        public BedrijfManager(IBedrijfRepository bedrijfRepository) {
            this._bedrijfRepository = bedrijfRepository;
        }

        public void VoegBedrijfToe(Bedrijf bedrijf) {
            if (_bedrijven.ContainsKey(bedrijf.Id)) throw new BedrijfManagerException("BedrijfManager - VoegBedrijfToe - bedrijf bestaat al");
            try
            {
                _bedrijven.Add(bedrijf.Id, bedrijf);
                _bedrijfRepository.VoegBedrijfToe(bedrijf);
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public void VerwijderBedrijf(uint id) {
            if (!_bedrijven.ContainsKey(id)) throw new BedrijfManagerException("BedrijfManager - VerwijderBedrijf - bedrijf bestaat niet");
            try
            {
                _bedrijven.Remove(id);
                _bedrijfRepository.VerwijderBedrijf(id);
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }
        public void BewerkBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new BedrijfManagerException("BedrijfManager - UpdateBedrijf - bedrijf mag niet leeg zijn");
            if (!_bedrijven.ContainsKey(bedrijf.Id)) throw new BedrijfManagerException("BedrijfManager - UpdateBedrijf - bedrijf bestaat niet");
            try
            {
                _bedrijven.Remove(bedrijf.Id);
                _bedrijven.Add(bedrijf.Id, bedrijf);
                _bedrijfRepository.BewerkBedrijf(bedrijf.Id, bedrijf);              
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public Bedrijf GeefBedrijf(uint id) {
            if (!_bedrijven.ContainsKey(id)) throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijf bestaat niet");
            try
            {
                return _bedrijven[id];
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public IEnumerable<Bedrijf> Geefbedrijven()
        {
            try
            {
                return _bedrijven.Values;
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }

        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            if (string.IsNullOrWhiteSpace(bedrijfsnaam)) throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijfsnaam mag niet leeg zijn");
            try
            {
                foreach (Bedrijf bedrijf in _bedrijven.Values) {
                    if (bedrijf.Naam.Equals(bedrijfsnaam)) return bedrijf;
                }
                throw new BedrijfManagerException("BedrijfManager - GeefBedrijf - bedrijf bestaat niet");
            }
            catch (Exception ex)
            {
                throw new BedrijfManagerException(ex.Message);
            }
        }
    }
}