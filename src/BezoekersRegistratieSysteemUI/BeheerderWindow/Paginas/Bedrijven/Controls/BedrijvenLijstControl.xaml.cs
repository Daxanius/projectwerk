using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Api.DTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls {
	/// <summary>
	/// Interaction logic for BedrijvenLijstControl.xaml
	/// </summary>
	public partial class BedrijvenLijstControl : UserControl, INotifyPropertyChanged {
		bool isLijstOpgevult = false;
		public BedrijvenLijstControl() {
			this.DataContext = this;
			InitializeComponent();
			BedrijvenGrid.ItemsSource = new List<BedrijfDTO>();
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

		private void UserControl_Loaded(object sender, RoutedEventArgs e) {
			if (isLijstOpgevult) return;


			Task.Run(() => {
				Dispatcher.Invoke(() => BedrijvenGrid.ItemsSource = (List<BedrijfDTO>)BedrijvenGrid.DataContext);
				UpdatePropperty(nameof(BedrijvenGrid.ItemsSource));
			});

			isLijstOpgevult = true;
		}

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}
