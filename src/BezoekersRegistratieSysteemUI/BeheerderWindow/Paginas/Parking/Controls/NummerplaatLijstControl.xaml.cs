using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking.Controls {
	public partial class NummerplaatLijstControl : UserControl, INotifyPropertyChanged {
		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<BedrijfDTO>),
		  typeof(NummerplaatLijstControl),
		  new PropertyMetadata(new ObservableCollection<BedrijfDTO>())
		);

		public ObservableCollection<ParkeerplaatsDTO> ItemSource {
			get => (ObservableCollection<ParkeerplaatsDTO>)GetValue(ItemSourceProperty);
			set => SetValue(ItemSourceProperty, value);
		}

		public NummerplaatLijstControl() {
			this.DataContext = this;
			InitializeComponent();

			//Kijk of je kan rechts klikken om iets te doen
			NummerplaatLijst.ContextMenuOpening += (sender, args) => args.Handled = true;
			ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		private protected void KlikOpParkeerplaatsOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			ParkeerplaatsDTO parkeerplaats = (ParkeerplaatsDTO)b.CommandParameter;

			int index = ItemSource.IndexOf(parkeerplaats);
			NummerplaatLijst.SelectedIndex = index;

			ContextMenu.DataContext = parkeerplaats;
			ContextMenu.IsOpen = true;
		}

		private void WijzigBedrijf_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is ParkeerplaatsDTO parkeerplaats) {
				//ParkingPage.Instance.bedrijfUpdatenPopup.Visibility = Visibility.Visible;
				//ParkingPage.Instance.bedrijfUpdatenPopup.ZetBedrijf(bedrijf);
			}
		}

		private async void VerwijderBedrijf_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is ParkeerplaatsDTO parkeerplaats) {
				CustomMessageBox warningMessage = new();
				ECustomMessageBoxResult result = warningMessage.Show("Ben je het zeker?", $"Wil je {parkeerplaats.Nummerplaat} verwijderen", ECustomMessageBoxIcon.Warning);

				if (result == ECustomMessageBoxResult.Bevestigen) {
					//await ApiController.VerwijderBedrijf(bedrijf.Id);
					//ItemSource.Remove(bedrijf);
					//BedrijfEvents.InvokeBedrijfVerwijderd(bedrijf);
				}
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
