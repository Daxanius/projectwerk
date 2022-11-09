using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
	public partial class WerknemersPage : Page, INotifyPropertyChanged {
		public int FullWidth { get; set; }
		public int FullHeight { get; set; }
		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }

		public WerknemersPage() {
			FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;

			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			WerknemerLijstControl.ItemSource = new(ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf));
		}

		private void UpdateGeselecteerdBedrijfOpScherm() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			WerknemerLijstControl.ItemSource = new(ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf));
		}

		private void AddWerknemer(object sender, MouseButtonEventArgs e) {
			WerknemersPopup.Visibility = Visibility.Visible;
		}

		#region Singleton
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
		#endregion

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}
