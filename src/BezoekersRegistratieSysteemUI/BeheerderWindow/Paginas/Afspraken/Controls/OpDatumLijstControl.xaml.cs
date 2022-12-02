using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	public partial class OpDatumLijstControl : UserControl {
		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<AfspraakDTO>),
		  typeof(OpDatumLijstControl),
		  new PropertyMetadata(new ObservableCollection<AfspraakDTO>())
		 );

		public ObservableCollection<AfspraakDTO> ItemSource {
			get { return (ObservableCollection<AfspraakDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
		  nameof(SelectedItem),
		  typeof(AfspraakDTO),
		  typeof(OpDatumLijstControl),
		  new PropertyMetadata(null)
		);

		public AfspraakDTO SelectedItem {
			get { return (AfspraakDTO)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
		  nameof(SelectedIndex),
		  typeof(int),
		  typeof(OpDatumLijstControl),
		  new PropertyMetadata(-1)
		);

		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public OpDatumLijstControl() {
			this.DataContext = this;
			InitializeComponent();
			ItemSource.CollectionChanged += ItemSource_CollectionChanged;
		}

		//Auto Columns Resize on Change
		private void ItemSource_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			GridView view = AfsprakenLijst.View as GridView;
			foreach (GridViewColumn c in view.Columns) {
				if (double.IsNaN(c.Width)) {
					c.Width = c.ActualWidth;
				}
				c.Width = double.NaN;
			}
		}

		private void KlikOpAfspraakOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			AfspraakDTO afspraak = (AfspraakDTO)b.CommandParameter;
			ContextMenu.DataContext = afspraak;
			ContextMenu.IsOpen = true;
		}

		private void WijzigAfspraak_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is WerknemerDTO werknemer) {

			}
		}

		private async void VerwijderAfspraak_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is AfspraakDTO afspraak) {
				if (afspraak.EindTijd.IsNietLeeg()) return;

				CustomMessageBox warningMessage = new();
				ECustomMessageBoxResult result = warningMessage.Show("Ben je het zeker?", $"Wil je deze afspraak verwijderen", ECustomMessageBoxIcon.Warning);

				if (result == ECustomMessageBoxResult.Bevestigen) {
					await ApiController.VerwijderAfspraak(afspraak);
					int index = ItemSource.IndexOf(afspraak);
					ItemSource.RemoveAt(index);
					AfspraakEvents.InvokeVerwijderAfspraak(afspraak);
					afspraak.Status = "Verwijderd";
					ItemSource.Insert(index, afspraak);
					AfspraakDTO? updatedAfspraak = ApiController.GeefAfsprakenOpDatumVanBedrijf(BeheerderWindow.GeselecteerdBedrijf.Id).FirstOrDefault(a => a.Id == afspraak.Id);
					if (updatedAfspraak is not null) {
						index = ItemSource.IndexOf(ItemSource.First(a => a.Id == afspraak.Id));
						ItemSource.RemoveAt(index);
						ItemSource.Insert(index, afspraak);
					}
				}
			}
		}
	}
}