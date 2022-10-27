using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Controlls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
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

namespace BezoekersRegistratieSysteemUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public double ScaleX { get; set; }

		public double ScaleY { get; set; }

		public MainWindow()
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

		private void ParkeerButton_Click(object sender, RoutedEventArgs e)
		{
			ParkeerWindow parkeerWindow = new ParkeerWindow();
			this.Close();
			parkeerWindow.ShowDialog();
		}

		private void AanmeldButton_Click(object sender, RoutedEventArgs e)
		{
			AanOfUitMeldenScherm aanmeldWindow = new AanOfUitMeldenScherm();
			this.Close();
			aanmeldWindow.ShowDialog();
		}

		private void AdminButton_Click(object sender, RoutedEventArgs e)
		{
			BeheerderDashboard parkeerWindow = new BeheerderDashboard();
			this.Close();
			parkeerWindow.ShowDialog();
		}
	}
}
