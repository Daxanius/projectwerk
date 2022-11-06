using BezoekersRegistratieSysteemREST.Model.Output;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.DTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

		private List<WerknemerDTO> _lijstMetWerknemersVanGeselecteerdBedrijf;
		public List<WerknemerDTO> LijstMetWerknemersVanGeselecteerdBedrijf {
			get { return _lijstMetWerknemersVanGeselecteerdBedrijf; }
			set {
				_lijstMetWerknemersVanGeselecteerdBedrijf = value;
				UpdatePropperty();
			}
		}

		#endregion

		private long? _geselecteerdBedrijfsId;

		public AanmeldGegevensPage() {
			this.DataContext = this;
			InitializeComponent();

			_geselecteerdBedrijfsId = RegistratieWindow.GeselecteerdBedrijf.Id;

			if (!_geselecteerdBedrijfsId.HasValue) {
				MessageBox.Show("Bedrijf is niet gekozen", "Error");
				((RegistratieWindow)Window.GetWindow(this)).FrameControl.Navigate(KiesBedrijfPage.Instance);
				return;
			}
			FetchWerknemersVoorBedrijf();
		}

		private async void FetchWerknemersVoorBedrijf() {
			(bool isValid, List<ApiWerknemerIn> werknemersVanGeselecteerdBedrijf) = await ApiController.Get<List<ApiWerknemerIn>>($"werknemer/bedrijf/id/{_geselecteerdBedrijfsId.Value}");
			if (isValid) {
				List<WerknemerDTO> werknemersDTOVanBedrijf = new();
				werknemersVanGeselecteerdBedrijf.ForEach(w => {
					WerknemerDTO werknemer = new(w.Id, w.Voornaam, w.Achternaam, w.WerknemerInfo);
					werknemersDTOVanBedrijf.Add(werknemer);
				});
				LijstMetWerknemersVanGeselecteerdBedrijf = werknemersDTOVanBedrijf;
			}
		}

		#region Action Buttons

		private void AnnulerenKlik(object sender, RoutedEventArgs e) {
			GaTerugNaarKiesBedrijf();
		}

		private void AanmeldenKlik(object sender, RoutedEventArgs e) {
			try {
				WerknemerDTO? werknemer = (WerknemerDTO)WerknemersLijst.SelectedValue;

				if (werknemer == null) {
					MessageBox.Show("Gelieve een werknemer te kiezen", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				BezoekerDTO bezoeker = new(Voornaam, Achternaam, Email, Bedrijf);

				if (werknemer.Id.HasValue) {
					MaakNieuweAfspraak(_geselecteerdBedrijfsId.Value, werknemer.Id.Value, bezoeker);
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
			registratieWindow.FrameControl.Navigate(KiesBedrijfPage.Instance);
		}

		private async void MaakNieuweAfspraak(long bedrijfsId, long werknemerId, BezoekerDTO bezoeker) {
			var rawBody = new { werknemerId = werknemerId, bedrijfId = bedrijfsId, bezoeker };
			string json = JsonConvert.SerializeObject(rawBody);

			(bool isvalid, AfspraakOutputDTO afspraak) = await ApiController.Post<AfspraakOutputDTO>("/afspraak", json);

			if (isvalid) {
				MessageBox.Show("U registratie is goed ontvangen");
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het registreren in het systeem", "Error /");
			}
		}

		#endregion


		private Border _selecteditem;
		private void KlikOpRow(object sender, MouseButtonEventArgs e) {
			//Er is 2 keer geklikt
			if (e.ClickCount == 2) {
				return;
			}

			if (_selecteditem is not null) {
				_selecteditem.Background = Brushes.Transparent;
				_selecteditem.BorderThickness = new Thickness(0);
			}
			StackPanel? listViewItem = sender as StackPanel;

			SolidColorBrush hightlightColor = (SolidColorBrush)Application.Current.Resources["LichtGrijsAccent"];

			Border border = (Border)listViewItem.Children[0];
			border.Background = hightlightColor;
			border.BorderThickness = new Thickness(0);
			border.BorderBrush = Brushes.LightGray;
			border.CornerRadius = new CornerRadius(20);
			border.Margin = new Thickness(0, 0, 20, 0);
			_selecteditem = border;
		}

		#region ProppertyChanged
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}