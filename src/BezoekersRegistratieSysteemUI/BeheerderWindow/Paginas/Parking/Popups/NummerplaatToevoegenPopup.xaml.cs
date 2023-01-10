using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking.Popups {
	public partial class NummerplaatToevoegenPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;

		#region Bind Propperties
		private string _naam = string.Empty;
		public string Nummerplaat {
			get { return _naam; }
			set {
				if (value == _naam) return;
				_naam = value;
				UpdatePropperty();
			}
		}

		private string _eindTijdText = string.Empty;
		public string EindTijdText {
			get { return _eindTijdText; }
			set {
				if (value == _eindTijdText) return;
				_eindTijdText = value;
				UpdatePropperty();
			}
		}
		private string _startTijdText = string.Empty;
		public string StartTijdText {
			get { return _startTijdText; }
			set {
				if (value == _startTijdText) return;
				_startTijdText = value;
				UpdatePropperty();
			}
		}
		#endregion

		public NummerplaatToevoegenPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay(null);
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			string nummerplaat = Nummerplaat.Trim();
			DateTime? startTijd = DateTime.Parse(StartTijdText.Trim());
			DateTime? eindTijd = null;

			if (EindTijdText is not null && EindTijdText.Trim().IsNietLeeg()) {
				eindTijd = DateTime.Parse(EindTijdText);
			}

			CustomMessageBox customMessageBox = new CustomMessageBox();

			if (nummerplaat.IsLeeg()) {
				customMessageBox.Show("Nummerplaat is verplicht", "Success", ECustomMessageBoxIcon.Error);;
				return;
			}

			if (eindTijd is not null && eindTijd <= startTijd) {
				customMessageBox.Show("EindTijd moet groter zijn dan StartTijd", "Success", ECustomMessageBoxIcon.Error);
				return;
			}

			ApiController.CheckNummerplaatIn(BeheerderWindow.GeselecteerdBedrijf.Id, DateTime.Now, eindTijd, nummerplaat);
			SluitOverlay(nummerplaat);
		}

		private void SluitOverlay(string nummerplaat) {
			Nummerplaat = string.Empty;
			EindTijdText = null;
			StartTijdText = null;
			Visibility = Visibility.Hidden;

			if (nummerplaat is not null) {
				CustomMessageBox customMessageBox = new();
				customMessageBox.Show($"{nummerplaat} toegevoegd", $"Success", ECustomMessageBoxIcon.Information);
			}
		}

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
