using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
