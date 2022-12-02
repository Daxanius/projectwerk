using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
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

namespace BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden {
	public partial class AanmeldGegevensPage : Page, INotifyPropertyChanged {

		#region Events

		public event PropertyChangedEventHandler? PropertyChanged;

		#endregion

		#region Variabelen

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

		private string _bedrijf = "";
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

			LijstMetWerknemersVanGeselecteerdBedrijf = ApiController.GeefWerknemersVanBedrijf(GeselecteerdBedrijf).ToList();
		}

		#region Functies
		private void AnnulerenKlik(object sender, RoutedEventArgs e) {
			GaTerugNaarKiesBedrijf();
		}

		private void AanmeldenKlik(object sender, RoutedEventArgs e) {
			try {
				#region Controle Input
				Voornaam = Voornaam.Trim();
				Achternaam = Achternaam.Trim();
				Email = Email.Trim();
				Bedrijf = Bedrijf.Trim();

				if (RegistratieWindow.GeselecteerdBedrijf is null) {
					MessageBox.Show("Er is geen bedrijf geselecteerd", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (Achternaam.IsLeeg()) {
					MessageBox.Show("Achternaam is verplicht", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (Voornaam.IsLeeg()) {
					MessageBox.Show("Voornaam is verplicht", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (Email.IsLeeg()) {
					MessageBox.Show("Email is verplicht", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (!Email.IsEmailGeldig()) {
					MessageBox.Show("Email is niet geldig!", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				WerknemerDTO? werknemer = (WerknemerDTO)WerknemersLijst.SelectedValue;

				if (werknemer is null) {
					MessageBox.Show("Gelieve een werknemer te kiezen", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				if (werknemer.Id is null) {
					MessageBox.Show("Werknemer id is null, gelieve het aanmeldscherm te herstarten", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				#endregion

				BezoekerDTO bezoeker = new(Voornaam, Achternaam, Email, Bedrijf);

				if (werknemer.Id.HasValue) {
					MessageBoxResult result = MessageBox.Show($"Zijn ingevoerde gegevens correct?" +
						$"\n\nNaam: {Voornaam} {Achternaam}\nEmail: {Email}\nBedrijf: {Bedrijf}", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
					if (result == MessageBoxResult.Yes)
						MaakNieuweAfspraak(GeselecteerdBedrijf.Id, werknemer.Id.Value, bezoeker);
					else return;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			GaTerugNaarKiesBedrijf();
		}

		private void GaTerugNaarKiesBedrijf() {
			Voornaam = "";
			Achternaam = "";
			Email = "";
			Bedrijf = "";

			RegistratieWindow registratieWindow = (RegistratieWindow)Window.GetWindow(this);
			registratieWindow.FrameControl.Content = KiesBedrijfPage.Instance;
		}

		private async void MaakNieuweAfspraak(long bedrijfsId, long werknemerId, BezoekerDTO bezoeker) {
			var rawBody = new { werknemerId = werknemerId, bedrijfId = bedrijfsId, bezoeker };
			string json = JsonConvert.SerializeObject(rawBody);

			(bool isvalid, AfspraakOutputDTO afspraak) = await ApiController.Post<AfspraakOutputDTO>("/afspraak", json);

			if (isvalid) {
				MessageBox.Show($"U registratie is goed ontvangen met als startijd: {afspraak.Starttijd:HH:mm - dd/MM/yyyy}");
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het registreren in het systeem", "Error /");
			}
		}

		private readonly Regex regexGeenCijfers = new Regex("[^a-zA-Z]+");
		private void IsDatePickerGeldigeText(object sender, TextCompositionEventArgs e) {
			e.Handled = regexGeenCijfers.IsMatch(e.Text);
		}
		#endregion

		#region ProppertyChanged
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}