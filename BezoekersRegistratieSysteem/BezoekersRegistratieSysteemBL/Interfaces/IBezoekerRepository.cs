using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Bezoeker storage hook
	/// </summary>
	public interface IBezoekerRepository {
		IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers();
		IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum);
		Bezoeker GeefBezoekerOpNaam(string naam);
		void VerwijderBezoeker(uint id);
		void VoegBezoekerToe(Bezoeker bezoeker);
		void WijzigBezoeker(uint id, Bezoeker bezoeker);
	}
}