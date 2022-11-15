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
		private List<AfspraakDTO> huidigeFilterAfspraken;
		public string Datum => DateTime.Now.ToString("dd.MM");
		#endregion

		public DashBoardPage() {
			this.DataContext = this;
			InitializeComponent();

			AfsprakenPopup.NieuweAfspraakToegevoegd += (AfspraakDTO afspraak) => {
				if (huidigeFilterAfspraken is null) huidigeFilterAfspraken = AfsprakenLijstControl.ItemSource.ToList();
				huidigeFilterAfspraken.Add(afspraak);
			};

			//this.NavigationService.Navigate()
			//TODO: :-)
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

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (AfsprakenLijstControl is null) return;
			if (huidigeFilterAfspraken is null) huidigeFilterAfspraken = AfsprakenLijstControl.ItemSource.ToList();

			ComboBox combobox = (sender as ComboBox);
			if (combobox.SelectedValue is null) return;

			string selected = ((ComboBoxItem)combobox.SelectedValue).Content.ToString();

			List<AfspraakDTO> filtered = huidigeFilterAfspraken;
			if (combobox.SelectedIndex != 0)
				filtered = huidigeFilterAfspraken.Where(x => x.Status.ToLower() == selected.ToLower()).ToList();

			AfsprakenLijstControl.ItemSource.Clear();
			foreach (AfspraakDTO afspraak in filtered) {
				AfsprakenLijstControl.ItemSource.Add(afspraak);
			}
		}
	}
}
