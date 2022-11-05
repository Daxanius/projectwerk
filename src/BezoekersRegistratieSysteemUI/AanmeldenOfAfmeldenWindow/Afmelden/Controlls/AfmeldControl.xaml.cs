using System;
using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.Controlls {

	/// <summary>
	/// Interaction logic for InputControl.xaml
	/// </summary>
	public partial class AfmeldControl : UserControl, INotifyPropertyChanged {

		/// <summary>
		/// Email van bezoeker
		/// </summary>
		private string _email = "TestEMAIL@GMAIL.BE";

		public string Email {
			get {
				return _email;
			}
			set {
				_email = value;
				UpdatePropperty();
			}
		}

		/// <summary>
		/// Dit weten jullie wel he :-)
		/// </summary>
		public AfmeldControl() {
			//Om data met de xaml te kunnen binden
			this.DataContext = this;
			InitializeComponent();
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void UpdatePropperty([CallerMemberName] string propertyName = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion ProppertyChanged

		/// <summary>
		/// De bezoeker klikt op afmelden
		/// </summary>
		/// <param name="sender">Button info</param>
		/// <param name="e">Click info</param>
		private void GaVerderButtonClickEvent(object sender, RoutedEventArgs e) {
			try {
				Email = Email.Trim();
				new MailAddress(Email);
				MeldBezoekerAf(Email);
				ToonPopupBezoekerAfgemeld();
			} catch (Exception) {
				MessageBox.Show("Email is niet in een juist formaat", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		/// <summary>
		/// HTTP POST request om een bezoeker al te melden
		/// </summary>
		/// <param name="email">Email van bezoeker</param>
		private void MeldBezoekerAf(string email) {
			//Var body = {
			//	“email”: “”,
			//}
			//[POST] /api/afspraak/end
		}

		/// <summary>
		/// De bezoeker is afgemeld en er wordt een popup getoont aan de bezoeker
		/// </summary>
		private async void ToonPopupBezoekerAfgemeld() {
			popupAfgemeld.IsOpen = true;
			afmeldControl.Opacity = .2;
			afmeldControl.IsHitTestVisible = false;

			await Task.Delay(1500);

			popupAfgemeld.IsOpen = false;

			afmeldControl.Opacity = 1;
			afmeldControl.IsHitTestVisible = true;

			GaTerug(null, null);
		}

		/// <summary>
		/// De bezoeker wil een terug naar het vorige scherm
		/// </summary>
		/// <param name="sender">Button info</param>
		/// <param name="e">Click info</param>
		private void GaTerug(object sender, MouseButtonEventArgs e) {
			Email = string.Empty;

			Window window = Window.GetWindow(this);
			AanOfUitMeldenScherm aanOfUitMeldenScherm = window.DataContext as AanOfUitMeldenScherm;
			aanOfUitMeldenScherm.ResetSchermNaarStart();
		}
	}
}