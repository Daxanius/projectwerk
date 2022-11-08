using BezoekersRegistratieSysteemUI.Api.Output;
using BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Aanmelden;
using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.BeheerderWindowDTO;
using BezoekersRegistratieSysteemUI.icons.IconsPresenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteemUI.AanmeldWindow.Paginas.Afmelden {
	/// <summary>
	/// Interaction logic for KiesBedrijfPage.xaml
	/// </summary>
	public partial class AfmeldPage : Page, INotifyPropertyChanged {
		private static AfmeldPage instance = null;
		private static readonly object padlock = new object();

		public static AfmeldPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new AfmeldPage();
					}
					return instance;
				}
			}
		}

		/// <summary>
		/// ///////////////////////////////////////////////////
		/// </summary>
		/// 

		#region Binding Propperties

		public event PropertyChangedEventHandler? PropertyChanged;

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

		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
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

		#region ProppertyChanged
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion ProppertyChanged

		private async void AfmeldenClick(object sender, RoutedEventArgs e) {
			try {
				if (Email is null) return;
				string email = Email.Trim();

				Regex regexEmail = new(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);

				if (!regexEmail.IsMatch(email)) {
					MessageBox.Show("Email is niet geldig!", "Error");
					return;
				}

				await ApiController.Put<object>($"/afspraak/end?email={email}");

				Email = string.Empty;

				MessageBox.Show("U bent afgemeld", "Joepi");

				await Task.Delay(TimeSpan.FromSeconds(2));
				((RegistratieWindow)((RegistratieWindow)Window.GetWindow(this)).DataContext).FrameControl.Content = KiesBedrijfPage.Instance;
				((RegistratieWindow)((RegistratieWindow)Window.GetWindow(this)).DataContext).SideBar.AanmeldenTab.Tag = "Selected";
				((RegistratieWindow)((RegistratieWindow)Window.GetWindow(this)).DataContext).SideBar.AfmeldenTab.Tag = "UnSelected";
			} catch (Exception ex) {
				if (ex.Message.Contains("NotFound")) {
					MessageBox.Show("Er is geen afspraak voor dit email adres");
					return;
				}
				MessageBox.Show(ex.Message, "Error");
			}
		}
	}
}
