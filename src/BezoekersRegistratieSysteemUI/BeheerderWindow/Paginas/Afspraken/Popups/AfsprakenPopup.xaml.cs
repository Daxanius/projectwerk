using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups {
	public partial class AfsprakenPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;
		public delegate void AfspraakToegevoegdEvent(AfspraakDTO afspraak);
		public static event AfspraakToegevoegdEvent NieuweAfspraakToegevoegd;

		#region Bind Propperties
		private WerknemerDTO _werknemer;
		public WerknemerDTO Werknemer {
			get { return _werknemer; }
			set {
				if (value is null || value == _werknemer) return;
				_werknemer = value;
			}
		}
		private string _bezoekerVoornaam;
		public string BezoekerVoornaam {
			get { return _bezoekerVoornaam; }
			set {
				if (value == _bezoekerVoornaam) return;
				_bezoekerVoornaam = value;
				UpdatePropperty();
			}
		}
		private string _bezoekerAchternaam;
		public string BezoekerAchternaam {
			get { return _bezoekerAchternaam; }
			set {
				if (value == _bezoekerAchternaam) return;
				_bezoekerAchternaam = value;
				UpdatePropperty();
			}
		}
		private string _bezoekerEmail;
		public string BezoekerEmail {
			get { return _bezoekerEmail; }
			set {
				if (value == _bezoekerEmail) return;
				_bezoekerEmail = value;
				UpdatePropperty();
			}
		}
		private string _bezoekerBedrijf;
		public string BezoekerBedrijf {
			get { return _bezoekerBedrijf; }
			set {
				if (value == _bezoekerBedrijf) return;
				_bezoekerBedrijf = value;
				UpdatePropperty();
			}
		}
		private string _startTijd = DateTime.Today.ToString("MM/dd/yyyy");
		public string StartTijd {
			get { return _startTijd; }
			set {
				if (value == _startTijd) return;
				_startTijd = value;
				UpdatePropperty();
			}
		}
		private string? _eindTijd = null;
		public string? EindTijd {
			get { return _eindTijd; }
			set {
				if (value == _eindTijd) return;
				_eindTijd = value;
				UpdatePropperty();
			}
		}
		#endregion

		public AfsprakenPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		#region VoegMedeWerkerToeEiland
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
			if (DateTime.TryParse(textBox.Text.Trim('-'), out DateTime dateTime)) {
				textBox.Text = dateTime.ToString("dd/MM/yyyy - HH:mm");
			}
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			#region Controle Input
			BezoekerVoornaam = BezoekerVoornaam.Trim();
			BezoekerAchternaam = BezoekerAchternaam.Trim();
			BezoekerEmail = BezoekerEmail.Trim();
			BezoekerBedrijf = BezoekerBedrijf.Trim();

			if (BeheerderWindow.GeselecteerdBedrijf is null) {
				MessageBox.Show("Er is geen bedrijf geselecteerd", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrWhiteSpace(BezoekerVoornaam)) {
				MessageBox.Show("Voornaam is niet geldig!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrWhiteSpace(BezoekerAchternaam)) {
				MessageBox.Show("Achternaam is niet geldig!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (string.IsNullOrWhiteSpace(BezoekerEmail)) {
				MessageBox.Show("Email is niet geldig!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			string email = BezoekerEmail.Trim();
			Regex regexEmail = new(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);

			if (!regexEmail.IsMatch(email)) {
				MessageBox.Show("Email is niet geldig!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			WerknemerDTO werknemer = Werknemer;

			if (werknemer is null) {
				MessageBox.Show("Gelieve een werknemer te kiezen", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			if (werknemer.Id is null) {
				MessageBox.Show("Werknemer id is null, gelieve het dashboard te herstarten", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			#endregion

			AfspraakInputDTO payload = new AfspraakInputDTO(new BezoekerInputDTO(BezoekerVoornaam, BezoekerAchternaam, BezoekerEmail, BezoekerBedrijf), null, null, werknemer.Id.Value, BeheerderWindow.GeselecteerdBedrijf.Id);
			AfspraakDTO afspraak = ApiController.PostAfspraak(payload);

			NieuweAfspraakToegevoegd?.Invoke(afspraak);

			SluitOverlay();
		}

		private void OpenMedewerkerKiezenPopup(object sender, MouseButtonEventArgs e) {
			MedeWerkerToevoegenEiland.Visibility = Visibility.Collapsed;
			KiesMedewerkerEiland.Visibility = Visibility.Visible;
			MedewerkersLijstVanBedrijf.ItemsSource = new ObservableCollection<WerknemerDTO>(ApiController.FetchWerknemersVanBedrijf(BeheerderWindow.GeselecteerdBedrijf));
		}

		private readonly Regex regexGeenCijfers = new Regex("[^a-zA-Z]+");
		private void IsInputGeldigZonderCijfers(object sender, TextCompositionEventArgs e) {
			e.Handled = regexGeenCijfers.IsMatch(e.Text);
		}

		private void SluitOverlay() {
			Werknemer = null;

			KiesWerknemerTextBlock.Text = "Kies Werknemer";
			BezoekerVoornaam = string.Empty;
			BezoekerAchternaam = string.Empty;
			BezoekerEmail = string.Empty;
			BezoekerBedrijf = string.Empty;
			StartTijd = string.Empty;
			EindTijd = null;

			AfsprakenPage afsprakenPage = AfsprakenPage.Instance;
			afsprakenPage.AfsprakenPopup.Visibility = Visibility.Hidden;
		}
		#endregion

		#region KiesWerknemerEiland
		private void KiesMedeWerkerClick(object sender, RoutedEventArgs e) {
			object selectedItem = MedewerkersLijstVanBedrijf.SelectedValue;
			if (selectedItem is not WerknemerDTO) return;
			WerknemerDTO werknemer = (WerknemerDTO)selectedItem;

			Werknemer = werknemer;
			KiesWerknemerTextBlock.Text = werknemer.ToString();
			GaTerugNaarVoegMedewerkerEiland(null, null);
		}

		private void GaTerugNaarVoegMedewerkerEiland(object sender, RoutedEventArgs e) {
			MedeWerkerToevoegenEiland.Visibility = Visibility.Visible;
			KiesMedewerkerEiland.Visibility = Visibility.Collapsed;
		}
		#endregion

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
