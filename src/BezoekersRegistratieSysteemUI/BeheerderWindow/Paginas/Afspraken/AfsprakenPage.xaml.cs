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
		public ObservableCollection<AfspraakDTO> HuidigeAfsprakenLijstData { get; set; } = new();
		public ObservableCollection<WerknemerDTO> MedewerkersVanGeselecteerdBedrijf { get; set; } = new();
		public ObservableCollection<AfspraakDTO> AfsprakenVanGeselecteerdeMedewerker { get; set; } = new();
		public ObservableCollection<AfspraakDTO> BezoekersLijstData { get; set; } = new();
		public ObservableCollection<AfspraakDTO> OpDatumLijstData { get; set; } = new();

		public AfsprakenPage() {
			AfsprakenVanGeselecteerdeMedewerker = new();

			HuidigeAfsprakenLijstData = AfsprakenVanGeselecteerdeMedewerker;
			BezoekersLijstData = AfsprakenVanGeselecteerdeMedewerker;
			OpDatumLijstData = AfsprakenVanGeselecteerdeMedewerker;

			BeheerderWindow.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijfOpScherm;

			this.DataContext = this;
			InitializeComponent();

			HuidigeAfsprakenLijst.ItemSource = HuidigeAfsprakenLijstData;
			WerknemerLijst.ItemSource = MedewerkersVanGeselecteerdBedrijf;
			WerknemerAfsprakenLijst.ItemSource = AfsprakenVanGeselecteerdeMedewerker;
			BezoekerAfsprakenLijst.ItemSource = BezoekersLijstData;
			OpDatumAfsprakenLijst.ItemSource = OpDatumLijstData;
		}

		private void UpdateGeselecteerdBedrijfOpScherm() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
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
			Border border = (Border)sender;
			StackPanel stackpanel = (StackPanel)border.Child;
			TextBlock textBlock = (TextBlock)stackpanel.Children[1];
			AfsprakenLijstControl afsprakenLijstControl;
			AfsprakenLijstControl werknemersLijstControl;
			BezoekersLijstControl bezoekersLijstControl;
			OpDatumLijstControl opDatumLijstControl;

			switch (textBlock.Text) {
				case "Huidige Afspraken":
				//Lazy Loading
				afsprakenLijstControl = (AfsprakenLijstControl)HuidigeAfsprakenLijst.DataContext;
				if (!afsprakenLijstControl.HeeftData) {
					afsprakenLijstControl.FetchData();
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[0].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[0].Visibility = Visibility.Visible;
				break;

				case "Afspraken Werknemer":
				//Lazy Loading
				afsprakenLijstControl = (AfsprakenLijstControl)WerknemerAfsprakenLijst.DataContext;
				if (!afsprakenLijstControl.HeeftData) {
					afsprakenLijstControl.FetchData();
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[1].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[1].Visibility = Visibility.Visible;
				break;

				case "Afspraken Bezoeker":
				//Lazy Loading
				bezoekersLijstControl = (BezoekersLijstControl)BezoekerAfsprakenLijst.DataContext;
				if (!bezoekersLijstControl.HeeftData) {
					bezoekersLijstControl.FetchData();
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[2].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[2].Visibility = Visibility.Visible;
				break;

				case "Afspraak Op Datum":
				//Lazy Loading
				opDatumLijstControl = (OpDatumLijstControl)OpDatumAfsprakenLijst.DataContext;
				if (!opDatumLijstControl.HeeftData) {
					opDatumLijstControl.FetchData();
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
