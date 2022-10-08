using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions;

namespace BezoekersRegistratieSysteemBL.Managers
{
    public class BedrijfsManager
    {
        private List<Bedrijf> _bedrijven = new List<Bedrijf>();

        public void VoegBedrijfToe(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new BedrijfManagerException("Bedrijf mag niet leeg zijn");
            _bedrijven.Add(bedrijf);
        }

        public void VerwijderBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new BedrijfManagerException("Bedrijf mag niet leeg zijn");
            _bedrijven.Remove(bedrijf);
        }

        public void BewerkBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new BedrijfManagerException("Bedrijf mag niet leeg zijn");
            _bedrijven.Remove(bedrijf);
            _bedrijven.Add(bedrijf);
        }

        public IReadOnlyList<Bedrijf> Geefbedrijven()
        {
            return _bedrijven.AsReadOnly();
        }

        public Bedrijf GeefBedrijf(string bedrijfsnaam)
        {
            if (string.IsNullOrWhiteSpace(bedrijfsnaam)) throw new BedrijfManagerException("Bedrijfsnaam mag niet leeg zijn");
            foreach (Bedrijf bedrijf in _bedrijven)
            {
                if (bedrijf.Naam == bedrijfsnaam) return bedrijf;
                else throw new BedrijfManagerException("Bedrijf bestaat niet");
            }
            return null;
        }
    }
}
