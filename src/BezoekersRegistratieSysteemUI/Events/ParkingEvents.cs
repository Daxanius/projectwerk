using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void NieuweParkeerplaatsInChecken(ParkeerplaatsDTO parkeerplaats);
	public delegate void ParkeerplaatsUitChecken(ParkeerplaatsDTO parkeerplaats);
	public delegate void ParkingContractUpdated(ParkingContractoutputDTO parkeerplaats);
	public class ParkingEvents {
		public static event NieuweParkeerplaatsInChecken NieuweNummerplaatInGeChecked;
		public static event ParkeerplaatsUitChecken NummerplaatUitChecken;
		public static event ParkingContractUpdated ParkingContractUpdated;
		public static void NieuweParkeerplaatsInChecken(ParkeerplaatsDTO parkeerplaats) => NieuweNummerplaatInGeChecked?.Invoke(parkeerplaats);
		public static void ParkeerplaatsUitChecken(ParkeerplaatsDTO parkeerplaats) => NummerplaatUitChecken?.Invoke(parkeerplaats);
		public static void UpdateParkingContract(ParkingContractoutputDTO parkingContract) => ParkingContractUpdated?.Invoke(parkingContract);
	}
}
