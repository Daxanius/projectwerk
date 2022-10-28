using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
using BezoekersRegistratieSysteemUI.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BezoekersRegistratieSysteemUI.Controlls;

/// <summary>
/// Interaction logic for KiesBedrijfControl.xaml
/// </summary>
public partial class KiesBedrijfControl : UserControl
{
	/// <summary>
	/// Lijst met alle bedrijven die de bezoeker kan bezoeken
	/// </summary>
	public ObservableCollection<BedrijfDTO> Bedrijven { get; set; } = new();

	/// <summary>
	/// Dit kennen jullie wel he ;-)
	/// </summary>
	public KiesBedrijfControl()
	{
		GetBedrijvenToeAanLijst();
		InitializeComponent();
	}

	/// <summary>
	/// De bezoeker heeft op een bedrijf uit de lijst geklikt
	/// </summary>
	/// <param name="sender">Control info</param>
	/// <param name="e">Click info</param>
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

			AanOfUitMeldenScherm? window = Window.GetWindow(this) as AanOfUitMeldenScherm;
			window?.SwitchNaarGegevensControl(bedrijfsNaam);

			bedrijfLijst.SelectedItem = null;
		}
	}

	/// <summary>
	/// HTTP GET request om alle bedrijven te verkrijgen van de datasource
	/// </summary>
	private void GetBedrijvenToeAanLijst()
	{
		//List<Bedrijf> bedrijven = [GET] /api/bedrijf => Alle bedrijven.
	}

	/// <summary>
	/// De bezoeker wil terug naar het vorige scherm
	/// </summary>
	/// <param name="sender">Button info</param>
	/// <param name="e">Click info</param>
	private void GaTerug(object sender, MouseButtonEventArgs e)
	{
		bedrijfLijst.SelectedItem = null;

		Window window = Window.GetWindow(this);
		AanOfUitMeldenScherm aanOfUitMeldenScherm = window.DataContext as AanOfUitMeldenScherm;
		aanOfUitMeldenScherm.ResetSchermNaarStart();
	}
}
