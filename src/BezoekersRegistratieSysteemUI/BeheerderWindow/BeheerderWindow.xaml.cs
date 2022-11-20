using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.Beheerder {

	public delegate void GeselecteerdbedrijfChanged();
	public partial class BeheerderWindow : Window, INotifyPropertyChanged {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Event
		public static event GeselecteerdbedrijfChanged UpdateGeselecteerdBedrijf;
		#endregion

		#region Variabelen

		private static BedrijfDTO _geselecteerdBedrijf;
		public static BedrijfDTO GeselecteerdBedrijf {
			get => _geselecteerdBedrijf;
			set {
				_geselecteerdBedrijf = value;
				UpdateGeselecteerdBedrijf?.Invoke();
			}
		}

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

			DashBoardPage.Instance.InitializeComponent();
			FrameControl.Content = DashBoardPage.Instance;
			FrameControl.Navigating += OnPageNavigation;
		}

		#region Functies

		private void OnPageNavigation(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e) {
			string path = e.Content.GetType().Name;
			string[] pathFolders = new string[] { "Afspraken", "Bedrijven", "Dashboard", "Werknemers", "Parking" };

			if (GeselecteerdBedrijf is not null) {
				SideBar.AfsprakenTab.IsEnabled = true;
				SideBar.WerknemersTab.IsEnabled = true;
				SideBar.ParkingTab.IsEnabled = true;
			} else {
				SideBar.AfsprakenTab.IsEnabled = false;
				SideBar.WerknemersTab.IsEnabled = false;
                SideBar.ParkingTab.IsEnabled = false;
            }

			foreach (string folder in pathFolders) {
				if (path.Contains(folder)) {

					StackPanel? stackPanel;
					TextBlock? textBlock;
					Icon? icon;

					foreach (Border border in SideBar.BorderContainer.Children) {
						border.Tag = "UnSelected";

						stackPanel = (StackPanel)border.Child;
						textBlock = (TextBlock)stackPanel.Children[1];
						icon = (Icon)stackPanel.Children[0];

						textBlock.FontWeight = FontWeights.Medium;
						icon.Opacity = .6;
					}

					switch (folder) {
						case "Dashboard":
						SideBar.DashboardTab.Tag = "Selected";

						stackPanel = (StackPanel)SideBar.DashboardTab.Child;
						textBlock = (TextBlock)stackPanel.Children[1];
						icon = (Icon)stackPanel.Children[0];

						textBlock.FontWeight = FontWeights.Bold;
						icon.Opacity = 1;
						break;

						case "Bedrijven":
						SideBar.BedrijvenTab.Tag = "Selected";

						stackPanel = (StackPanel)SideBar.BedrijvenTab.Child;
						textBlock = (TextBlock)stackPanel.Children[1];
						icon = (Icon)stackPanel.Children[0];

						textBlock.FontWeight = FontWeights.Bold;
						icon.Opacity = 1;
						break;

						case "Afspraken":
						SideBar.AfsprakenTab.Tag = "Selected";

						stackPanel = (StackPanel)SideBar.AfsprakenTab.Child;
						textBlock = (TextBlock)stackPanel.Children[1];
						icon = (Icon)stackPanel.Children[0];

						textBlock.FontWeight = FontWeights.Bold;
						icon.Opacity = 1;
						break;

						case "Werknemers":
						SideBar.WerknemersTab.Tag = "Selected";

						stackPanel = (StackPanel)SideBar.WerknemersTab.Child;
						textBlock = (TextBlock)stackPanel.Children[1];
						icon = (Icon)stackPanel.Children[0];

						textBlock.FontWeight = FontWeights.Bold;
						icon.Opacity = 1;
						break;

                        case "Parking":
                        SideBar.ParkingTab.Tag = "Selected";

                        stackPanel = (StackPanel)SideBar.ParkingTab.Child;
                        textBlock = (TextBlock)stackPanel.Children[1];
                        icon = (Icon)stackPanel.Children[0];

                        textBlock.FontWeight = FontWeights.Bold;
                        icon.Opacity = 1;
                        break;
                    }
					return;
				}
			}
		}

		#endregion

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void UpdatePropperty([CallerMemberName] string propertyName = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged

		#region Singleton
		private static BeheerderWindow instance = null;
		private static readonly object padlock = new object();

		public static BeheerderWindow Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new BeheerderWindow();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}