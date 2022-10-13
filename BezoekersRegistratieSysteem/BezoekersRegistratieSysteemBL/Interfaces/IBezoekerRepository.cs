using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces {
	/// <summary>
	/// Bezoeker storage hook
	/// </summary>
	public interface IBezoekerRepository {
		void VoegBezoekerToe(Bezoeker bezoeker);
        void VerwijderBezoeker(uint id);
		void WijzigBezoeker(uint id, Bezoeker bezoeker);
        Bezoeker GeefBezoeker(uint id);
		Bezoeker GeefBezoekerOpNaam(string naam);
		IReadOnlyList<Bezoeker> GeefAanwezigeBezoekers();
		IReadOnlyList<Bezoeker> GeefBezoekersOpDatum(DateTime datum);
	}
}