using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class BezoekerManager {
		private readonly IBezoekerRepository _bezoekerRepository;

		public BezoekerManager(IBezoekerRepository bezoekerRepository) {
			this._bezoekerRepository = bezoekerRepository;
		}

		public Bezoeker VoegBezoekerToe(Bezoeker bezoeker) {
			if (_bezoekerRepository.BestaatBezoeker(bezoeker)) throw new BezoekerManagerException("BezoekerManager - VoegBezoekerToe - bezoeker bestaat al");
			try {
				return _bezoekerRepository.VoegBezoekerToe(bezoeker);
			} catch (Exception ex) {
				throw new BezoekerManagerException(ex.Message);
			}
		}

		public void VerwijderBezoeker(Bezoeker bezoeker) {
			if (!_bezoekerRepository.BestaatBezoeker(bezoeker)) throw new BezoekerManagerException("BezoekerManager - VerwijderBezoeker - bezoeker bestaat niet");
			try {
				_bezoekerRepository.VerwijderBezoeker(bezoeker.Id);
			} catch (Exception ex) {
				throw new BezoekerManagerException(ex.Message);
			}
		}

		public void WijzigBezoeker(Bezoeker bezoeker) {
			if (bezoeker == null) throw new BezoekerManagerException("BezoekerManager - WijzigBezoeker - bezoeker mag niet leeg zijn");
			if (!_bezoekerRepository.BestaatBezoeker(bezoeker)) throw new BezoekerManagerException("BezoekerManager - WijzigBezoeker - bezoeker bestaat niet");
			if (_bezoekerRepository.GeefBezoeker(bezoeker.Id).BezoekerIsGelijk(bezoeker)) throw new BezoekerManagerException("BezoekerManager - WijzigBezoeker - bezoeker is niet gewijzigd");
			try {
				_bezoekerRepository.WijzigBezoeker(bezoeker);
			} catch (Exception ex) {
				throw new BezoekerManagerException(ex.Message);
			}
		}

		public Bezoeker GeefBezoeker(uint id) {
			if (!_bezoekerRepository.BestaatBezoeker(id)) throw new BezoekerManagerException("BezoekerManager - GeefBezoeker - bezoeker bestaat niet");
			try {
				return _bezoekerRepository.GeefBezoeker(id);
			} catch (Exception ex) {
				throw new BezoekerManagerException(ex.Message);
			}
		}

		public IReadOnlyList<Bezoeker> GeefBezoekerOpNaam(string voornaam, string achternaam) {
			if (string.IsNullOrWhiteSpace(voornaam) || string.IsNullOrWhiteSpace(achternaam)) throw new BezoekerManagerException("BezoekerManager - GeefBezoekerOpNaam - naam mag niet leeg zijn");
			try {
				return _bezoekerRepository.GeefBezoekerOpNaam(voornaam, achternaam);
			} catch (Exception ex) {
				throw new BezoekerManagerException(ex.Message);
			}
		}

		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			try {
				return _bezoekerRepository.GeefAanwezigeBezoekers();
			} catch (Exception ex) {
				throw new BezoekerManagerException(ex.Message);
			}
		}

		public IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum) {
			try {
				return _bezoekerRepository.GeefBezoekersOpDatum(datum);
			} catch (Exception ex) {
				throw new BezoekerManagerException(ex.Message);
			}
		}
	}
}