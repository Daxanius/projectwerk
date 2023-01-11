using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.MessageBoxes;
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
		private CustomMessageBox _mb = new();

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
					_mb = new();
					_mb.Show("Nummerplaat is leeg!", "Error", ECustomMessageBoxIcon.Error);
					return;
				}

				//TODO STAN
				bool isValid = await ApiController.Post($"/parkeerplaats/checkout/{Nummerplaat}");
                if (isValid) {
					_mb = new();
                    _mb.Show($"U bent afgemeld.", "Success", ECustomMessageBoxIcon.Information);
                } else {
					_mb = new();
                    _mb.Show("Er is iets fout gegaan bij het afmelden in het systeem", "Error", ECustomMessageBoxIcon.Error);
                }
                Nummerplaat = "";


			} catch (Exception ex) {
				_mb = new();
				if (ex.Message.Contains("NotFound")) {
					_mb.Show("Deze nummerplaat is ons niet bekend.", "Error", ECustomMessageBoxIcon.Error);
					return;
				}
				_mb.Show(ex.Message, "Error", ECustomMessageBoxIcon.Error);
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
