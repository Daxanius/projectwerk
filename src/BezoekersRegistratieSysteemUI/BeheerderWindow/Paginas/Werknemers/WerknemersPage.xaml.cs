using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers {
	public partial class WerknemersPage : Page, INotifyPropertyChanged {
		public int FullWidth { get; set; }
		public int FullHeight { get; set; }
		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }

		private List<WerknemerDTO> initieleWerknemers;
		private string _zoekText;
		public string ZoekText {
			get => _zoekText;
			set {
				if (!string.IsNullOrWhiteSpace(value)) {
					_zoekText = value;

					List<WerknemerDTO> result = initieleWerknemers.Where(w => w.Voornaam.Contains(_zoekText) ||
					w.Achternaam.Contains(_zoekText) ||
					w.Email.Contains(_zoekText) || 
					w.Functie.Contains(_zoekText) ||
					w.Status.ToString().Contains(_zoekText)).ToList();

					WerknemerLijstControl.ItemSource.Clear();

					foreach (WerknemerDTO bedrijf in result) {
						WerknemerLijstControl.ItemSource.Add(bedrijf);
					}

				} else if (value.Length == 0) {
					WerknemerLijstControl.ItemSource.Clear();
					foreach (WerknemerDTO bedrijf in initieleWerknemers) {
						WerknemerLijstControl.ItemSource.Add(bedrijf);
					}
				}
			}
		}

		public WerknemersPage() {
			FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;

			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			WerknemersPopup.NieuweWerknemerToegevoegd += (WerknemerDTO werknemer) => WerknemerLijstControl.ItemSource.Add(werknemer);

			LaadWerknemersVanDatabase();
		}

		private void UpdateGeselecteerdBedrijfOpScherm() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			LaadWerknemersVanDatabase();
		}

		private void LaadWerknemersVanDatabase() {
			List<WerknemerDTO> werknemers = ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf).ToList();
			WerknemerLijstControl.ItemSource.Clear();
			foreach (WerknemerDTO werknemer in werknemers) {
				WerknemerLijstControl.ItemSource.Add(werknemer);
			}

			initieleWerknemers = new(WerknemerLijstControl.ItemSource);
		}

		private void AddWerknemer(object sender, MouseButtonEventArgs e) {
			WerknemersPopup.Visibility = Visibility.Visible;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			LaadWerknemersVanDatabase();
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

		private void ZoekTermChanged(object sender, TextChangedEventArgs e) {
			Task.Run(() => Dispatcher.Invoke(() => ZoekText = ZoekTextTextbox.Text));
		}
	}
}
