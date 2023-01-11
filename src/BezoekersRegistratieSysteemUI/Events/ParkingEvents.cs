using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void NieuweParkeerplaatsInChecken(ParkeerplaatsDTO parkeerplaats);
	public delegate void ParkeerplaatsUitChecken(ParkeerplaatsDTO parkeerplaats);
	public class ParkingEvents {
		public static event NieuweParkeerplaatsInChecken NieuweNummerplaatInGeChecked;
		public static event ParkeerplaatsUitChecken NummerplaatUitChecken;
		public static void NieuweParkeerplaatsInChecken(ParkeerplaatsDTO parkeerplaats) => NieuweNummerplaatInGeChecked?.Invoke(parkeerplaats);
		public static void ParkeerplaatsUitChecken(ParkeerplaatsDTO parkeerplaats) => NummerplaatUitChecken?.Invoke(parkeerplaats);
	}
}
