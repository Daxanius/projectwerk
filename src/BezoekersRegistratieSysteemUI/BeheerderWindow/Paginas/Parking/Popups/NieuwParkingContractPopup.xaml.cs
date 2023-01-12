using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking.Popups {
	public partial class NieuwParkingContractPopup : UserControl, INotifyPropertyChanged {
		public event PropertyChangedEventHandler? PropertyChanged;

		private int _aantalPlaatsen = 0;
		public int AantalPlaatsen {
			get => _aantalPlaatsen;
			set {
				if (value != _aantalPlaatsen) {
					_aantalPlaatsen = value;
					UpdatePropperty();
				}
			}
		}

		public NieuwParkingContractPopup() {
			this.DataContext = this;
			InitializeComponent();

			startTijdTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy");
			EindTijdDatePicker.SelectedDate = DateTime.Now.AddYears(1);
		}

		private void MaakButton_Click(object sender, RoutedEventArgs e) {

			if (!EindTijdDatePicker.SelectedDate.HasValue) {
				new CustomMessageBox().Show("Je moet een EindTijd kiezen", "Error", ECustomMessageBoxIcon.Error);
				return;
			}

			DateTime StartTijd = DateTime.Now;
			DateTime EindTijd = EindTijdDatePicker.SelectedDate.Value;

			if (StartTijd >= EindTijd) {
				new CustomMessageBox().Show("StartTijd mag niet groter of gelijk zijn dan EindTijd", "Error", ECustomMessageBoxIcon.Error);
				return;
			}

			if (AantalPlaatsen <= 0) {
				new CustomMessageBox().Show("Aantal plaatsen moet groter dan 0 zijn", "Error", ECustomMessageBoxIcon.Error);
				return;
			}

			long bedrijfId = BeheerderWindow.GeselecteerdBedrijf.Id;

			var parkingContract = ApiController.GeefParkingContract(bedrijfId);
			if (parkingContract is null) {
				ApiController.VoegParkingContractToe(bedrijfId, StartTijd, EindTijd, AantalPlaatsen);
			} else {
				ApiController.VerwijderParkingContract(bedrijfId);
				ApiController.VoegParkingContractToe(bedrijfId, StartTijd, EindTijd, AantalPlaatsen);
			}

			parkingContract = ApiController.GeefParkingContract(bedrijfId);

			ParkingEvents.UpdateParkingContract(parkingContract);
			Visibility = Visibility.Collapsed;
			
		}

		#region ProppertyChanged

		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
