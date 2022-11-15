using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups {
	public partial class WerknemersPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;
		public delegate void NieuweWerknemerToegevoegdEvent(WerknemerDTO werknemer);
		public static event NieuweWerknemerToegevoegdEvent NieuweWerknemerToegevoegd;

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

		public WerknemersPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			List<WerknemerInfoInputDTO> werknemerInfo = new();

			Voornaam = Voornaam.Trim();
			Achternaam = Achternaam.Trim();
			Email = Email.Trim();
			Functie = Functie.Trim();

			if (string.IsNullOrWhiteSpace(Voornaam)) {
				MessageBox.Show("Voornaam mag niet leeg zijn");
				return;
			};

			if (string.IsNullOrWhiteSpace(Achternaam)) {
				MessageBox.Show("Achternaam mag niet leeg zijn");
				return;
			};

			if (string.IsNullOrWhiteSpace(Email)) {
				MessageBox.Show("Email mag niet leeg zijn");
				return;
			};

			if (string.IsNullOrWhiteSpace(Functie)) {
				MessageBox.Show("Functie mag niet leeg zijn");
				return;
			};

			werknemerInfo.Add(new WerknemerInfoInputDTO(BeheerderWindow.GeselecteerdBedrijf.Id, Email, new List<string>() { Functie }));
			WerknemerDTO werknemer = ApiController.PostWerknemer(new WerknemerInputDTO(Voornaam, Achternaam, werknemerInfo));
			NieuweWerknemerToegevoegd?.Invoke(werknemer);

			MessageBox.Show($"Werknemer: {werknemer.Voornaam} {werknemer.Achternaam} is toegevoegd");
			SluitOverlay();
		}

		private readonly Regex regexGeenCijfers = new("[^a-zA-Z]+");
		private void IsInputGeldigZonderCijfers(object sender, TextCompositionEventArgs e) {
			e.Handled = regexGeenCijfers.IsMatch(e.Text);
		}

		private void SluitOverlay() {
			Voornaam = "";
			Achternaam = "";
			Email = "";
			Functie = "";

			WerknemersPage werknemersPage = WerknemersPage.Instance;
			werknemersPage.WerknemersPopup.Visibility = Visibility.Hidden;
		}

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
