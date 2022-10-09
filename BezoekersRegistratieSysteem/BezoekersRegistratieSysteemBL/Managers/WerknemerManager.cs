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
        public void VoegWerknemerToe(string voornaam, string achternaam, string email, Bedrijf bedrijf, string functie)
        {
            Werknemer werknemer = new Werknemer(voornaam, achternaam, email, bedrijf, functie);
            _werknemerRepository.VoegWerknemerToe(werknemer);
        }

        public void VerwijderWerknemer(Werknemer werknemer)
        {
            if (werknemer == null) throw new WerknemerManagerException("Werknemer mag niet leeg zijn");
            _werknemerRepository.VerwijderWerknemer(werknemer.Id);
        }

        public void WijzigWerknemer(Werknemer werknemer)
        {
            if (werknemer == null) throw new WerknemerManagerException("Werknemer mag niet leeg zijn");
            _werknemerRepository.WijzigWerknemer(werknemer.Id, werknemer);
        }

        //We need to solve this.... possibly idk... send help
        //public IReadOnlyList<Werknemer> GeefAanwezigeWerknemers()
        //{
        //    return _werknemerRepository.GeefAanwezigeWerknemers();
        //}

        public Werknemer GeefWerknemerOpNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new WerknemerManagerException("Naam mag niet leeg zijn");
            return _werknemerRepository.GeefWerknemerOpNaam(naam);
        }

        public IReadOnlyList<Werknemer> GeefWerknemersPerBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null) throw new WerknemerManagerException("Bedrijf mag niet leeg zijn");
            return _werknemerRepository.GeefWerknemersPerBedrijf(bedrijf.Id);
        }

        public IReadOnlyList<Werknemer> GeefWerknemersPerFunctie(string functie)
        {
            //Wildcard
            if (string.IsNullOrWhiteSpace(functie)) throw new WerknemerManagerException("Functie mag niet leeg zijn");
            return _werknemerRepository.GeefWerknemersPerFunctie(functie);
        }
    }
}
