using BezoekersRegistratieSysteemUI.Model;
using System.Windows;

namespace BezoekersRegistratieSysteemUI.AanmeldWindow {
	/// <summary>
	/// Interaction logic for RegistratieWindow.xaml
	/// </summary>
	public partial class RegistratieWindow : Window {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Variabelen
		public static BedrijfDTO GeselecteerdBedrijf { get; set; }
		#endregion

		public RegistratieWindow() {

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
	}
}
