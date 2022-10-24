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

namespace BezoekersRegistratieSysteem.UI.Controlls
{
	/// <summary>
	/// Interaction logic for KiesBedrijfControl.xaml
	/// </summary>
	public partial class KiesBedrijfControl : UserControl
	{
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

				AanmeldingControl.BedrijfsNaam = bedrijfsNaam;
			}
		}
	}
}
