using BezoekersRegistratieSysteem.UI.Controlls;
using System.Windows;

namespace BezoekersRegistratieSysteem.UI {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public double ScaleX { get; set; }

		public double ScaleY { get; set; }

		public MainWindow() {

			double schermResolutieHeight = System.Windows.SystemParameters.MaximizedPrimaryScreenHeight;
			double schermResolutieWidth = System.Windows.SystemParameters.MaximizedPrimaryScreenWidth;

			double defaultResHeight = 1080.0;
			double defaultResWidth = 1920.0;

			ScaleX = (schermResolutieWidth / defaultResWidth);
			ScaleY = (schermResolutieHeight / defaultResHeight);

			this.DataContext = this;
			InitializeComponent();
		}

		private void ParkeerButton_Click(object sender, RoutedEventArgs e) {
			ParkeerWindow parkeerWindow = new ParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}

		private void AanmeldButton_Click(object sender, RoutedEventArgs e) {
			AanOfUitMeldenScherm aanmeldWindow = new AanOfUitMeldenScherm();
			this.Close();
			aanmeldWindow.ShowDialog();
		}

		private void AdminButton_Click(object sender, RoutedEventArgs e) {
			ParkeerWindow parkeerWindow = new ParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}
	}
}
