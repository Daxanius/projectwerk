using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bedrijven.Controls {
	public partial class BedrijvenLijstControl : UserControl, INotifyPropertyChanged {
		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<BedrijfDTO>),
		  typeof(BedrijvenLijstControl),
		  new PropertyMetadata(new ObservableCollection<BedrijfDTO>())
		);

		public ObservableCollection<BedrijfDTO> ItemSource {
			get => (ObservableCollection<BedrijfDTO>)GetValue(ItemSourceProperty);
			set => SetValue(ItemSourceProperty, value);
		}

		public BedrijvenLijstControl() {
			this.DataContext = this;
			InitializeComponent();

			//Kijk of je kan rechts klikken om iets te doen
			BedrijvenLijst.ContextMenuOpening += (sender, args) => args.Handled = true;

			ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		private protected void KlikOpBedrijfOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			BedrijfDTO bedrijf = (BedrijfDTO)b.CommandParameter;

			int index = ItemSource.IndexOf(bedrijf);
			BedrijvenLijst.SelectedIndex = index;

			if (BeheerderWindow.GeselecteerdBedrijf is not null && BeheerderWindow.GeselecteerdBedrijf.Equals(bedrijf))
				VerwijderMenuItem.Visibility = Visibility.Collapsed;
			else
				VerwijderMenuItem.Visibility = Visibility.Visible;

			ContextMenu.DataContext = bedrijf;
			ContextMenu.IsOpen = true;
		}

		private void WijzigBedrijf_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is BedrijfDTO bedrijf) {
				MessageBox.Show("Ik heb een design nodig weude");
			}
		}

		private async void VerwijderBedrijf_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is BedrijfDTO bedrijf) {
				await ApiController.VerwijderBedrijf(bedrijf.Id);
				ItemSource.Remove(bedrijf); 
			}
		}

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}
