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

namespace BezoekersRegistratieSysteem.UI.Aanmelden.User_Control
{
	/// <summary>
	/// Interaction logic for InputControl.xaml
	/// </summary>
	public partial class GegevensControl : UserControl, INotifyPropertyChanged
	{
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

		private void GaVerderButtonClickEvent(object sender, RoutedEventArgs e)
		{
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
				});
			});

			//Ga terug naar start, als je langs start gaat krijg je 200 euro.
		}
	}
}
