using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups {
	public partial class WerknemersToevoegenPopup : UserControl, INotifyPropertyChanged {
		#region Bind Propperties
		private string _voornaam = string.Empty;
		public string Voornaam {
			get { return _voornaam; }
			set {
				if (value == _voornaam) return;
				_voornaam = value;
				UpdatePropperty();
			}
		}
		private string _achternaam = string.Empty;
		public string Achternaam {
			get { return _achternaam; }
			set {
				if (value == _achternaam) return;
				_achternaam = value;
				UpdatePropperty();
			}
		}
		private string _emailnaam = string.Empty;
		public string Email {
			get { return _emailnaam; }
			set {
				if (value == _emailnaam) return;
				_emailnaam = value;
				UpdatePropperty();
			}
		}
		private string _functie = string.Empty;
		public string Functie {
			get { return _functie; }
			set {
				if (value == _functie) return;
				_functie = value;
				UpdatePropperty();
			}
		}
		#endregion

		public WerknemersToevoegenPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay(null, null);
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			List<WerknemerInfoInputDTO> werknemerInfo = new();

			Voornaam = Voornaam.Trim();
			Achternaam = Achternaam.Trim();
			Email = Email.Trim();
			Functie = Functie.Trim();

			if (Voornaam.IsLeeg()) {
				new CustomMessageBox().Show("Voornaam mag niet leeg zijn", "Fout", ECustomMessageBoxIcon.Error);
				return;
			};

			if (Achternaam.IsLeeg()) {
				new CustomMessageBox().Show("Achternaam mag niet leeg zijn", "Fout", ECustomMessageBoxIcon.Error);
				return;
			};

			if (Email.IsLeeg()) {
				new CustomMessageBox().Show("Email mag niet leeg zijn", "Fout", ECustomMessageBoxIcon.Error);
				return;
			};

			if (Functie.IsLeeg()) {
				new CustomMessageBox().Show("Functie mag niet leeg zijn", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			var werknemersMetZelfdeNaam = ApiController.BestaatWerknemerInPark(Voornaam, Achternaam);
			IEnumerable<WerknemerInfoInputDTO> werknemerInfoInputDTOs = new List<WerknemerInfoInputDTO>() {
					new WerknemerInfoInputDTO(BeheerderWindow.GeselecteerdBedrijf.Id, Email, new List<string>() { Functie })
				};

			WerknemerInputDTO werknemerInputDTO = new WerknemerInputDTO(Voornaam, Achternaam, werknemerInfoInputDTOs);

			if (werknemersMetZelfdeNaam.Count > 0) {
				WerknemersPage werknemersPage = WerknemersPage.Instance;
				this.Visibility = Visibility.Collapsed;
				werknemersPage.WerknemerBestaatPopup.ZetData(werknemersMetZelfdeNaam, Email, Functie, () => SluitOverlay(Voornaam, Achternaam));
				werknemersPage.WerknemerBestaatPopup.Visibility = Visibility.Visible;
			} else {
				WerknemerDTO werknemer = ApiController.MaakWerknemer(werknemerInputDTO);
				werknemer.Status = "Vrij";
				WerknemerEvents.InvokeNieuweWerkenemer(werknemer);
				SluitOverlay(Voornaam, Achternaam);
			}
		}

		private readonly Regex regexGeenCijfers = new("[^a-zA-Z]+");
		private void IsInputGeldigZonderCijfers(object sender, TextCompositionEventArgs e) {
			e.Handled = regexGeenCijfers.IsMatch(e.Text);
		}

		private void SluitOverlay(string? voornaam, string? achternaam) {
			Voornaam = "";
			Achternaam = "";
			Email = "";
			Functie = "";

			WerknemersPage werknemersPage = WerknemersPage.Instance;
			werknemersPage.WerknemersPopup.Visibility = Visibility.Collapsed;

			if (voornaam is not null && achternaam is not null) {
				CustomMessageBox warningMessage = new();
				warningMessage.Show($"{voornaam} {achternaam} is toegevoegd", "Success", ECustomMessageBoxIcon.Information);
			}
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
