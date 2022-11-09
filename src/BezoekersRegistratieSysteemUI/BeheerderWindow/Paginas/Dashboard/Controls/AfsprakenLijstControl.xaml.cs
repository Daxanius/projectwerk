using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
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
	/// <summary>
	/// Interaction logic for AfsprakenLijstControl.xaml
	/// </summary>
	public partial class AfsprakenLijstControl : UserControl {
		#region Afspraken
		public ObservableCollection<AfspraakDTO> Afspraken { get; set; }
		#endregion

		public AfsprakenLijstControl() {

			Afspraken = new();
			this.DataContext = this;
			InitializeComponent();

			AfsprakenPopup.NieuweAfspraakToegevoegd += (AfspraakDTO afspraak) => {
				Task.Run(() => {
					Dispatcher.Invoke(() => {
						Afspraken.Add(afspraak);
						List<AfspraakDTO> afspraken = Afspraken.ToList();
						Afspraken.Clear();
						afspraken.OrderByDescending(a => a.StartTijd).ThenByDescending(a => a.Bezoeker.Voornaam).ToList().ForEach(a => Afspraken.Add(a));
					});
				});
			};

			FetchAlleAfspraken();
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

		#region API Requests
		private async void FetchAlleAfspraken() {
			(bool isvalid, List<AfspraakOutputDTO> apiAfspraken) = await ApiController.Get<List<AfspraakOutputDTO>>("afspraak?dag=" + DateTime.Now.ToString("MM/dd/yyy"));
			if (isvalid) {
				apiAfspraken.ForEach((api) => {
					WerknemerDTO werknemer = new WerknemerDTO(api.Werknemer.Id, api.Werknemer.Naam.Split(";")[0], api.Werknemer.Naam.Split(";")[1], null);
					BezoekerDTO bezoeker = new BezoekerDTO(api.Bezoeker.Id, api.Bezoeker.Naam.Split(";")[0], api.Bezoeker.Naam.Split(";")[1], api.Bezoeker.Email, api.Bezoeker.BezoekerBedrijf);
					Afspraken.Add(new AfspraakDTO(api.Id, bezoeker, api.Bedrijf.Naam, werknemer, api.Starttijd, api.Eindtijd));
				});
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het ophalen van de afspraaken", "Error /afspraak");
			}
		}
		#endregion
	}
}
