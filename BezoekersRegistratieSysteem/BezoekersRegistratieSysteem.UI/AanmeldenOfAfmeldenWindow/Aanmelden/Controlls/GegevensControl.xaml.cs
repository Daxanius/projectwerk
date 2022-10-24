using BezoekersRegistratieSysteem.UI.Aanmelden.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
	/// Interaction logic for InputControl.xaml
	/// </summary>
	public partial class GegevensControl : UserControl, INotifyPropertyChanged
	{

		private string _voornaam;
		public string Voornaam {
			get {
				return _voornaam;
			}
			set {
				_voornaam = value;
				UpdatePropperty();
			}
		}

		private string _achternaam;
		public string Achternaam {
			get {
				return _achternaam;
			}
			set {
				_achternaam = value;
				UpdatePropperty();
			}
		}

		private string _email;
		public string Email {
			get {
				return _email;
			}
			set {
				_email = value;
				UpdatePropperty();
			}
		}

		private string _bedrijf;
		public string Bedrijf {
			get {
				return _bedrijf;
			}
			set {
				_bedrijf = value;
				UpdatePropperty();
			}
		}

		private string _werknemer;
		public string Werknemer {
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

		private List<string> _werkNemersLijst = new() { "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder", "Weude", "Balding", "Balder" };
		public List<string> WerknemersLijst {
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

			BedrijfsNaam = bedrijfsNaam;
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
				GegevensInfoDTO gegevensInfo = new(Voornaam, Achternaam, Email, Bedrijf, Werknemer);
			} catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			gegevensControl.Opacity = .25;
			gegevensControl.IsHitTestVisible = false;
			popup.IsOpen = true;

			Task.Run(() =>
			{
				Thread.Sleep(TimeSpan.FromSeconds(3));
				Dispatcher.Invoke(() =>
				{
					popup.IsOpen = false;
					gegevensControl.Opacity = 1;
					gegevensControl.IsHitTestVisible = true;

					Window window = Window.GetWindow(this);
					AanOfUitMeldenScherm aanOfUitMeldenScherm = window.DataContext as AanOfUitMeldenScherm;
					aanOfUitMeldenScherm.ResetSchermNaarStart();
				});
			});

			Voornaam = string.Empty;
			Achternaam = string.Empty;
			Email = string.Empty;
			Bedrijf = string.Empty;
			Werknemer = string.Empty;
		}
	}
}