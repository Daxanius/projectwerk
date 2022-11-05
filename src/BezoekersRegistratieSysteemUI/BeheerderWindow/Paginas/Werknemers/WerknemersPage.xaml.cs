using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
	public partial class WerknemersPage : Page {
		private static WerknemersPage instance = null;
		private static readonly object padlock = new object();

		public static WerknemersPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new WerknemersPage();
					}
					return instance;
				}
			}
		}

		/// <summary>
		/// ///////////////////////////////////////////////////
		/// </summary>

		public ObservableCollection<WerknemerDTO> WerknemersVanGeselecteerdBedrijf { get; set; } = new();

		public BedrijfDTO GeselecteerdBedrijf { get; set; }

		public int FullWidth { get; set; }
		public int FullHeight { get; set; }

		public WerknemersPage() {
			GeselecteerdBedrijf = BeheerderWindow.GeselecteerdBedrijf;

			FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;

			this.DataContext = this;
			InitializeComponent();


			WerknemerLijstControl.ItemSource = WerknemersVanGeselecteerdBedrijf;
		}

		private void AddWerknemer(object sender, MouseButtonEventArgs e) {
			WerknemersPopup.Visibility = Visibility.Visible;
		}
	}
}
