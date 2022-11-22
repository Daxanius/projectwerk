using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	public partial class WerknemersLijstControl : UserControl {
		public bool HeeftData { get; set; }

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

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
		  nameof(SelectedItem),
		  typeof(WerknemerDTO),
		  typeof(WerknemersLijstControl),
		  new PropertyMetadata(null)
		);

		public WerknemerDTO SelectedItem {
			get { return (WerknemerDTO)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
		  nameof(SelectedIndex),
		  typeof(int),
		  typeof(WerknemersLijstControl),
		  new PropertyMetadata(-1)
		);

		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public WerknemersLijstControl() {
			this.DataContext = this;
			InitializeComponent();

			//Kijk of je kan rechts klikken om iets te doen
			WerknemerLijst.ContextMenuOpening += (sender, args) => args.Handled = true;

			WerknemerEvents.VerwijderWerknemerEvent += VerwijderWerknemer_Event;
		}
		private void VerwijderWerknemer_Event(WerknemerDTO werknemer) {
			if (this.ItemSource.Contains(werknemer)) {
				this.ItemSource.Remove(werknemer);
			}
		}

		private void KlikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			WerknemerDTO? werknemer = b?.CommandParameter as WerknemerDTO;
		}

		public void SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (SelectedItem is null) return;
			WerknemerDTO werknemer = SelectedItem;
			AfsprakenPage.Instance.GeselecteerdeWerknemer = werknemer;
		}
	}
}
