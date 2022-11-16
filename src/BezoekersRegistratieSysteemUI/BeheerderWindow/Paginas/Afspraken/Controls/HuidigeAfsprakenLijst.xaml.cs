using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System.Collections.ObjectModel;
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
		}

		private void KlikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			AfspraakDTO? afspraak = b?.CommandParameter as AfspraakDTO;

			OpenAfspraakDetail(afspraak);
		}

		private void OpenAfspraakDetail(AfspraakDTO afspraak) {

		}

		private void KlikOpAfspraakOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			AfspraakDTO afspraak = (AfspraakDTO)b.CommandParameter;
		}
	}
}