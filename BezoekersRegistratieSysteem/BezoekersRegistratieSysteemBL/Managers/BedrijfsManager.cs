using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
    public class BedrijfsManager {
        private List<Bedrijf> _bedrijven = new List<Bedrijf>();

        private IBedrijfRepository _bedrijfRepository;

        public BedrijfsManager(IBedrijfRepository bedrijfRepository) {
            this._bedrijfRepository = bedrijfRepository;
        }

        public void VoegBedrijfToe(string naam, string btw, string adres, string email, string telefoonnummer) {
            Bedrijf bedrijf = new Bedrijf(naam, btw, adres, email, telefoonnummer);
            if (_bedrijven.Contains(bedrijf)) throw new BedrijfManagerException("Bedrijf bestaat al");
            _bedrijven.Add(bedrijf);
            _bedrijfRepository.VoegBedrijfToe(bedrijf);
        }

        public void VerwijderBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new BedrijfManagerException("Bedrijf mag niet leeg zijn");
            if (!_bedrijven.Contains(bedrijf)) throw new BedrijfManagerException("Bedrijf bestaat niet");
            _bedrijven.Remove(bedrijf);
            _bedrijfRepository.VerwijderBedrijf(bedrijf.Id);
        }

        public void BewerkBedrijf(Bedrijf bedrijf) {
            if (bedrijf == null) throw new BedrijfManagerException("Bedrijf mag niet leeg zijn");
            if (!_bedrijven.Contains(bedrijf)) throw new BedrijfManagerException("Bedrijf bestaat niet");
            _bedrijven.Remove(bedrijf);
            _bedrijven.Add(bedrijf);
            _bedrijfRepository.WijzigBedrijf(bedrijf.Id, bedrijf);
        }

        public IReadOnlyList<Bedrijf> Geefbedrijven() {
            return _bedrijven.AsReadOnly();
        }

        public Bedrijf GeefBedrijf(string bedrijfsnaam) {
            if (string.IsNullOrWhiteSpace(bedrijfsnaam)) throw new BedrijfManagerException("Bedrijfsnaam mag niet leeg zijn");
            foreach (Bedrijf bedrijf in _bedrijven) {
                if (bedrijf.Naam.Equals(bedrijfsnaam)) return bedrijf;
            }
            throw new BedrijfManagerException("Bedrijf bestaat niet");
        }
    }
}