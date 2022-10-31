using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Controlls;
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
		}

		private void VeranderTab(object sender, RoutedEventArgs e) {
			Button button = (Button)sender;

			string tab = (string)button.Content;

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
	}
}
