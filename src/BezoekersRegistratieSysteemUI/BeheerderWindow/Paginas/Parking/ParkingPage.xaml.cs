using BezoekersRegistratieSysteemUI.Api;
using BezoekersRegistratieSysteemUI.Beheerder;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Afspraken.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Parking.Controls;
using BezoekersRegistratieSysteemUI.BeheerderWindowPaginas.Werknemers.Controls;
using BezoekersRegistratieSysteemUI.Events;
using BezoekersRegistratieSysteemUI.Grafiek;
using BezoekersRegistratieSysteemUI.Model;
using BezoekersRegistratieSysteemUI.Nutsvoorzieningen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        private List<ParkeerplaatsDTO> _initieleZoekTermParkeerplaats;
        private string _zoekText;
        public string ZoekText
        {
            get => _zoekText;
            set
            {
                if (value.IsNietLeeg())
                {
                    _zoekText = value.ToLower();

                    List<ParkeerplaatsDTO> result = _initieleZoekTermParkeerplaats.Where(n => n.Nummerplaat.ToLower().Contains(_zoekText) || 
                    n.Starttijd.ToShortTimeString().ToLower().Contains(_zoekText)).ToList();

                    NummerplaatLijstControl.ItemSource.Clear();

                    foreach (ParkeerplaatsDTO nummerplaat in result)
                    {
                        NummerplaatLijstControl.ItemSource.Add(nummerplaat);
                    }

                }
                else if (value.Length == 0)
                {
                    NummerplaatLijstControl.ItemSource.Clear();
                    foreach (ParkeerplaatsDTO nummerplaat in _initieleZoekTermParkeerplaats)
                    {
                        NummerplaatLijstControl.ItemSource.Add(nummerplaat);
                    }
                }
            }
        }

        public ParkingPage() {
            this.DataContext = this;
            InitializeComponent();

            UpdateGeselecteerdBedrijf_Event();

            FullWidth = (int)SystemParameters.PrimaryScreenWidth;
			FullHeight = (int)SystemParameters.PrimaryScreenHeight;

			BedrijfEvents.GeselecteerdBedrijfChanged += UpdateGeselecteerdBedrijf_Event;

			this.DataContext = this;
			InitializeComponent();
			InitializeGraph();
        }

        private void ZoekTermChanged(object sender, TextChangedEventArgs e)
        {
            Task.Run(() => Dispatcher.Invoke(() => ZoekText = ZoekTextTextbox.Text));
        }

        private void UpdateGeselecteerdBedrijf_Event() {
			InitializeGraph();
            NummerplaatLijstControl.ItemSource.Clear();
            UpdatePropperty(nameof(GeselecteerdBedrijf));
            UpdateHuidigeNummerplatenOpScherm();
        }

        private void UpdateHuidigeNummerplatenOpScherm()
        {
            foreach (ParkeerplaatsDTO parkeerplaats in ApiController.GeefNummerplaten(GeselecteerdBedrijf.Id).ToList().OrderBy(n => n.Starttijd))
            {
				NummerplaatLijstControl.ItemSource.Add(parkeerplaats);
            }
            _initieleZoekTermParkeerplaats = new(NummerplaatLijstControl.ItemSource);
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

		private void InitializeGraph()
		{
			Grafiek.Datasets.Clear();
            Grafiek1.Datasets.Clear();
			
            var dataDag = ApiController.GeefParkeerplaatsDagoverzichtVanBedrijf(GeselecteerdBedrijf.Id);
            var dataWeek = ApiController.GeefParkeerplaatsWeekoverzichtVanBedrijf(GeselecteerdBedrijf.Id);

            Grafiek.KolomLabels = dataDag.CheckInsPerUur.Keys.ToList();
            Grafiek1.KolomLabels = dataWeek.GeparkeerdenTotaalPerWeek.Keys.ToList();

            GrafiekDataset dataSetCheckinsPerUur = new()
            {
                Data = dataDag.CheckInsPerUur.Values.ToList().ConvertAll(x => (double)x),
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#272944"),
                Label = "Huidig"
            };

            GrafiekDataset dataSetTotaalGeparkeerden = new()
            {
                Data = dataDag.GeparkeerdenTotaalPerUur.Values.ToList().ConvertAll(x => (double)x),
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#404BDA"),
                Label = "Totaal"
            };

            GrafiekDataset dataSetWeek = new()
            {
                Data = dataWeek.GeparkeerdenTotaalPerWeek.Values.ToList().ConvertAll(x => (double)x),
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#404BDA")
            };

            Grafiek.Datasets.Add(dataSetTotaalGeparkeerden);
            Grafiek.Datasets.Add(dataSetCheckinsPerUur);
            Grafiek1.Datasets.Add(dataSetWeek);

            Grafiek1.InvalidateVisual();
            Grafiek.InvalidateVisual();
        }
    }
}
