using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	public partial class BezoekersLijstControl : UserControl {
		public bool HeeftData { get; set; }
		public BezoekerDTO GeselecteerdeBezoeker;

		public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
		  nameof(ItemSource),
		  typeof(ObservableCollection<BezoekerDTO>),
		  typeof(BezoekersLijstControl),
		  new PropertyMetadata(new ObservableCollection<BezoekerDTO>())
		 );

		public ObservableCollection<BezoekerDTO> ItemSource {
			get { return (ObservableCollection<BezoekerDTO>)GetValue(ItemSourceProperty); }
			set { SetValue(ItemSourceProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
		  nameof(SelectedItem),
		  typeof(BezoekerDTO),
		  typeof(BezoekersLijstControl),
		  new PropertyMetadata(null)
		);

		public BezoekerDTO SelectedItem {
			get { return (BezoekerDTO)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
		  nameof(SelectedIndex),
		  typeof(int),
		  typeof(BezoekersLijstControl),
		  new PropertyMetadata(-1)
		);

		public int SelectedIndex {
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public BezoekersLijstControl() {
			this.DataContext = this;
			InitializeComponent();
		}

		private void KlikOpActionButtonOpRow(object sender, RoutedEventArgs e) {
			Button? b = sender as Button;
			BezoekerDTO? bezoeker = b?.CommandParameter as BezoekerDTO;

			OpenBezoekerDetail(bezoeker);
		}

		private void OpenBezoekerDetail(BezoekerDTO bezoeker) {

		}

		public void SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (SelectedItem is null) return;
			BezoekerDTO bezoeker = SelectedItem;
			//AfsprakenPage.Instance.GeselecteerdeBezoeker = bezoeker;
		}

		private void KlikOpBezoekerOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			BezoekerDTO bezoeker = (BezoekerDTO)b.CommandParameter;
		}
	}
}