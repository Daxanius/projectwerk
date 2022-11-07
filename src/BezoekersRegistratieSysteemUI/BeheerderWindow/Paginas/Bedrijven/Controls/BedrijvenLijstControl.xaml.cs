using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.DTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls {
	/// <summary>
	/// Interaction logic for BedrijvenLijstControl.xaml
	/// </summary>
	public partial class BedrijvenLijstControl : UserControl {
		#region Bedrijven
		public List<BedrijfDTO> _bedrijven;
		#endregion

		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<BedrijfDTO>),
		  typeof(BedrijvenLijstControl),
		  new PropertyMetadata(new ObservableCollection<BedrijfDTO>()));

		public ObservableCollection<BedrijfDTO> ItemSource {
			get { return (ObservableCollection<BedrijfDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		public BedrijvenLijstControl() {
			this.DataContext = this;
			InitializeComponent();
		}

		#region Action Button

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

		private void KlikOpBedrijfOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			BedrijfDTO bedrijf = (BedrijfDTO)b.CommandParameter;
		}

		#endregion

		#region API Requests
		private async void FetchAlleBedrijven() {
			_bedrijven = new();
			(bool isvalid, List<ApiBedrijfIN> bedrijven) = await ApiController.Get<List<ApiBedrijfIN>>("/bedrijf");

			if (isvalid) {
				bedrijven.ForEach((api) => {
					_bedrijven.Add(new BedrijfDTO(api.Id, api.Naam, api.Btw, api.TelefoonNummer, api.Email, api.Adres));
				});
			} else {
				MessageBox.Show("Er is iets fout gegaan bij het ophalen van de bedrijven", "Error /bedrijf");
			}
			BedrijvenGrid.ItemsSource = _bedrijven;
		}
		#endregion
	}
}
