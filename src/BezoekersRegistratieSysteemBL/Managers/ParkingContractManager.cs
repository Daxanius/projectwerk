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
        /// ParkingContractManager constructor krijgt een instantie van de IParkingContractRepository interface als parameter.
        /// </summary>
        /// <param name="parkingContractRepository">Interface</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_parkingContractRepository] gelijk aan een instantie van de IParkingContractRepository.</remarks>
        public ParkingContractManager(IParkingContractRepository parkingContractRepository)
        {
            this._parkingContractRepository = parkingContractRepository;
        }

        /// <summary>
        /// Voegt parkingContract toe in de databank adhv een parkingContract object.
        /// </summary>
        /// <param name="parkingContract">ParkingContract object dat toegevoegd wenst te worden.</param>
        /// <exception cref="ParkingContractManagerException">"ParkingContractManager - VoegParkingContractToe - ParkingContract mag niet leeg zijn"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkingContractManager - VoegParkingContractToe - ParkingContract bestaat al"</exception>
		/// <exception cref="ParkingContractManagerException">ex.Message</exception>
        public void VoegParkingContractToe(ParkingContract parkingContract)
        {
            try
            {
                if (parkingContract == null)
                    throw new ParkingContractManagerException("ParkingContractManager - VoegParkingContractToe - ParkingContract mag niet leeg zijn");
                if (_parkingContractRepository.BestaatParkingContract(parkingContract))
                    throw new ParkingContractManagerException("ParkingContractManager - VoegParkingContractToe - ParkingContract bestaat al");
                if (_parkingContractRepository.IsOverLappend(parkingContract)) {
                    throw new ParkingContractManagerException("ParkingContractManager - VoegParkingContractToe - ParkingContract overlapt");
                }
                _parkingContractRepository.VoegParkingContractToe(parkingContract);
                _parkeerplaatsManager.VoegParkingContractBedrijfToe(parkingContract);
            }
            catch (Exception ex)
            {
                throw new ParkingContractManagerException(ex.Message);
            }
        }

        /// <summary>
        /// Verwijdert gewenste parkingContract adhv een parkingContract object.
        /// </summary>
        /// <param name="parkingContract">ParkingContract object dat verwijderd wenst te worden.</param>
        /// <exception cref="ParkingContractManagerException">"ParkingContractManager - VerwijderParkingContract - ParkingContract mag niet leeg zijn"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkingContractManager - VerwijderParkingContract - ParkingContract bestaat niet"</exception>
		/// <exception cref="ParkingContractManagerException">ex.Message</exception>
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

        /// <summary>
        /// Bewerkt gegevens van een parkingContract adhv parkingContract object.
        /// </summary>
        /// <param name="parkingContract">ParkingContract object dat gewijzigd wenst te worden.</param>
        /// <exception cref="BedrijfManagerException">"ParkingContractManager - BewerkParkingContract - ParkingContract mag niet leeg zijn"</exception>
        /// <exception cref="BedrijfManagerException">"ParkingContractManager - BewerkParkingContract - ParkingContract bestaat niet"</exception>
        /// <exception cref="BedrijfManagerException">ex.Message</exception>
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

        /// <summary>
        /// Haalt parkingContract op adhv parameter bedrijf id.
        /// </summary>
		/// <param name="bedrijf">Bedrijf id van het gewenste parkingContract.</param>
        /// <returns>Gewenst parkingContract object</returns>
		/// <exception cref="BedrijfManagerException">"ParkingContractManager - GeefParkingContract - Bedrijf mag niet leeg zijn"</exception>
        /// <exception cref="BedrijfManagerException">ex.Message</exception>
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