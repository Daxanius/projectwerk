using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemBL.Managers
{
    public class ParkeerplaatsManager
    {
        private readonly Dictionary<Bedrijf, List<string>> _nummerplaten = new();
        private ParkingContractManager _parkingContractManager;

        /// <summary>
        /// Private lokale Interface variabele.
        /// </summary>
        private readonly IParkeerplaatsRepository _parkeerplaatsRepository;

        /// <summary>
        /// ParkeerplaatsManager constructor krijgt een instantie van de IParkeerplaatRepository interface als parameter.
        /// </summary>
        /// <param name="parkeerplaatsRepository">Interface</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_parkeerplaatsRepository] gelijk aan een instantie van de IParkeerplaatRepository.</remarks>
        public ParkeerplaatsManager(IParkeerplaatsRepository parkeerplaatsRepository)
        {
            this._parkeerplaatsRepository = parkeerplaatsRepository;
        }

        public void CheckNummerplaatIn(Parkeerplaats parkeerplaats)
        {
            if (parkeerplaats == null)
                throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf mag niet leeg zijn");
            if (!_nummerplaten.ContainsKey(parkeerplaats.Bedrijf))
                throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf heeft geen parking contract");
            if (_nummerplaten[parkeerplaats.Bedrijf].Contains(parkeerplaats.Nummerplaat))
                throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Nummerplaat bestaat al");
            if (_parkingContractManager.GeefParkingContract(parkeerplaats.Bedrijf).AantalPlaatsen == _nummerplaten[parkeerplaats.Bedrijf].Count)
                throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Parking is vol");
            try
            {
                _parkeerplaatsRepository.CheckNummerplaatIn(parkeerplaats);
                _nummerplaten[parkeerplaats.Bedrijf].Add(parkeerplaats.Nummerplaat);
            }
            catch (Exception ex)
            {
                throw new ParkeerplaatsManagerException(ex.Message);
            }
        }

        public void CheckNummerplaatUit(string nummerplaat)
        {
            if (string.IsNullOrWhiteSpace(nummerplaat))
                throw new ParkeerManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat mag niet leeg zijn");
            if (!_parkeerplaatsRepository.BestaatNummerplaat(nummerplaat))
                throw new ParkeerManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat bestaat niet");
            try
            {
                _parkeerplaatsRepository.CheckNummerplaatUit(nummerplaat);
                _nummerplaten.Values.Where(n => n.Equals(nummerplaat)).Select(n => n.Remove(nummerplaat));
            }
            catch (Exception ex)
            {
                throw new ParkeerplaatsManagerException(ex.Message);
            }
        }

        public IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null)
                throw new ParkeerManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - mag niet leeg zijn");
            if (!_nummerplaten.ContainsKey(bedrijf))
                throw new ParkeerManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - Bedrijf bestaat niet");
            try
            {
                return _parkeerplaatsRepository.GeefNummerplatenPerBedrijf(bedrijf);
            }
            catch (Exception ex)
            {
                throw new ParkeerplaatsManagerException(ex.Message);
            }
        }

        public void VoegParkingContractBedrijfToe(ParkingContract parkingContract)
        {
            if (parkingContract == null)
                throw new ParkeerManagerException("ParkeerManager - VoegBedrijfToe - ParkingContract mag niet leeg zijn");
            if (_nummerplaten.ContainsKey(parkingContract.Bedrijf))
                throw new ParkeerManagerException("ParkeerManager - VoegBedrijfToe - Bedrijf bestaat al");
            _nummerplaten.Add(parkingContract.Bedrijf, new List<string>());
        }
        
        public void VerwijderParkingContractBedrijf(ParkingContract parkingContract)
        {
            if (parkingContract == null)
                throw new ParkeerManagerException("ParkeerManager - VerwijderBedrijf - ParkingContract mag niet leeg zijn");
            if (!_nummerplaten.ContainsKey(parkingContract.Bedrijf))
                throw new ParkeerManagerException("ParkeerManager - VerwijderBedrijf - Bedrijf bestaat niet");
            if (!_nummerplaten[parkingContract.Bedrijf].Any())
                throw new ParkeerManagerException("ParkeerManager - VerwijderBedrijf - Er zijn nog geparkeerden bij dit bedrijf");
            _nummerplaten.Remove(parkingContract.Bedrijf);
        }
    }
}
