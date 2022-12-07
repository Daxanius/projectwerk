using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System.Collections.Generic;
using System.ComponentModel;
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
			SluitOverlay(null);
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			List<WerknemerInfoInputDTO> werknemerInfo = new();

			Voornaam = Voornaam.Trim();
			Achternaam = Achternaam.Trim();
			Email = Email.Trim();
			Functie = Functie.Trim();

			if (Voornaam.IsLeeg()) {
				MessageBox.Show("Voornaam mag niet leeg zijn");
				return;
			};

			if (Achternaam.IsLeeg()) {
				MessageBox.Show("Achternaam mag niet leeg zijn");
				return;
			};

			if (Email.IsLeeg()) {
				MessageBox.Show("Email mag niet leeg zijn");
				return;
			};

			if (Functie.IsLeeg()) {
				MessageBox.Show("Functie mag niet leeg zijn");
				return;
			};

			werknemerInfo.Add(new WerknemerInfoInputDTO(BeheerderWindow.GeselecteerdBedrijf.Id, Email, new List<string>() { Functie }));
			WerknemerDTO werknemer = ApiController.MaakWerknemer(new WerknemerInputDTO(Voornaam, Achternaam, werknemerInfo));
			werknemer.Status = "Vrij";
			WerknemerEvents.InvokeNieuweWerkenemer(werknemer);

			SluitOverlay(werknemer);
		}

		private readonly Regex regexGeenCijfers = new("[^a-zA-Z]+");
		private void IsInputGeldigZonderCijfers(object sender, TextCompositionEventArgs e) {
			e.Handled = regexGeenCijfers.IsMatch(e.Text);
		}

		private void SluitOverlay(WerknemerDTO werknemer) {
			Voornaam = "";
			Achternaam = "";
			Email = "";
			Functie = "";

			WerknemersPage werknemersPage = WerknemersPage.Instance;
			werknemersPage.WerknemersPopup.Visibility = Visibility.Hidden;

			if (werknemer is not null) {
				CustomMessageBox warningMessage = new();
				warningMessage.Show($"{werknemer.Voornaam} {werknemer.Achternaam} is toegevoegd", "Success", ECustomMessageBoxIcon.Information);
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
