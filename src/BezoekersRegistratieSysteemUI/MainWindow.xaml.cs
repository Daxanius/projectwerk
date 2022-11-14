using BezoekersRegistratieSysteemUI.AanmeldWindow;
using BezoekersRegistratieSysteemUI.Beheerder;
using System.Windows;

namespace BezoekersRegistratieSysteemUI {
	public partial class MainWindow : Window {
		public double ScaleX { get; set; }

		public double ScaleY { get; set; }

		public MainWindow() {
			double schermResolutieHeight = System.Windows.SystemParameters.MaximizedPrimaryScreenHeight;
			double schermResolutieWidth = System.Windows.SystemParameters.MaximizedPrimaryScreenWidth;

			double defaultResHeight = 1080.0;
			double defaultResWidth = 1920.0;

			double aspectratio = schermResolutieWidth / schermResolutieHeight;
			double change = aspectratio > 2 ? 0.3 : aspectratio > 1.5 ? 0 : aspectratio > 1 ? -0.05 : -0.3;

			ScaleX = (schermResolutieWidth / defaultResWidth);
			ScaleY = (schermResolutieHeight / defaultResHeight) + change;

			this.DataContext = this;
			InitializeComponent();
		}

		private void ParkeerButton_Click(object sender, RoutedEventArgs e) {
			ParkeerWindow parkeerWindow = new ParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}

		private void AanmeldButton_Click(object sender, RoutedEventArgs e) {
			RegistratieWindow registratieWindow = new RegistratieWindow();
			this.Close();
			registratieWindow.ShowDialog();
		}

		private void AdminButton_Click(object sender, RoutedEventArgs e) {
			BeheerderWindow parkeerWindow = new BeheerderWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}
	}
}