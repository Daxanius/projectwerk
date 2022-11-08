using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken {
	/// <summary>
	/// Interaction logic for DashBoardPage.xaml
	/// </summary>
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

		/// <summary>
		/// ////////////////////////////////////////////////////////////////////////////////
		/// </summary>


		private static string _selectedDatum = DateTime.Now.ToString("dd/MM/yyyy");
		private string _oudeValidDate = _selectedDatum;
		private static BezoekerDTO? _geselecteerdeBezoeker;
		private static WerknemerDTO? _geselecteerdeWerknemer;
		private HuidigeAfsprakenLijst afsprakenAfsprakenLijstControl;
		private HuidigeAfsprakenLijst bezoekersAfsprakenLijstControl;
		private OpDatumLijstControl opDatumAfsprakenLijstControl;

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
				BezoekerAfsprakenLijst.ItemSource = new(ApiController.FetchBezoekerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeBezoeker));
				UpdatePropperty();
			}
		}
		public WerknemerDTO GeselecteerdeWerknemer {
			get => _geselecteerdeWerknemer;
			set {
				if (value == null || _geselecteerdeWerknemer?.Id == value.Id) return;
				_geselecteerdeWerknemer = value;
				WerknemerAfsprakenLijst.ItemSource = new(ApiController.FetchWerknemerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, GeselecteerdeWerknemer));
				UpdatePropperty();
			}
		}

		public AfsprakenPage() {
			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			afsprakenAfsprakenLijstControl = (HuidigeAfsprakenLijst)WerknemerAfsprakenLijst.DataContext;
			bezoekersAfsprakenLijstControl = (HuidigeAfsprakenLijst)BezoekerAfsprakenLijst.DataContext;
			opDatumAfsprakenLijstControl = (OpDatumLijstControl)OpDatumAfsprakenLijst.DataContext;

			UpdateGeselecteerdBedrijfOpScherm();

			NavigeerNaarTab("Huidige Afspraken");
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
				if (!afsprakenAfsprakenLijstControl.HeeftData) {
					WerknemerLijst.ItemSource = new(ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf));
					afsprakenAfsprakenLijstControl.HeeftData = true;
				}

				break;

				case "Afspraken Bezoeker":
				NavigeerNaarTab("Afspraken Bezoeker");
				if (!bezoekersAfsprakenLijstControl.HeeftData) {
					ObservableCollection<BezoekerDTO> afspraken = new(ApiController.FetchBezoekersVanBedrijf(GeselecteerdBedrijf.Id, DateTime.Now));
					BezoekerLijst.ItemSource = afspraken;
					bezoekersAfsprakenLijstControl.HeeftData = true;
				}
				break;

				case "Afspraak Op Datum":
				NavigeerNaarTab("Afspraak Op Datum");
				if (!opDatumAfsprakenLijstControl.HeeftData) {
					//OpDatumAfsprakenLijst.ItemSource =
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

		private void DatePicker_LostKeyboardFocus(object sender, RoutedEventArgs e) {
			ControleerInputOpDatum(sender);
		}

		private void DatePickerInput_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
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
