using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups {
	public partial class AfsprakenPopup : UserControl, INotifyPropertyChanged {

		#region Event

		public delegate void AfspraakToegevoegdEvent(AfspraakDTO afspraak);
		public static event AfspraakToegevoegdEvent NieuweAfspraakToegevoegd;

		#endregion

		#region Variabelen
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
		private string _bezoekerBedrijf = "";
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
		private void IsDatePickerGeldigeText(object sender, TextCompositionEventArgs e) {
			e.Handled = e.Text.Any(char.IsDigit);
		}

		private void DatePicker_LostKeyboardFocus(object sender, RoutedEventArgs e) => ControleerInputOpDatum(sender);

		private void DatePickerInput_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ControleerInputOpDatum(sender);

		private void ControleerInputOpDatum(object sender) {
			TextBox textBox = sender as TextBox;
			if (DateTime.TryParse(textBox.Text.Trim('-'), out DateTime dateTime)) {
				textBox.Text = dateTime.ToString("dd/MM/yyyy - HH:mm");
			}
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) => SluitOverlay();

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

			ValideerInput.IsLeeg(new Dictionary<string, string?>() { { "Voornaam", BezoekerVoornaam }, { "Achternaam", BezoekerAchternaam }, { "Email", BezoekerEmail } });

			if (!BezoekerEmail.IsEmailGeldig()) {
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

			MessageBox.Show($"Afspraak toegevoegd", "Success");

			NieuweAfspraakToegevoegd?.Invoke(afspraak);

			SluitOverlay();
		}

		private void OpenMedewerkerKiezenPopup(object sender, MouseButtonEventArgs e) {
			MedeWerkerToevoegenEiland.Visibility = Visibility.Collapsed;
			KiesMedewerkerEiland.Visibility = Visibility.Visible;

			MedewerkersLijstVanBedrijf.ItemsSource = new ObservableCollection<WerknemerDTO>(ApiController.FetchWerknemersVanBedrijf(BeheerderWindow.GeselecteerdBedrijf));
		}

		private void IsInputGeldigZonderCijfers(object sender, TextCompositionEventArgs e) {
			e.Handled = e.Text.Any(char.IsDigit);
		}

		private void SluitOverlay() {
			Werknemer = null;

			KiesWerknemerTextBlock.Text = "Kies Werknemer";

			BezoekerVoornaam = BezoekerVoornaam.ZetLeeg();
			BezoekerAchternaam = BezoekerAchternaam.ZetLeeg();
			BezoekerEmail = BezoekerEmail.ZetLeeg();
			BezoekerBedrijf = BezoekerBedrijf.ZetLeeg();
			StartTijd = StartTijd.ZetLeeg();

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

		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
