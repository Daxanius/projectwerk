using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {

    public class ParkingContractManager
    {
        private ParkeerplaatsManager _parkeerplaatsManager;

        /// <summary>
        /// Private lokale Interface variabele.
        /// </summary>
        private readonly IParkingContractRepository _parkingContractRepository;

        /// <summary>
        /// BedrijfManager constructor krijgt een instantie van de IBedrijfRepository interface als parameter.
        /// </summary>
        /// <param name="parkingContractRepository">Interface</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_bedrijfRepository] gelijk aan een instantie van de IBedrijfRepository.</remarks>
        public ParkingContractManager(IParkingContractRepository parkingContractRepository)
        {
            this._parkingContractRepository = parkingContractRepository;
        }

        /// <summary>
        /// Voegt een ParkingContract toe aan de database.
        /// </summary>
        public void VoegParkingContractToe(ParkingContract parkingContract)
        {
            if (parkingContract == null)
                throw new ParkingContractManagerException("ParkingContractManager - VoegParkingContractToe - ParkingContract mag niet leeg zijn");
            if (_parkingContractRepository.BestaatParkingContract(parkingContract))
                throw new ParkingContractManagerException("ParkingContractManager - VoegParkingContractToe - ParkingContract bestaat al");
            try
            {
                _parkingContractRepository.VoegParkingContractToe(parkingContract);
                _parkeerplaatsManager.VoegParkingContractBedrijfToe(parkingContract);
            }
            catch (Exception ex)
            {
                throw new ParkingContractManagerException(ex.Message);
            }
        }

        public void VerwijderParkingContract(ParkingContract parkingContract)
        {
            if (parkingContract == null)
                throw new ParkingContractManagerException("ParkingContractManager - VerwijderParkingContract - ParkingContract mag niet leeg zijn");
            if (!_parkingContractRepository.BestaatParkingContract(parkingContract))
                throw new ParkingContractManagerException("ParkingContractManager - VerwijderParkingContract - ParkingContract bestaat niet");
            try
            {
                _parkingContractRepository.VerwijderParkingContract(parkingContract);
                _parkeerplaatsManager.VerwijderParkingContractBedrijf(parkingContract);
            }
            catch (Exception ex)
            {
                throw new ParkingContractManagerException(ex.Message);
            }
        }
        
        public void BewerkParkingContract(ParkingContract parkingContract)
        {
            if (parkingContract == null)
                throw new ParkingContractManagerException("ParkingContractManager - BewerkParkingContract - ParkingContract mag niet leeg zijn");
            if (!_parkingContractRepository.BestaatParkingContract(parkingContract))
                throw new ParkingContractManagerException("ParkingContractManager - BewerkParkingContract - ParkingContract bestaat niet");
            try
            {
                _parkingContractRepository.BewerkParkingContract(parkingContract);
            }
            catch (Exception ex)
            {
                throw new ParkingContractManagerException(ex.Message);
            }
        }

        public ParkingContract GeefParkingContract(Bedrijf bedrijf)
        {
            if (bedrijf == null)
                throw new ParkingContractManagerException("ParkingContractManager - GeefParkingContract - Bedrijf mag niet leeg zijn");
            try
            {
                return _parkingContractRepository.GeefParkingContract(bedrijf.Id);
            }
            catch (Exception ex)
            {
                throw new ParkingContractManagerException(ex.Message);
            }
        }
    }
}