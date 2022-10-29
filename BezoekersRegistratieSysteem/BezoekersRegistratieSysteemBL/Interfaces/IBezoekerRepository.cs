using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
	/// <summary>
	/// Bezoeker storage hook
	/// </summary>
	public interface IBezoekerRepository
	{
		Bezoeker VoegBezoekerToe(Bezoeker bezoeker);
		void VerwijderBezoeker(long id);
		void WijzigBezoeker(Bezoeker bezoeker);

		bool BestaatBezoeker(Bezoeker bezoeker);
		bool BestaatBezoeker(long bezoekerId);

		Bezoeker GeefBezoeker(long id);
		IReadOnlyList<Bezoeker> GeefBezoekerOpNaam(string voornaam, string achternaam);

		IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers();

		IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum);
	}
}