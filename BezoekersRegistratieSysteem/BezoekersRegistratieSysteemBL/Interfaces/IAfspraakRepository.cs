using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Afspraak storage hook
	/// </summary>
	public interface IAfspraakRepository {
		Afspraak VoegAfspraakToe(Afspraak afspraak);
		void VerwijderAfspraak(uint afspraakId);
		void BewerkAfspraak(Afspraak afspraak);
		void BeeindigAfspraakBezoeker(uint id);
		void BeeindigAfspraakSysteem(uint id);
		void BeeindigAfspraakOpEmail(uint id, string email);
		Afspraak GeefAfspraak(uint afspraakid);
		bool BestaatAfspraak(Afspraak afspraak);
		bool BestaatAfspraak(uint afspraakid);

		bool BestaatLopendeAfspraak(Afspraak afspraak);

        IReadOnlyList<Afspraak> GeefHuidigeAfspraken();
		IReadOnlyList<Afspraak> GeefHuidigeAfsprakenPerBedrijf(uint bedrijfId);
		Afspraak GeefHuidigeAfspraakPerWerknemer(uint werknemerId);
		IReadOnlyList<Afspraak> GeefAlleAfsprakenPerWerknemer(uint werknemerId);
		IReadOnlyList<Afspraak> GeefAfsprakenPerWerknemerOpDag(uint werknemerId, DateTime datum);
		IReadOnlyList<Afspraak> GeefAfsprakenPerDag(DateTime datum);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBedrijfOpDag(uint id, DateTime datum);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpNaam(string voornaam, string achternaam);
        IReadOnlyList<Afspraak> GeefAfsprakenPerBezoekerOpEmail(string email);
		Afspraak GeefHuidigeAfspraakBezoeker(uint id);
	}
}