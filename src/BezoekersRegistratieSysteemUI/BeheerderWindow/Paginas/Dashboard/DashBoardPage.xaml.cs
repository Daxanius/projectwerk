using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas {
	public partial class DashBoardPage : Page {
		#region Variabele
		private List<AfspraakDTO>? huidigeFilterAfspraken;
		public string Datum => DateTime.Now.ToString("dd.MM");
		#endregion

		public DashBoardPage() {
			this.DataContext = this;
			InitializeComponent();

			huidigeFilterAfspraken = ApiController.GeefAfspraken().ToList();

			App.RefreshTimer.Tick += AutoUpdateIntervalAfspraken_Event;
			AfsprakenPopup.NieuweAfspraakToegevoegd += (AfspraakDTO afspraak) => {
				if (huidigeFilterAfspraken is null) huidigeFilterAfspraken = AfsprakenLijstControl.ItemSource.ToList();
				huidigeFilterAfspraken.Add(afspraak);
			};

			//this.NavigationService.Navigate()
			//TODO: :-)
		}

		private void AutoUpdateIntervalAfspraken_Event(object? sender, EventArgs e) {
			Task.Run(() => {
				Dispatcher.Invoke(() => {
					AfsprakenLijstControl.AutoUpdateIntervalAfspraken();
					huidigeFilterAfspraken = new(AfsprakenLijstControl.ItemSource);
					ComboBox_SelectionChanged(FilterAfsprakenComboBox, null);
				});
			});
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ComboBox combobox = (sender as ComboBox);

			if (combobox.SelectedValue is null) return;
			if (AfsprakenLijstControl is null) return;

			string selected = ((ComboBoxItem)combobox.SelectedValue).Content.ToString();

			List<AfspraakDTO> filtered = huidigeFilterAfspraken;
			if (combobox.SelectedIndex != 0) {
				filtered = huidigeFilterAfspraken.Where(a => a.Status.ToLower() == selected.ToLower()).ToList();
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
