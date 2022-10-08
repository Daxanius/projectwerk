using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers
{
    public class BezoekerManager
    {
        private IBezoekerRepository _bezoekerRepository;

        public BezoekerManager(IBezoekerRepository bezoekerRepository)
        {
            this._bezoekerRepository = bezoekerRepository;
        }
        public void VoegBezoekerToe(uint id, string voornaam, string achternaam, string email, string bedrijf)
        {
            Bezoeker bezoeker = new Bezoeker(id, voornaam, achternaam, email, bedrijf);
            _bezoekerRepository.VoegBezoekerToe(bezoeker);
        }

        public void VerwijderBezoeker(Bezoeker bezoeker)
        {
            if (bezoeker == null) throw new BezoekerManagerException("Bezoeker mag niet leeg zijn");
            _bezoekerRepository.VerwijderBezoeker(bezoeker.Id);
        }

        public void WijzigBezoeker(Bezoeker bezoeker)
        {
            if (bezoeker == null) throw new BezoekerManagerException("Bezoeker mag niet leeg zijn");
            _bezoekerRepository.WijzigBezoeker(bezoeker.Id, bezoeker);
        }
        
        public void GeefAanwezigeBezoekers()
        {
            _bezoekerRepository.GeefAanwezigeBezoekers();
        }

        public void GeefBezoekersOpDatum(DateTime datum)
        {
            if (datum == null) throw new BezoekerManagerException("Datum mag niet leeg zijn");
            _bezoekerRepository.GeefBezoekersOpDatum(datum);
        }

        public void GeefBezoekersOpNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new BezoekerManagerException("Naam mag niet leeg zijn");
            _bezoekerRepository.GeefBezoekersOpNaam(naam);
        }
    }
}
