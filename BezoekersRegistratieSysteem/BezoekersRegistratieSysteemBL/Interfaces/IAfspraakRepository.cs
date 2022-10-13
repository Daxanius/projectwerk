using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Afspraak storage hook
	/// </summary>
	public interface IAfspraakRepository {
		void BeeindigAfspraak(uint afspraakId);
		void BewerkAfspraak(Afspraak afspraak);
		Afspraak GeefAfspraakOpId(uint afspraakid);
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
		IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum);
		IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId);
		IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId);
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint werknemerId);
		void VerwijderAfspraak(uint afspraakId);
		void VoegAfspraakToe(Afspraak afspraak);
	}
}