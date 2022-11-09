using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Beheerder;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups {
	/// <summary>
	/// Interaction logic for WerknemersPopup.xaml
	/// </summary>
	public partial class WerknemersPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;

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

		private void VoegNieuweFunctieToe(object sender, MouseButtonEventArgs e) {

		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			List<WerknemerInfoInputDTO> werknemerInfo = new();
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
			WerknemerOutputDTO werknemer = ApiController.PostWerknemer(new WerknemerInputDTO(Voornaam, Achternaam, werknemerInfo));
			SluitOverlay();
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
