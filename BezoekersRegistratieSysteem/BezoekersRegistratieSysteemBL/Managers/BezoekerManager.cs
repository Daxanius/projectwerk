using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class BezoekerManager {
		private readonly IBezoekerRepository _bezoekerRepository;

		public BezoekerManager(IBezoekerRepository bezoekerRepository) {
			this._bezoekerRepository = bezoekerRepository;
		}

		public void VoegBezoekerToe(string voornaam, string achternaam, string email, string bedrijf) {
			Bezoeker bezoeker = new(voornaam, achternaam, email, bedrijf);
			_bezoekerRepository.VoegBezoekerToe(bezoeker);
		}

		public void VerwijderBezoeker(uint id) {
			_bezoekerRepository.VerwijderBezoeker(id);
		}
		public void WijzigBezoeker(Bezoeker bezoeker) {
            if (bezoeker == null) throw new BezoekerManagerException("BezoekerManager - WijzigBezoeker - bezoeker mag niet leeg zijn");
            _bezoekerRepository.WijzigBezoeker(bezoeker.Id, bezoeker);
		}

		public Bezoeker GeefBezoeker(uint id) {
            return _bezoekerRepository.GeefBezoeker(id);
        }
        
		public Bezoeker GeefBezoekerOpNaam(string naam) {
            if (string.IsNullOrWhiteSpace(naam)) throw new BezoekerManagerException("BezoekerManager - GeefBezoekerOpNaam - naam mag niet leeg zijn");
            return _bezoekerRepository.GeefBezoekerOpNaam(naam);
		}

		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			return _bezoekerRepository.GeefAanwezigeBezoekers();
		}

		public IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum) {
			return _bezoekerRepository.GeefBezoekersOpDatum(datum);
		}
	}
}