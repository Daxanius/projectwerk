using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Events;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI {
	public partial class App : Application {
		private GlobalEvents globalEvents;
		public App() {
			globalEvents = new GlobalEvents();
			BeeindigAlleOnAfgeslotenAfspraken();

			System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("fr-FR");
			System.Threading.Thread.CurrentThread.CurrentCulture = ci;
		}

		private void BeeindigAlleOnAfgeslotenAfspraken() {
			ApiController.BeeindigAlleOnAfgeslotenAfspraken();
		}

		private void VangOntsnapteExceptions(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
			if (e.Exception.InnerException is not null && e.Exception.InnerException.Message.Contains("No connection could be made because the target machine actively refused it"))
				MessageBox.Show("Kan niet verbinden met de database, Ze staat wellicht niet aan ;-)", "Verbindings Fout", MessageBoxButton.OK, MessageBoxImage.Error);
			//else if (e.Exception.InnerException is not null)
			//	MessageBox.Show(e.Exception.InnerException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			else
				MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			e.Handled = true;
		}
	}
}