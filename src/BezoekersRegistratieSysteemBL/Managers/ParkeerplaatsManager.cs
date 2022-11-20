using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemBL.Exceptions.ManagerException;
using BezoekersRegistratieSysteemBL.Interfaces;

namespace BezoekersRegistratieSysteemBL.Managers {
	public class ParkeerplaatsManager {
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
		public ParkeerplaatsManager(IParkeerplaatsRepository parkeerplaatsRepository) {
			this._parkeerplaatsRepository = parkeerplaatsRepository;
		}

		/// <summary>
		/// Checkt nummerplaat in Bij bedrijf in de databank adhv een parkeerplaats object.
		/// </summary>
		/// <param name="parkeerplaats">parkeerplaats object dat toegevoegd wenst te worden aan bedrijf.</param>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VoegNummerplaatToe - Bedrijf mag niet leeg zijn"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VoegNummerplaatToe - Bedrijf heeft geen parking contract"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VoegNummerplaatToe - Nummerplaat bestaat al"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VoegNummerplaatToe - Parking is vol"</exception>
		/// <exception cref="ParkingContractManagerException">ex.Message</exception>
		public void CheckNummerplaatIn(Parkeerplaats parkeerplaats) {
			if (parkeerplaats == null)
				throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf mag niet leeg zijn");
			if (!_nummerplaten.ContainsKey(parkeerplaats.Bedrijf))
				throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Bedrijf heeft geen parking contract");
			if (_nummerplaten[parkeerplaats.Bedrijf].Contains(parkeerplaats.Nummerplaat))
				throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Nummerplaat bestaat al");
			if (_parkingContractManager.GeefParkingContract(parkeerplaats.Bedrijf).AantalPlaatsen == _nummerplaten[parkeerplaats.Bedrijf].Count)
				throw new ParkeerManagerException("ParkeerManager - VoegNummerplaatToe - Parking is vol");
			try {
				_parkeerplaatsRepository.CheckNummerplaatIn(parkeerplaats);
				_nummerplaten[parkeerplaats.Bedrijf].Add(parkeerplaats.Nummerplaat);
			} catch (Exception ex) {
				throw new ParkeerplaatsManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Checkt nummerplaat uit Bij bedrijf in de databank adhv de parameter nummerplaat.
		/// </summary>
		/// <param name="nummerplaat">Nummerplaat die toegevoegd wenst te worden aan bedrijf.</param>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VerwijderNummerplaat - Nummerplaat mag niet leeg zijn"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VerwijderNummerplaat - Nummerplaat bestaat niet"</exception>
		/// <exception cref="ParkingContractManagerException">ex.Message</exception>
		public void CheckNummerplaatUit(string nummerplaat) {
			if (string.IsNullOrWhiteSpace(nummerplaat))
				throw new ParkeerManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat mag niet leeg zijn");
			if (!_parkeerplaatsRepository.BestaatNummerplaat(nummerplaat))
				throw new ParkeerManagerException("ParkeerManager - VerwijderNummerplaat - Nummerplaat bestaat niet");
			try {
				_parkeerplaatsRepository.CheckNummerplaatUit(nummerplaat);
				_nummerplaten.Values.Where(n => n.Equals(nummerplaat)).Select(n => n.Remove(nummerplaat));
			} catch (Exception ex) {
				throw new ParkeerplaatsManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Stelt lijst van nummerplaten per bedrijf samen met enkel lees rechten.
		/// </summary>
		/// <param name="bedrijf">Bedrijf object die toegevoegd wenst te worden aan bedrijf.</param>
		/// <returns>IReadOnlyList van nummerplaten.</returns>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - GeefNummerplatenPerBedrijf - mag niet leeg zijn"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - GeefNummerplatenPerBedrijf - Bedrijf bestaat niet"</exception>
		/// <exception cref="ParkingContractManagerException">ex.Message</exception>
		public IReadOnlyList<string> GeefNummerplatenPerBedrijf(Bedrijf bedrijf) {
			if (bedrijf == null)
				throw new ParkeerManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - mag niet leeg zijn");
			if (!_nummerplaten.ContainsKey(bedrijf))
				throw new ParkeerManagerException("ParkeerManager - GeefNummerplatenPerBedrijf - Bedrijf bestaat niet");
			try {
				return _parkeerplaatsRepository.GeefNummerplatenPerBedrijf(bedrijf);
			} catch (Exception ex) {
				throw new ParkeerplaatsManagerException(ex.Message);
			}
		}

		/// <summary>
		/// Voegt parkingContract toe aan bedrijf in de databank adhv een parkingContract object.
		/// </summary>
		/// <param name="parkingContract">ParkingContract object dat toegevoegd wenst te worden aan bedrijf.</param>
		/// <exception cref="ParkingContractManagerException">"ParkingContractManager - VoegParkingContractToe - ParkingContract mag niet leeg zijn"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkingContractManager - VoegParkingContractToe - ParkingContract bestaat al"</exception>
		/// <exception cref="ParkingContractManagerException">ex.Message</exception>
		public void VoegParkingContractBedrijfToe(ParkingContract parkingContract) {
			if (parkingContract == null)
				throw new ParkeerManagerException("ParkeerManager - VoegBedrijfToe - ParkingContract mag niet leeg zijn");
			if (_nummerplaten.ContainsKey(parkingContract.Bedrijf))
				throw new ParkeerManagerException("ParkeerManager - VoegBedrijfToe - Bedrijf bestaat al");
			_nummerplaten.Add(parkingContract.Bedrijf, new List<string>());
		}

		/// <summary>
		/// Verwijdert gewenste parkingContract van bedrijf adhv een parkingContract object.
		/// </summary>
		/// <param name="parkingContract">ParkingContract object dat verwijderd wenst te worden van bedrijf.</param>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VerwijderBedrijf - ParkingContract mag niet leeg zijn"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VerwijderBedrijf - Bedrijf bestaat niet"</exception>
		/// <exception cref="ParkingContractManagerException">"ParkeerManager - VerwijderBedrijf - Er zijn nog geparkeerden bij dit bedrijf"</exception>
		public void VerwijderParkingContractBedrijf(ParkingContract parkingContract) {
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
