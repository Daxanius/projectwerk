using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class ParkeerplaatsManager {
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
		public ParkeerplaatsManager(IParkeerplaatsRepository parkeerplaatsRepository, IParkingContractRepository parkingContractRepository) {
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
		public void CheckNummerplaatIn(Parkeerplaats parkeerplaats) {
			if (parkeerplaats == null)
				throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf mag niet leeg zijn");
			if (_parkingContractRepository.GeefParkingContract(parkeerplaats.Bedrijf.Id) == null)
				throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf heeft geen parking contract");
			if (_parkeerplaatsRepository.BestaatNummerplaat(parkeerplaats.Nummerplaat))
				throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Nummerplaat bestaat al");
			if (_parkingContractRepository.GeefParkingContract(parkeerplaats.Bedrijf.Id).AantalPlaatsen <= _parkeerplaatsRepository.GeefHuidigBezetteParkeerplaatsenVoorBedrijf(parkeerplaats.Bedrijf.Id))
				throw new ParkeerplaatsManagerException("ParkeerManager - VoegNummerplaatToe - Parking is vol");
			try {
				_parkeerplaatsRepository.CheckNummerplaatIn(parkeerplaats);
			} catch (Exception ex) {
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
		public void CheckNummerplaatUit(string nummerplaat) {
			if (string.IsNullOrWhiteSpace(nummerplaat))
				throw new ParkeerplaatsManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat mag niet leeg zijn");
			if (!_parkeerplaatsRepository.BestaatNummerplaat(nummerplaat))
				throw new ParkeerplaatsManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat bestaat niet");
			try {
				_parkeerplaatsRepository.CheckNummerplaatUit(nummerplaat);
			} catch (Exception ex) {
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
		public IReadOnlyList<Parkeerplaats> GeefNummerplatenPerBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new ParkeerplaatsManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - mag niet leeg zijn");
			if (_parkingContractRepository.GeefParkingContract(bedrijf.Id) == null)
				throw new ParkeerplaatsManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - Bedrijf heeft geen parking contract");
			try {
				return _parkeerplaatsRepository.GeefNummerplatenPerBedrijf(bedrijf);
			} catch (Exception ex) {
				throw new ParkeerplaatsManagerException(ex.Message);
			}
		}

		public int GeefHuidigBezetteParkeerplaatsenVoorBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new ParkeerplaatsManagerException("ParkeerManager - GeefHuidigBezetteParkeerplaatsenPerBedrijf - Bedrijf mag niet leeg zijn");
			return _parkeerplaatsRepository.GeefHuidigBezetteParkeerplaatsenVoorBedrijf(bedrijf.Id);
		}

		public GrafiekDagDetail GeefUuroverzichtParkingVoorBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new ParkeerplaatsManagerException("ParkeerManager - GeefGrafiekPerBedrijf - Bedrijf mag niet leeg zijn");
			return _parkeerplaatsRepository.GeefUuroverzichtParkingVoorBedrijf(bedrijf.Id);
		}

		public GrafiekDag GeefWeekoverzichtParkingVoorBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new ParkeerplaatsManagerException("ParkeerManager - GeefGrafiekPerBedrijf - Bedrijf mag niet leeg zijn");
			return _parkeerplaatsRepository.GeefWeekoverzichtParkingVoorBedrijf(bedrijf.Id);
		}
	}
}
