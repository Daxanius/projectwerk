using BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bezoekers;
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
			string tab = (string)((Label)((StackPanel)((Border)sender).Child).Children[1]).Content;

			foreach (Border border in BorderContainer.Children) {
				border.Tag = "UnSelected";
				((Label)((StackPanel)(border).Child).Children[1]).FontWeight = FontWeights.Normal;
				((Icon)((StackPanel)border.Child).Children[0]).Opacity = .6;
			}

			((Border)sender).Tag = "Selected";
			((Label)((StackPanel)((Border)sender).Child).Children[1]).FontWeight = FontWeights.Bold;
			((Icon)((StackPanel)((Border)sender).Child).Children[0]).Opacity = 1;


			Window window = Window.GetWindow(this);
			BeheerderWindow beheerderWindow = (BeheerderWindow)window.DataContext;

			switch (tab) {
				case "Dashboard":
				beheerderWindow.FrameControl.Navigate(new DashBoardPage());
				break;
				case "Bedrijven":
				beheerderWindow.FrameControl.Navigate(new BedrijvenPage());
				break;
				case "Afspraken":
				beheerderWindow.FrameControl.Navigate(new AfsprakenPage());
				break;
				case "Werknemers":
				beheerderWindow.FrameControl.Navigate(new WerknemersPage());
				break;
				case "Bezoekers":
				beheerderWindow.FrameControl.Navigate(new BezoekersPage());
				break;
			}
		}

		private void ToonAanwezigeBezoekers(object sender, MouseButtonEventArgs e) {

		}
	}
}
