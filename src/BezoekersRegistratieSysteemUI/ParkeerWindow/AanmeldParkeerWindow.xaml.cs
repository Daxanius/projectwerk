using BezoekersRegistratieSysteemUI.Model;
using System.Windows;

namespace BezoekersRegistratieSysteemUI.ParkeerWindow {
	/// <summary>
	/// Interaction logic for RegistratieWindow.xaml
	/// </summary>
	public partial class AanmeldParkeerWindow : Window {
		#region Scaling
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		#endregion

		#region Variabelen
		public static BedrijfDTO GeselecteerdBedrijf { get; set; }
		#endregion

		public AanmeldParkeerWindow() {

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

		#region Singleton
		private static AanmeldParkeerWindow instance = null;
		private static readonly object padlock = new object();

		public static AanmeldParkeerWindow Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new AanmeldParkeerWindow();
					}
					return instance;
				}
			}
		}
		#endregion
	}
}
