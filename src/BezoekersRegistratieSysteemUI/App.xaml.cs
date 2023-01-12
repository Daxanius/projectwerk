using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using System;
using System.Windows;

namespace BezoekersRegistratieSysteemUI {
	public partial class App : Application {
		private GlobalEvents globalEvents;
		public App() {
			globalEvents = new GlobalEvents();
			BeeindigAlleOnAfgeslotenAfspraken();

			if (!BezoekersRegistratieSysteemUI.Properties.Settings.Default.IsSynchronized) {
				BezoekersRegistratieSysteemUI.Properties.Settings.Default.Save();
			}

			if (string.IsNullOrWhiteSpace(BezoekersRegistratieSysteemUI.Properties.Settings.Default.URI)) {
				new CustomMessageBox().Show("Endpoint URI is niet ingesteld!", "Fout", ECustomMessageBoxIcon.Error);
				Environment.Exit(1);
				return;
			}

			ApiController.BaseAddres = BezoekersRegistratieSysteemUI.Properties.Settings.Default.URI ?? "http://localhost:5049/api/";

			System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("fr-FR");
			System.Threading.Thread.CurrentThread.CurrentCulture = ci;
		}

		private void BeeindigAlleOnAfgeslotenAfspraken() {
			ApiController.BeeindigAlleOnAfgeslotenAfspraken();
		}

		private void VangOntsnapteExceptions(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
			if (e.Exception.InnerException is not null && e.Exception.InnerException.Message.Contains("No connection could be made because the target machine actively refused it"))
				new CustomMessageBox().Show("Kan niet verbinden met de database, Ze staat wellicht niet aan ;-)", "Verbindings Fout", ECustomMessageBoxIcon.Error);
			//else if (e.Exception.InnerException is not null)
			//	MessageBox.Show(e.Exception.InnerException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			else
				new CustomMessageBox().Show(e.Exception.Message, "Error", ECustomMessageBoxIcon.Error);
			e.Handled = true;
		}
	}
}