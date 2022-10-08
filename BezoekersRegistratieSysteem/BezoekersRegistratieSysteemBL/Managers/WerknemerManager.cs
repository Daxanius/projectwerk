using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers
{
    public class WerknemerManager
    {
        private IWerknemerRepository _werknemerRepository;

        public WerknemerManager(IWerknemerRepository werknemerRepository)
        {
            this._werknemerRepository = werknemerRepository;
        }
        public void VoegWerknemerToe(uint id, string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie)
        {
            Werknemer werknemer = new Werknemer(id, voornaam, achternaam, email, bedrijf, functie);
            _werknemerRepository.VoegWerknemerToe(werknemer);
        }

        public void VerwijderWerknemer(Werknemer werknemer)
        {
            if (werknemer == null) throw new WerknemerManagerException("Werknemer mag niet leeg zijn");
            _werknemerRepository.VerwijderWerknemer(werknemer);
        }

        public void WijzigWerknemer(Werknemer werknemer)
        {
            if (werknemer == null) throw new WerknemerManagerException("Werknemer mag niet leeg zijn");
            _werknemerRepository.WijzigWerknemer(werknemer.Id, werknemer);
        }

        //public IReadOnlyList<Werknemer> GeefAanwezigeWerknemers()
        //{
        //    return _werknemerRepository.GeefAanwezigeWerknemers();
        //}

        public void GeefWerknemerOpNaam(string naam)
        {
            _werknemerRepository.GeefWerknemerOpNaam(naam);
        }

        public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(Bedrijf bedrijf)
        {
            return _werknemerRepository.GeefWerknemersPerBedrijf(bedrijf.Id);
        }

        public IReadOnlyList<Werknemer> GeefWerknemersPerFunctie(string functie)
        {
            //Wildcard
            return _werknemerRepository.GeefWerknemersPerFunctie(functie);
        }
    }
}
