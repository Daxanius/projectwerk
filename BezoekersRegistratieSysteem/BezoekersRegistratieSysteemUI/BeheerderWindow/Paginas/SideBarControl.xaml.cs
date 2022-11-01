using BezoekersRegistratieSysteemUI.Beheerder;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Paginas {
	/// <summary>
	/// Interaction logic for SideBarDashboard.xaml
	/// </summary>
	public partial class SideBarControl : UserControl {
		public SideBarControl() {
			InitializeComponent();

			foreach (Border border in BorderContainer.Children) {
				border.Tag = "UnSelected";
			}
		}

		private void VeranderTab(object sender, MouseButtonEventArgs e) {
			string tab = (string)((Label)((StackPanel)((Border)sender).Child).Children[1]).Content;

			foreach (Border border in BorderContainer.Children) {
				border.Tag = "UnSelected";
				((Label)((StackPanel)(border).Child).Children[1]).FontWeight = FontWeights.Normal;
			}

			((Border)sender).Tag = "Selected";
			((Label)((StackPanel)((Border)sender).Child).Children[1]).FontWeight = FontWeights.Bold;


			Window window = Window.GetWindow(this);
			BeheerderDashboard beheerderDashboard = (BeheerderDashboard)window.DataContext;

			switch (tab) {
				case "Dashboard":
				beheerderDashboard.FrameControl.Source = new Uri("/BeheerderWindow/Paginas/DashBoard/DashBoardPage.xaml", UriKind.Relative);
				break;
				case "Bedrijven":
				beheerderDashboard.FrameControl.Source = new Uri("/BeheerderWindow/Paginas/Bedrijven/BedrijvenPage.xaml", UriKind.Relative);
				break;
				case "Afspraken":
				beheerderDashboard.FrameControl.Source = new Uri("/BeheerderWindow/Paginas/Afspraken/AfsprakenPage.xaml", UriKind.Relative);
				break;
				case "Werknemers":
				beheerderDashboard.FrameControl.Source = new Uri("/BeheerderWindow/Paginas/Werknemers/WerknemersPage.xaml", UriKind.Relative);
				break;
				case "Bezoekers":
				beheerderDashboard.FrameControl.Source = new Uri("/BeheerderWindow/Paginas/Bezoekers/BezoekersPage.xaml", UriKind.Relative);
				break;
			}
		}

		private void ToonAanwezigeBezoekers(object sender, MouseButtonEventArgs e) {

		}
	}
}
