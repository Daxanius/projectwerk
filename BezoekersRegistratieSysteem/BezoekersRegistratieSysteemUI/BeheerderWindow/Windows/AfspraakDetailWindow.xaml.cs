using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
using System.Windows;

namespace BezoekersRegistratieSysteemUI.BeheerderWindow.Controlls.DetailControls {

	/// <summary>
	/// Interaction logic for AfspraakDetailControl.xaml
	/// </summary>
	public partial class AfspraakDetailWindow : Window {
		public AfspraakDTO Afspraak;

		public AfspraakDetailWindow(AfspraakDTO afspraak) {
			Afspraak = afspraak;
			this.DataContext = this;
			InitializeComponent();
		}

		private void WijzigBezoeker(object sender, RoutedEventArgs e) {
		}

		private void WijzigWerknemer(object sender, RoutedEventArgs e) {
		}

		private void WijzigTijdstip(object sender, RoutedEventArgs e) {
		}

		private void VerwijderAfspraak(object sender, RoutedEventArgs e) {
		}
	}
}