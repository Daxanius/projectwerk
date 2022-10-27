using BezoekersRegistratieSysteemUI.Exceptions;
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

namespace BezoekersRegistratieSysteemUI.Controlls
{
	/// <summary>
	/// Interaction logic for KiesBedrijfControl.xaml
	/// </summary>
	public partial class KiesBedrijfControl : UserControl
	{
		private string _bedrijfsNaam;
		public KiesBedrijfControl()
		{
			InitializeComponent();
		}

		private void SelecteerBedrijfHandler(object sender, MouseButtonEventArgs e)
		{
			ListBoxItem bedrijfItemUitLijst = (ListBoxItem)sender;
			if (bedrijfItemUitLijst != null)
			{
				//Selecteer het item
				bedrijfItemUitLijst.IsSelected = true;

				//Krijg naam van bedrijf uit aangeklikt ListViewItem
				string bedrijfsNaam = (string)((ListBoxItem)bedrijfLijst.SelectedValue).Content;

				if (string.IsNullOrWhiteSpace(bedrijfsNaam))
					return;

				kiesBedrijfControl.Opacity = .15;
				kiesBedrijfControl.IsHitTestVisible = false;
				popup.IsOpen = true;

				_bedrijfsNaam = bedrijfsNaam;
			}
		}

		private void ConformeerPopup(object sender, RoutedEventArgs e)
		{
			popup.IsOpen = false;
			if (string.IsNullOrWhiteSpace(_bedrijfsNaam))
				throw new KiesBedrijfException("Bedrijf is niet geselecteerd");

			AanOfUitMeldenScherm? window = Window.GetWindow(this) as AanOfUitMeldenScherm;
			window?.SwitchNaarGegevensControl(_bedrijfsNaam);

			_bedrijfsNaam = string.Empty;
			bedrijfLijst.SelectedItem = null;

			kiesBedrijfControl.Opacity = 1;
			kiesBedrijfControl.IsHitTestVisible = true;
			popup.IsOpen = false;
		}

		private void WijzigPopup(object sender, RoutedEventArgs e)
		{
			popup.IsOpen = false;
			kiesBedrijfControl.Opacity = 1;
			kiesBedrijfControl.IsHitTestVisible = true;
			_bedrijfsNaam = string.Empty;
		}

		private void GaTerug(object sender, MouseButtonEventArgs e)
		{
			_bedrijfsNaam = string.Empty;
			bedrijfLijst.SelectedItem = null;

			Window window = Window.GetWindow(this);
			AanOfUitMeldenScherm aanOfUitMeldenScherm = window.DataContext as AanOfUitMeldenScherm;
			aanOfUitMeldenScherm.ResetSchermNaarStart();
		}
	}
}
