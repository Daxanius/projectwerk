using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void AfspraakToegevoegdEvent(AfspraakDTO afspraak);
	public class AfspraakEvents {
		public static event AfspraakToegevoegdEvent NieuweAfspraakToegevoegd;

		public static void InvokeNieuweAfspraakToegevoegd(AfspraakDTO afspraak) => NieuweAfspraakToegevoegd?.Invoke(afspraak);
	}
}
