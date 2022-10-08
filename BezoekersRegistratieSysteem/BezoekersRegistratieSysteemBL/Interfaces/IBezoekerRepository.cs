using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
    public interface IBezoekerRepository
    {
        void GeefAanwezigeBezoekers();
        void GeefBezoekersOpDatum(DateTime datum);
        void GeefBezoekersOpNaam(string naam);
        void VerwijderBezoeker(uint id);
        void VoegBezoekerToe(Bezoeker bezoeker);
        void WijzigBezoeker(uint id, Bezoeker bezoeker);
    }
}