using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteem.UI.Controlls
{
	/// <summary>
	/// Interaction logic for AanOfUitMeldenScherm.xaml
	/// </summary>
	public partial class AanOfUitMeldenScherm : Window
	{
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }

		public AanOfUitMeldenScherm()
		{
			double schermResolutieHeight = System.Windows.SystemParameters.MaximizedPrimaryScreenHeight;
			double schermResolutieWidth = System.Windows.SystemParameters.MaximizedPrimaryScreenWidth;

			double defaultResHeight = 1080.0;
			double defaultResWidth = 1920.0;

			ScaleX = (schermResolutieWidth / defaultResWidth);
			ScaleY = (schermResolutieHeight / defaultResHeight);

			this.DataContext = this;
			InitializeComponent();
		}

		private void NavigeerNaarAfmeldScherm(object sender, RoutedEventArgs e)
		{
			afmeldenControl.Visibility = Visibility.Visible;
			aanmeldingPanel.Visibility = Visibility.Collapsed;
			kiesBedrijfControl.Visibility = Visibility.Collapsed;
			gegevensControl.Visibility = Visibility.Collapsed;
			selectAanOfUitmelden.Visibility = Visibility.Collapsed;
		}

		private void NavigeerNaarAanmeldScherm(object sender, RoutedEventArgs e)
		{
			aanmeldingPanel.Visibility = Visibility.Visible;
			kiesBedrijfControl.Visibility = Visibility.Visible;
			gegevensControl.Visibility = Visibility.Collapsed;
			selectAanOfUitmelden.Visibility = Visibility.Collapsed;
		}

		public void ResetSchermNaarStart()
		{
			selectAanOfUitmelden.Visibility = Visibility.Visible;
			aanmeldingPanel.Visibility = Visibility.Collapsed;

			gegevensControl.Visibility = Visibility.Collapsed;
			kiesBedrijfControl.Visibility = Visibility.Collapsed;
			afmeldenControl.Visibility = Visibility.Collapsed;
		}

		public void SwitchNaarGegevensControl(string bedrijf)
		{
			aanmeldingPanel.Visibility = Visibility.Visible;
			gegevensControl.Visibility = Visibility.Visible;
			selectAanOfUitmelden.Visibility = Visibility.Collapsed;
			kiesBedrijfControl.Visibility = Visibility.Collapsed;
			GegevensControl? gegevenscontrol = gegevensControl.DataContext as GegevensControl;
			gegevenscontrol.ZetGeselecteerdBedrijf(bedrijf);
		}
	}
}
