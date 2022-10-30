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

			string tab = button.Content.ToString();

			Window window = Window.GetWindow(this);
			BeheerderDashboard beheerderDashboard = window.DataContext as BeheerderDashboard;
			TabControl tabControl = beheerderDashboard.TabNavigeerder;

			switch (tab) {
				case "Dashboard":
				tabControl.TabIndex = 0;
				ZetTabsOpNietGeselecteed(beheerderDashboard);
				beheerderDashboard.DashboardTab.IsSelected = true;
				break;
				case "Bedrijven":
				tabControl.TabIndex = 1;
				ZetTabsOpNietGeselecteed(beheerderDashboard);
				beheerderDashboard.BedrijvenTab.IsSelected = true;
				break;
				case "Afspraken":
				tabControl.TabIndex = 2;
				ZetTabsOpNietGeselecteed(beheerderDashboard);
				beheerderDashboard.AfsprakenTab.IsSelected = true;
				break;
				case "Werknemers":
				tabControl.TabIndex = 3;
				ZetTabsOpNietGeselecteed(beheerderDashboard);
				beheerderDashboard.WerknemersTab.IsSelected = true;
				break;
				case "Bezoekers":
				tabControl.TabIndex = 4;
				ZetTabsOpNietGeselecteed(beheerderDashboard);
				beheerderDashboard.BezoekersTab.IsSelected = true;
				break;
			}
		}

		void ZetTabsOpNietGeselecteed(BeheerderDashboard beheerderDashboard) {
			beheerderDashboard.DashboardTab.IsSelected = false;
			beheerderDashboard.BedrijvenTab.IsSelected = false;
			beheerderDashboard.AfsprakenTab.IsSelected = false;
			beheerderDashboard.WerknemersTab.IsSelected = false;
			beheerderDashboard.BezoekersTab.IsSelected = false;
		}
	}
}
