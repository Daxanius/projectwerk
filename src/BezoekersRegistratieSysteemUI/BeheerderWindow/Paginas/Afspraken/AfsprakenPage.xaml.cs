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
		private static BezoekerDTO _geselecteerdebezoeker;

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
		public BezoekerDTO Geselecteerdebezoeker {
			get => _geselecteerdebezoeker; set {
				if (_geselecteerdebezoeker?.Id == value.Id) return;
				_geselecteerdebezoeker = value;
				BezoekerAfsprakenLijst.ItemSource = new(ApiController.FetchBezoekerAfsprakenVanBedrijf(GeselecteerdBedrijf.Id, Geselecteerdebezoeker));
				UpdatePropperty();
			}
		}
		public ObservableCollection<AfspraakDTO> Afspraken { get; set; } = new ObservableCollection<AfspraakDTO>();

		public AfsprakenPage() {
			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			HuidigeAfsprakenLijst.ItemSource = Afspraken;
			Afspraken.Clear();
			ApiController.FetchAfsprakenVanBedrijf(GeselecteerdBedrijf.Id);
		}

		private void UpdateGeselecteerdBedrijfOpScherm() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
			Afspraken.Clear();
			ApiController.FetchAfsprakenVanBedrijf(GeselecteerdBedrijf.Id);
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

		private void SelecteerFilterOpties(object sender, MouseButtonEventArgs e) {
			TextBlock textBlock = (TextBlock)((StackPanel)((Border)sender).Child).Children[1];
			HuidigeAfsprakenLijst afsprakenAfsprakenLijstControl;
			HuidigeAfsprakenLijst werknemersAfsprakenLijstControl;
			HuidigeAfsprakenLijst bezoekersAfsprakenLijstControl;
			OpDatumLijstControl opDatumAfsprakenLijstControl;

			switch (textBlock.Text) {
				case "Huidige Afspraken":
				//Lazy Loading
				ResetFilterSelection();
				FilterContainerHeaders.Children[0].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[0].Visibility = Visibility.Visible;
				break;

				case "Afspraken Werknemer":
				//Lazy Loading
				afsprakenAfsprakenLijstControl = (HuidigeAfsprakenLijst)WerknemerAfsprakenLijst.DataContext;
				if (!afsprakenAfsprakenLijstControl.HeeftData) {
					WerknemerLijst.ItemSource = new(ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf));
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[1].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[1].Visibility = Visibility.Visible;
				break;

				case "Afspraken Bezoeker":
				//Lazy Loading
				bezoekersAfsprakenLijstControl = (HuidigeAfsprakenLijst)BezoekerAfsprakenLijst.DataContext;
				if (!bezoekersAfsprakenLijstControl.HeeftData) {
					BezoekerLijst.ItemSource = new(ApiController.FetchBezoekersVanBedrijf(GeselecteerdBedrijf.Id, DateTime.Now));
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[2].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[2].Visibility = Visibility.Visible;
				break;

				case "Afspraak Op Datum":
				//Lazy Loading
				opDatumAfsprakenLijstControl = (OpDatumLijstControl)OpDatumAfsprakenLijst.DataContext;
				if (!opDatumAfsprakenLijstControl.HeeftData) {
					//opDatumLijstControl.FetchData();
				}
				//Lazy Loading
				ResetFilterSelection();
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

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}
