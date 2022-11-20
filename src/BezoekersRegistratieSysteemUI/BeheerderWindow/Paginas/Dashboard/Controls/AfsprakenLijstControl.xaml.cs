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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Dashboard.Controls {
	public partial class AfsprakenLijstControl : UserControl {
		#region Variabelen
		public ObservableCollection<AfspraakDTO> ItemSource { get; set; }
		#endregion

		public AfsprakenLijstControl() {
			ItemSource = new();
			this.DataContext = this;
			InitializeComponent();

			AfsprakenPopup.NieuweAfspraakToegevoegd += (AfspraakDTO afspraak) => {
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
			List<AfspraakDTO> afspraken = ApiController.GeefAfspraken().ToList();
			return afspraken;
		}

		private void KlikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			AfspraakDTO? afspraak = b?.CommandParameter as AfspraakDTO;

			OpenAfspraakDetail(afspraak);
		}

		private void OpenAfspraakDetail(AfspraakDTO afspraak) {

		}

		private Border _selecteditem;

		private void KlikOpRow(object sender, MouseButtonEventArgs e) {
			//Er is 2 keer geklikt
			if (e.ClickCount == 2) {
				return;
			}

			if (_selecteditem is not null) {
				_selecteditem.Background = Brushes.Transparent;
				_selecteditem.BorderThickness = new Thickness(0);
			}
			StackPanel? listViewItem = sender as StackPanel;

			Border border = (Border)listViewItem.Children[0];
			border.Background = Brushes.White;
			border.BorderThickness = new Thickness(1);
			border.BorderBrush = Brushes.WhiteSmoke;
			border.CornerRadius = new CornerRadius(20);
			border.Margin = new Thickness(0, 0, 20, 0);
			_selecteditem = border;
		}
	}
}
