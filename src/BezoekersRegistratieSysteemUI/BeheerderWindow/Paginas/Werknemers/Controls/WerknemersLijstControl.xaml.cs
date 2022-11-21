using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Controls {
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

			//Kijk of je kan rechts klikken om iets te doen
			WerknemerLijst.ContextMenuOpening += (sender, args) => args.Handled = true;
			ContextMenu.ContextMenuClosing += (object sender, ContextMenuEventArgs e) => ContextMenu.DataContext = null;
		}

		private void KlikOpWerknemerOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			WerknemerDTO werknemer = (WerknemerDTO)b.CommandParameter;
			int index = ItemSource.IndexOf(werknemer);
			WerknemerLijst.SelectedIndex = index;
			ContextMenu.DataContext = werknemer;
			ContextMenu.IsOpen = true;
		}

		private void WijzigWerknemer_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is WerknemerDTO werknemer) {

			}
		}

		private async void VerwijderWerknemer_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is WerknemerDTO werknemer && werknemer.Id.HasValue) {
				await ApiController.VerwijderWerknemerVanBedrijf(werknemer.Id.Value);
				ItemSource.Remove(werknemer);
				WerknemerEvents.InvokeVerwijderWerknemer(werknemer);
			}
		}
	}
}
