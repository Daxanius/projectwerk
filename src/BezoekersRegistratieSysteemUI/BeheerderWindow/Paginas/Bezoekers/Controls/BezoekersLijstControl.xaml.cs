using BezoekersRegistratieSysteemUI.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Bezoekers.Controls {
	public partial class BezoekersLijstControl : UserControl {
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

		public BezoekersLijstControl() {
			this.DataContext = this;
			InitializeComponent();
		}
	}
}
