using BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers;
using BezoekersRegistratieSysteemUI.Controlls;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas {
	/// <summary>
	/// Interaction logic for SideBarDashboard.xaml
	/// </summary>
	public partial class SideBarControlAdmin : UserControl {
		public SideBarControlAdmin() {
			InitializeComponent();

			foreach (Border border in BorderContainer.Children) {
				((Icon)((StackPanel)border.Child).Children[0]).Opacity = .6;
			}
		}

		private void VeranderTab(object sender, MouseButtonEventArgs e) {
			string tab = ((TextBlock)((StackPanel)((Border)sender).Child).Children[1]).Text;

			foreach (Border border in BorderContainer.Children) {
				border.Tag = "UnSelected";
				((TextBlock)((StackPanel)(border).Child).Children[1]).FontWeight = FontWeights.Normal;
				((Icon)((StackPanel)border.Child).Children[0]).Opacity = .6;
			}

			((Border)sender).Tag = "Selected";
			((TextBlock)((StackPanel)((Border)sender).Child).Children[1]).FontWeight = FontWeights.Bold;
			((Icon)((StackPanel)((Border)sender).Child).Children[0]).Opacity = 1;


			Window window = Window.GetWindow(this);
			BeheerderWindow beheerderWindow = (BeheerderWindow)window.DataContext;

			switch (tab) {
				case "Dashboard":
				beheerderWindow.FrameControl.Navigate(DashBoardPage.Instance);
				break;
				case "Bedrijven":
				beheerderWindow.FrameControl.Navigate(BedrijvenPage.Instance);
				break;
				case "Afspraken":
				beheerderWindow.FrameControl.Navigate(AfsprakenPage.Instance);
				break;
				case "Werknemers":
				beheerderWindow.FrameControl.Navigate(WerknemersPage.Instance);
				break;
			}
		}

		private bool _isAanwezigeBezoekersPressed;
		private void ToggleAanwezigeBezoekersAchtergrond(object sender, MouseButtonEventArgs e) {
			if(!_isAanwezigeBezoekersPressed) {
				ToonAanwezigenText.Foreground = Application.Current.Resources["MainAchtergrond"] as SolidColorBrush;
				ToonAanwezigenContainer.Background = Application.Current.Resources["GewoonBlauw"] as SolidColorBrush;
				ToonAanwezigenIcon.IconSource = "../WitLijstIcon.xaml";
				_isAanwezigeBezoekersPressed = true;
			} else {
				ToonAanwezigenText.Foreground = Application.Current.Resources["MainBlack"] as SolidColorBrush;
				ToonAanwezigenContainer.Background = Application.Current.Resources["MainAchtergrond"] as SolidColorBrush;
				ToonAanwezigenIcon.IconSource = "../LijstIcon.xaml";
				_isAanwezigeBezoekersPressed = false;
			}
		}
	}
}
