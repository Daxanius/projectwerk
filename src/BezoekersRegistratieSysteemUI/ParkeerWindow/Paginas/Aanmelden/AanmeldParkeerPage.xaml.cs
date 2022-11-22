﻿using BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas.Aanmelden;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.ParkeerWindow;

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

				if (Nummerplaat.IsLeeg()) {
					MessageBox.Show("Nummerplaat is leeg!", "Error");
					return;
				}

					CustomMessageBox messagebox = new CustomMessageBox();
					var result = messagebox.Show($"Zijn ingevoerde gegevens correct?\n\nNummerplaat: {Nummerplaat}", "Bevestiging", ECustomMessageBoxIcon.Question);
                    //if (result==ECustomMessageBoxResult.Sluit)
                        //MaakParkeerplaats
                    //else return;

                //await ApiController.Put<object>($"/parkeerplaats/ckeckin={Nummerplaat}");

                Nummerplaat = "";

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            GaTerugNaarKiesBedrijf();
		}

        private void GaTerugNaarKiesBedrijf()
        {
            Nummerplaat = "";

            AanmeldParkeerWindow aanmeldParkeerWindow = (AanmeldParkeerWindow)Window.GetWindow(this);
            aanmeldParkeerWindow.FrameControl.Content = KiesBedrijfPage.Instance;
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
