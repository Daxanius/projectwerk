using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using BezoekersRegistratieSysteemUI.Events;
using System.Collections.ObjectModel;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers {
	public partial class WerknemersPage : Page, INotifyPropertyChanged {
		#region Variabelen
		public int FullWidth { get; set; }
		public int FullHeight { get; set; }
		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }

		private ObservableCollection<WerknemerDTO> _werknemerLijst;
		private List<WerknemerDTO> _initieleZoekTermWerknemers;
		private List<WerknemerDTO> _huidigeFilterAfspraken;
		private string _zoekText;
		public string ZoekText {
			get => _zoekText;
			set {
				if (value.IsNietLeeg()) {
					_zoekText = value.ToLower();

					List<WerknemerDTO> result = _initieleZoekTermWerknemers.Where(w => w.Voornaam.ToLower().Contains(_zoekText) ||
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
					foreach (WerknemerDTO bedrijf in _initieleZoekTermWerknemers) {
						WerknemerLijstControl.ItemSource.Add(bedrijf);
					}
				}
			}
		}
		#endregion

		public WerknemersPage() {
			FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;

			this.DataContext = this;
			InitializeComponent();

			WerknemerLijstControl.ItemSource ??= new ObservableCollection<WerknemerDTO>();
			_werknemerLijst = WerknemerLijstControl.ItemSource;

			BedrijfEvents.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijf_Event;
			WerknemerEvents.NieuweWerknemerToegevoegd += (WerknemerDTO werknemer) => _werknemerLijst.Add(werknemer);

			UpdateWerknemersMetNieuweDataOpScherm();
		}

		private void UpdateGeselecteerdBedrijf_Event() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			UpdateWerknemersMetNieuweDataOpScherm();
		}

		private void UpdateWerknemersMetNieuweDataOpScherm() {
			List<WerknemerDTO> werknemers = ApiController.GeefWerknemersVanBedrijf(GeselecteerdBedrijf).ToList();
			_werknemerLijst.Clear();
			foreach (WerknemerDTO werknemer in werknemers) {
				_werknemerLijst.Add(werknemer);
			}

			_initieleZoekTermWerknemers = new(_werknemerLijst);
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
			_huidigeFilterAfspraken ??= _werknemerLijst.ToList();

			ComboBox combobox = (sender as ComboBox);
			if (combobox.SelectedValue is null) return;

			string selected = ((ComboBoxItem)combobox.SelectedValue).Content.ToString();

			List<WerknemerDTO> filtered = _huidigeFilterAfspraken;
			if (combobox.SelectedIndex != 0) {
				if (selected == "Vrij")
					filtered = _huidigeFilterAfspraken.Where(x => x.Status == "Vrij").ToList();
				else
					filtered = _huidigeFilterAfspraken.Where(x => x.Status == "Bezet").ToList();
			}

			_werknemerLijst.Clear();
			foreach (WerknemerDTO afspraak in filtered) {
				_werknemerLijst.Add(afspraak);
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
