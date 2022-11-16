using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bezoekers;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas {
	public partial class SideBarControlAdmin : UserControl {
		public SideBarControlAdmin() {
			InitializeComponent();

			foreach (Border border in BorderContainer.Children) {
				StackPanel stackPanel = (StackPanel)border.Child;
				Icon icon = (Icon)stackPanel.Children[0];
				icon.Opacity = .6;
			}
		}

		private void VeranderTab(object sender, MouseButtonEventArgs e) {
			Border border = (Border)sender;
			StackPanel stackPanel = (StackPanel)border.Child;
			TextBlock textBlock = (TextBlock)stackPanel.Children[1];
			string tab = textBlock.Text;

			ResetSelectedTabs(border);

			Window window = Window.GetWindow(this);
			BeheerderWindow beheerderWindow = (BeheerderWindow)window.DataContext;

			ToggleAanwezigeBezoekersAchtergrond(aanOfUit: false);

			switch (tab) {
				case "Dashboard":
				beheerderWindow.FrameControl.Content = DashBoardPage.Instance;
				break;
				case "Bedrijven":
				beheerderWindow.FrameControl.Content = BedrijvenPage.Instance;
				break;
				case "Afspraken":
				beheerderWindow.FrameControl.Content = AfsprakenPage.Instance;
				break;
				case "Werknemers":
				beheerderWindow.FrameControl.Content = WerknemersPage.Instance;
				break;
                case "Parking":
                beheerderWindow.FrameControl.Content = ParkingPage.Instance;
                break;
            }
		}

		private void ResetSelectedTabs(Border? nieuweActiveTabBorder = null) {
			foreach (Border border in BorderContainer.Children) {
				StackPanel stackPanel = (StackPanel)border.Child;
				TextBlock textBlock = (TextBlock)stackPanel.Children[1];
				Icon icon = (Icon)stackPanel.Children[0];

				border.Tag = "UnSelected";
				textBlock.FontWeight = FontWeights.Medium;
				icon.Opacity = .6;
				if (textBlock.IsEnabled)
					textBlock.Opacity = 1;
			}

			if (nieuweActiveTabBorder is not null) {
				StackPanel stackPanel = (StackPanel)nieuweActiveTabBorder.Child;
				TextBlock textBlock = (TextBlock)stackPanel.Children[1];
				Icon icon = (Icon)stackPanel.Children[0];

				nieuweActiveTabBorder.Tag = "Selected";
				textBlock.FontWeight = FontWeights.Bold;
				icon.Opacity = 1;
				if (textBlock.IsEnabled)
					textBlock.Opacity = 1;
			}
		}

		#region Bezoekers pagina knop
		private void ToonBezoekersPage(object sender, MouseButtonEventArgs e) {
			Window window = Window.GetWindow(this);
			BeheerderWindow beheerderWindow = (BeheerderWindow)window.DataContext;

			ToggleAanwezigeBezoekersAchtergrond(aanOfUit: true);

			ResetSelectedTabs();

			beheerderWindow.FrameControl.Content = new BezoekerPage();
		}

		private void ToggleAanwezigeBezoekersAchtergrond(bool aanOfUit = false) {
			if (aanOfUit) {
				ToonAanwezigenText.Foreground = Application.Current.Resources["MainAchtergrond"] as SolidColorBrush;
				ToonAanwezigenContainer.Background = Application.Current.Resources["GewoonBlauw"] as SolidColorBrush;
				ToonAanwezigenIcon.IconSource = "../WitLijstIcon.xaml";
			} else {
				ToonAanwezigenText.Foreground = Application.Current.Resources["MainBlack"] as SolidColorBrush;
				ToonAanwezigenContainer.Background = Application.Current.Resources["MainAchtergrond"] as SolidColorBrush;
				ToonAanwezigenIcon.IconSource = "../LijstIcon.xaml";
			}
		}
		#endregion
	}
}
