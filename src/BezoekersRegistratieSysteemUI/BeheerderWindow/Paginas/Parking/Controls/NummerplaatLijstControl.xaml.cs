using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
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
		  typeof(ObservableCollection<ParkeerplaatsDTO>),
		  typeof(NummerplaatLijstControl),
		  new PropertyMetadata(new ObservableCollection<ParkeerplaatsDTO>())
		);

		public ObservableCollection<ParkeerplaatsDTO> ItemSource {
			get => (ObservableCollection<ParkeerplaatsDTO>)GetValue(ItemSourceProperty);
			set => SetValue(ItemSourceProperty, value);
		}

		public NummerplaatLijstControl() {
			this.DataContext = this;
			InitializeComponent();

			ParkingEvents.NieuweNummerplaatInGeChecked += NieuweNummerplaatInGeChecked_Event;
			ParkingEvents.NummerplaatUitChecken += NummerplaatUitChecken_Event;

			//Kijk of je kan rechts klikken om iets te doen
			NummerplaatLijst.ContextMenuOpening += (sender, args) => args.Handled = true;
			ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		private void NummerplaatUitChecken_Event(ParkeerplaatsDTO parkeerplaats) {
			int index = ItemSource.IndexOf(parkeerplaats);
			if (index > -1) {
				ItemSource.RemoveAt(index);
			}
		}

		private void NieuweNummerplaatInGeChecked_Event(ParkeerplaatsDTO parkeerplaats) {
			if (parkeerplaats.Eindtijd is null)
				ItemSource.Add(parkeerplaats);
		}

		private protected void KlikOpParkeerplaatsOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			ParkeerplaatsDTO parkeerplaats = (ParkeerplaatsDTO)b.CommandParameter;

			int index = ItemSource.IndexOf(parkeerplaats);
			NummerplaatLijst.SelectedIndex = index;

			ContextMenu.DataContext = parkeerplaats;
			ContextMenu.IsOpen = true;
		}

		private void VerwijderNummerPlaat_Click(object sender, RoutedEventArgs e) {
			ParkeerplaatsDTO parkeerplaats = ((MenuItem)sender).DataContext as ParkeerplaatsDTO;
			ApiController.CheckNummerplaatOut(parkeerplaats);
			ParkingEvents.ParkeerplaatsUitChecken(parkeerplaats);
		}

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged
	}
}
