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

				GeselecteerdeBezoekerAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.FetchBezoekerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeBezoeker)) {
					GeselecteerdeBezoekerAfsprakenLijst.ItemSource.Add(afspraak);
				}
				UpdatePropperty();
			}
		}

		public WerknemerDTO GeselecteerdeWerknemer {
			get => _geselecteerdeWerknemer;
			set {
				if (value == null || _geselecteerdeWerknemer?.Id == value.Id) return;
				_geselecteerdeWerknemer = value;

				GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.FetchWerknemerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeWerknemer)) {
					GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Add(afspraak);
				}

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

		private List<WerknemerDTO> initieleWerknemers;
		private string _zoekTextWerknemers;
		public string ZoekTextWerknemers {
			get => _zoekTextWerknemers;
			set {
				if (!string.IsNullOrWhiteSpace(value)) {
					_zoekTextWerknemers = value;

					List<WerknemerDTO> result = initieleWerknemers.Where(w => w.Voornaam.Contains(_zoekTextWerknemers) ||
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
					foreach (WerknemerDTO werknemer in initieleWerknemers) {
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
			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;
			AfsprakenPopup.NieuweAfspraakToegevoegd += UpdateAfsprakenOpScherm;

			this.DataContext = this;
			InitializeComponent();

			//Voeg nieuwe werknemer toe aan lijst met werknemers van bedrijf
			WerknemersPopup.NieuweWerknemerToegevoegd += (WerknemerDTO werknemer) => {
				WerknemerLijst.ItemSource.Add(werknemer);
				initieleWerknemers.Add(werknemer);
			};

			UpdateGeselecteerdBedrijfOpScherm();

			NavigeerNaarTab("Huidige Afspraken");
		}

		private void UpdateAfsprakenOpScherm(AfspraakDTO afspraak) {
			// Als de afspraak nog bezig is voegen we hem toe aan de huidige
			// afspraken lijst
			if (string.IsNullOrWhiteSpace(afspraak.EindTijd)) {
				Task.Run(() => {
					Dispatcher.Invoke(() => {
						HuidigeAfsprakenLijst.ItemSource.Add(afspraak);
						List<AfspraakDTO> afspraken = HuidigeAfsprakenLijst.ItemSource.ToList();

						HuidigeAfsprakenLijst.ItemSource.Clear();
						afspraken.OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam).ToList().ForEach(a => HuidigeAfsprakenLijst.ItemSource.Add(a));
					});
				});
			}

			BezoekerLijst.ItemSource.Add(afspraak.Bezoeker);
			initieleBezoekers = BezoekerLijst.ItemSource.ToList();

			OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
		}

		private void UpdateGeselecteerdBedrijfOpScherm() {
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
			opDatumAfsprakenLijstControl.HeeftData = false;

			NavigeerNaarTab("Huidige Afspraken");

			UpdatePropperty(nameof(GeselecteerdBedrijf));

			foreach (AfspraakDTO afspraak in ApiController.FetchAfsprakenVanBedrijf(GeselecteerdBedrijf.Id)) {
				HuidigeAfsprakenLijst.ItemSource.Add(afspraak);
			}
		}

		private void NavigeerNaarTab(object sender, MouseButtonEventArgs e) {
			TextBlock textBlock = (TextBlock)((StackPanel)((Border)sender).Child).Children[1];

			switch (textBlock.Text) {
				case "Huidige Afspraken":
				NavigeerNaarTab("Huidige Afspraken");
				break;

				case "Afspraken Werknemer":
				NavigeerNaarTab("Afspraken Werknemer");
				if (!werknemersAfsprakenLijstControl.HeeftData) {
					foreach (WerknemerDTO werknemer in ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf)) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}
					werknemersAfsprakenLijstControl.HeeftData = true;
					initieleWerknemers = WerknemerLijst.ItemSource.ToList();
				} else {
				}
				break;

				case "Afspraken Bezoeker":
				NavigeerNaarTab("Afspraken Bezoeker");

				if (!bezoekersAfsprakenLijstControl.HeeftData) {
					foreach (BezoekerDTO bezoeker in ApiController.FetchBezoekersVanBedrijf(GeselecteerdBedrijf.Id, DateTime.Now)) {
						BezoekerLijst.ItemSource.Add(bezoeker);
					}
					initieleBezoekers = BezoekerLijst.ItemSource.ToList();
					bezoekersAfsprakenLijstControl.HeeftData = true;
				}
				break;

				case "Afspraak Op Datum":
				NavigeerNaarTab("Afspraak Op Datum");
				if (!opDatumAfsprakenLijstControl.HeeftData) {
					foreach (AfspraakDTO afspraak in ApiController.FetchAfsprakenOpDatumVanBedrijf(GeselecteerdBedrijf.Id, Datum)) {
						OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
					}
					opDatumAfsprakenLijstControl.HeeftData = true;
				}
				break;
			}
		}

		private void NavigeerNaarTab(string tabIndex) {
			ResetFilterSelection();
			switch (tabIndex) {
				case "Huidige Afspraken":
				FilterContainerHeaders.Children[0].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[0].Visibility = Visibility.Visible;
				break;

				case "Afspraken Werknemer":
				FilterContainerHeaders.Children[1].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[1].Visibility = Visibility.Visible;
				break;

				case "Afspraken Bezoeker":
				FilterContainerHeaders.Children[2].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[2].Visibility = Visibility.Visible;
				break;

				case "Afspraak Op Datum":
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
		}

		private void OpenAfsprakenPopup(object sender, MouseButtonEventArgs e) {
			AfsprakenPopup.Visibility = Visibility.Visible;
		}

		private void ResetDatumFilter(object sender, MouseButtonEventArgs e) {
			StackPanel parent = (StackPanel)((Icon)sender).Parent;
			Border border = (Border)parent.Children[0];
			TextBox textBox = (TextBox)border.Child;
			textBox.Text = DateTime.Now.ToString();
			ControleerInputOpDatum(textBox);
		}

		private readonly Regex _regex = new Regex("[^0-9./]+");

		private void IsDatePickerGeldigeText(object sender, TextCompositionEventArgs e) {
			e.Handled = _regex.IsMatch(e.Text);
		}

		private void ZoekTermChangedWerknemers(object sender, TextChangedEventArgs e) {
			Task.Run(() => Dispatcher.Invoke(() => ZoekTextWerknemers = ZoekTermTextBoxWerknemers.Text));
		}

		private void ZoekTermChangedBezoekers(object sender, TextChangedEventArgs e) {
			Task.Run(() => Dispatcher.Invoke(() => ZoekTextBezoekers = ZoekTermTextBoxBezoekers.Text));
		}

		private void ValideerDatum(object sender, KeyboardFocusChangedEventArgs e) {
			ControleerInputOpDatum(sender);
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
