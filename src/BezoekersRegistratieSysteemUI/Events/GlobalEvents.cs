using System;
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void RefreshData();
	public class GlobalEvents {
		public static RefreshData RefreshData;
		public static RefreshData RefreshDataTimout;
		public readonly static DispatcherTimer _refreshTimer = new DispatcherTimer();
		public readonly static DispatcherTimer _refreshTimerTimout = new DispatcherTimer();
		public GlobalEvents() {
			_refreshTimer.Interval = TimeSpan.FromSeconds(500);
			_refreshTimer.Start();
			_refreshTimer.Tick += (object? sender, EventArgs e) => RefreshData?.Invoke();

			//Timout timer for main timer
			_refreshTimerTimout.Interval = TimeSpan.FromSeconds(500);
			_refreshTimerTimout.Tick += (object? sender, EventArgs e) => RefreshDataTimout?.Invoke();
			//_refreshTimerTimout.Start();
		}
	}
}
