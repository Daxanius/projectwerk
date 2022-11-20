using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void NieuweWerknemerToegevoegdEvent(WerknemerDTO werknemer);
	public static class WerknemerEvents {
		public static event NieuweWerknemerToegevoegdEvent NieuweWerknemerToegevoegd;

		public static void InvokeUpdateGeselecteerdBedrijf(WerknemerDTO werknemer) => NieuweWerknemerToegevoegd?.Invoke(werknemer);
	}
}
