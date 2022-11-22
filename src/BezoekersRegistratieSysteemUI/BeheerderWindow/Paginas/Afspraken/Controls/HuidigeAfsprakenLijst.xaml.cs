using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.MessageBoxes;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	public partial class HuidigeAfsprakenLijst : UserControl {
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

		public HuidigeAfsprakenLijst() {
			this.DataContext = this;
			InitializeComponent();
			AfspraakEvents.VerwijderAfspraak += VerwijderAfspraak_Event;
		}

		private void VerwijderAfspraak_Event(AfspraakDTO afspraak) {
			if (ItemSource.Where(_afspraak => _afspraak.Id == _afspraak.Id).Count() > 0) {
				ItemSource.Remove(afspraak);
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
					ItemSource.Remove(afspraak);
					AfspraakEvents.InvokeVerwijderAfspraak(afspraak);
				}
			}
		}
	}
}