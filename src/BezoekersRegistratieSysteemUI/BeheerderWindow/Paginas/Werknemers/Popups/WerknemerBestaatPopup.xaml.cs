using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups {
	public partial class WerknemerBestaatPopup : UserControl {
		private Action _callBack;
		private string _email;
		private string _functie;

		public WerknemerBestaatPopup() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			WerknemerDTO? selectedWerknemer = (WerknemerDTO)WerknemersMetZelfdeNaam.SelectedItem;
			if (selectedWerknemer is null) return;

			WerknemerInfoInputDTO werknemerInfoInputDTO = new WerknemerInfoInputDTO(BeheerderWindow.GeselecteerdBedrijf.Id, _email, new List<string>() { _functie });
			ApiController.VoegWerknemerFunctieToe(selectedWerknemer.Id.Value, werknemerInfoInputDTO);

			WerknemerEvents.UpdateAlleWerknemerScherm();
			WerknemersPage.Instance.WerknemersPopup.Visibility = Visibility.Collapsed;
			_callBack();
			SluitOverlay();
		}

		private void NieuweWerknemerButton_Click(object sender, RoutedEventArgs e) {
			WerknemerDTO? selectedWerknemer = (WerknemerDTO)WerknemersMetZelfdeNaam.SelectedItem;
			if (selectedWerknemer is null) return;
			
			WerknemerInfoInputDTO werknemerInfoDTO = new WerknemerInfoInputDTO(BeheerderWindow.GeselecteerdBedrijf.Id, _email, new List<string>() { _functie });
			WerknemerInputDTO werknemerInputDTO = new WerknemerInputDTO(selectedWerknemer.Voornaam, selectedWerknemer.Achternaam, new List<WerknemerInfoInputDTO>() { werknemerInfoDTO });
				
			WerknemerDTO werknemer = ApiController.MaakWerknemer(werknemerInputDTO);
			werknemer.Status = "Vrij";

			WerknemerEvents.InvokeNieuweWerkenemer(werknemer);
			WerknemersPage.Instance.WerknemersPopup.Visibility = Visibility.Collapsed;
			
			_callBack();
			SluitOverlay();
		}

		private void TerugButton_Click(object sender, RoutedEventArgs e) {
			WerknemersPage.Instance.WerknemersPopup.Visibility = Visibility.Visible;
			SluitOverlay();
		}

		private void SluitOverlay() {
			WerknemersPage.Instance.WerknemerBestaatPopup.Visibility = Visibility.Collapsed;
		}

		public void ZetData(List<WerknemerDTO> werknemers, string email, string functies, Action callBack) {
			_callBack = callBack;
			_email = email;
			_functie = functies;

			WerknemersMetZelfdeNaam.ItemsSource = werknemers;
		}
	}
}
