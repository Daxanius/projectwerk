using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	public partial class BezoekersAfsprakenLijst : UserControl {
		#region Variabelen
		public bool HeeftData { get; set; }
		private Border _selecteditem;

		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<AfspraakDTO>),
		  typeof(BezoekersAfsprakenLijst),
		  new PropertyMetadata(new ObservableCollection<AfspraakDTO>())
		 );

		public ObservableCollection<AfspraakDTO> ItemSource {
			get { return (ObservableCollection<AfspraakDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
		  nameof(AfspraakDTO),
		  typeof(AfspraakDTO),
		  typeof(BezoekersAfsprakenLijst),
		  new PropertyMetadata(null)
		);

		public AfspraakDTO SelectedItem {
			get { return (AfspraakDTO)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
		  nameof(SelectedIndex),
		  typeof(int),
		  typeof(BezoekersAfsprakenLijst),
		  new PropertyMetadata(-1)
		);

		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}
		#endregion

		public BezoekersAfsprakenLijst() {
			this.DataContext = this;
			InitializeComponent();

			AfspraakEvents.VerwijderAfspraak += VerwijderAfspraak_Event;
		}

		#region Functies
		private void VerwijderAfspraak_Event(AfspraakDTO afspraak) {
			if (ItemSource.Contains(afspraak)) {
				ItemSource.Remove(afspraak);
			}
		}
		#endregion
	}
}