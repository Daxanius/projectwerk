using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
    public class BedrijfsManager {
        private readonly Dictionary<uint, Bedrijf> _bedrijven = new();

        private readonly IBedrijfRepository _bedrijfRepository;

        public BedrijfsManager(IBedrijfRepository bedrijfRepository) {
            this._bedrijfRepository = bedrijfRepository;
        }

        public void VoegBedrijfToe(string naam, string btw, string adres, string email, string telefoonnummer) {
            Bedrijf bedrijf = new(naam, btw, adres, email, telefoonnummer);
            if (_bedrijven.ContainsKey(bedrijf.Id)) throw new BedrijfManagerException("Bedrijf bestaat al");
            _bedrijven.Add(bedrijf.Id, bedrijf);
            _bedrijfRepository.VoegBedrijfToe(bedrijf);
        }

        public void VerwijderBedrijf(uint id) {
            if (!_bedrijven.ContainsKey(id)) throw new BedrijfManagerException("Bedrijf bestaat niet");
            _bedrijven.Remove(id);
            _bedrijfRepository.VerwijderBedrijf(id);
        }

        public void BewerkBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new BedrijfManagerException("Bedrijf mag niet leeg zijn");
            if (!_bedrijven.ContainsKey(bedrijf.Id)) throw new BedrijfManagerException("Bedrijf bestaat niet");
            _bedrijven.Remove(bedrijf.Id);
            _bedrijven.Add(bedrijf.Id, bedrijf);
            _bedrijfRepository.WijzigBedrijf(bedrijf.Id, bedrijf);
        }

        public IEnumerable<Bedrijf> Geefbedrijven() {
            return _bedrijven.Values;
        }

        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            if (string.IsNullOrWhiteSpace(bedrijfsnaam)) throw new BedrijfManagerException("Bedrijfsnaam mag niet leeg zijn");
            foreach (Bedrijf bedrijf in _bedrijven.Values) {
                if (bedrijf.Naam.Equals(bedrijfsnaam)) return bedrijf;
            }
            throw new BedrijfManagerException("Bedrijf bestaat niet");
        }
    }
}