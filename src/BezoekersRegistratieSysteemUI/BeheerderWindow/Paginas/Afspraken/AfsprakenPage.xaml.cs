﻿using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken {
	public partial class AfsprakenPage : Page, INotifyPropertyChanged {
		#region Variabelen
		private static WerknemerDTO? _geselecteerdeWerknemer;

		private int _geselecteerdeHuidigeAfspraakIndex = -1;
		private int _geselecteerdeWerknemerAfspraakIndex = -1;
		private int _geselecteerdeAfspraakOpDatumIndex = -1;

		private bool _huidigeAfsprakenTabIsGeselecteerd = false;
		private bool _afsprakenWerknemerTabIsGeselecteerd = false;
		private bool _afsprakenOpDatumTabIsGeselecteerd = false;

		private HuidigeAfsprakenLijst afsprakenAfsprakenLijstControl = new();
		private WerknemerAfsprakenLijst werknemersAfsprakenLijstControl = new();
		private OpDatumAfsprakenLijstControl opDatumAfsprakenLijstControl = new();

		public BedrijfDTO GeselecteerdBedrijf => BeheerderWindow.GeselecteerdBedrijf;

		public WerknemerDTO GeselecteerdeWerknemer {
			get => _geselecteerdeWerknemer;
			set {
				if (value == null || _geselecteerdeWerknemer?.Id == value.Id) return;
				_geselecteerdeWerknemer = value;

				UpdateWerknemerAfsprakenOpScherm();
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

		public string? DatePickerOpDatum = null;
		public string? DatePickerWerknemerDatum = null;

		private List<WerknemerDTO> initieleZoekBalkWerknemers = new();
		private string _zoekTextWerknemers;
		public string ZoekTextWerknemers {
			get => _zoekTextWerknemers;
			set {
				if (value.IsNietLeeg()) {
					_zoekTextWerknemers = value.ToLower();

					List<WerknemerDTO> result = initieleZoekBalkWerknemers.Where(w => w.Voornaam.ToLower().Contains(_zoekTextWerknemers) ||
					w.Achternaam.ToLower().Contains(_zoekTextWerknemers) ||
					w.Email.ToLower().Contains(_zoekTextWerknemers) ||
					w.Functie.ToLower().Contains(_zoekTextWerknemers) ||
					w.Status.ToString().ToLower().Contains(_zoekTextWerknemers)).ToList();

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

		#endregion

		public AfsprakenPage() {
			this.DataContext = this;
			InitializeComponent();

			UpdateGeselecteerdBedrijf_Event();
			NavigeerNaarTab("Huidige Afspraken");

			//DatePicker
			DatePicker_Werknemer.DisplayDateEnd = DateTime.Now;
			DatePicker_Werknemer.SelectedDate = DateTime.Now.Date;

			DatePicker_OpDatum.DisplayDateEnd = DateTime.Now;
			DatePicker_OpDatum.SelectedDate = DateTime.Now.Date;

			//Events
			GlobalEvents.RefreshData += AutoUpdateIntervalAfspraken_Event;			
			BedrijfEvents.GeselecteerdBedrijfChanged += UpdateGeselecteerdBedrijf_Event;
			AfspraakEvents.NieuweAfspraakToegevoegd += NieuweAfspraakToegevoegd_Event;
			WerknemerEvents.NieuweWerknemerToegevoegd += NieuweWerknemerToegevoegd_Event;
		}

		#region Functies
		private void DatumFilterTextBox_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
			if (((DatePicker)sender).SelectedDate.HasValue == false) return;
			if (((DatePicker)sender).Equals(DatePicker_Werknemer)) {
				DatePickerWerknemerDatum = ((DatePicker)sender).SelectedDate.Value.ToString("dd/MM/yyy") ?? DateTime.Now.ToString("dd/MM/yyy");
			} else if (((DatePicker)sender).Equals(DatePicker_OpDatum)) {
				DatePickerOpDatum = ((DatePicker)sender).SelectedDate.Value.ToString("dd/MM/yyy") ?? DateTime.Now.ToString("dd/MM/yyy");
			}
			AutoUpdateIntervalAfspraken_Event();
		}
		private void OpenDatePickerMenu_Click(object sender, RoutedEventArgs e) {
			Button button = (Button)sender;
			string tag = button.Tag.ToString();

			if (tag.ToLower() == "opdatum") {
				DatePicker_OpDatum.IsDropDownOpen = true;
			} else if (tag.ToLower() == "werknemer")
				DatePicker_Werknemer.IsDropDownOpen = true;
		}

		private void ResetDatumFilter(object sender, MouseButtonEventArgs e) {
			StackPanel stackPanel = (StackPanel)((Icon)sender).Parent;
			Border border = (Border)stackPanel.Children[0];
			DatePicker datePicker = (DatePicker)border.Child;
			datePicker.SelectedDate = DateTime.Now.Date;
		}

		private void ZoekTermChangedWerknemers(object sender, TextChangedEventArgs e) => Task.Run(() => Dispatcher.Invoke(() => ZoekTextWerknemers = ZoekTermTextBoxWerknemers.Text));
		private void ValideerDatum(object sender, KeyboardFocusChangedEventArgs e) => ControleerInputOpDatum(sender);

		private void AutoUpdateIntervalAfspraken_Event() {
			if (_huidigeAfsprakenTabIsGeselecteerd)
				UpdateHuidigeAfsprakenOpScherm();
			else if (_afsprakenWerknemerTabIsGeselecteerd)
				UpdateWerknemerAfsprakenOpScherm();
			else if (_afsprakenOpDatumTabIsGeselecteerd)
				UpdateOpDatumAfsprakenOpScherm();
		}
		private void UpdateGeselecteerdBedrijf_Event() {
			afsprakenAfsprakenLijstControl.ItemSource.Clear();
			werknemersAfsprakenLijstControl.ItemSource.Clear();
			opDatumAfsprakenLijstControl.ItemSource.Clear();

			WerknemerLijst.ItemSource.Clear();
			OpDatumAfsprakenLijst.ItemSource.Clear();

			ResetFilterSelection();

			afsprakenAfsprakenLijstControl.HeeftData = false;
			werknemersAfsprakenLijstControl.HeeftData = false;

			NavigeerNaarTab("Huidige Afspraken");
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			UpdateHuidigeAfsprakenOpScherm();
		}
		private void NieuweAfspraakToegevoegd_Event(AfspraakDTO afspraak) {
			Task.Run(() => {
				Dispatcher.Invoke(() => {
					if (afspraak.StartTijdDate.Day == DateTime.Parse(Datum).Day) {
						// Als de afspraak nog bezig is voegen we hem toe aan de huidige
						// afspraken lijst
						if (afspraak.EindTijd.IsLeeg()) {
							HuidigeAfsprakenLijst.ItemSource.Add(afspraak);
							List<AfspraakDTO> afspraken = HuidigeAfsprakenLijst.ItemSource.ToList();
							HuidigeAfsprakenLijst.ItemSource.Clear();

							afspraken = afspraken.OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam).ToList();
							afspraken.ForEach(a => HuidigeAfsprakenLijst.ItemSource.Add(a));

							OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
						}
					}
				});
			});
		}
		private void NieuweWerknemerToegevoegd_Event(WerknemerDTO werknemer) {
			WerknemerLijst.ItemSource.Add(werknemer);
			initieleZoekBalkWerknemers.Add(werknemer);
		}

		private void UpdateOpDatumAfsprakenOpScherm() {
			if (OpDatumAfsprakenLijst.SelectedItem is AfspraakDTO) {
				_geselecteerdeAfspraakOpDatumIndex = OpDatumAfsprakenLijst.SelectedIndex;
			}

			if (Datum is not null && GeselecteerdBedrijf is not null) {
				OpDatumAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.GeefAfsprakenOpDatumVanBedrijf(GeselecteerdBedrijf.Id, DatePickerOpDatum).OrderByDescending(a => a.StartTijd)) {
					OpDatumAfsprakenLijst.ItemSource.Add(afspraak);
				}
			}

			if (_geselecteerdeAfspraakOpDatumIndex != -1) {
				OpDatumAfsprakenLijst.SelectedIndex = _geselecteerdeAfspraakOpDatumIndex;
			}
		}
		private void UpdateWerknemerAfsprakenOpScherm() {
			if (_geselecteerdeWerknemer is not null && GeselecteerdBedrijf is not null) {
				if (GeselecteerdeWerknemerAfsprakenLijst.SelectedItem is AfspraakDTO) {
					_geselecteerdeWerknemerAfspraakIndex = GeselecteerdeWerknemerAfsprakenLijst.SelectedIndex;
				}

				GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Clear();
				foreach (AfspraakDTO afspraak in ApiController.GeefWerknemerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeWerknemer, DatePickerWerknemerDatum).OrderByDescending(a => a.StartTijd)) {
					GeselecteerdeWerknemerAfsprakenLijst.ItemSource.Add(afspraak);
				}

				if (_geselecteerdeWerknemerAfspraakIndex != -1) {
					GeselecteerdeWerknemerAfsprakenLijst.SelectedIndex = _geselecteerdeWerknemerAfspraakIndex;
				}
			}
		}
		private void UpdateHuidigeAfsprakenOpScherm() {
			if (HuidigeAfsprakenLijst.SelectedItem is AfspraakDTO) {
				_geselecteerdeHuidigeAfspraakIndex = HuidigeAfsprakenLijst.SelectedIndex;
			}

			HuidigeAfsprakenLijst.ItemSource.Clear();
			foreach (AfspraakDTO afspraak in ApiController.GeefAfsprakenVanBedrijf(GeselecteerdBedrijf.Id).OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam)) {
				if (afspraak.StartTijdDate.Day == DateTime.Now.Day && afspraak.EindTijd.IsLeeg()) {
					HuidigeAfsprakenLijst.ItemSource.Add(afspraak);
				}
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
					UpdateHuidigeAfsprakenOpScherm();
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

				case "Afspraak Op Datum":
					NavigeerNaarTab("Afspraak Op Datum");
					UpdateOpDatumAfsprakenOpScherm();
					break;
			}
		}
		private void OpenAfsprakenPopup_Click(object sender, MouseButtonEventArgs e) {
			afsprakenPopup.StartTijd = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
			afsprakenPopup.Visibility = Visibility.Visible;
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

				case "Afspraak Op Datum":
					_afsprakenOpDatumTabIsGeselecteerd = true;
					FilterContainerHeaders.Children[2].Opacity = 1;
					((Grid)FilterContainer.Children[0]).Children[2].Visibility = Visibility.Visible;
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
			_afsprakenOpDatumTabIsGeselecteerd = false;
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
