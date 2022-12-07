using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Popups;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	public partial class HuidigeAfsprakenLijst : UserControl {
		#region Variabelen
		public bool HeeftData { get; set; }

		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<AfspraakDTO>),
		  typeof(HuidigeAfsprakenLijst),
		  new PropertyMetadata(new ObservableCollection<AfspraakDTO>())
		 );

		public ObservableCollection<AfspraakDTO> ItemSource {
			get { return (ObservableCollection<AfspraakDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
		  nameof(SelectedItem),
		  typeof(AfspraakDTO),
		  typeof(HuidigeAfsprakenLijst),
		  new PropertyMetadata(null)
		);

		public AfspraakDTO SelectedItem {
			get { return (AfspraakDTO)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
		  nameof(SelectedIndex),
		  typeof(int),
		  typeof(HuidigeAfsprakenLijst),
		  new PropertyMetadata(-1)
		);

		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}
		#endregion

		public HuidigeAfsprakenLijst() {
			this.DataContext = this;
			InitializeComponent();
			AfspraakEvents.VerwijderAfspraak += VerwijderAfspraak_Event;
			AfspraakEvents.UpdateAfspraak += AfspraakGewijzig_Event;
		}

		#region Functies
		private void KlikOpAfspraakOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			AfspraakDTO afspraak = (AfspraakDTO)b.CommandParameter;
			ContextMenu.DataContext = afspraak;
			ContextMenu.IsOpen = true;
		}
		private void WijzigAfspraak_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is AfspraakDTO afspraak && afspraak is not null) {
				AfsprakenPage.Instance.updateAfsprakenPopup.Visibility = Visibility.Visible;
				AfsprakenPage.Instance.updateAfsprakenPopup.ZetAfspraak(afspraak);
			}
		}
		private void AfspraakGewijzig_Event(AfspraakDTO afspraak) {
			if (afspraak is null) return;
			int index = ItemSource.IndexOf(ItemSource.FirstOrDefault(a => a.Id == afspraak.Id));
			if (index > -1) {
				ItemSource.RemoveAt(index);
				ItemSource.Insert(index, afspraak);
			}
		}
		private async void VerwijderAfspraak_Click(object sender, RoutedEventArgs e) {
			if (ContextMenu.DataContext is AfspraakDTO afspraak) {
				if (afspraak.EindTijd.IsNietLeeg() || afspraak.Status.ToLower() == "verwijderd") return;

				CustomMessageBox warningMessage = new();
				ECustomMessageBoxResult result = warningMessage.Show("Ben je het zeker?", $"Wil je deze afspraak verwijderen", ECustomMessageBoxIcon.Warning);

				if (result == ECustomMessageBoxResult.Bevestigen) {
					await ApiController.VerwijderAfspraak(afspraak);
					ItemSource.Remove(afspraak);
					AfspraakEvents.InvokeVerwijderAfspraak(afspraak);
				}
			}
		}
		private void VerwijderAfspraak_Event(AfspraakDTO afspraak) {
			if (ItemSource.Where(_afspraak => _afspraak.Id == _afspraak.Id).Count() > 0) {
				ItemSource.Remove(afspraak);
			}
		}
		#endregion
	}
}