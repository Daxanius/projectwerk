using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Grafiek;
using BezoekersRegistratieSysteemUI.Model;
using System.ComponentModel;
using System.Linq;
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

			BedrijfEvents.GeselecteerdBedrijfChanged += UpdateGeselecteerdBedrijf_Event;

			this.DataContext = this;
			InitializeComponent();

			var dataDag = ApiController.GeefParkeerplaatsDagoverzichtVanBedrijf(GeselecteerdBedrijf.Id);
			var dataWeek = ApiController.GeefParkeerplaatsWeekoverzichtVanBedrijf(GeselecteerdBedrijf.Id);

			Grafiek.KolomLabels = dataDag.CheckInsPerUur.Keys.ToList();
			Grafiek1.KolomLabels = dataWeek.GeparkeerdenTotaalPerWeek.Select(i => i.Item1).ToList();

			GrafiekDataset dataSetCheckinsPerUur = new() {
				Data = dataDag.CheckInsPerUur.Values.ToList().ConvertAll(x => (double)x),
				Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#404BDA"),
				Label = "Checkins"
			};

			GrafiekDataset dataSetTotaalGeparkeerden = new() {
				Data = dataDag.GeparkeerdenTotaalPerUur.Values.ToList().ConvertAll(x => (double)x),
				Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#272944"),
				Label = "Totaal Geparkeerden"
			};

			GrafiekDataset dataSetWeek = new() {
				Data = dataWeek.GeparkeerdenTotaalPerWeek.Select(i => (double)i.Item2).ToList(),
				Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#404BDA")
			};

			Grafiek.Datasets.Add(dataSetCheckinsPerUur);
			Grafiek.Datasets.Add(dataSetTotaalGeparkeerden);
			Grafiek1.Datasets.Add(dataSetWeek);
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
