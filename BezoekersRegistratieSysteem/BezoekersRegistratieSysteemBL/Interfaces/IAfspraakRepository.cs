using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Afspraak storage hook
	/// </summary>
	public interface IAfspraakRepository {
		void BeeindigAfspraak(uint id);
		void BewerkAfspraak(uint id);
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
		IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDatum(uint id, DateTime datum);
		IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint id);
		IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint id);
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint id);
		void VerwijderAfspraak(uint id);
		void VoegAfspraakToe(Afspraak afspraak);
	}
}