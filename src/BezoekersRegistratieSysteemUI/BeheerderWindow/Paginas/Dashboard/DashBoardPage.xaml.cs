using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas {
	public partial class DashBoardPage : Page {
		#region Variabele
		private List<AfspraakDTO>? huidigeFilterAfspraken = ApiController.GeefAfspraken().ToList();
		public string Datum => DateTime.Now.ToString("dd.MM");
		#endregion

		public DashBoardPage() {
			this.DataContext = this;
			InitializeComponent();

			GlobalEvents.RefreshData += AutoUpdateIntervalAfspraken_Event;
			GlobalEvents.RefreshDataTimout += TimeOutLoading;
			AfspraakEvents.NieuweAfspraakToegevoegd += NieuweAfspraakToegevoegd_Event;

			//this.NavigationService.Navigate()
			//TODO: :-)
		}
		public void TimeOutLoading() {
			GlobalEvents._refreshTimer.Start();
			GlobalEvents._refreshTimerTimout.Stop();
		}

		private void NieuweAfspraakToegevoegd_Event(AfspraakDTO afspraak) {
			huidigeFilterAfspraken ??= AfsprakenLijstControl.ItemSource.ToList();
			huidigeFilterAfspraken.Add(afspraak);
		}

		private void AutoUpdateIntervalAfspraken_Event() {
			NavigationService? navigating = NavigationService.GetNavigationService(this);
			if (navigating is null || navigating.Content is not DashBoardPage) return;

			Dispatcher.Invoke(() => {
				AfsprakenLijstControl.AutoUpdateIntervalAfspraken();
				huidigeFilterAfspraken = new(AfsprakenLijstControl.ItemSource);
				ComboBox_SelectionChanged(FilterAfsprakenComboBox, null);
			});
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ComboBox combobox = (sender as ComboBox);

			if (combobox.SelectedValue is null) return;
			if (AfsprakenLijstControl is null) return;

			string selected = ((ComboBoxItem)combobox.SelectedValue).Content.ToString();

			List<AfspraakDTO> filtered = huidigeFilterAfspraken;
			if (combobox.SelectedIndex != 0) {
				if (selected.ToLower() == "lopend") {
					filtered = huidigeFilterAfspraken.Where(a => a.Status.ToLower() == "lopend").ToList();
				} else {
					filtered = huidigeFilterAfspraken.Where(a => a.Status.ToLower() != "lopend").ToList();
				}
				filtered = filtered.OrderByDescending(a => a.StartTijd).ToList();
			}

			AfsprakenLijstControl.ItemSource.Clear();
			foreach (AfspraakDTO afspraak in filtered) {
				AfsprakenLijstControl.ItemSource.Add(afspraak);
			}
		}

		#region Singleton
		private static DashBoardPage instance = null;
		private static readonly object padlock = new object();

		public static DashBoardPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new DashBoardPage();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
