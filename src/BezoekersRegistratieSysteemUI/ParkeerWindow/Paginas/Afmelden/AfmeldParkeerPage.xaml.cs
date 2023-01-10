using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas.Afmelden {
	public partial class AfmeldParkeerPage : Page, INotifyPropertyChanged {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Variabelen

		private string _nummerplaat;

		public string Nummerplaat {
			get { return _nummerplaat; }

			set {
				if (value == _nummerplaat) return;
				_nummerplaat = value.Trim();
				UpdatePropperty();
			}
		}

		#endregion

		public AfmeldParkeerPage() {

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
		private async void AfmeldenClick(object sender, RoutedEventArgs e) {
			try {

				if (Nummerplaat.IsLeeg()) {
					MessageBox.Show("Nummerplaat is leeg!", "Error");
					return;
				}

				//TODO STAN
				bool isValid = await ApiController.Post($"/parkeerplaats/checkout?nummerplaat={Nummerplaat}");
                if (isValid) {
                    MessageBox.Show($"U bent afgemeld.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                } else {
                    MessageBox.Show("Er is iets fout gegaan bij het afmelden in het systeem", "Error /");
                }
                Nummerplaat = "";


			} catch (Exception ex) {
				if (ex.Message.Contains("NotFound")) {
					MessageBox.Show("Deze nummerplaat is ons niet bekend.");
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
		private static AfmeldParkeerPage instance = null;
		private static readonly object padlock = new object();

		public static AfmeldParkeerPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new AfmeldParkeerPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
