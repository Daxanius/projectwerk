using BezoekersRegistratieSysteemUI.Model;
using System;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void Werknemer(WerknemerDTO werknemer);
	public delegate void Void();
	public static class WerknemerEvents {
		public static event Werknemer NieuweWerknemerToegevoegd;
		public static event Werknemer VerwijderWerknemer;
		public static event Void UpdateWerknemerScherm;
		public static event Werknemer UpdateWerknemer;

		public static void InvokeNieuweWerkenemer(WerknemerDTO werknemer) => NieuweWerknemerToegevoegd?.Invoke(werknemer);
		public static void InvokeUpdateWerkenemer(WerknemerDTO werknemer) => UpdateWerknemer?.Invoke(werknemer);
		public static void UpdateAlleWerknemerScherm() => UpdateWerknemerScherm?.Invoke();
		public static void InvokeVerwijderWerknemer(WerknemerDTO werknemer) => VerwijderWerknemer?.Invoke(werknemer);
	}
}
