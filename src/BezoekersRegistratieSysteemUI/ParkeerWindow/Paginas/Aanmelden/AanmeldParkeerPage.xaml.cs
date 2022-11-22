using BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas.Aanmelden;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas.Aanmelden {
	public partial class AanmeldParkeerPage : Page, INotifyPropertyChanged {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Variabelen

		private string _nummerplaat;

		public string Nummerplaat
        {
			get { return _nummerplaat; }

			set {
				if (value == _nummerplaat) return;
                _nummerplaat = value.Trim();
				UpdatePropperty();
			}
		}

		#endregion

		public AanmeldParkeerPage() {

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

		#region Functies
		private async void AanmeldenClick(object sender, RoutedEventArgs e) {
			try {
				if (!Nummerplaat.IsEmailGeldig()) {
					MessageBox.Show("Email is niet geldig!", "Error");
					return;
				}

				if (Nummerplaat.IsLeeg()) {
					MessageBox.Show("Email is leeg!", "Error");
					return;
				}

				await ApiController.Put<object>($"/afspraak/end?email={Nummerplaat}");

                Nummerplaat = "";

				MessageBox.Show("U bent afgemeld", "", MessageBoxButton.OK, MessageBoxImage.Information);

				await Task.Delay(TimeSpan.FromSeconds(2));

				//RegistratieWindow registratieWindow = RegistratieWindow.Instance;
				//registratieWindow = (RegistratieWindow)registratieWindow.DataContext;

				//registratieWindow.FrameControl.Content = KiesBedrijfPage.Instance;
				//registratieWindow.SideBar.AanmeldenTab.Tag = "Selected";
				//registratieWindow.SideBar.AfmeldenTab.Tag = "UnSelected";

			} catch (Exception ex) {
				if (ex.Message.Contains("NotFound")) {
					MessageBox.Show("Er is geen afspraak voor dit email adres");
					return;
				}
				MessageBox.Show(ex.Message, "Error");
			}
		}
		#endregion

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged

		#region Singleton
		private static AanmeldParkeerPage instance = null;
		private static readonly object padlock = new object();

		public static AanmeldParkeerPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new AanmeldParkeerPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
