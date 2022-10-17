using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class BezoekerManager {
		private readonly IBezoekerRepository _bezoekerRepository;

		public BezoekerManager(IBezoekerRepository bezoekerRepository) {
			this._bezoekerRepository = bezoekerRepository;
		}

		public void VoegBezoekerToe(Bezoeker bezoeker) {
			try
			{
				_bezoekerRepository.VoegBezoekerToe(bezoeker);
			}
            catch (Exception ex)
            {
                throw new BezoekerManagerException(ex.Message);
            }
        }

		public void VerwijderBezoeker(uint id) {
			try
			{
				_bezoekerRepository.VerwijderBezoeker(id);
			}
            catch (Exception ex)
            {
                throw new BezoekerManagerException(ex.Message);
            }
        }
        
		public void WijzigBezoeker(Bezoeker bezoeker) {
            if (bezoeker == null) throw new BezoekerManagerException("BezoekerManager - WijzigBezoeker - bezoeker mag niet leeg zijn");
			try
			{
                //if (!_bezoekerRepo.BestaatBezoeker(bezoeker)) throw new BezoekerManagerException("BezoekerManager - WijzigBezoeker - bezoeker bestaat niet");
                //if (_bezoekerRepo.GeefBezoeker(bezoeker.Id).BezoekerIsGelijk(bezoeker))
                _bezoekerRepository.WijzigBezoeker(bezoeker.Id, bezoeker);
			}
            catch (Exception ex)
            {
                throw new BezoekerManagerException(ex.Message);
            }
        }

		public Bezoeker GeefBezoeker(uint id) {
			try
			{
				return _bezoekerRepository.GeefBezoeker(id);
			}
            catch (Exception ex)
            {
                throw new BezoekerManagerException(ex.Message);
            }
        }
        
		public Bezoeker GeefBezoekerOpNaam(string naam) {
            if (string.IsNullOrWhiteSpace(naam)) throw new BezoekerManagerException("BezoekerManager - GeefBezoekerOpNaam - naam mag niet leeg zijn");
			try
			{
				return _bezoekerRepository.GeefBezoekerOpNaam(naam);
			}
            catch (Exception ex)
            {
                throw new BezoekerManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers() {
			try
			{
				return _bezoekerRepository.GeefAanwezigeBezoekers();
			}
            catch (Exception ex)
            {
                throw new BezoekerManagerException(ex.Message);
            }
        }

		public IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum) {
			try
			{
				return _bezoekerRepository.GeefBezoekersOpDatum(datum);
			}
            catch (Exception ex)
            {
                throw new BezoekerManagerException(ex.Message);
            }
        }
	}
}