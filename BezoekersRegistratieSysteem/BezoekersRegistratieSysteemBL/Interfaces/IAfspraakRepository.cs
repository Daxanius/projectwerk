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
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerWerknemerPerBedrijf(long werknemerId, long bedrijfId);
        IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemerPerBedrijf(long werknemerId, long bedrijfId);
        IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDagPerBedrijf(long werknemerId, DateTime datum, long bedrijfId);
		//
        
        //Dag
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
        //
        
        //Bezoeker
		Afspraak GeefHuidigeAfspraakBezoekerPerBerijf(long bezoekerId, long bedrijfId);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaamOfEmailPerBedrijf(string voornaam, string achternaam, string email, long bedrijfId);
		IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpDagPerBedrijf(long bezoekerId, DateTime datum, long bedrijfId);
        //

		IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers();
	}
}