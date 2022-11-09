using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken {
	public partial class AfsprakenPage : Page, INotifyPropertyChanged {
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

		private static string _selectedDatum = DateTime.Now.ToString("dd/MM/yyyy");
		private string _oudeValidDate = _selectedDatum;
		private static BezoekerDTO? _geselecteerdeBezoeker;
		private static WerknemerDTO? _geselecteerdeWerknemer;

		private HuidigeAfsprakenLijst afsprakenAfsprakenLijstControl = new();
		private BezoekersAfsprakenLijst bezoekersAfsprakenLijstControl = new();
		private WerknemerAfsprakenLijst werknemersLijstControl = new();
		private OpDatumLijstControl opDatumAfsprakenLijstControl = new();

		public string Datum {
			get {
				return _selectedDatum;
			}
			set {
				_selectedDatum = value;
				UpdatePropperty();
			}
		}

		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }
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

		public AfsprakenPage() {
			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			//Voeg nieuwe werknemer toe aan lijst met werknemers van bedrijf
			WerknemersPopup.NieuweWerknemerToegevoegd += (WerknemerDTO werknemer) => WerknemerLijst.ItemSource.Add(werknemer);
			AfsprakenPopup.NieuweAfspraakToegevoegd += UpdateAfsprakenOpScherm;

			UpdateGeselecteerdBedrijfOpScherm();

			NavigeerNaarTab("Huidige Afspraken");
		}

		private void UpdateAfsprakenOpScherm(AfspraakDTO afspraak) {
			GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Add(afspraak);
			GeselecteerdeBezoekerAfsprakenLijst.ItemSource.Add(afspraak);
		} 

		private void UpdateGeselecteerdBedrijfOpScherm() {
			bezoekersAfsprakenLijstControl.ItemSource.Clear();
			afsprakenAfsprakenLijstControl.ItemSource.Clear();
			opDatumAfsprakenLijstControl.ItemSource.Clear();

			ResetFilterSelection();

			afsprakenAfsprakenLijstControl.HeeftData = false;
			bezoekersAfsprakenLijstControl.HeeftData = false;
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
				if (!werknemersLijstControl.HeeftData) {
					foreach (WerknemerDTO werknemer in ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf)) {
						WerknemerLijst.ItemSource.Add(werknemer);
					}
					werknemersLijstControl.HeeftData = true;
				} else {
				}
				break;

				case "Afspraken Bezoeker":
				NavigeerNaarTab("Afspraken Bezoeker");

				if (!bezoekersAfsprakenLijstControl.HeeftData) {
					BezoekerLijst.ItemSource.Clear();
					foreach (BezoekerDTO bezoeker in ApiController.FetchBezoekersVanBedrijf(GeselecteerdBedrijf.Id, DateTime.Now)) {
						BezoekerLijst.ItemSource.Add(bezoeker);
					}
					bezoekersAfsprakenLijstControl.HeeftData = true;
				}
				break;

				case "Afspraak Op Datum":
				NavigeerNaarTab("Afspraak Op Datum");
				if (!opDatumAfsprakenLijstControl.HeeftData) {
					OpDatumAfsprakenLijst.ItemSource = new(ApiController.FetchAfsprakenOpDatumVanBedrijf(GeselecteerdBedrijf.Id, Datum));
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
	}
}
