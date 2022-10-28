using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
	/// <summary>
	/// Afspraak storage hook
	/// </summary>
	public interface IAfspraakRepository
	{
		Afspraak VoegAfspraakToe(Afspraak afspraak);
		void VerwijderAfspraak(long afspraakId);
		void BewerkAfspraak(Afspraak afspraak);
		void BeeindigAfspraakBezoeker(long id);
		void BeeindigAfspraakSysteem(long id);
		Afspraak GeefAfspraak(long afspraakid);
		bool BestaatAfspraak(Afspraak afspraak);
		bool BestaatAfspraak(long afspraakid);

		IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(long bedrijfId);
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemer(long werknemerId);
		IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(long werknemerId);
		IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(long werknemerId, DateTime datum);
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
	}
}