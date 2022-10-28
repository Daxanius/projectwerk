using BezoekersRegistratieSysteemUI.AanmeldenOfAfmeldenWindow.Aanmelden.DTO;
using BezoekersRegistratieSysteemUI.BeheerderWindow.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BezoekersRegistratieSysteemUI.Controlls
{
	/// <summary>
	/// Interaction logic for InputControl.xaml
	/// </summary>
	public partial class GegevensControl : UserControl, INotifyPropertyChanged
	{

		private string _voornaam = "Test VOORNAAM";
		public string Voornaam {
			get {
				return _voornaam;
			}
			set {
				_voornaam = value;
				UpdatePropperty();
			}
		}

		private string _achternaam = "Test ACHTERNAAM";
		public string Achternaam {
			get {
				return _achternaam;
			}
			set {
				_achternaam = value;
				UpdatePropperty();
			}
		}

		private string _email = "Test EMAIL";
		public string Email {
			get {
				return _email;
			}
			set {
				_email = value;
				UpdatePropperty();
			}
		}

		private string _bedrijf = "Test BEDRIJF";
		public string Bedrijf {
			get {
				return _bedrijf;
			}
			set {
				_bedrijf = value;
				UpdatePropperty();
			}
		}

		private WerknemerMetFunctieDTO _werknemer;
		public WerknemerMetFunctieDTO Werknemer {
			get {
				return _werknemer;
			}
			set {
				_werknemer = value;
				UpdatePropperty();
			}
		}

		public bool CanSubmit { get; set; }

		private string _bedrijfsNaam = "";
		public string BedrijfsNaam {
			get {
				return _bedrijfsNaam;
			}
			set {
				_bedrijfsNaam = value;
				UpdatePropperty();
			}
		}

		private List<WerknemerMetFunctieDTO> _werkNemersLijst = new() { new(1, "Weude", "Van Dirk", "CEO"), new(2, "Bjorn", "Not Balding", "CEO2"), new(3, "Balder", "Rust", "CEO3") };
		public List<WerknemerMetFunctieDTO> WerknemersLijst {
			get {
				return _werkNemersLijst;
			}
			set {
				_werkNemersLijst = value;
				UpdatePropperty();
			}
		}

		public GegevensControl()
		{
			this.DataContext = this;
			InitializeComponent();
		}

		//Zet bedrijf belijk aan prop Bedrijf
		public void ZetGeselecteerdBedrijf(string bedrijfsNaam)
		{
			if (string.IsNullOrWhiteSpace(bedrijfsNaam))
			{
				MessageBox.Show("Bedrijf is leeg", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			KrijgWerknemersVanBedrijf(bedrijfsNaam);
			BedrijfsNaam = bedrijfsNaam;
		}

		private IEnumerable<WerknemerMetFunctieDTO> KrijgWerknemersVanBedrijf(string bedrijfsNaam)
		{
			//Int bedrijfId = [GET] /api/bedrijf/{ bedrijfsNaam} => Krijg bedrijfId van bedrijfsNaam.
			//List<WerknemersDTO> werknemers = [GET] /api/werknemer/{ bedrijfId} => Krijg alle werknemers, van een bepaald bedrijf.

			return null;
		}

		#region ProppertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		public void UpdatePropperty([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		//Klik op Ga verder knop
		private void GaVerderButtonClickEvent(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Werknemer == null)
				{
					MessageBox.Show("Gelieve een werknemer te kiezen", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				BezoekerDTO nieuweBezoekerData = new(Voornaam, Achternaam, Email, Bedrijf);

				if (Werknemer.Id.HasValue)
				{
					MaakNieuweAfspraak(Werknemer.Id.Value, nieuweBezoekerData);
				}
			} catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			gegevensControl.Opacity = .15;
			gegevensControl.IsHitTestVisible = false;
			popupConform.IsOpen = true;
		}

		private void MaakNieuweAfspraak(int werknemerId, BezoekerDTO bezoeker)
		{
			//var body = {
			//	"bezoeker": {
			//		"voornaam": "string",
			//		"achternaam": "string",
			//		"email": "string",
			//		"bedrijf": "string"
			//	},
			//	"werknemerId": 0
			//}

			//AfspraakDTO insertedAfspraak = [POST] /api/afspraak = Maak een afspraak en return ze dan met id en starttijd.
		}

		private async void ConformeerPopup(object sender, RoutedEventArgs e)
		{
			popupConform.IsOpen = false;
			popupAangemeld.IsOpen = true;

			await Task.Delay(1500);

			popupAangemeld.IsOpen = false;
			gegevensControl.Opacity = 1;
			gegevensControl.IsHitTestVisible = true;

			Window window = Window.GetWindow(this);
			AanOfUitMeldenScherm aanOfUitMeldenScherm = window.DataContext as AanOfUitMeldenScherm;
			aanOfUitMeldenScherm.ResetSchermNaarStart();

			Voornaam = string.Empty;
			Achternaam = string.Empty;
			Email = string.Empty;
			Bedrijf = string.Empty;
			Werknemer = null;
		}

		private void WijzigPopup(object sender, RoutedEventArgs e)
		{
			gegevensControl.Opacity = 1;
			gegevensControl.IsHitTestVisible = true;
			popupConform.IsOpen = false;
		}

		private void PlaceholdersListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			ListBoxItem? item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
			if (item is not null)
			{
				WerknemerMetFunctieDTO selectedItem = item.Content as WerknemerMetFunctieDTO;
				Werknemer = selectedItem;
			}
		}

		private void GaTerug(object sender, MouseButtonEventArgs e)
		{
			Voornaam = string.Empty;
			Achternaam = string.Empty;
			Email = string.Empty;
			Bedrijf = string.Empty;
			Werknemer = null;

			Window window = Window.GetWindow(this);
			AanOfUitMeldenScherm aanOfUitMeldenScherm = window.DataContext as AanOfUitMeldenScherm;
			aanOfUitMeldenScherm.gegevensControl.Visibility = Visibility.Collapsed;
			aanOfUitMeldenScherm.kiesBedrijfControl.Visibility = Visibility.Visible;
		}
	}
}