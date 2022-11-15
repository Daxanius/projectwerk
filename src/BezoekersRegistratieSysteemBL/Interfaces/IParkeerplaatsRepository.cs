using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
    public interface IParkeerplaatsRepository
    {
        bool BestaatNummerplaat(string nummerplaat);
        void CheckNummerplaatIn(Parkeerplaats parkeerplaats);
        void CheckNummerplaatUit(string nummerplaat);
        IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf);
    }
}