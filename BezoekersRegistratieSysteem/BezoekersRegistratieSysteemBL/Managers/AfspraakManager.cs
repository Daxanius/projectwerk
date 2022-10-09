using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class AfspraakManager {
		private readonly IAfspraakRepository _afspraakRepository;

		public AfspraakManager(IAfspraakRepository afspraakRepository) {
			this._afspraakRepository = afspraakRepository;
		}

		public void MaakAfspraak(DateTime starttijd, Bezoeker bezoeker, Werknemer werknemer) {
			Afspraak afspraak = new(starttijd, bezoeker, werknemer);
			_afspraakRepository.VoegAfspraakToe(afspraak);
		}

		public void VerwijderAfspraak(uint id) {
			_afspraakRepository.VerwijderAfspraak(id);
		}

		public void BewerkAfspraak(Afspraak afspraak) {
			if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn");
			_afspraakRepository.BewerkAfspraak(afspraak.Id);
		}

		public void BeeindigAfspraak(Afspraak afspraak) {
			if (afspraak == null) throw new AfspraakManagerException("afspraak mag niet leeg zijn");
			if (afspraak.Eindtijd is not null) throw new AfspraakManagerException("afspraak reeds beëindigd");
			_afspraakRepository.BeeindigAfspraak(afspraak.Id);
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfspraken() {
			return _afspraakRepository.GeefHuidigeAfspraken();
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null) throw new AfspraakManagerException("bedrijf mag niet leeg zijn");
			return _afspraakRepository.GeefHuidigeAfsprakenPerBedrijf(bedrijf.Id);
		}

		public IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(Werknemer werknemer) {
			if (werknemer == null) throw new AfspraakManagerException("werknemer mag niet leeg zijn");
			return _afspraakRepository.GeefAlleAfsprakenPerWerknemer(werknemer.Id);
		}

		public IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(Werknemer werknemer) {
			if (werknemer == null) throw new AfspraakManagerException("werknemer mag niet leeg zijn");
			return _afspraakRepository.GeefHuidigeAfsprakenPerWerknemer(werknemer.Id);
		}

		public IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDatum(Werknemer werknemer, DateTime datum) {
			if (werknemer == null) throw new AfspraakManagerException("werknemer mag niet leeg zijn");
			return _afspraakRepository.GeefAfsprakenPerWerknemerOpDatum(werknemer.Id, datum);
		}

		public IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum) {
			return _afspraakRepository.GeefAfsprakenPerDag(datum);
		}
	}
}