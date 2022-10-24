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
		public AanOfUitMeldenScherm()
		{
			this.DataContext = this;
			InitializeComponent();
		}

		private void NavigeerNaarAfmeldScherm(object sender, RoutedEventArgs e)
		{
			ToggleSelectieScherm();
		}

		private void NavigeerNaarAanmeldScherm(object sender, RoutedEventArgs e)
		{
			ToggleSelectieScherm();
			aanmeldingControl.Visibility = Visibility.Visible;
		}

		private void ToggleSelectieScherm()
		{
			if (selectAanOfUitmelden.Visibility == Visibility.Visible)
			{
				selectAanOfUitmelden.Visibility = Visibility.Collapsed;
			} else
			{
				selectAanOfUitmelden.Visibility = Visibility.Visible;
			}
		}

		public void ResetSchermNaarStart()
		{
			selectAanOfUitmelden.Visibility = Visibility.Visible;
			aanmeldingControl.Visibility = Visibility.Collapsed;
		}
	}
}
