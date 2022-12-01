using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Grafiek;
using BezoekersRegistratieSysteemUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking {
	/// <summary>
	/// Interaction logic for ParkingPage.xaml
	/// </summary>
	public partial class ParkingPage : Page, INotifyPropertyChanged {
		public int FullWidth { get; set; }
		public int FullHeight { get; set; }
		public BedrijfDTO GeselecteerdBedrijf { get => BeheerderWindow.GeselecteerdBedrijf; }
		public ParkingPage() {
			FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;


            BedrijfEvents.UpdateGeselecteerdBedrijf += UpdateGeselecteerdBedrijf_Event;

			this.DataContext = this;
			InitializeComponent();
		}

		private void UpdateGeselecteerdBedrijf_Event() {
			UpdatePropperty(nameof(GeselecteerdBedrijf));
		}

		#region Singleton
		private static ParkingPage instance = null;
		private static readonly object padlock = new object();

		public static ParkingPage Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new ParkingPage();
					}
					return instance;
				}
			}
		}
		#endregion

		#region ProppertyChanged
		public event PropertyChangedEventHandler? PropertyChanged;
		public void UpdatePropperty([CallerMemberName] string propertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
        #endregion ProppertyChanged

        private void LineGraph_Loaded(object sender, RoutedEventArgs e)
        {
			Grafiek.Width = LineGraph.RenderSize.Width*0.85;
			Grafiek.Height = LineGraph.RenderSize.Height*0.65;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
			if (LineGraph.RenderSize.Width == 0)
			{
				return;
			}
            Grafiek.Width = LineGraph.RenderSize.Width * 0.85;
            Grafiek.Height = LineGraph.RenderSize.Height * 0.65;
        }

        private void BarGraph_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
