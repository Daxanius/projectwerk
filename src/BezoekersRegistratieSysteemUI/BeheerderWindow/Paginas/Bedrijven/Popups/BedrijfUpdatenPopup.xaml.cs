using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Popups {
	public partial class BedrijfUpdatenPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;

		#region Bind Propperties
		public long Id { get; set; }
		private string _naam = string.Empty;
		public string Naam {
			get { return _naam; }
			set {
				if (value == _naam) return;
				_naam = value;
				UpdatePropperty();
			}
		}
		private string _telefoonNummer = string.Empty;
		public string TelefoonNummer {
			get { return _telefoonNummer; }
			set {
				if (value == _telefoonNummer) return;
				_telefoonNummer = value;
				UpdatePropperty();
			}
		}
		private string _btwNummer = string.Empty;
		public string BTW {
			get { return _btwNummer; }
			set {
				if (value == _btwNummer) return;
				_btwNummer = value;
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
		private string _adres = string.Empty;
		public string Adres {
			get { return _adres; }
			set {
				if (value == _adres) return;
				_adres = value;
				UpdatePropperty();
			}
		}
		#endregion

		public BedrijfUpdatenPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay(null);
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			string naam = Naam.Trim();
			string telefoonNummer = TelefoonNummer.Trim();
			string email = Email.Trim();
			string adres = Adres.Trim();

			if (naam.IsLeeg()) {
				new CustomMessageBox().Show("Naam is verplicht", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (telefoonNummer.IsLeeg() || !telefoonNummer.IsTelefoonNummerGeldig()) {
				new CustomMessageBox().Show("TelefoonNummer is verplicht", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (email.IsLeeg()) {
				new CustomMessageBox().Show("Email is verplicht", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			if (adres.IsLeeg()) {
				new CustomMessageBox().Show("Adres is verplicht", "Fout", ECustomMessageBoxIcon.Error);
				return;
			}

			BedrijfInputDTO nieuwBedrijf = new BedrijfInputDTO(naam, BTW, telefoonNummer, email, adres);
			BedrijfDTO bedrijf = ApiController.UpdateBedrijf(Id, nieuwBedrijf);
			BedrijfEvents.InvokeBedrijfGeupdate(bedrijf);

			SluitOverlay(null);
		}

		private void SluitOverlay(BedrijfDTO? bedrijf) {
			Naam = string.Empty;
			TelefoonNummer = string.Empty;
			BTW = string.Empty;
			Email = string.Empty;
			Adres = string.Empty;
			Visibility = Visibility.Hidden;

			if (bedrijf is not null) {
				new CustomMessageBox().Show($"{bedrijf.Naam} Successvol Geupdate", $"Success", ECustomMessageBoxIcon.Information);
			}
		}

		public void ZetBedrijf(BedrijfDTO bedrijf) {
			Naam = bedrijf.Naam;
			TelefoonNummer = bedrijf.TelefoonNummer;
			BTW = bedrijf.BTW;
			Email = bedrijf.Email;
			Adres = bedrijf.Adres;
			Id = bedrijf.Id;
		}

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
