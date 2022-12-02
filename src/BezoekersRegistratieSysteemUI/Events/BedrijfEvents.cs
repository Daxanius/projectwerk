using BezoekersRegistratieSysteemUI.Model;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void GeselecteerdBedrijfChanged();
	public delegate void NieuwBedrijfToeGevoegd(BedrijfDTO bedrijf);
	public delegate void BedrijfVerwijderd(BedrijfDTO bedrijf);
	public delegate void BedrijfGeupdate(BedrijfDTO bedrijf);
	public static class BedrijfEvents {
		public static event GeselecteerdBedrijfChanged UpdateGeselecteerdBedrijf;
		public static event NieuwBedrijfToeGevoegd NieuwBedrijfToeGevoegd;
		public static event BedrijfVerwijderd BedrijfVerwijderd;
		public static event BedrijfGeupdate BedrijfGeupdate;

		public static void InvokeUpdateGeselecteerdBedrijf() => UpdateGeselecteerdBedrijf?.Invoke();
		public static void InvokeNieuwBedrijfToeGevoegd(BedrijfDTO bedrijf) => NieuwBedrijfToeGevoegd?.Invoke(bedrijf);
		public static void InvokeBedrijfVerwijderd(BedrijfDTO bedrijf) => BedrijfVerwijderd?.Invoke(bedrijf);
		public static void InvokeBedrijfGeupdate(BedrijfDTO bedrijf) => BedrijfGeupdate?.Invoke(bedrijf);
	}
}
