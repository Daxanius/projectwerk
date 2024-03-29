﻿using BezoekersRegistratieSysteemBL.Interfaces;
using BezoekersRegistratieSysteemDL.ADOMS;
using BezoekersRegistratieSysteemDL.ADOMySQL;

namespace BezoekerRegistratieSysteemDLPicker {
	public class BezoekersRegistratieBeheerRepo {
		public BezoekersRegistratieBeheerRepo(string conString, RepoType repoType) {
			try {
				switch (repoType) {
					case RepoType.MSSERVER:
						afspraakrepository = new AfspraakRepoMsServer(conString);
						bedrijfRepository = new BedrijfRepoMsServer(conString);
						parkeerplaatsRepository = new ParkeerPlaatsMsServer(conString);
						parkingContractRepository = new ParkingContractMsServer(conString);
						werknemerRepository = new WerknemerRepoMsServer(conString);
						break;
					case RepoType.MYSQL:
						afspraakrepository = new AfspraakRepoMySQL(conString);
						bedrijfRepository = new BedrijfRepoMySQL(conString);
						parkeerplaatsRepository = new ParkeerPlaatsMySQL(conString);
						parkingContractRepository = new ParkingContractMySQL(conString);
						werknemerRepository = new WerknemerRepoMySQL(conString);
						break;
					default:
						throw new Exception("Database type not found.");
				}
			} catch (Exception ex) {
				throw new DLPickerException($"{this.GetType()}: {System.Reflection.MethodBase.GetCurrentMethod().Name} {ex.Message}", ex);
			}
		}

		public IAfspraakRepository afspraakrepository { get; }
		public IBedrijfRepository bedrijfRepository { get; }
		public IParkeerplaatsRepository parkeerplaatsRepository { get; }
		public IParkingContractRepository parkingContractRepository { get; }
		public IWerknemerRepository werknemerRepository { get; }

	}
}
