using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BezoekersRegistratieSysteemUI.Events;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls {
	public partial class AfsprakenLijstControl : UserControl {
		#region Variabelen
		public ObservableCollection<AfspraakDTO> ItemSource { get; set; }
		#endregion

		public AfsprakenLijstControl() {
			ItemSource = new();
			this.DataContext = this;
			InitializeComponent();

			AfspraakEvents.NieuweAfspraakToegevoegd += (AfspraakDTO afspraak) => {
				Task.Run(() => {
					Dispatcher.Invoke(() => {
						ItemSource.Add(afspraak);
						List<AfspraakDTO> afspraken = ItemSource.ToList();
						ItemSource.Clear();
						afspraken.OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam).ToList().ForEach(a => ItemSource.Add(a));
					});
				});
			};

			UpdateAfsprakenOpSchermMetNieuweData(HaalAlleAfspraken());

			//Kijk of je kan rechts klikken om iets te doen
			AfsprakenLijst.ContextMenuOpening += (sender, args) => args.Handled = true;
			ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		public void AutoUpdateIntervalAfspraken() {
			UpdateAfsprakenOpSchermMetNieuweData(HaalAlleAfspraken());
		}

		public void UpdateAfsprakenOpSchermMetNieuweData(List<AfspraakDTO> afspraken) {
			ItemSource.Clear();
			foreach (AfspraakDTO afspraak in afspraken) {
				ItemSource.Add(afspraak);
			}
		}

		public List<AfspraakDTO> HaalAlleAfspraken() {
			return ApiController.GeefAfspraken().ToList();
		}

		private void KlikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			AfspraakDTO? afspraak = b?.CommandParameter as AfspraakDTO;

			OpenAfspraakDetail(afspraak);
		}

		private void OpenAfspraakDetail(AfspraakDTO afspraak) {

		}

		private void WijzigAfspraken_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is AfspraakDTO afspraak) {

			}
		}

		private async void VerwijderAfspraken_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is AfspraakDTO afspraak) {

			}
		}
	}
}
