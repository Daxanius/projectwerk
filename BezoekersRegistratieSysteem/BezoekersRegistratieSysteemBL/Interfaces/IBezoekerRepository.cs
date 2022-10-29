using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {

	/// <summary>
	/// Bezoeker storage hook
	/// </summary>
	public interface IBezoekerRepository {

		Bezoeker VoegBezoekerToe(Bezoeker bezoeker);

		void VerwijderBezoeker(uint id);

		void WijzigBezoeker(Bezoeker bezoeker);

		bool BestaatBezoeker(Bezoeker bezoeker);

		bool BestaatBezoeker(uint bezoekerId);

		Bezoeker GeefBezoeker(uint id);

		IReadOnlyList<Bezoeker> GeefBezoekerOpNaam(string voornaam, string achternaam);

		IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers();

		IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum);
	}
}