using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.MessageBoxes;
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
		private CustomMessageBox _mb = new();

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
					_mb = new();
					_mb.Show("Email is niet geldig!", "Error", ECustomMessageBoxIcon.Error);
					return;
				}

				if (Email.IsLeeg()) {
					_mb = new();
					_mb.Show("Email is leeg!", "Error", ECustomMessageBoxIcon.Error);
					return;
				}

				await ApiController.Put<object>($"/afspraak/end?email={Email}");

				Email = "";

				_mb = new();
				_mb.Show("U bent afgemeld", "Success", ECustomMessageBoxIcon.Information);

				await Task.Delay(TimeSpan.FromSeconds(2));

			} catch (Exception ex) {
				_mb = new();
				if (ex.Message.Contains("NotFound")) {
					_mb.Show("Er is geen afspraak voor dit email adres", "Error", ECustomMessageBoxIcon.Error);
					return;
				}
				_mb.Show(ex.Message, "Error", ECustomMessageBoxIcon.Error);
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
