﻿using BezoekersRegistratieSysteemREST.Model.Output;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Controls {
	/// <summary>
	/// Interaction logic for BedrijvenLijstControl.xaml
	/// </summary>
	public partial class WerknemersLijstControl : UserControl {
		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<WerknemerDTO>),
		  typeof(WerknemersLijstControl),
		  new PropertyMetadata(new ObservableCollection<WerknemerDTO>())
		);

		public ObservableCollection<WerknemerDTO> ItemSource {
			get { return (ObservableCollection<WerknemerDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}
		public WerknemersLijstControl() {
			this.DataContext = this;
			InitializeComponent();
			FetchAlleWerknemers();
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
			StackPanel listViewItem = (StackPanel)sender;

			Border border = (Border)listViewItem.Children[0];
			border.Background = Brushes.White;
			border.BorderThickness = new Thickness(1);
			border.BorderBrush = Brushes.WhiteSmoke;
			border.CornerRadius = new CornerRadius(20);
			border.Margin = new Thickness(0, 0, 20, 0);
			_selecteditem = border;
		}

		private void KlikOpWerknemerOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			WerknemerDTO werknemer = (WerknemerDTO)b.CommandParameter;
		}

		#region API Requests
		private async void FetchAlleWerknemers() {
			(bool isvalid, List<WerknemerOutputDTO> apiWerknemers) = await ApiController.Get<List<WerknemerOutputDTO>>("werknemer/bedrijf/id/" + BeheerderWindow.GeselecteerdBedrijf.Id);
			if (isvalid) {
				apiWerknemers.ForEach((api) => {
					List<WerknemerInfoDTO> lijstWerknemerInfo = new(api.WerknemerInfo.Select(w => new WerknemerInfoDTO(BeheerderWindow.GeselecteerdBedrijf, w.Email, w.Functies)).ToList());
					WerknemerInfoOutputDTO werknemerInfo = api.WerknemerInfo.First(w => w.BedrijfId == BeheerderWindow.GeselecteerdBedrijf.Id);
					ItemSource.Add(new WerknemerDTO(api.Id, api.Voornaam, api.Achternaam, werknemerInfo.Email, werknemerInfo.Functies, true));
				});
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het ophalen van de bedrijven", "Error /bedrijf");
			}
		}
		#endregion
	}
}
