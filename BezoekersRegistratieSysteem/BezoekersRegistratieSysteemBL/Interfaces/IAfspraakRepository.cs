using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Afspraak storage hook
	/// </summary>
	public interface IAfspraakRepository {
		Afspraak VoegAfspraakToe(Afspraak afspraak);
		void VerwijderAfspraak(long afspraakId);
		void BewerkAfspraak(Afspraak afspraak);
		void BeeindigAfspraakBezoeker(long afspraakId);
		void BeeindigAfspraakSysteem(long afspraakId);
		void BeeindigAfspraakOpEmail(string bezoekerMail);
		Afspraak GeefAfspraak(long afspraakId);
		bool BestaatAfspraak(Afspraak afspraak);
		bool BestaatAfspraak(long afspraakid);

		bool BestaatLopendeAfspraak(Afspraak afspraak);

        //Bedrijf
        IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(long bedrijfId);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(long id, DateTime datum);
        //

        //Werknemer
        IReadOnlyList<Afspraak> GeefHuidigeAfspraakPerWerknemer(long werknemerId);
		IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(long werknemerId);
		IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(long werknemerId, DateTime datum);
		//
        
        //Dag
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
        //
        
        //Bezoeker
		Afspraak GeefHuidigeAfspraakBezoeker(long id);
		IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaamOfEmail(string voornaam, string achternaam, string email);
		IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpDag(long id, DateTime datum);
        //

		//NEW AND IMPROVED
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemerOpBedrijf(long werknemerId, long bedrijfId);
        IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemerOpBedrijf(long werknemerId, long bedrijfId);
        IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDagOpBedrijf(long werknemerId, DateTime datum, long bedrijfId);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(string voornaam, string achternaam, string email, long bedrijfId);
		IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpDagPerBedrijf(long werknemerId, DateTime datum, long bedrijfId);
		Afspraak GeefHuidigeAfspraakBezoekerPerBerijf(long werknemerId, long bedrijfId);
		IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers();
		//
	}
}