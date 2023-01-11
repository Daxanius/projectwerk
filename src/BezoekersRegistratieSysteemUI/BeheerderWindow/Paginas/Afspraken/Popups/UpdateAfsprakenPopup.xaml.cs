using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups {
	public partial class UpdateAfsprakenPopup : UserControl, INotifyPropertyChanged {
		#region Variabelen
		private Border? _selecteditem = null;

		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<WerknemerDTO>),
		  typeof(UpdateAfsprakenPopup),
		  new PropertyMetadata(new ObservableCollection<WerknemerDTO>())
		 );

		public ObservableCollection<WerknemerDTO> ItemSource {
			get { return (ObservableCollection<WerknemerDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		private AfspraakDTO oudeAfspraak;

		private WerknemerDTO? _werknemer;
		public WerknemerDTO? Werknemer {
			get { return _werknemer; }
			set {
				if (value == _werknemer) return;
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
		private string _startTijd = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
		public string StartTijd {
			get => _startTijd;
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

		public UpdateAfsprakenPopup() {
			this.DataContext = this;
			InitializeComponent();

			BedrijfEvents.GeselecteerdBedrijfChanged += UpdateGeselecteerdBedrijf_Event;
		}

		#region Functies
		private void DatePickerInput_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ControleerInputOpDatum(sender);
		private void DatePickerInput_LostKeyboardFocus(object sender, RoutedEventArgs e) => ControleerInputOpDatum(sender);
		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) => SluitOverlay();
		private void IsInputGeldigZonderCijfers(object sender, TextCompositionEventArgs e) => e.Handled = e.Text.Any(char.IsDigit);

		private void ControleerInputOpDatum(object sender) {
			TextBox textBox = sender as TextBox;
			if (DateTime.TryParse(textBox.Text, out DateTime dateTime)) {
				textBox.Text = dateTime.ToString("HH:mm dd/MM/yyyy");
				textBox.BorderBrush = Brushes.Transparent;
				textBox.BorderThickness = new Thickness(0);
			} else {
				if (textBox.Name == "EindTijdTextBox" && EindTijdTextBox.Text.IsLeeg()) {
					textBox.BorderBrush = Brushes.Transparent;
					textBox.BorderThickness = new Thickness(0);
					return;
				}
				textBox.BorderBrush = Brushes.LightSalmon;
				textBox.BorderThickness = new Thickness(1);
			}
		}
		private async void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			#region Controle Input

			if (BeheerderWindow.GeselecteerdBedrijf is null) {
				new CustomMessageBox().Show("Er is geen bedrijf geselecteerd!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (BezoekerVoornaam.IsLeeg()) {
				new CustomMessageBox().Show("Voornaam is niet geldig!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (BezoekerAchternaam.IsLeeg()) {
				new CustomMessageBox().Show("Achternaam is niet geldig!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (BezoekerEmail.IsLeeg()) {
				new CustomMessageBox().Show("Email is niet geldig!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (!BezoekerEmail.IsEmailGeldig()) {
				new CustomMessageBox().Show("Email is niet geldig!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (Werknemer is null) {
				new CustomMessageBox().Show("Gelieve een werknemer te kiezen!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (Werknemer.Id is null) {
				new CustomMessageBox().Show("Werknemer id is null, gelieve het dashboard te herstarten!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (StartTijd.IsLeeg()) {
				new CustomMessageBox().Show("StartTijd is verplicht!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			DateTime startTijdDatum = DateTime.Parse(StartTijd.Trim());
			if (startTijdDatum > DateTime.Now) {
				new CustomMessageBox().Show("StartTijd mag niet in de toekomst liggen!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			DateTime? eindTijdDatum = null;
			if (EindTijd is not null && !DateTime.TryParse(EindTijd.Trim(), out DateTime dateTime) && EindTijd.IsNietLeeg()) {
				new CustomMessageBox().Show("EindTijd is niet geldig!", "Fout", ECustomMessageBoxIcon.Error);
				return;
			} else {
				if (EindTijd is not null && EindTijd.IsNietLeeg())
					eindTijdDatum = DateTime.Parse(EindTijd.Trim());

				DateTime maxEindtijdVandaag = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
				if (eindTijdDatum > maxEindtijdVandaag) {
					new CustomMessageBox().Show($"EindTijd mag niet later dan {maxEindtijdVandaag.ToString("dd/MM/yyyy HH:mm:ss")}!", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (eindTijdDatum == startTijdDatum) {
					new CustomMessageBox().Show($"EindTijd mag niet gelijk zijn aan StartTijd", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (eindTijdDatum < startTijdDatum) {
					new CustomMessageBox().Show($"EindTijd moet later zijn dan de StartTijd!", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}
			}

			#endregion

			var bezoekerInput = new BezoekerInputDTO(BezoekerVoornaam, BezoekerAchternaam, BezoekerEmail, BezoekerBedrijf);
			var afspraakInput = new AfspraakInputDTO(bezoekerInput, startTijdDatum, eindTijdDatum, Werknemer.Id.Value, BeheerderWindow.GeselecteerdBedrijf.Id);

			await ApiController.UpdateAfspraak(afspraakInput, oudeAfspraak.Id, oudeAfspraak.Bezoeker.Id);

			AfspraakDTO afspraak = await ApiController.GeefAfspraak(oudeAfspraak.Id);
			AfspraakEvents.InvokeUpdateAfspraak(afspraak);

			SluitOverlay();

			new CustomMessageBox().Show("Afspraak Is Gewijzigd", $"Success", ECustomMessageBoxIcon.Information);
		}
		private void OpenMedewerkerKiezenPopup(object sender, MouseButtonEventArgs e) {
			MedeWerkerToevoegenEiland.Visibility = Visibility.Collapsed;
			KiesMedewerkerEiland.Visibility = Visibility.Visible;

			MedewerkersLijstVanBedrijf.ItemsSource = new ObservableCollection<WerknemerDTO>(ApiController.GeefWerknemersVanBedrijf(BeheerderWindow.GeselecteerdBedrijf));
		}
		private void SluitOverlay() {
			Werknemer = null;

			KiesWerknemerTextBlock.Text = "Kies Werknemer";

			BezoekerVoornaam = "";
			BezoekerAchternaam = "";
			BezoekerEmail = "";
			BezoekerBedrijf = "";
			StartTijd = "";

			EindTijd = null;

			AfsprakenPage afsprakenPage = AfsprakenPage.Instance;
			afsprakenPage.updateAfsprakenPopup.Visibility = Visibility.Hidden;
		}
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
		private void UpdateGeselecteerdBedrijf_Event() {
			Werknemer = null;
			KiesWerknemerTextBlock.Text = "Kies werknemer";
		}
		public void ZetAfspraak(AfspraakDTO afspraak) {
			oudeAfspraak = afspraak;

			Werknemer = afspraak.Werknemer;
			BezoekerVoornaam = afspraak.Bezoeker.Voornaam;
			BezoekerAchternaam = afspraak.Bezoeker.Achternaam;
			BezoekerEmail = afspraak.Bezoeker.Email;
			BezoekerBedrijf = afspraak.Bezoeker.Bedrijf;
			StartTijd = afspraak.StartTijd;
			EindTijd = afspraak.EindTijd;
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
