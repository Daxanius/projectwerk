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
			bool isStartTijdOk = DateTime.TryParse(StartTijdText.Trim(), out DateTime startTijd);

			if (!isStartTijdOk) {
				new CustomMessageBox().Show("StartTijd is niet geldig", "Error", ECustomMessageBoxIcon.Error);
				return;
			}

			DateTime? eindTijd = null;

			if (EindTijdText is not null && EindTijdText.Trim().IsNietLeeg()) {
				bool isEindTijdOk = DateTime.TryParse(EindTijdText.Trim(), out DateTime parsedEindTijd);

				if (!isEindTijdOk) {
					new CustomMessageBox().Show("StartTijd is niet geldig", "Error", ECustomMessageBoxIcon.Error);
					return;
				}

				eindTijd = parsedEindTijd;

				if (eindTijd <= startTijd) {
					new CustomMessageBox().Show("EindTijd moet groter zijn dan StartTijd", "Success", ECustomMessageBoxIcon.Error);
					return;
				}
			}

			if (nummerplaat.IsLeeg()) {
				new CustomMessageBox().Show("Nummerplaat is verplicht", "Success", ECustomMessageBoxIcon.Error); ;
				return;
			}

			ApiController.CheckNummerplaatIn(BeheerderWindow.GeselecteerdBedrijf.Id, startTijd, eindTijd, nummerplaat);

			ParkeerplaatsDTO parkeerplaats = new ParkeerplaatsDTO(BeheerderWindow.GeselecteerdBedrijf, startTijd, nummerplaat);
			ParkingEvents.NieuweParkeerplaatsInChecken(parkeerplaats);

			SluitOverlay(nummerplaat);
		}

		private void SluitOverlay(string nummerplaat) {
			Nummerplaat = string.Empty;
			EindTijdText = null;
			StartTijdText = null;
			Visibility = Visibility.Hidden;

			if (nummerplaat is not null) {
				new CustomMessageBox().Show($"{nummerplaat} toegevoegd", $"Success", ECustomMessageBoxIcon.Information);
			}
		}

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
