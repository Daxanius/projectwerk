using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.MessageBoxes;
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
				new CustomMessageBox().Show("Bedrijf is niet gekozen", "Error", ECustomMessageBoxIcon.Error);
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
				if (RegistratieWindow.GeselecteerdBedrijf is null) {
					new CustomMessageBox().Show("Er is geen bedrijf geselecteerd", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (Achternaam.IsLeeg()) {
					new CustomMessageBox().Show("Achternaam is verplicht", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (Voornaam.IsLeeg()) {
					new CustomMessageBox().Show("Voornaam is verplicht", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (Email.IsLeeg()) {
					new CustomMessageBox().Show("Email is verplicht", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (!Email.IsEmailGeldig()) {
					new CustomMessageBox().Show("Email is niet geldig!", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				Voornaam = Voornaam.Trim();
				Achternaam = Achternaam.Trim();
				Email = Email.Trim();
				Bedrijf = Bedrijf.Trim();

				WerknemerDTO? werknemer = (WerknemerDTO)WerknemersLijst.SelectedValue;

				if (werknemer is null) {
					new CustomMessageBox().Show("Gelieve een werknemer te kiezen", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				if (werknemer.Id is null) {
					new CustomMessageBox().Show("Werknemer id is null, gelieve het aanmeldscherm te herstarten", "Fout", ECustomMessageBoxIcon.Error);
					return;
				}

				#endregion

				BezoekerDTO bezoeker = new(Voornaam, Achternaam, Email, Bedrijf);

				if (werknemer.Id.HasValue) {
					ECustomMessageBoxResult result = new CustomMessageBox().Show($"Zijn ingevoerde gegevens correct?" +
						$"\n\nNaam: {Voornaam} {Achternaam}\nEmail: {Email}\nBedrijf: {Bedrijf}", "Bevestiging", ECustomMessageBoxIcon.Question);
					if (result == ECustomMessageBoxResult.Bevestigen)
						MaakNieuweAfspraak(GeselecteerdBedrijf.Id, werknemer.Id.Value, bezoeker);
					else return;
				}
			} catch (Exception ex) {
				new CustomMessageBox().Show(ex.Message, "Fout", ECustomMessageBoxIcon.Error);
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
				new CustomMessageBox().Show($"Uw registratie werd goed ontvangen.", "Success", ECustomMessageBoxIcon.Information);
			} else {
				new CustomMessageBox().Show("Er is iets fout gegaan bij het registreren in het systeem", "Error", ECustomMessageBoxIcon.Error);
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