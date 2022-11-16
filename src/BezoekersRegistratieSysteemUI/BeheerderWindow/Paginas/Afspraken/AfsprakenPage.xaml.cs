using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken {
	public partial class AfsprakenPage : Page, INotifyPropertyChanged {
		#region Variabelen
		private static BezoekerDTO? _geselecteerdeBezoeker;
		private static WerknemerDTO? _geselecteerdeWerknemer;

		private int _geselecteerdeHuidigeAfspraakIndex = -1;
		private int _geselecteerdeWerknemerAfspraakIndex = -1;
		private int _geslecteerdeBezoekerIndex = -1;
		private int _geslecteerdeBezoekerAfsprakenIndex = -1;
		private int _geselecteerdeAfspraakOpDatumIndex = -1;

		private bool _huidigeAfsprakenTabIsGeselecteerd = false;
		private bool _afsprakenWerknemerTabIsGeselecteerd = false;
		private bool _afsprakenBezoekerTabIsGeselecteerd = false;
		private bool _afsprakenOpDatumTabIsGeselecteerd = false;

		private HuidigeAfsprakenLijst afsprakenAfsprakenLijstControl = new();
		private BezoekersAfsprakenLijst bezoekersAfsprakenLijstControl = new();
		private WerknemerAfsprakenLijst werknemersAfsprakenLijstControl = new();
		private OpDatumLijstControl opDatumAfsprakenLijstControl = new();

		public BedrijfDTO GeselecteerdBedrijf => BeheerderWindow.GeselecteerdBedrijf;
		public BezoekerDTO GeselecteerdeBezoeker {
			get => _geselecteerdeBezoeker;
			set {
				if (value == null || _geselecteerdeBezoeker?.Id == value.Id) return;
				_geselecteerdeBezoeker = value;

				UpdateBezoekerAfsprakenOpSchermMetNieuweData();
				UpdatePropperty();
			}
		}

		public WerknemerDTO GeselecteerdeWerknemer {
			get => _geselecteerdeWerknemer;
			set {
				if (value == null || _geselecteerdeWerknemer?.Id == value.Id) return;
				_geselecteerdeWerknemer = value;

				UpdateWerknemerAfsprakenOpSchermMetNieuweData();
				UpdatePropperty();
			}
		}

		private static string _selectedDatum = DateTime.Now.ToString("dd/MM/yyyy");
		private string _oudeValidDate = _selectedDatum;
		public string Datum {
			get => _selectedDatum;
			set {
				_selectedDatum = value;
				UpdatePropperty();
			}
		}

		private List<WerknemerDTO> initieleZoekBalkWerknemers = new();
		private string _zoekTextWerknemers;
		public string ZoekTextWerknemers {
			get => _zoekTextWerknemers;
			set {
				if (!string.IsNullOrWhiteSpace(value)) {
					_zoekTextWerknemers = value;

					List<WerknemerDTO> result = initieleZoekBalkWerknemers.Where(w => w.Voornaam.Contains(_zoekTextWerknemers) ||
					w.Achternaam.Contains(_zoekTextWerknemers) ||
					w.Email.Contains(_zoekTextWerknemers) ||
					w.Functie.Contains(_zoekTextWerknemers) ||
					w.Status.ToString().Contains(_zoekTextWerknemers)).ToList();

					WerknemerLijst.ItemSource.Clear();

					foreach (WerknemerDTO werknemer in result) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}

				} else if (value.Length == 0) {
					WerknemerLijst.ItemSource.Clear();
					foreach (WerknemerDTO werknemer in initieleZoekBalkWerknemers) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}
				}
			}
		}

		private List<BezoekerDTO> initieleBezoekers;
		private string _zoekTextBezoekers;
		public string ZoekTextBezoekers {
			get => _zoekTextBezoekers;
			set {
				if (!string.IsNullOrWhiteSpace(value)) {
					_zoekTextBezoekers = value;

					List<BezoekerDTO> result = initieleBezoekers.Where(b => b.Voornaam.Contains(_zoekTextBezoekers) ||
					b.Achternaam.Contains(_zoekTextBezoekers) ||
					b.Email.Contains(_zoekTextBezoekers) ||
					b.Bedrijf.Contains(_zoekTextBezoekers)).ToList();

					BezoekerLijst.ItemSource.Clear();

					foreach (BezoekerDTO bezoeker in result) {
						BezoekerLijst.ItemSource.Add(bezoeker);
					}

				} else if (value.Length == 0) {
					BezoekerLijst.ItemSource.Clear();
					foreach (BezoekerDTO bezoeker in initieleBezoekers) {
						BezoekerLijst.ItemSource.Add(bezoeker);
					}
				}
			}
		}
		#endregion

		public AfsprakenPage() {
			this.DataContext = this;
			InitializeComponent();

			UpdateGeselecteerdBedrijf_Event();
			NavigeerNaarTab("Huidige Afspraken");

			//Events
			App.RefreshTimer.Tick += AutoUpdateIntervalAfspraken_Event;
			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijf_Event;
			AfsprakenPopup.NieuweAfspraakToegevoegd += NieuweAfspraakToegevoegd_Event;
			WerknemersPopup.NieuweWerknemerToegevoegd += (WerknemerDTO werknemer) => {
				WerknemerLijst.ItemSource.Add(werknemer);
				initieleZoekBalkWerknemers.Add(werknemer);
			};
		}

		#region Functies
		private void ZoekTermChangedWerknemers(object sender, TextChangedEventArgs e) => Task.Run(() => Dispatcher.Invoke(() => ZoekTextWerknemers = ZoekTermTextBoxWerknemers.Text));
		private void ZoekTermChangedBezoekers(object sender, TextChangedEventArgs e) => Task.Run(() => Dispatcher.Invoke(() => ZoekTextBezoekers = ZoekTermTextBoxBezoekers.Text));
		private void ValideerDatum(object sender, KeyboardFocusChangedEventArgs e) => ControleerInputOpDatum(sender);

		private void AutoUpdateIntervalAfspraken_Event(object? sender, EventArgs e) {
			if (_huidigeAfsprakenTabIsGeselecteerd)
				UpdateHuidigeAfsprakenOpSchermMetNieuweData();
			else if (_afsprakenWerknemerTabIsGeselecteerd)
				UpdateWerknemerAfsprakenOpSchermMetNieuweData();
			else if (_afsprakenBezoekerTabIsGeselecteerd)
				UpdateBezoekerAfsprakenOpSchermMetNieuweData();
			else if (_afsprakenOpDatumTabIsGeselecteerd)
				UpdateOpDatumAfsprakenOpSchermMetNieuweData();
		}
		private void NieuweAfspraakToegevoegd_Event(AfspraakDTO afspraak) {
			Task.Run(() => {
				Dispatcher.Invoke(() => {
					// Als de afspraak nog bezig is voegen we hem toe aan de huidige
					// afspraken lijst
					if (string.IsNullOrWhiteSpace(afspraak.EindTijd)) {
						HuidigeAfsprakenLijst.ItemSource.Add(afspraak);
						List<AfspraakDTO> afspraken = HuidigeAfsprakenLijst.ItemSource.ToList();
						HuidigeAfsprakenLijst.ItemSource.Clear();

						afspraken = afspraken.OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam).ToList();
						afspraken.ForEach(a => HuidigeAfsprakenLijst.ItemSource.Add(a));
					}

					BezoekerLijst.ItemSource.Add(afspraak.Bezoeker);
					initieleBezoekers = BezoekerLijst.ItemSource.ToList();

					OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
				});
			});
		}
		private void UpdateGeselecteerdBedrijf_Event() {
			bezoekersAfsprakenLijstControl.ItemSource.Clear();
			afsprakenAfsprakenLijstControl.ItemSource.Clear();
			werknemersAfsprakenLijstControl.ItemSource.Clear();
			opDatumAfsprakenLijstControl.ItemSource.Clear();

			BezoekerLijst.ItemSource.Clear();
			WerknemerLijst.ItemSource.Clear();
			OpDatumAfsprakenLijst.ItemSource.Clear();

			ResetFilterSelection();

			afsprakenAfsprakenLijstControl.HeeftData = false;
			bezoekersAfsprakenLijstControl.HeeftData = false;
			werknemersAfsprakenLijstControl.HeeftData = false;

			NavigeerNaarTab("Huidige Afspraken");
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			UpdateHuidigeAfsprakenOpSchermMetNieuweData();
		}

		private void UpdateOpDatumAfsprakenOpSchermMetNieuweData() {
			if (OpDatumAfsprakenLijst.SelectedItem is AfspraakDTO) {
				_geselecteerdeAfspraakOpDatumIndex = OpDatumAfsprakenLijst.SelectedIndex;
			}

			if (Datum is not null && GeselecteerdBedrijf is not null) {
				OpDatumAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.GeefAfsprakenOpDatumVanBedrijf(GeselecteerdBedrijf.Id, Datum).OrderByDescending(a => a.StartTijd)) {
					OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
				}
			}

			if (_geselecteerdeAfspraakOpDatumIndex != -1) {
				OpDatumAfsprakenLijst.SelectedIndex = _geselecteerdeAfspraakOpDatumIndex;
			}
		}
		private void UpdateBezoekerAfsprakenOpSchermMetNieuweData() {
			if (GeselecteerdBedrijf is not null) {
				if (BezoekerLijst.SelectedItem is BezoekerDTO) {
					_geslecteerdeBezoekerIndex = BezoekerLijst.SelectedIndex;
				}

				if (GeselecteerdeBezoeker is not null) {
					if (GeselecteerdeBezoekerAfsprakenLijst.SelectedItem is AfspraakDTO) {
						_geslecteerdeBezoekerAfsprakenIndex = GeselecteerdeBezoekerAfsprakenLijst.SelectedIndex;
					}

					GeselecteerdeBezoekerAfsprakenLijst.ItemSource.Clear();
					foreach (AfspraakDTO afspraak in ApiController.GeefBezoekerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeBezoeker).OrderByDescending(a => a.StartTijd)) {
						GeselecteerdeBezoekerAfsprakenLijst.ItemSource.Add(afspraak);
					}

					if (_geslecteerdeBezoekerAfsprakenIndex != -1) {
						GeselecteerdeBezoekerAfsprakenLijst.SelectedIndex = _geslecteerdeBezoekerAfsprakenIndex;
					}
				} else {
					GeselecteerdeBezoekerAfsprakenLijst.ItemSource.Clear();
				}

				BezoekerLijst.ItemSource.Clear();
				foreach (BezoekerDTO bezoeker in ApiController.GeefBezoekersVanBedrijf(GeselecteerdBedrijf.Id, DateTime.Now).OrderByDescending(b => b.Voornaam)) {
					BezoekerLijst.ItemSource.Add(bezoeker);
				}

				if (_geslecteerdeBezoekerIndex != -1) {
					BezoekerLijst.SelectedIndex = _geslecteerdeBezoekerIndex;
				}
			}
		}
		private void UpdateWerknemerAfsprakenOpSchermMetNieuweData() {
			if (_geselecteerdeWerknemer is not null && GeselecteerdBedrijf is not null) {
				if (GeselecteerdeWerknemerAfsprakenLijst.SelectedItem is AfspraakDTO) {
					_geselecteerdeWerknemerAfspraakIndex = GeselecteerdeWerknemerAfsprakenLijst.SelectedIndex;
				}

				GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.GeefWerknemerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeWerknemer).OrderByDescending(a => a.StartTijd)) {
					GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Add(afspraak);
				}

				if (_geselecteerdeWerknemerAfspraakIndex != -1) {
					GeselecteerdeWerknemerAfsprakenLijst.SelectedIndex = _geselecteerdeWerknemerAfspraakIndex;
				}
			}
		}
		private void UpdateHuidigeAfsprakenOpSchermMetNieuweData() {
			if (HuidigeAfsprakenLijst.SelectedItem is AfspraakDTO) {
				_geselecteerdeHuidigeAfspraakIndex = HuidigeAfsprakenLijst.SelectedIndex;
			}

			HuidigeAfsprakenLijst.ItemSource.Clear();
			foreach (AfspraakDTO afspraak in ApiController.GeefAfsprakenVanBedrijf(GeselecteerdBedrijf.Id).OrderByDescending(a => a.StartTijd)) {
				HuidigeAfsprakenLijst.ItemSource.Add(afspraak);
			}

			if (_geselecteerdeHuidigeAfspraakIndex != -1) {
				HuidigeAfsprakenLijst.SelectedIndex = _geselecteerdeHuidigeAfspraakIndex;
			}
		}
		
		private void Navigeer_Click(object sender, MouseButtonEventArgs e) {
			TextBlock textBlock = (TextBlock)((StackPanel)((Border)sender).Child).Children[1];

			switch (textBlock.Text) {
				case "Huidige Afspraken":
				NavigeerNaarTab("Huidige Afspraken");
				UpdateHuidigeAfsprakenOpSchermMetNieuweData();
				break;

				case "Afspraken Werknemer":
				NavigeerNaarTab("Afspraken Werknemer");
				if (!werknemersAfsprakenLijstControl.HeeftData) {
					foreach (WerknemerDTO werknemer in ApiController.GeefWerknemersVanBedrijf(GeselecteerdBedrijf).OrderByDescending(a => a.Voornaam)) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}
					werknemersAfsprakenLijstControl.HeeftData = true;
					initieleZoekBalkWerknemers = WerknemerLijst.ItemSource.ToList();
				}
				break;

				case "Afspraken Bezoeker":
				NavigeerNaarTab("Afspraken Bezoeker");
				if (!bezoekersAfsprakenLijstControl.HeeftData) {
					var bezoekers = ApiController.GeefBezoekersVanBedrijf(GeselecteerdBedrijf.Id, DateTime.Now).OrderByDescending(b => b.Voornaam);
					foreach (BezoekerDTO bezoeker in bezoekers) {
						BezoekerLijst.ItemSource.Add(bezoeker);
					}
					initieleBezoekers = BezoekerLijst.ItemSource.ToList();
					bezoekersAfsprakenLijstControl.HeeftData = true;
				}
				break;

				case "Afspraak Op Datum":
				NavigeerNaarTab("Afspraak Op Datum");
				UpdateOpDatumAfsprakenOpSchermMetNieuweData();
				break;
			}
		}
		private void OpenAfsprakenPopup_Click(object sender, MouseButtonEventArgs e) {
			AfsprakenPopup.StartTijd = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
			AfsprakenPopup.Visibility = Visibility.Visible;
		}
		
		private void NavigeerNaarTab(string tabIndex) {
			ResetFilterSelection();
			switch (tabIndex) {
				case "Huidige Afspraken":
				_huidigeAfsprakenTabIsGeselecteerd = true;
				FilterContainerHeaders.Children[0].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[0].Visibility = Visibility.Visible;
				break;

				case "Afspraken Werknemer":
				_afsprakenWerknemerTabIsGeselecteerd = true;
				FilterContainerHeaders.Children[1].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[1].Visibility = Visibility.Visible;
				break;

				case "Afspraken Bezoeker":
				_afsprakenBezoekerTabIsGeselecteerd = true;
				FilterContainerHeaders.Children[2].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[2].Visibility = Visibility.Visible;
				break;

				case "Afspraak Op Datum":
				_afsprakenOpDatumTabIsGeselecteerd = true;
				FilterContainerHeaders.Children[3].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[3].Visibility = Visibility.Visible;
				break;
			}
		}
		private void ResetFilterSelection() {
			for (int i = 0; i < FilterContainerHeaders.Children.Count; i++) {
				FilterContainerHeaders.Children[i].Opacity = .6;
				((Grid)FilterContainer.Children[0]).Children[i].Visibility = Visibility.Collapsed;
			}

			_huidigeAfsprakenTabIsGeselecteerd = false;
			_afsprakenWerknemerTabIsGeselecteerd = false;
			_afsprakenBezoekerTabIsGeselecteerd = false;
			_afsprakenOpDatumTabIsGeselecteerd = false;
		}
		private void ResetDatumFilter(object sender, MouseButtonEventArgs e) {
			StackPanel parent = (StackPanel)((Icon)sender).Parent;
			Border border = (Border)parent.Children[0];
			TextBox textBox = (TextBox)border.Child;
			textBox.Text = DateTime.Now.ToString();
			ControleerInputOpDatum(textBox);
		}
		private void IsDatePickerGeldigeText(object sender, TextCompositionEventArgs e) {
			Regex _regex = new Regex("[^0-9./]+");
			e.Handled = _regex.IsMatch(e.Text);
		}
		private void ControleerInputOpDatum(object sender) {
			TextBox textBox = sender as TextBox;
			if (DateTime.TryParse(textBox.Text, out DateTime dateTime)) {
				textBox.Text = dateTime.ToString("dd/MM/yyyy");
				_oudeValidDate = textBox.Text;
			} else {
				textBox.Text = _oudeValidDate;
			}
		}
		#endregion

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged

		#region Singleton
		private static AfsprakenPage instance = null;
		private static readonly object padlock = new object();

		public static AfsprakenPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new AfsprakenPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
