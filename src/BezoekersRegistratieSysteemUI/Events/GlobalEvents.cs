using System;
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void RefreshData();
	public class GlobalEvents {
		public static RefreshData RefreshData;
		public static RefreshData RefreshDataTimout;
		public readonly static DispatcherTimer RefreshTimer = new DispatcherTimer();
		public readonly static DispatcherTimer RefreshTimerTimout = new DispatcherTimer();
		public GlobalEvents() {
			RefreshTimer.Interval = TimeSpan.FromSeconds(5);
			RefreshTimer.Start();
			RefreshTimer.Tick += (object? sender, EventArgs e) => RefreshData?.Invoke();

			//Timout timer for main timer
			RefreshTimerTimout.Interval = TimeSpan.FromSeconds(5);
			RefreshTimerTimout.Tick += (object? sender, EventArgs e) => RefreshDataTimout?.Invoke();
			//_refreshTimerTimout.Start();
		}
		public static void RefreshAllData() => RefreshData?.Invoke();
	}
}
