using BezoekersRegistratieSysteemBL.Domeinen;

namespace BezoekersRegistratieSysteemBL.Interfaces
{
    public interface IWerknemerRepository
    {
        void VoegWerknemerToe(Werknemer werknemer);
        void VerwijderWerknemer(uint id);
        void WijzigWerknemer(uint id, Werknemer werknemer);
        void GeefWerknemerOpNaam(string naam);
        //IReadOnlyList<Werknemer> GeefAanwezigeWerknemers();
        IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(uint id);
        IReadOnlyList<Werknemer> GeefWerknemersPerFunctie(string functie);
    }
}