using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls {
	/// <summary>
	/// Interaction logic for AfsprakenLijstControl.xaml
	/// </summary>
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
			AfsprakenPage.Instance.GeselecteerdeBezoeker = bezoeker;
		}

		private Border _selecteditem;
		private void VeranderKleurRowOnKlik(object sender, MouseButtonEventArgs e) {
			if (_selecteditem is not null) {
				_selecteditem.Background = Brushes.Transparent;
				_selecteditem.BorderThickness = new Thickness(0);
			}
			StackPanel? listViewItem = sender as StackPanel;

			Border border = (Border)listViewItem.Children[0];
			border.Background = Brushes.White;
			border.BorderThickness = new Thickness(1);
			border.BorderBrush = Brushes.WhiteSmoke;
			border.CornerRadius = new CornerRadius(20);
			border.Margin = new Thickness(0, 0, 20, 0);
			_selecteditem = border;

			BezoekerDTO bezoeker = (BezoekerDTO)BezoekerLijst.SelectedValue;
			AfsprakenPage.Instance.GeselecteerdeBezoeker = bezoeker;
		}

		private void KlikOpBezoekerOptions(object sender, RoutedEventArgs e) {
			Button b = (Button)sender;
			BezoekerDTO bezoeker = (BezoekerDTO)b.CommandParameter;
		}
	}
}