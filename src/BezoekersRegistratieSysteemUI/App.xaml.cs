using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI {
	public partial class App : Application {

		public readonly static DispatcherTimer RefreshTimer = new DispatcherTimer();
		public App() {
			RefreshTimer.Interval = TimeSpan.FromSeconds(2);
			RefreshTimer.Start();
		}

		private void VangOntsnapteExceptions(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
			if (e.Exception.InnerException is not null)
				MessageBox.Show(e.Exception.InnerException.Message, "Error", MessageBoxButton.OK);
			else
				MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK);
			e.Handled = true;
		}
	}
}