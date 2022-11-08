using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden {
	/// <summary>
	/// Interaction logic for AanmeldGegevensPage.xaml
	/// </summary>
	public partial class AanmeldGegevensPage : Page, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;

		#region Binding Propperties

		private string _voornaam;
		public string Voornaam {
			get { return _voornaam; }
			set {
				if (value == _voornaam) return;
				_voornaam = value.Trim();
				UpdatePropperty();
			}
		}

		private string _achternaam;
		public string Achternaam {
			get { return _achternaam; }
			set {
				if (value == _achternaam) return;
				_achternaam = value.Trim();
				UpdatePropperty();
			}
		}

		private string _email;
		public string Email {
			get { return _email; }

			set {
				if (value == _email) return;
				_email = value.Trim();
				UpdatePropperty();
			}
		}

		private string _bedrijf;
		public string Bedrijf {
			get { return _bedrijf; }
			set {
				if (value == _bedrijf) return;
				_bedrijf = value.Trim();
				UpdatePropperty();
			}
		}

		private BedrijfDTO _geselecteerdBedrijf;
		public BedrijfDTO GeselecteerdBedrijf {
			get { return _geselecteerdBedrijf; }
			set {
				if (value.Naam == _geselecteerdBedrijf?.Naam) return;
				_geselecteerdBedrijf = value;
				UpdatePropperty();
			}
		}

		private List<WerknemerDTO> _lijstMetWerknemersVanGeselecteerdBedrijf;
		public List<WerknemerDTO> LijstMetWerknemersVanGeselecteerdBedrijf {
			get { return _lijstMetWerknemersVanGeselecteerdBedrijf; }
			set {
				_lijstMetWerknemersVanGeselecteerdBedrijf = value;
				UpdatePropperty();
			}
		}

		#endregion

		public AanmeldGegevensPage() {
			this.DataContext = this;
			InitializeComponent();

			GeselecteerdBedrijf = RegistratieWindow.GeselecteerdBedrijf;

			if (GeselecteerdBedrijf is null) {
				MessageBox.Show("Bedrijf is niet gekozen", "Error");
				((RegistratieWindow)Window.GetWindow(this)).FrameControl.Content = KiesBedrijfPage.Instance;
				return;
			}
			LijstMetWerknemersVanGeselecteerdBedrijf = ApiController.FetchWerknemersVanBedrijf(GeselecteerdBedrijf).ToList();
		}

		#region Action Buttons

		private void AnnulerenKlik(object sender, RoutedEventArgs e) {
			GaTerugNaarKiesBedrijf();
		}

		private void AanmeldenKlik(object sender, RoutedEventArgs e) {
			try {
				#region Controle Input

				if (string.IsNullOrWhiteSpace(Voornaam)) {
					MessageBox.Show("Voornaam is niet geldig!", "Error");
					return;
				}

				if (string.IsNullOrWhiteSpace(Achternaam)) {
					MessageBox.Show("Achternaam is niet geldig!", "Error");
					return;
				}

				if (string.IsNullOrWhiteSpace(Email)) {
					MessageBox.Show("Email is niet geldig!", "Error");
					return;
				}

				string email = Email.Trim();
				Regex regexEmail = new(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);

				if (!regexEmail.IsMatch(email)) {
					MessageBox.Show("Email is niet geldig!", "Error");
					return;
				}

				if (string.IsNullOrWhiteSpace(Bedrijf)) {
					MessageBox.Show("Bedrijf is niet geldig!", "Error");
					return;
				}

				#endregion

				WerknemerDTO? werknemer = (WerknemerDTO)WerknemersLijst.SelectedValue;

				if (werknemer == null) {
					MessageBox.Show("Gelieve een werknemer te kiezen", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				BezoekerDTO bezoeker = new(Voornaam.Trim(), Achternaam.Trim(), Email.Trim(), Bedrijf.Trim());

				if (werknemer.Id.HasValue) {
					MaakNieuweAfspraak(GeselecteerdBedrijf.Id, werknemer.Id.Value, bezoeker);
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			GaTerugNaarKiesBedrijf();
		}

		private void GaTerugNaarKiesBedrijf() {
			Voornaam = string.Empty;
			Achternaam = string.Empty;
			Email = string.Empty;
			Bedrijf = string.Empty;

			RegistratieWindow registratieWindow = (RegistratieWindow)Window.GetWindow(this);
			registratieWindow.FrameControl.Content = KiesBedrijfPage.Instance;
		}

		private async void MaakNieuweAfspraak(long bedrijfsId, long werknemerId, BezoekerDTO bezoeker) {
			var rawBody = new { werknemerId = werknemerId, bedrijfId = bedrijfsId, bezoeker };
			string json = JsonConvert.SerializeObject(rawBody);

			(bool isvalid, AfspraakOutputDTO afspraak) = await ApiController.Post<AfspraakOutputDTO>("/afspraak", json);

			if (isvalid) {
				MessageBox.Show($"U registratie is goed ontvangen met als startijd: {afspraak.Starttijd.ToString("HH:mm - dd/MM/yyyy")}");
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het registreren in het systeem", "Error /");
			}
		}

		private Border _selecteditem;
		private void KlikOpRow(object sender, MouseButtonEventArgs e) {
			//Er is 2 keer geklikt
			//if (e.ClickCount == 2) {
			//	return;
			//}

			if (_selecteditem is not null) {
				_selecteditem.Background = Brushes.Transparent;
				_selecteditem.BorderThickness = new Thickness(0);
			}
			StackPanel? listViewItem = sender as StackPanel;

			SolidColorBrush hightlightColor = (SolidColorBrush)Application.Current.Resources["LichtGrijsAccent"];

			Border border = (Border)listViewItem.Children[0];
			border.Background = hightlightColor;
			border.BorderThickness = new Thickness(0);
			border.BorderBrush = Brushes.Black;
			border.CornerRadius = new CornerRadius(20);
			border.Margin = new Thickness(0, 0, 20, 0);
			_selecteditem = border;
		}

		#endregion

		#region ProppertyChanged
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}