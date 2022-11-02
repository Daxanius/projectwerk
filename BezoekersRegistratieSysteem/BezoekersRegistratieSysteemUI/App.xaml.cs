using System.Windows;

namespace BezoekersRegistratieSysteemUI {

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private void VangOntsnapteExceptions(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
			MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK);
			e.Handled = true;
		}
	}
}