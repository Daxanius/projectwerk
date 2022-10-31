using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Paginas.Bedrijven.Controls {
	/// <summary>
	/// Interaction logic for BedrijvenLijstControl.xaml
	/// </summary>
	public partial class BedrijvenLijstControl : UserControl {
		#region Bedrijven
		public ObservableCollection<BedrijfDTO> Bedrijven { get; set; }
		#endregion

		public BedrijvenLijstControl() {
			Bedrijven = new() {
			new BedrijfDTO(1, "Hogent", "BE0676747521", "04926349246", "Hogent@gmail.com", "Kerkstraat 101", null),
			new BedrijfDTO(2, "Odice", "BE0676747521", "04926349246", "Odice@gmail.com", "Kerkstraat 102", null),
			new BedrijfDTO(3, "Artevelde", "BE0676747521", "04926349246", "Artevelde@gmail.com", "Kerkstraat 103", null),
			new BedrijfDTO(4, "Allphi", "BE0676747521", "04926349246", "Allphi@gmail.com", "Kerkstraat 104", null),
			new BedrijfDTO(5, "Scheppers", "BE0676747521", "04926349246", "Scheppers@gmail.com", "Kerkstraat 105", null),
			new BedrijfDTO(6, "Artevelde", "BE0676747521", "04926349246", "Artevelde@gmail.com", "Kerkstraat 106", null),
			new BedrijfDTO(7, "De Bolster", "BE0676747521", "04926349246", "DeBolster@gmail.com", "Kerkstraat 108", null),
			new BedrijfDTO(8, "Brauzz", "BE0676747521", "04926349246", "Brauzz@gmail.com", "Kerkstraat 111", null),
			new BedrijfDTO(9, "Apple", "BE0676747521", "04926349246", "Apple@gmail.com", "Kerkstraat 121", null),
			};

			this.DataContext = this;
			InitializeComponent();
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

		private void KlikOpBedrijfOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			BedrijfDTO bedrijf = (BedrijfDTO)b.CommandParameter;
		}
	}
}
