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
		void BeeindigAfspraakOpEmail(string email);
		Afspraak GeefAfspraak(long afspraakid);
		bool BestaatAfspraak(Afspraak afspraak);
		bool BestaatAfspraak(long afspraakid);

		bool BestaatLopendeAfspraak(Afspraak afspraak);

        //Bedrijf
        IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(long bedrijfId);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(long id, DateTime datum);
        //
        
        //Werknemer
		Afspraak GeefHuidigeAfspraakPerWerknemer(long werknemerId);
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
	}
}