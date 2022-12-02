using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Popups {
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class WerknemerBestaatPopup : UserControl {
		public WerknemerBestaatPopup() {
			InitializeComponent();
		}

		private void AnnulerenButton_Click(object sender, RoutedEventArgs e) {
			SluitOverlay();
		}

		private void BevestigenButton_Click(object sender, RoutedEventArgs e) {
		}

		private void SluitOverlay() {
		}
	}
}
