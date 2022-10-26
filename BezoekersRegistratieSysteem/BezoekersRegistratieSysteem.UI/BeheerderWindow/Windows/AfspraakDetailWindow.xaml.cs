using BezoekersRegistratieSysteem.UI.BeheerderWindow.DTO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BezoekersRegistratieSysteem.UI.BeheerderWindow.Controlls.DetailControls
{
	/// <summary>
	/// Interaction logic for AfspraakDetailControl.xaml
	/// </summary>
	public partial class AfspraakDetailWindow : Window
	{
		public AfspraakDTO Afspraak;

		public AfspraakDetailWindow(AfspraakDTO afspraak)
		{
			Afspraak = afspraak;
			this.DataContext = this;
			InitializeComponent();
		}

		private void WijzigBezoeker(object sender, RoutedEventArgs e)
		{

		}

		private void WijzigWerknemer(object sender, RoutedEventArgs e)
		{

		}

		private void WijzigTijdstip(object sender, RoutedEventArgs e)
		{

		}

		private void VerwijderAfspraak(object sender, RoutedEventArgs e)
		{

		}
	}
}
