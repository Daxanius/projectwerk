using BezoekersRegistratieSysteemUI.Model;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void NieuweWerknemerToegevoegdEvent(WerknemerDTO werknemer);
	public delegate void VerwijderWerknemerEvent(WerknemerDTO werknemer);
	public static class WerknemerEvents {
		public static event NieuweWerknemerToegevoegdEvent NieuweWerknemerToegevoegd;
		public static event VerwijderWerknemerEvent VerwijderWerknemerEvent;

		public static void InvokeUpdateGeselecteerdBedrijf(WerknemerDTO werknemer) => NieuweWerknemerToegevoegd?.Invoke(werknemer);
		public static void InvokeVerwijderWerknemer(WerknemerDTO werknemer) => VerwijderWerknemerEvent?.Invoke(werknemer);
	}
}
