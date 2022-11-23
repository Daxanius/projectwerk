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
using System.Windows.Threading;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls {
	public partial class AfsprakenLijstControl : UserControl {
		#region Variabelen
		public ObservableCollection<AfspraakDTO> ItemSource { get; set; }
		#endregion

		public AfsprakenLijstControl() {
			ItemSource = new();
			this.DataContext = this;
			InitializeComponent();

			ItemSource.CollectionChanged += ItemSource_CollectionChanged;

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

		//Auto Columns Resize on Change
		private void ItemSource_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			GridView view = AfsprakenLijst.View as GridView;
			foreach (GridViewColumn c in view.Columns) {
				if (double.IsNaN(c.Width)) {
					c.Width = c.ActualWidth;
				}
				c.Width = double.NaN;
			}
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

		private void WijzigAfspraken_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is AfspraakDTO afspraak) {

			}
		}

		private async void VerwijderAfspraken_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is AfspraakDTO afspraak) {

			}
		}

		private void AfsprakenLijst_ScrollChanged(object sender, ScrollChangedEventArgs e) {
			if (GlobalEvents._refreshTimerTimout.IsEnabled == true) {
				return;
			}
			GlobalEvents._refreshTimerTimout.Start();
			GlobalEvents._refreshTimer.Stop();
        }
	}
}
