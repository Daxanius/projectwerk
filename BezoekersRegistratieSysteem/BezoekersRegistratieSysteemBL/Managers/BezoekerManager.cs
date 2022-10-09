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
			Bezoeker bezoeker = new Bezoeker(voornaam, achternaam, email, bedrijf);
			_bezoekerRepository.VoegBezoekerToe(bezoeker);
		}

		public void VerwijderBezoeker(Bezoeker bezoeker) {
			if (bezoeker == null) throw new BezoekerManagerException("Bezoeker mag niet leeg zijn");
			_bezoekerRepository.VerwijderBezoeker(bezoeker.Id);
		}

		public void WijzigBezoeker(Bezoeker bezoeker) {
			if (bezoeker == null) throw new BezoekerManagerException("Bezoeker mag niet leeg zijn");
			_bezoekerRepository.WijzigBezoeker(bezoeker.Id, bezoeker);
		}

		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			return _bezoekerRepository.GeefAanwezigeBezoekers();
		}

		public IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum) {
			return _bezoekerRepository.GeefBezoekersOpDatum(datum);
		}

		public Bezoeker GeefBezoekerOpNaam(string naam) {
			if (string.IsNullOrWhiteSpace(naam)) throw new BezoekerManagerException("Naam mag niet leeg zijn");
			return _bezoekerRepository.GeefBezoekerOpNaam(naam);
		}
	}
}