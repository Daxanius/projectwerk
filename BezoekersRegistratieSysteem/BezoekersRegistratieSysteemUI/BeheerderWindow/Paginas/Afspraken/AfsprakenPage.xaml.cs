using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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
	public partial class AfsprakenPage : Page {
		private static string _selectedDatum = DateTime.Now.ToString("dd/MM/yyyy");
		private string _oudeValidDate = _selectedDatum;

		public string Datum {
			get {
				return _selectedDatum;
			}
			set => _selectedDatum = value;
		}

		public BedrijfDTO GeselecteerdBedrijf { get; set; }
		public ObservableCollection<string> MedewerkersVanGeselecteerdBedrijf { get; set; } = new();

		public AfsprakenPage() {
			GeselecteerdBedrijf = BeheerderWindow.GeselecteerdBedrijf;
			MedewerkersVanGeselecteerdBedrijf.Add("Weude - CEO");
			MedewerkersVanGeselecteerdBedrijf.Add("Balding - CEOO");
			MedewerkersVanGeselecteerdBedrijf.Add("Baldering - CEOOOO");
			MedewerkersVanGeselecteerdBedrijf.Add("Stan - CEOOOOO");
			this.DataContext = this;
			InitializeComponent();
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

			switch (textBlock.Text) {
				case "Huidige Afspraken":
				//Lazy Loading
				afsprakenLijstControl = (AfsprakenLijstControl)HuidigeAfsprakenLijst.DataContext;
				if (!afsprakenLijstControl.HeeftData) {
					afsprakenLijstControl.FetchHuidigeAfspraken();
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[0].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[0].Visibility = Visibility.Visible;
				break;

				case "Afspraken Werknemer":
				//Lazy Loading
				afsprakenLijstControl = (AfsprakenLijstControl)WerknemerAfsprakenLijst.DataContext;
				if (!afsprakenLijstControl.HeeftData) {
					afsprakenLijstControl.FetchWerknemerAfspraken(null);
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[1].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[1].Visibility = Visibility.Visible;
				break;

				case "Afspraken Bezoeker":
				//Lazy Loading
				afsprakenLijstControl = (AfsprakenLijstControl)BezoekerAfsprakenLijst.DataContext;
				if (!afsprakenLijstControl.HeeftData) {
					afsprakenLijstControl.FetchBezoekerAfspraken(null);
				}

				ResetFilterSelection();
				FilterContainerHeaders.Children[2].Opacity = 1;
				((Grid)FilterContainer.Children[0]).Children[2].Visibility = Visibility.Visible;
				break;

				case "Afspraak Op Datum":
				//Lazy Loading
				afsprakenLijstControl = (AfsprakenLijstControl)OpDatumAfsprakenLijst.DataContext;
				if (!afsprakenLijstControl.HeeftData) {
					afsprakenLijstControl.FetchAfsprakenOpDatumData(DateTime.Now);
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
	}
}
