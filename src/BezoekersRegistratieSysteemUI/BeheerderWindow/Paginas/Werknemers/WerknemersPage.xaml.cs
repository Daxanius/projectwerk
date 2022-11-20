using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
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
		#region Variabelen
		public int FullWidth { get; set; }
		public int FullHeight { get; set; }
		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }

		private List<WerknemerDTO> initieleZoekTermWerknemers;
		private List<WerknemerDTO> huidigeFilterAfspraken;
		private string _zoekText;
		public string ZoekText {
			get => _zoekText;
			set {
				if (!string.IsNullOrWhiteSpace(value)) {
					_zoekText = value.ToLower();

					List<WerknemerDTO> result = initieleZoekTermWerknemers.Where(w => w.Voornaam.ToLower().Contains(_zoekText) ||
					w.Achternaam.ToLower().Contains(_zoekText) ||
					w.Email.ToLower().Contains(_zoekText) ||
					w.Functie.ToLower().Contains(_zoekText) ||
					w.Status.ToString().ToLower().Contains(_zoekText)).ToList();

					WerknemerLijstControl.ItemSource.Clear();

					foreach (WerknemerDTO bedrijf in result) {
						WerknemerLijstControl.ItemSource.Add(bedrijf);
					}

				} else if (value.Length == 0) {
					WerknemerLijstControl.ItemSource.Clear();
					foreach (WerknemerDTO bedrijf in initieleZoekTermWerknemers) {
						WerknemerLijstControl.ItemSource.Add(bedrijf);
					}
				}
			}
		}
		#endregion

		public WerknemersPage() {
			FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;

			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			WerknemersPopup.NieuweWerknemerToegevoegd += (WerknemerDTO werknemer) => WerknemerLijstControl.ItemSource.Add(werknemer);

			UpdateWerknemersMetNieuweDataOpScherm();
		}

		private void UpdateGeselecteerdBedrijfOpScherm() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			UpdateWerknemersMetNieuweDataOpScherm();
		}

		private void UpdateWerknemersMetNieuweDataOpScherm() {
			List<WerknemerDTO> werknemers = ApiController.GeefWerknemersVanBedrijf(GeselecteerdBedrijf).ToList();
			WerknemerLijstControl.ItemSource.Clear();
			foreach (WerknemerDTO werknemer in werknemers) {
				WerknemerLijstControl.ItemSource.Add(werknemer);
			}

			initieleZoekTermWerknemers = new(WerknemerLijstControl.ItemSource);
		}
		private void ZoekTermChanged(object sender, TextChangedEventArgs e) {
			Task.Run(() => Dispatcher.Invoke(() => ZoekText = ZoekTextTextbox.Text));
		}

		private void AddWerknemer(object sender, MouseButtonEventArgs e) {
			WerknemersPopup.Visibility = Visibility.Visible;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			UpdateWerknemersMetNieuweDataOpScherm();
			FilterComboBox.SelectedIndex = 0;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (WerknemerLijstControl is null) return;
			if (huidigeFilterAfspraken is null) huidigeFilterAfspraken = WerknemerLijstControl.ItemSource.ToList();

			ComboBox combobox = (sender as ComboBox);
			if (combobox.SelectedValue is null) return;

			string selected = ((ComboBoxItem)combobox.SelectedValue).Content.ToString();

			List<WerknemerDTO> filtered = huidigeFilterAfspraken;
			if (combobox.SelectedIndex != 0) {
				if (selected == "Vrij")
					filtered = huidigeFilterAfspraken.Where(x => x.Status == "Vrij").ToList();
				else
					filtered = huidigeFilterAfspraken.Where(x => x.Status == "Bezet").ToList();
			}

			WerknemerLijstControl.ItemSource.Clear();
			foreach (WerknemerDTO afspraak in filtered) {
				WerknemerLijstControl.ItemSource.Add(afspraak);
			}
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
