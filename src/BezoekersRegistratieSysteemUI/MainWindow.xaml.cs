using BezoekersRegistratieSysteemUI.AanmeldWindow;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.ParkeerWindow;
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

		private void ParkeerCheckInButton_Click(object sender, RoutedEventArgs e) {
			AanmeldParkeerWindow parkeerWindow = new AanmeldParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}

		private void ParkeerCheckOutButton_Click(object sender, RoutedEventArgs e) {
			AfmeldParkeerWindow parkeerWindow = new AfmeldParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}

		private void AanmeldButton_Click(object sender, RoutedEventArgs e) {
			RegistratieWindow registratieWindow = new RegistratieWindow();
			this.Close();
			registratieWindow.ShowDialog();
		}

		private void AdminButton_Click(object sender, RoutedEventArgs e) {
			BeheerderWindow beheerderWindow = new BeheerderWindow();
			this.Close();
			beheerderWindow.ShowDialog();
		}
	}
}