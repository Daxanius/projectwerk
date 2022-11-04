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
			set => _selectedDatum = value;
		}
		public BedrijfDTO GeselecteerdBedrijf { get; set; }
		public ObservableCollection<AfspraakDTO> HuidigeAfsprakenLijstData { get; set; } = new();
		public ObservableCollection<WerknemerDTO> MedewerkersVanGeselecteerdBedrijf { get; set; } = new();
		public ObservableCollection<AfspraakDTO> AfsprakenVanGeselecteerdeMedewerker { get; set; } = new();
		public ObservableCollection<AfspraakDTO> BezoekersLijstData { get; set; } = new();
		public ObservableCollection<AfspraakDTO> OpDatumLijstData { get; set; } = new();

		public AfsprakenPage() {
			GeselecteerdBedrijf = BeheerderWindow.GeselecteerdBedrijf;

			BedrijfDTO bedrijf = new(1, "Hogent", "Btw", "Telnummer", "Email", "Adres", null);

			MedewerkersVanGeselecteerdBedrijf.Add(new WerknemerDTO(1, "Stan", "Persoons", "Stan.Persoons@student.hogent.be", bedrijf, false));
			MedewerkersVanGeselecteerdBedrijf.Add(new WerknemerDTO(2, "Stan1", "Persoons1", "Stan1.Persoons@student.hogent.be", bedrijf, false));
			MedewerkersVanGeselecteerdBedrijf.Add(new WerknemerDTO(3, "Stan2", "Persoons2", "Stan2.Persoons@student.hogent.be", bedrijf, true));
			MedewerkersVanGeselecteerdBedrijf.Add(new WerknemerDTO(4, "Stan3", "Persoons3", "Stan3.Persoons@student.hogent.be", bedrijf, true));
			MedewerkersVanGeselecteerdBedrijf.Add(new WerknemerDTO(5, "Stan4", "Persoons4", "Stan4.Persoons@student.hogent.be", bedrijf, false));
			MedewerkersVanGeselecteerdBedrijf.Add(new WerknemerDTO(6, "Stan5", "Persoons5", "Stan5.Persoons@student.hogent.be", bedrijf, false));

			AfsprakenVanGeselecteerdeMedewerker = new() { new AfspraakDTO(1, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(1, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now, DateTime.Now.AddHours(8)),
				new AfspraakDTO(4, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(4, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(3)),
				new AfspraakDTO(5, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(5, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(1)),
				new AfspraakDTO(6, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(6, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(2)),
				new AfspraakDTO(7, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(7, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(3)),
				new AfspraakDTO(8, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(8, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(1)),
				new AfspraakDTO(9, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(9, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(10, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(10, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)),
				new AfspraakDTO(11, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(11, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(1), DateTime.Now.AddHours(6)),
				new AfspraakDTO(12, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(12, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(2)),
				new AfspraakDTO(13, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(13, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(3)),
				new AfspraakDTO(14, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(14, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(1)),
				new AfspraakDTO(15, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(15, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, false), DateTime.Now.AddHours(2)),
				new AfspraakDTO(16, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(16, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(3)),
				new AfspraakDTO(17, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(17, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(1)),
				new AfspraakDTO(18, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(18, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(2)),
				new AfspraakDTO(19, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(19, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(3)),
				new AfspraakDTO(20, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(20, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(1)),
				new AfspraakDTO(21, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(21, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(2), DateTime.Now.AddHours(7)),
				new AfspraakDTO(22, new BezoekerDTO("Stan", "Persoons", "stan@gmail.com", "hogent"), "Hogent", new WerknemerDTO(22, "Weude", "VanDirk", "Weude@VanDirk.be", bedrijf, true), DateTime.Now.AddHours(3), DateTime.Now.AddHours(7)) };

			HuidigeAfsprakenLijstData = AfsprakenVanGeselecteerdeMedewerker;
			BezoekersLijstData = AfsprakenVanGeselecteerdeMedewerker;
			OpDatumLijstData = AfsprakenVanGeselecteerdeMedewerker;

			this.DataContext = this;
			InitializeComponent();

			HuidigeAfsprakenLijst.ItemSource = HuidigeAfsprakenLijstData;
			WerknemerLijst.ItemSource = MedewerkersVanGeselecteerdBedrijf;
			WerknemerAfsprakenLijst.ItemSource = AfsprakenVanGeselecteerdeMedewerker;
			BezoekerAfsprakenLijst.ItemSource = BezoekersLijstData;
			OpDatumAfsprakenLijst.ItemSource = OpDatumLijstData;
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

		private void HuidigeAfsprakenLijst_Loaded(object sender, RoutedEventArgs e)
		{

		}
	}
}
