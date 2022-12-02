using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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
			BedrijfEvents.BedrijfGeupdate += BedrijfGeUpdate_Event;
			ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		private void BedrijfGeUpdate_Event(BedrijfDTO bedrijf) {
			if (bedrijf is null) return;
			var bedrijfInLijst = ItemSource.FirstOrDefault(b => b.Id == bedrijf.Id);
			int index = ItemSource.IndexOf(bedrijfInLijst!);
			if (index >= 0) {
				ItemSource.RemoveAt(index);
				ItemSource.Insert(index, bedrijf);
			}
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
				BedrijvenPage.Instance.bedrijfUpdatenPopup.Visibility = Visibility.Visible;
				BedrijvenPage.Instance.bedrijfUpdatenPopup.ZetBedrijf(bedrijf);
			}
		}

		private async void VerwijderBedrijf_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is BedrijfDTO bedrijf) {
				CustomMessageBox warningMessage = new();
				ECustomMessageBoxResult result = warningMessage.Show("Ben je het zeker?", $"Wil je {bedrijf.Naam} verwijderen", ECustomMessageBoxIcon.Warning);

				if (result == ECustomMessageBoxResult.Bevestigen) {
					await ApiController.VerwijderBedrijf(bedrijf.Id);
					ItemSource.Remove(bedrijf);
					BedrijfEvents.InvokeBedrijfVerwijderd(bedrijf);
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
