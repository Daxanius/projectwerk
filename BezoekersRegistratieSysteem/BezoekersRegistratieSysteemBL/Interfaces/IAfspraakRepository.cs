using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Afspraak storage hook
	/// </summary>
	public interface IAfspraakRepository {
		void VoegAfspraakToe(Afspraak afspraak);
		void VerwijderAfspraak(uint afspraakId);
		void BewerkAfspraak(Afspraak afspraak);
		void BeeindigAfspraakBezoeker(uint id);
		void BeeindigAfspraakSysteem(uint id);
		Afspraak GeefAfspraak(uint afspraakid);

		void BestaatAfspraak(Afspraak afspraak);
        
		IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId);
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(uint werknemerId);
		IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId);
		IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum);
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
	}
}