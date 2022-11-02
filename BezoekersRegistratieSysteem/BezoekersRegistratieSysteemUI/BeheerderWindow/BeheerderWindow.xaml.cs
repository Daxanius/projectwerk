using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.ComponentModel;
using System.Printing.IndexedProperties;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.Beheerder {

	/// <summary>
	/// Interaction logic for AanOfUitMeldenScherm.xaml
	/// </summary>
	public partial class BeheerderWindow : Window, INotifyPropertyChanged {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Public propperty
		public static BedrijfDTO GeselecteerdBedrijf { get; private set; }
		#endregion

		public BeheerderWindow() {

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
			string path = e.Content.GetType().Name;
			string[] pathFolders = new string[] { "Afspraken", "Bedrijven", "Bezoekers", "Dashboard", "Werknemers" };

			if(GeselecteerdBedrijf is not null) {
				SideBar.AfsprakenTab.IsEnabled = true;
				SideBar.BezoekersTab.IsEnabled = true;
				SideBar.WerknemersTab.IsEnabled = true;
			} else {
				SideBar.AfsprakenTab.IsEnabled = false;
				SideBar.BezoekersTab.IsEnabled = false;
				SideBar.WerknemersTab.IsEnabled = false;
			}

			foreach (string folder in pathFolders) {
				if (path.Contains(folder)) {

					foreach (Border border in SideBar.BorderContainer.Children) {
						border.Tag = "UnSelected";
						((Label)((StackPanel)border.Child).Children[1]).FontWeight = FontWeights.Medium;
						((Icon)((StackPanel)border.Child).Children[0]).Opacity = .6;
					}

					switch (folder) {
						case "Dashboard":
						SideBar.DashboardTab.Tag = "Selected";
						((Icon)((StackPanel)SideBar.DashboardTab.Child).Children[0]).Opacity = 1;
						((Label)((StackPanel)SideBar.DashboardTab.Child).Children[1]).FontWeight = FontWeights.Bold;
						break;

						case "Bedrijven":
						SideBar.BedrijvenTab.Tag = "Selected";
						((Icon)((StackPanel)SideBar.BedrijvenTab.Child).Children[0]).Opacity = 1;
						((Label)((StackPanel)SideBar.BedrijvenTab.Child).Children[1]).FontWeight = FontWeights.Bold;
						break;

						case "Afspraken":
						SideBar.AfsprakenTab.Tag = "Selected";
						((Icon)((StackPanel)SideBar.AfsprakenTab.Child).Children[0]).Opacity = 1; ;
						((Label)((StackPanel)SideBar.AfsprakenTab.Child).Children[1]).FontWeight = FontWeights.Bold;
						break;

						case "Bezoekers":
						SideBar.BezoekersTab.Tag = "Selected";
						((Icon)((StackPanel)SideBar.BezoekersTab.Child).Children[0]).Opacity = 1;
						((Label)((StackPanel)SideBar.BezoekersTab.Child).Children[1]).FontWeight = FontWeights.Bold;
						break;

						case "Werknemers":
						SideBar.WerknemersTab.Tag = "Selected";
						((Icon)((StackPanel)SideBar.WerknemersTab.Child).Children[0]).Opacity = 1;
						((Label)((StackPanel)SideBar.WerknemersTab.Child).Children[1]).FontWeight = FontWeights.Bold;
						break;
					}
					return;
				}
			}
		}

		public void ZetGeselecteerdBedrijf(BedrijfDTO bedrijf) {
			GeselecteerdBedrijf = bedrijf;
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void UpdatePropperty([CallerMemberName] string propertyName = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}