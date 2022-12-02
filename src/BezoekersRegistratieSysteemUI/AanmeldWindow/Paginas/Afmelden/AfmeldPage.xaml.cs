using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Afmelden {
	public partial class AfmeldPage : Page, INotifyPropertyChanged {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Variabelen

		private string _email;

		public string Email {
			get { return _email; }

			set {
				if (value == _email) return;
				_email = value.Trim();
				UpdatePropperty();
			}
		}

		#endregion

		public AfmeldPage() {

			double schermResolutieHeight = System.Windows.SystemParameters.MaximizedPrimaryScreenHeight;
			double schermResolutieWidth = System.Windows.SystemParameters.MaximizedPrimaryScreenWidth;

			double defaultResHeight = 1080.0;
			double defaultResWidth = 1920.0;

			double aspectratio = schermResolutieWidth / schermResolutieHeight;
			double change = aspectratio > 2 ? 0.3 : aspectratio > 1.5 ? 0 : aspectratio > 1 ? -0.05 : -0.3;

			ScaleX = (schermResolutieWidth / defaultResWidth);
			ScaleY = (schermResolutieHeight / defaultResHeight) + change;

			this.DataContext = this;
			InitializeComponent();
		}

		#region Functies
		private async void AfmeldenClick(object sender, RoutedEventArgs e) {
			try {
				if (!Email.IsEmailGeldig()) {
					MessageBox.Show("Email is niet geldig!", "Error");
					return;
				}

				if (Email.IsLeeg()) {
					MessageBox.Show("Email is leeg!", "Error");
					return;
				}

				await ApiController.Put<object>($"/afspraak/end?email={Email}");

				Email = "";

				MessageBox.Show("U bent afgemeld", "", MessageBoxButton.OK, MessageBoxImage.Information);

				await Task.Delay(TimeSpan.FromSeconds(2));

			} catch (Exception ex) {
				if (ex.Message.Contains("NotFound")) {
					MessageBox.Show("Er is geen afspraak voor dit email adres");
					return;
				}
				MessageBox.Show(ex.Message, "Error");
			}
		}
		#endregion

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged

		#region Singleton
		private static AfmeldPage instance = null;
		private static readonly object padlock = new();

		public static AfmeldPage Instance {
			get {
				lock (padlock) {
					instance ??= new AfmeldPage();
					return instance;
				}
			}
		}
		#endregion
	}
}
