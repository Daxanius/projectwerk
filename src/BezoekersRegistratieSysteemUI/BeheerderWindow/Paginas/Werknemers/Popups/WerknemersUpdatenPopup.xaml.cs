using BezoekersRegistratieSysteemBL.Domeinen;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups {
	public partial class WerknemersUpdatenPopup : UserControl, INotifyPropertyChanged {
		#region Bind Propperties
		private WerknemerDTO oudeWerknemer;

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

		public ObservableCollection<string> Functies { get; set; } = new();
		#endregion

		public WerknemersUpdatenPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay(null);
		}

		private async void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			List<WerknemerInfoInputDTO> werknemerInfo = new();

			Voornaam = Voornaam.Trim();
			Achternaam = Achternaam.Trim();
			Email = Email.Trim();

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

			werknemerInfo.Add(new WerknemerInfoInputDTO(BeheerderWindow.GeselecteerdBedrijf.Id, Email, Functies));
			await ApiController.UpdateWerknemer(new WerknemerInputDTO(Voornaam, Achternaam, werknemerInfo), oudeWerknemer.Id.Value);
			WerknemerDTO? werknemer = await ApiController.GeefWerknemer(oudeWerknemer.Id.Value);
			WerknemerEvents.InvokeUpdateGeselecteerdBedrijf(werknemer);

			SluitOverlay(werknemer);
		}

		private readonly Regex regexGeenCijfers = new("[^a-zA-Z]+");
		private void IsInputGeldigZonderCijfers(object sender, TextCompositionEventArgs e) {
			e.Handled = regexGeenCijfers.IsMatch(e.Text);
		}

		private void VerwijderFunctieVanFunctieLijst(object sender, RoutedEventArgs e) {
			object selectedItem = FunctieLijst.SelectedValue;
			if (selectedItem is not string) return;
			string functie = (string)selectedItem;

			Functies.Remove(functie);
		}
		private void GaTerugNaarWerknemerUpdatenEiland(object sender, RoutedEventArgs e) {
			FunctiesUpdatenEiland.Visibility = Visibility.Collapsed;
			WerknemerUpdatenEiland.Visibility = Visibility.Visible;
			FunctieLijst.SelectedIndex = -1;
		}

		private void GaTerugNaarFunctiesUpdatenEiland(object sender, RoutedEventArgs e) {
			NieuweFunctieTextBlock.Text = "";
			VoegNieuweFunctieToeEiland.Visibility = Visibility.Collapsed;
			FunctiesUpdatenEiland.Visibility = Visibility.Visible;
		}

		private void OpenFunctiesUpdatenEiland(object sender, MouseButtonEventArgs e) {
			FunctiesUpdatenEiland.Visibility = Visibility.Visible;
			WerknemerUpdatenEiland.Visibility = Visibility.Collapsed;
		}

		private void OpenVoegNieuweFunctieToeEilandClick(object sender, RoutedEventArgs e) {
			VoegNieuweFunctieToeEiland.Visibility = Visibility.Visible;
			FunctiesUpdatenEiland.Visibility = Visibility.Collapsed;
		}

		private void VoegieuweFunctieToeAanMedewerker(object sender, RoutedEventArgs e) {
			string functie = NieuweFunctieTextBlock.Text.Trim();
			if (functie.IsNietLeeg()) {
				if (Functies.Any(_f => _f.ToLower() == functie.ToLower())) {
					CustomMessageBox customMessageBox = new();
					customMessageBox.Show("Functie bestaat al", "Error", ECustomMessageBoxIcon.Error);
					return;
				}
				Functies.Add(functie);
				GaTerugNaarFunctiesUpdatenEiland(null, null);
			}
			NieuweFunctieTextBlock.Text = "";
		}

		private void SluitOverlay(WerknemerDTO werknemer) {
			Voornaam = "";
			Achternaam = "";
			Email = "";
			Functie = "";

			WerknemersPage werknemersPage = WerknemersPage.Instance;
			werknemersPage.UpdateWerknemersPopup.Visibility = Visibility.Hidden;

			if (werknemer is not null) {
				CustomMessageBox warningMessage = new();
				warningMessage.Show($"{werknemer.Voornaam} {werknemer.Achternaam} is gewijzigd", "Success", ECustomMessageBoxIcon.Information);
			}
		}

		public void ZetWerknemer(WerknemerDTO werknemer) {
			Voornaam = werknemer.Voornaam;
			Achternaam = werknemer.Achternaam;
			Email = werknemer.Email;
			Functie = werknemer.Functie;

			Functies.Clear();

			var werknemerInfo = werknemer.WerknemerInfoLijst.ToList().Where(werknemerInfo => werknemerInfo.Bedrijf.Id == BeheerderWindow.GeselecteerdBedrijf.Id).First();

			werknemerInfo.Functies.ToList().ForEach(Functies.Add);

			oudeWerknemer = werknemer;
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
