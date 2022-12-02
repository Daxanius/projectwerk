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

        /// <summary>
        /// Private lokale Interface variabele.
        /// </summary>
        private readonly IParkeerplaatsRepository _parkeerplaatsRepository;
        private readonly IParkingContractRepository _parkingContractRepository;

        /// <summary>
        /// ParkeerplaatsManager constructor krijgt een instantie van de IParkeerplaatRepository interface als parameter.
        /// </summary>
        /// <param name="parkeerplaatsRepository">Interface</param>
        /// <remarks>Deze constructor stelt de lokale variabele [_parkeerplaatsRepository] gelijk aan een instantie van de IParkeerplaatRepository.</remarks>
        public ParkeerplaatsManager(IParkeerplaatsRepository parkeerplaatsRepository, IParkingContractRepository parkingContractRepository)
        {
            _parkeerplaatsRepository = parkeerplaatsRepository;
            _parkingContractRepository = parkingContractRepository;
        }

        /// <summary>
        /// Checkt nummerplaat in Bij bedrijf in de databank adhv een parkeerplaats object.
        /// </summary>
        /// <param name="parkeerplaats">parkeerplaats object dat toegevoegd wenst te worden aan bedrijf.</param>
        /// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VoegNummerplaatToe - Bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VoegNummerplaatToe - Bedrijf heeft geen parking contract"</exception>
		/// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VoegNummerplaatToe - Nummerplaat bestaat al"</exception>
		/// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VoegNummerplaatToe - Parking is vol"</exception>
        /// <exception cref="ParkeerplaatsManagerException">ex.Message</exception>
        public void CheckNummerplaatIn(Parkeerplaats parkeerplaats)
        {
            if (parkeerplaats == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf mag niet leeg zijn");
            if (_parkingContractRepository.GeefParkingContract(parkeerplaats.Bedrijf.Id) == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf heeft geen parking contract");
            if (_parkeerplaatsRepository.BestaatNummerplaat(parkeerplaats.Nummerplaat))
                throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Nummerplaat bestaat al");
            if (_parkingContractRepository.GeefParkingContract(parkeerplaats.Bedrijf.Id).AantalPlaatsen == _parkeerplaatsRepository.GeefHuidigBezetteParkeerplaatsenPerBedrijf(parkeerplaats.Bedrijf.Id))
                throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Parking is vol");
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

        /// <summary>
        /// Checkt nummerplaat uit Bij bedrijf in de databank adhv de parameter nummerplaat.
        /// </summary>
        /// <param name="nummerplaat">Nummerplaat die toegevoegd wenst te worden aan bedrijf.</param>
        /// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VerwijderNummerplaat - Nummerplaat mag niet leeg zijn"</exception>
		/// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VerwijderNummerplaat - Nummerplaat bestaat niet"</exception>
        /// <exception cref="ParkeerplaatsManagerException">ex.Message</exception>
        public void CheckNummerplaatUit(string nummerplaat)
        {
            if (string.IsNullOrWhiteSpace(nummerplaat))
                throw new ParkeerplaatsManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat mag niet leeg zijn");
            if (!_parkeerplaatsRepository.BestaatNummerplaat(nummerplaat))
                throw new ParkeerplaatsManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat bestaat niet");
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

        /// <summary>
        /// Stelt lijst van nummerplaten per bedrijf samen met enkel lees rechten.
        /// </summary>
        /// <param name="bedrijf">Bedrijf object die toegevoegd wenst te worden aan bedrijf.</param>
        /// <returns>IReadOnlyList van nummerplaten.</returns>
        /// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - GeefNummerplatenPerBedrijf - mag niet leeg zijn"</exception>
		/// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - GeefNummerplatenPerBedrijf - Bedrijf bestaat niet"</exception>
        /// <exception cref="ParkeerplaatsManagerException">ex.Message</exception>
        public IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - mag niet leeg zijn");
            if (_parkingContractRepository.GeefParkingContract(bedrijf.Id) == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - Bedrijf heeft geen parking contract");
            try
            {
                return _parkeerplaatsRepository.GeefNummerplatenPerBedrijf(bedrijf);
            }
            catch (Exception ex)
            {
                throw new ParkeerplaatsManagerException(ex.Message);
            }
        }

        /// <summary>
        /// Voegt parkingContract toe aan bedrijf in de databank adhv een parkingContract object.
        /// </summary>
        /// <param name="parkingContract">ParkingContract object dat toegevoegd wenst te worden aan bedrijf.</param>
        /// <exception cref="ParkeerplaatsManagerException">"ParkingContractManager - VoegParkingContractToe - ParkingContract mag niet leeg zijn"</exception>
		/// <exception cref="ParkeerplaatsManagerException">"ParkingContractManager - VoegParkingContractToe - ParkingContract bestaat al"</exception>
		/// <exception cref="ParkeerplaatsManagerException">ex.Message</exception>
        public void VoegParkingContractBedrijfToe(ParkingContract parkingContract)
        {
            if (parkingContract == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - VoegBedrijfToe - ParkingContract mag niet leeg zijn");
            if (_parkingContractRepository.GeefParkingContract(parkingContract.Bedrijf.Id) != null)
                throw new ParkeerplaatsManagerException("ParkeerManager - VoegBedrijfToe - Bedrijf heeft reeds een parking contract");
            _nummerplaten.Add(parkingContract.Bedrijf, new List<string>());
        }

        /// <summary>
        /// Verwijdert gewenste parkingContract van bedrijf adhv een parkingContract object.
        /// </summary>
        /// <param name="parkingContract">ParkingContract object dat verwijderd wenst te worden van bedrijf.</param>
        /// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VerwijderBedrijf - ParkingContract mag niet leeg zijn"</exception>
        /// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VerwijderBedrijf - Bedrijf bestaat niet"</exception>
        /// <exception cref="ParkeerplaatsManagerException">"ParkeerManager - VerwijderBedrijf - Er zijn nog geparkeerden bij dit bedrijf"</exception>
        public void VerwijderParkingContractBedrijf(ParkingContract parkingContract)
        {
            if (parkingContract == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - VerwijderBedrijf - ParkingContract mag niet leeg zijn");
            if (_parkingContractRepository.GeefParkingContract(parkingContract.Bedrijf.Id) == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - VerwijderBedrijf - Bedrijf heeft geen parking contract");
            if (!_nummerplaten[parkingContract.Bedrijf].Any())
                throw new ParkeerplaatsManagerException("ParkeerManager - VerwijderBedrijf - Er zijn nog geparkeerden bij dit bedrijf");
            _nummerplaten.Remove(parkingContract.Bedrijf);
        }

        public int GeefHuidigBezetteParkeerplaatsenPerBedrijf(Bedrijf bedrijf)
        {
            if (bedrijf == null)
                throw new ParkeerplaatsManagerException("ParkeerManager - GeefHuidigBezetteParkeerplaatsenPerBedrijf - Bedrijf mag niet leeg zijn");
            return _parkeerplaatsRepository.GeefHuidigBezetteParkeerplaatsenPerBedrijf(bedrijf.Id);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param></param>
        ///// <exception cref="ParkeerplaatsManagerException"></exception>
        //public void BewerkParkeerplaats(Parkeerplaats parkeerplaats)
        //{
        //    if (parkeerplaats == null)
        //        throw new ParkeerplaatsManagerException("ParkeerManager - BewerkParkeerplaats - Parkeerplaats mag niet leeg zijn");
        //    if (!_parkeerplaatsRepository.BestaatParkeerplaats(parkeerplaats))
        //        throw new ParkeerplaatsManagerException("ParkeerManager - BewerkParkeerplaats - Parkeerplaats bestaat niet");
        //    try
        //    {
        //        _parkeerplaatsRepository.BewerkParkeerplaats(parkeerplaats);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ParkeerplaatsManagerException(ex.Message);
        //    }
        //}
    }
}
