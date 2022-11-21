﻿using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Popups {
	public partial class BedrijvenPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;

		#region Bind Propperties
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
		public string BtwNummer {
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

		public BedrijvenPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			Naam = Naam.Trim();
			BtwNummer = BtwNummer.Trim();
			TelefoonNummer = TelefoonNummer.Trim();
			Email = Email.Trim();
			Adres = Adres.Trim();

			if (Naam.IsLeeg()) {
				MessageBox.Show("Naam is verplicht");
				return;
			}

			if (BtwNummer.IsLeeg()) {
				MessageBox.Show("BtwNummer is verplicht");
				return;
			}

			if (TelefoonNummer.IsLeeg()) {
				MessageBox.Show("TelefoonNummer is verplicht");
				return;
			}

			if (Email.IsLeeg()) {
				MessageBox.Show("Email is verplicht");
				return;
			}

			if (Adres.IsLeeg()) {
				MessageBox.Show("Adres is verplicht");
				return;
			}

			BedrijfInputDTO nieuwBedrijf = new BedrijfInputDTO(Naam, BtwNummer, TelefoonNummer, Email, Adres);
			BedrijfDTO bedrijf = ApiController.MaakBedrijf(nieuwBedrijf);

			MessageBox.Show($"{Naam} successvol toegevoegd", "Bedrijf toegevoegd", MessageBoxButton.OK, MessageBoxImage.Information);

			BedrijfEvents.InvokeNieuwBedrijfToeGevoegd(bedrijf);

			SluitOverlay();
		}

		private void SluitOverlay() {
			Naam = string.Empty;
			TelefoonNummer = string.Empty;
			BtwNummer = string.Empty;
			Email = string.Empty;
			Adres = string.Empty;
			Visibility = Visibility.Hidden;
		}

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
