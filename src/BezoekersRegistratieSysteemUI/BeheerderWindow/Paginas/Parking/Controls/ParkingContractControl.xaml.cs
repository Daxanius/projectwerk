using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking.Controls {
	public partial class ParkingContractControl : UserControl {

		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }
		
        public ParkingContractControl() {

            BedrijfEvents.GeselecteerdBedrijfChanged += UpdateGeselecteerdBedrijf_Event;
            GlobalEvents.RefreshData += UpdateGeselecteerdBedrijf_Event;
            ParkingEvents.NieuweNummerplaatInGeChecked += NieuweNummerplaatInGeChecked_Event;
			ParkingEvents.NummerplaatUitChecken += NummerplaatUitChecken_Event;

            this.DataContext = this;
			InitializeComponent();
            InitializeContractInfo();
        }

		private void NummerplaatUitChecken_Event(ParkeerplaatsDTO parkeerplaats) {
			bool isOk = int.TryParse(BezetAantalParkeerplaatsenBedrijf.Content.ToString(), out int aantal);
			if (!isOk || aantal <= 0) return;
			BezetAantalParkeerplaatsenBedrijf.Content = aantal - 1;
		}

		private void NieuweNummerplaatInGeChecked_Event(ParkeerplaatsDTO parkeerplaats) {
			bool isOk = int.TryParse(BezetAantalParkeerplaatsenBedrijf.Content.ToString(), out int aantal);
			if (!isOk) return;
			BezetAantalParkeerplaatsenBedrijf.Content = aantal + 1;
		}

		private void UpdateGeselecteerdBedrijf_Event()
        {
            InitializeContractInfo();
        }

		private void InitializeContractInfo()
		{
            var parkingContract = ApiController.GeefParkingContract(GeselecteerdBedrijf.Id);
            TotaalAantalParkeerplaatsenBedrijf.Content = parkingContract.AantalPlaatsen;
            BezetAantalParkeerplaatsenBedrijf.Content = ApiController.GeefHuidigBezetteParkeerplaatsenVoorBedrijf(GeselecteerdBedrijf.Id);
            StartDatumParkingContract.Content = parkingContract.Starttijd;
            EindDatumParkingContract.Content = parkingContract.Eindtijd;
		}
	}
}
