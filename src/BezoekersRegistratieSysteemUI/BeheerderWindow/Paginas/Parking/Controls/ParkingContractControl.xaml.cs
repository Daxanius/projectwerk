using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking.Controls {
	public partial class ParkingContractControl : UserControl, INotifyPropertyChanged {

		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }

		public ParkingContractControl() {

			BedrijfEvents.GeselecteerdBedrijfChanged += UpdateGeselecteerdBedrijf_Event;

			this.DataContext = this;
			InitializeComponent();
			InitializeContractInfo();

			//Kijk of je kan rechts klikken om iets te doen
			//NummerplaatLijst.ContextMenuOpening += (sender, args) => args.Handled = true;
			//ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		private void UpdateGeselecteerdBedrijf_Event() {
			InitializeContractInfo();
		}

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged

		private void InitializeContractInfo() {
			var parkingContract = ApiController.GeefParkingContract(GeselecteerdBedrijf.Id);
			TotaalAantalParkeerplaatsenBedrijf.Content = parkingContract.AantalPlaatsen;
			BezetAantalParkeerplaatsenBedrijf.Content = ApiController.GeefHuidigBezetteParkeerplaatsenVoorBedrijf(GeselecteerdBedrijf.Id);
			StartDatumParkingContract.Content = parkingContract.Starttijd;
			EindDatumParkingContract.Content = parkingContract.Eindtijd;
		}
	}
}
