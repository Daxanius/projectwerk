using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI.Events {
	public delegate void RefreshData();
	public class GlobalEvents {
		public static RefreshData RefreshData;
		public static RefreshData RefreshDataTimout;
		public readonly static DispatcherTimer _refreshTimer = new DispatcherTimer();
		public readonly static DispatcherTimer _refreshTimerTimout = new DispatcherTimer();
		public GlobalEvents() {
			_refreshTimer.Interval = TimeSpan.FromSeconds(5);
			_refreshTimer.Start();
			_refreshTimer.Tick += (object? sender, EventArgs e) => RefreshData?.Invoke();

            //Timout timer for main timer
            _refreshTimerTimout.Interval = TimeSpan.FromSeconds(5);
            _refreshTimerTimout.Tick += (object? sender, EventArgs e) => RefreshDataTimout?.Invoke();
            //_refreshTimerTimout.Start();
		}
	}
}
