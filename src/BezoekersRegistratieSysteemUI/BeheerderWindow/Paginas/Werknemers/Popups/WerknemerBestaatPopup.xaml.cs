using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Input;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups {
	public partial class WerknemerBestaatPopup : UserControl {
		private string _naam;
		private WerknemerInputDTO _werknemerInfo;
		private Action _callBack;

		public bool? GekozenWaarde { get; set; } = null;

		public WerknemerBestaatPopup() {
			this.DataContext = this;
			InitializeComponent();
		}


		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			GekozenWaarde = false;
			SluitOverlay();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
			GekozenWaarde = true;
			SluitOverlay();
		}

		private void TerugButton_Click(object sender, RoutedEventArgs e) {
			GekozenWaarde = null;
			SluitOverlay();
		}

		private void SluitOverlay() {

			WerknemersPage werknemersPage = WerknemersPage.Instance;

			if (GekozenWaarde.HasValue) {
				WerknemerDTO werknemer = ApiController.MaakWerknemer(_werknemerInfo, GekozenWaarde.Value);
				werknemer.Status = "Vrij";
				WerknemerEvents.InvokeNieuweWerkenemer(werknemer);
				werknemersPage.WerknemersPopup.Visibility = Visibility.Collapsed;
				_callBack();
			} else {
				werknemersPage.WerknemersPopup.Visibility = Visibility.Visible;
			}

			werknemersPage.WerknemerBestaatPopup.Visibility = Visibility.Collapsed;

			_naam = "";
			_werknemerInfo = null;
		}

		public void ZetData(WerknemerInputDTO werknemerInfo, string naam, Action callBack) {
			_naam = naam;
			_werknemerInfo = werknemerInfo;
			_callBack = callBack;

			NaamTextBox.Content = _naam;
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged
	}
}
