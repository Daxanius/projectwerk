using System.Windows;

namespace BezoekersRegistratieSysteemUI {

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private void VangOntsnapteExceptions(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
			if (e.Exception.InnerException is not null)
				MessageBox.Show(e.Exception.Message + "\n\n Inner ex:" + e.Exception.InnerException.Message, "Error", MessageBoxButton.OK);
			else
				MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK);
			e.Handled = true;
		}
	}
}