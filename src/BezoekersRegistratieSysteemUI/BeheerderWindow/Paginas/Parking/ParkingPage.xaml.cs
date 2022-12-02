using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Grafiek;
using BezoekersRegistratieSysteemUI.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

			//Grafiek.Lijnen.Clear(); Grafiek.Kolommen.Clear();

			Grafiek.KolomLabels = new() { "0u", "1u", "2u", "3u", "4u", "5u", "6u", "7u", "8u", "9u", "10u", "12u", "13u", "14u", "15u", "16u", "17u", "18u", "19u", "20u", "21u", "22u", "23u" };
			Grafiek.Datasets.Add(new GrafiekDataset { Data = new() { 0, 0, 0, 1, 9, 18, 20, 3, 19, 56, 21, 48, 45, 33, 91, 28, 24, 63, 100, 78, 32, 48, 69 }, Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#272944"), StrokeThickness = 5, Label = "Totaal" });
			Grafiek.Datasets.Add(new GrafiekDataset { Data = new() { 69, 48, 32, 78, 100, 63, 24, 28, 91, 33, 45, 48, 21, 56, 19, 3, 20, 18, 9, 1, 0, 0, 0 }, Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#404BDA"), StrokeThickness = 5, Label = "Gemiddelde" });
			Grafiek.TextPadding = 20;

			Grafiek1.KolomLabels = new() { "MA", "DI", "WO", "DO", "VR", "ZA", "ZO" };
			Grafiek1.Datasets.Add(new GrafiekDataset { Data = new() { 69, 48, 32, 78, 100, 63, 24 }, Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#404BDA") });
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

		private void LineGraph_Loaded(object sender, RoutedEventArgs e) {
			Grafiek.Width = LineGraph.RenderSize.Width * 0.75;
			Grafiek.Height = LineGraph.RenderSize.Height * 0.65;

		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e) {
			if (LineGraph.RenderSize.Width == 0) {
				return;
			}
			Grafiek.Width = LineGraph.RenderSize.Width * 0.75;
			Grafiek.Height = LineGraph.RenderSize.Height * 0.65;

			Grafiek1.Width = LineGraph.RenderSize.Width * 0.35;
			Grafiek1.Height = LineGraph.RenderSize.Height * 0.65;
		}

		private void BarGraph_Loaded(object sender, RoutedEventArgs e) {
			Grafiek1.Width = LineGraph.RenderSize.Width * 0.35;
			Grafiek1.Height = LineGraph.RenderSize.Height * 0.65;
		}
	}
}
