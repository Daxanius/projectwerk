using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
using System;
using System.ComponentModel;
using System.Printing.IndexedProperties;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BezoekersRegistratieSysteemUI.Beheerder {

	/// <summary>
	/// Interaction logic for AanOfUitMeldenScherm.xaml
	/// </summary>
	public partial class BeheerderDashboard : Window, INotifyPropertyChanged {
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }

		public BedrijfDTO _geselecteerdBedrijf { get; private set; }

		public string Datum {
			get {
				return DateTime.Now.ToString("dd.MM");
			}
		}

		public BeheerderDashboard() {

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

		public void ZetGeselecteerdBedrijf(BedrijfDTO bedrijf) {
			_geselecteerdBedrijf = bedrijf;
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void UpdatePropperty([CallerMemberName] string propertyName = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}