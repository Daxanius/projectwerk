using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
    public interface IBedrijfRepository
    {
        void VerwijderBedrijf(uint id);
        void VoegBedrijfToe(Bedrijf bedrijf);
        void WijzigBedrijf(uint id, Bedrijf bedrijf);
    }
}