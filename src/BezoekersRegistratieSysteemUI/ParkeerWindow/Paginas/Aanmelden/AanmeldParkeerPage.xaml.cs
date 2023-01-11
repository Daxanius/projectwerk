using BezoekersRegistratieSysteemUI.AanmeldWindow;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.ParkeerWindow.Paginas.Aanmelden {
	public partial class AanmeldParkeerPage : Page, INotifyPropertyChanged {
		#region Events

		public event PropertyChangedEventHandler? PropertyChanged;

		#endregion

		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Variabelen

		private string _nummerplaat;
		private CustomMessageBox _mb;

		public string Nummerplaat {
			get { return _nummerplaat; }

			set {
				if (value == _nummerplaat) return;
				_nummerplaat = value.Trim();
				UpdatePropperty();
			}
		}

		private BedrijfDTO _geselecteerdBedrijf;
		public BedrijfDTO GeselecteerdBedrijf {
			get { return _geselecteerdBedrijf; }
			set {
				if (value.Naam == _geselecteerdBedrijf?.Naam) return;
				_geselecteerdBedrijf = value;
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

			_mb = new();

			GeselecteerdBedrijf = AanmeldParkeerWindow.GeselecteerdBedrijf;

			if (GeselecteerdBedrijf is null) {
				_mb = new();
				_mb.Show("Bedrijf is niet gekozen", "Error", ECustomMessageBoxIcon.Error);
				((AanmeldParkeerWindow)Window.GetWindow(this)).FrameControl.Content = KiesBedrijfPage.Instance;
				return;
			}
		}

		#region Functies
		private async void AanmeldenKlik(object sender, RoutedEventArgs e) {
			try {

				if (AanmeldParkeerWindow.GeselecteerdBedrijf is null)
				{
					_mb = new();
					_mb.Show("Er is geen bedrijf geselecteerd", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (Nummerplaat.IsLeeg()) {
					_mb = new();
					_mb.Show("Nummerplaat is leeg", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				_mb = new();
				var result = _mb.Show($"Zijn ingevoerde gegevens correct?\n\nNummerplaat: {Nummerplaat}", "Bevestiging", ECustomMessageBoxIcon.Question);

				if (result == ECustomMessageBoxResult.Bevestigen)
					MaakNieuweCheckIn(GeselecteerdBedrijf.Id, Nummerplaat);

				else return;

				Nummerplaat = "";

			} catch (Exception ex) {
				_mb.Show(ex.Message.Substring(ex.Message.LastIndexOf("- ")), "Fout", ECustomMessageBoxIcon.Error);
				return;
			}
            GaTerugNaarKiesBedrijf();
		}

		private void AnnulerenKlik(object sender, RoutedEventArgs e) {
			GaTerugNaarKiesBedrijf();
		}

		private void GaTerugNaarKiesBedrijf() {
			Nummerplaat = "";

			AanmeldParkeerWindow aanmeldParkeerWindow = (AanmeldParkeerWindow)Window.GetWindow(this);
			aanmeldParkeerWindow.FrameControl.Content = KiesBedrijfPage.Instance;
		}
        private async void MaakNieuweCheckIn(long bedrijfsId, string nummerplaat)
        {
			try {
				var rawBody = new { bedrijfId = bedrijfsId, checkinTijd = DateTime.Now, nummerplaat = nummerplaat };
				string json = JsonConvert.SerializeObject(rawBody);
				bool isvalid = await ApiController.Post("/parkeerplaats/checkin", json);

				if (isvalid) {
					_mb = new();
					_mb.Show("Uw registratie werd goed ontvangen.", "Success", ECustomMessageBoxIcon.Information);
				} else {
					_mb = new();
					_mb.Show("Er is iets fout gegaan bij het registreren in het systeem", "Error /", ECustomMessageBoxIcon.Error);
				}
			} catch (Exception ex) {
				_mb = new();
				_mb.Show(ex.Message.Substring(ex.Message.LastIndexOf("- ")), "Fout", ECustomMessageBoxIcon.Error);
			}
		}
        #endregion

        #region ProppertyChanged
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
