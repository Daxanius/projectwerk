using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Afspraak storage hook
	/// </summary>
	public interface IAfspraakRepository {
		Afspraak VoegAfspraakToe(Afspraak afspraak);
		void VerwijderAfspraak(long afspraakId);
		void BewerkAfspraak(Afspraak afspraak);
		void BeeindigAfspraakBezoeker(long id);
		void BeeindigAfspraakSysteem(long id);
		void BeeindigAfspraakOpEmail(long id, string email);
		Afspraak GeefAfspraak(long afspraakid);
		bool BestaatAfspraak(Afspraak afspraak);
		bool BestaatAfspraak(long afspraakid);

		bool BestaatLopendeAfspraak(Afspraak afspraak);

        IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(long bedrijfId);
		Afspraak GeefHuidigeAfspraakPerWerknemer(long werknemerId);
		IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(long werknemerId);
		IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(long werknemerId, DateTime datum);
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(long id, DateTime datum);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaam(string voornaam, string achternaam);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpEmail(string email);
		Afspraak GeefHuidigeAfspraakBezoeker(long id);
	}
}