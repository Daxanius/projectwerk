using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
using System;
using System.ComponentModel;
using System.Printing.IndexedProperties;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.Beheerder {

	/// <summary>
	/// Interaction logic for AanOfUitMeldenScherm.xaml
	/// </summary>
	public partial class BeheerderDashboard : Window, INotifyPropertyChanged {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Private Attributes
		private BedrijfDTO _geselecteerdBedrijf { get; set; }
		#endregion

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

			FrameControl.Navigating += OnPageNavigation;
		}

		private void OnPageNavigation(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e) {
			string path = e.Uri.OriginalString;
			string[] pathFolders = new string[] { "Afspraken", "Bedrijven", "Bezoekers", "Dashboard", "Werknemers" };

			foreach (string folder in pathFolders) {
				if (path.Contains(folder)) {

					foreach (Border border in SideBar.BorderContainer.Children) {
						border.Tag = "UnSelected";
					}

					switch (folder) {
						case "Dashboard":
						SideBar.DashboardTab.Tag = "Selected";
						break;

						case "Bedrijven":
						SideBar.BedrijvenTab.Tag = "Selected";
						break;

						case "Afspraken":
							SideBar.AfsprakenTab.Tag = "Selected";
						break;

						case "Bezoekers":
							SideBar.BezoekersTab.Tag = "Selected";
						break;

						case "Werknemers":
							SideBar.WerknemersTab.Tag = "Selected";
						break;
					}
					return;
				}
			}
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